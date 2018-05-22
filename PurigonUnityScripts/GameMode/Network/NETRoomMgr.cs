using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NETRoomMgr : MonoBehaviour {

    public GameObject userPurigon;
    public GameObject[] userPosition;
    GameObject userInfo;

    public Vector3 purigonScale;
    public Vector3 purigonLocation;
    public Vector3 purigonRotation;

    public string purigonPrefName;
    public string userName;
    public int userCHAR;

    public int[] posUserExistArr = new int[6] { 0,0,0,0,0,0};
    
    int userPositionNum;

    private PhotonView scenePv;
    Room currRoom;
    
    public Text txtLogMsg;
    public Text txtUserInputMsg;

    public bool currentPlayerReady = false;
    public bool allPlayerReady = false;
    Hashtable playerCustomSettings = new Hashtable();

    private void Awake()
    {
        scenePv = GetComponent<PhotonView>();

        PhotonNetwork.isMessageQueueRunning = true;
        //GetConnectPlayerCount();
        GameObject.Find("MapDropdown").GetComponent<Dropdown>().value = PlayerPrefs.GetInt("CurrentMapNo", 1) - 1;

        userName = PlayerPrefs.GetString("userName", "UserName");
        userCHAR = PlayerPrefs.GetInt("CharID", 1);

        if (!PhotonNetwork.isMasterClient) {
            //Debug.Log("before: " + GetPosUserExistString());
            posUserExistArr = UpdatePosUserExistArr((string)PhotonNetwork.masterClient.CustomProperties["PositionArrString"]);
            //Debug.Log("after: " + GetPosUserExistString());
        }

        for (int i = 0; i < posUserExistArr.Length; i++)
        {
            if (posUserExistArr[i] == 0)
            {
                userPositionNum = i;
                Debug.Log("current user position: " + userPositionNum);
                PlayerPrefs.SetInt("userPositionNum", userPositionNum);
                break;
            }
        }
        UpdatePosUserExistArr(GetPosUserExistString());
        ShowPurigonPrefab(userCHAR);
    }
    
    private void Start()
    {
        currRoom = PhotonNetwork.room;
        
        PhotonNetwork.automaticallySyncScene = true;
        playerCustomSettings.Add("Ready", false);
        playerCustomSettings.Add("PositionArrString", GetPosUserExistString());

        PhotonNetwork.player.SetCustomProperties(playerCustomSettings);
        
        string msg = "\n<color=#990000>[" + PhotonNetwork.player.NickName + "]님이 방에 들어왔습니다.</color>";
        scenePv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        
    }


    private void Update()
    {
        //Debug.Log("Master: "+(string)PhotonNetwork.masterClient.CustomProperties["PositionArrString"]);
        //Debug.Log(GetPosUserExistString());

        if (PhotonNetwork.isMasterClient)
        {
            currentPlayerReady = true;
            playerCustomSettings["Ready"] = true;
            playerCustomSettings["PositionArrString"] = GetPosUserExistString();
            PhotonNetwork.player.SetCustomProperties(playerCustomSettings);
            GameObject.Find("ReadyBtnTxt").GetComponent<Text>().text = "START!";
            //if(allPlayerReady) GameObject.Find("ReadyButton").GetComponent<Image>().color = Color.green;
        }
        else
        {
            if (currentPlayerReady)
                GameObject.Find("ReadyButton").GetComponent<Image>().color = Color.green;
            else
                GameObject.Find("ReadyButton").GetComponent<Image>().color = Color.white;
        }
        
    }

    void ShowPurigonPrefab(int currentCharNum)
    {

        switch (currentCharNum)
        {
            case 1:
                purigonPrefName = "IDLEBalanceType";
                break;
            case 2:
                purigonPrefName = "IDLELightType";
                break;
            case 3:
                purigonPrefName = "IDLEHealthyType";
                break;
            case 4:
                purigonPrefName = "IDLESpeedType";
                break;
            default:
                purigonPrefName = "IDLEBalanceType";
                break;

        }

        
        userInfo = userPosition[userPositionNum];

        purigonLocation = new Vector3(userPosition[userPositionNum].transform.position.x + 45, userPosition[userPositionNum].transform.position.y - 78, userPosition[userPositionNum].transform.position.z - 40);
        userPurigon = PhotonNetwork.Instantiate("MyPrefabs/Purigon_Idle/" + purigonPrefName, purigonLocation, Quaternion.identity, 0);
        scenePv.RPC("PositionInfoUpdate", PhotonTargets.AllBuffered, userPositionNum, 1, userPurigon.GetComponent<PhotonView>().owner.NickName);

        /*
        for (int i = 0; i < userPosition.Length; i++) {
            if (userPosition[i].GetComponent<RoomUserExistCheck>().userExists == false) {
                purigonLocation = new Vector3(userPosition[i].transform.position.x + 45, userPosition[i].transform.position.y - 78, userPosition[i].transform.position.z - 40);
                userPurigon = PhotonNetwork.Instantiate("MyPrefabs/Purigon_Idle/" + purigonPrefName, purigonLocation, Quaternion.identity, 0);
                userInfo = userPosition[i];
                scenePv.RPC("PositionInfoUpdate", PhotonTargets.AllBuffered, true, userName);// userPurigon.GetComponent<PhotonView>().owner.NickName);
                break;
            }
        }
        */
        //Debug.Log(userPurigon.GetComponent<PhotonView>().owner.NickName +"");
        //userPurigon.GetComponent<DisplayUserName>().userName = userInfo.GetComponentInChildren<Text>();

        userPurigon.transform.localScale = purigonScale;
        userPurigon.transform.Rotate(purigonRotation);
        
    }

    string GetPosUserExistString() {
        string posUserExistArrString = "";
        for (int i = 0; i < posUserExistArr.Length; i++)
        {
            if (i == posUserExistArr.Length - 1)
                posUserExistArrString += posUserExistArr[i];
            else posUserExistArrString += posUserExistArr[i] + ",";
        }
        return posUserExistArrString;
    }

    int[] UpdatePosUserExistArr(string str) {
        posUserExistArr = Array.ConvertAll(str.Split(','), int.Parse);
        return posUserExistArr;
    }

    /*
    void OnPhotonPlayerConnect(PhotonPlayer newPlayer) {
        GetConnectPlayerCount();
    }
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer) {
        GetConnectPlayerCount();
    }

    void GetConnectPlayerCount() {
        Room currRoom = PhotonNetwork.room;
        txtConnectCount.text = currRoom.PlayerCount.ToString() + "/" + currRoom.MaxPlayers.ToString();
    }
    */

    [PunRPC]
    void LogMsg(string msg) {
        txtLogMsg.text = txtLogMsg.text + msg;
    }
    /*
    [PunRPC]
    void PositionInfoUpdate(bool isExist, string name)
    {
        userInfo.GetComponent<RoomUserExistCheck>().userExists = isExist;
        userInfo.GetComponentInChildren<Text>().text = name;
    }*/

    [PunRPC]
    void PositionInfoUpdate(int positionNum, int isExist, string name)
    {
        posUserExistArr[positionNum] = isExist;
        userPosition[positionNum].GetComponentInChildren<Text>().text = name;
    }

    public void OnClickExitRoom() {

        string msg = "\n<color=#990000>[" + PhotonNetwork.player.NickName + "]님이 방을 떠났습니다.</color>";
        scenePv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
        scenePv.RPC("PositionInfoUpdate", PhotonTargets.AllBuffered, userPositionNum, 0, userPurigon.GetComponent<PhotonView>().owner.NickName);
        if (PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                PhotonNetwork.playerList[i].CustomProperties["PositionArrString"] = PhotonNetwork.masterClient.CustomProperties["PositionArrString"];
            }
        }

        PhotonNetwork.LeaveRoom();
    }

    public void OnClickChatEnter() {
        if(txtUserInputMsg.text != "") {
            string msg = "\n<color=#000000>" + PhotonNetwork.player.NickName + ": " + txtUserInputMsg.text + "</color>";
            scenePv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
        }
        txtUserInputMsg.text = "";
    }

    public void OnClickReadyBtn() {
        if (PhotonNetwork.isMasterClient)
        {
            if (currRoom.PlayerCount >= 1) //Need to change to 2 (1 for testing)
            {
                //check whether all players are ready
                allPlayerReady = true;
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {
                    Debug.Log(PhotonNetwork.playerList[i].NickName + " Ready: " + PhotonNetwork.playerList[i].CustomProperties["Ready"]);
                    if ((bool)PhotonNetwork.playerList[i].CustomProperties["Ready"] == false)
                        allPlayerReady = false;
                }

                if (allPlayerReady)
                {
                    PhotonNetwork.room.IsOpen = false;

                    int mapID = GameObject.Find("MapDropdown").GetComponent<Dropdown>().value + 1;
                    PlayerPrefs.SetInt("CurrentMapNo", 1);
                    
                    if (mapID == 1) PhotonNetwork.LoadLevel("Multi01_Forest");
                    else if (mapID == 2) PhotonNetwork.LoadLevel("Multi02_Jungle");
                    else if (mapID == 3) PhotonNetwork.LoadLevel("Multi03_Underwater");
                }
                else {
                    Debug.Log("PLAYERS ARE NOT ALL READY!");
                }
            }
            else
            {
                Debug.Log("YOU NEED MORE THAN TWO PLAYERS TO PLAY GAME!");
            }
        }
        else { //for all other clients
            currentPlayerReady = !currentPlayerReady;
            playerCustomSettings["Ready"] = currentPlayerReady;
            PhotonNetwork.player.SetCustomProperties(playerCustomSettings);
            
            scenePv.RPC("ReadyStatusColor", PhotonTargets.AllBuffered, userPositionNum, currentPlayerReady);
            
        }
    }

    [PunRPC]
    void ReadyStatusColor(int userPositionNum, bool currentPlayerReady)
    {
        if(currentPlayerReady)
            userPosition[userPositionNum].GetComponent<Image>().color = new Color32(63, 201, 37, 100);
        else
            userPosition[userPositionNum].GetComponent<Image>().color = new Color32(255, 255, 255, 100);
    }



    void OnLeftRoom()
    {
        SceneManager.LoadScene("MainScene");
    }


}
