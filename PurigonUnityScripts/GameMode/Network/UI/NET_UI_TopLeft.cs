using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;



public class NET_UI_TopLeft : MonoBehaviour {

    [Serializable]
    public class DistanceName {
        public float FlightDistance;
        public string UserName;

        public DistanceName(float distance, string name) {
            FlightDistance = distance;
            UserName = name;
        }
    }

    public string[] UserData;
    public Vector3[] UIPosition = new Vector3[6];
    public DistanceName[] distanceArr;
    
    public GameObject userPurigonUI;
    public GameObject HpBar;
    public Text nickName;
    public Text distanceTxt;

    float maxHP;
    float charHP;
    string username;
    float flightRecordDistance = 0.0f;
    int currentRanking = 0;

    private PhotonView pv = null;
    Hashtable playerCustomSettings = new Hashtable();

    void Start() {
        pv = this.GetComponent<PhotonView>();
        userPurigonUI = this.gameObject;
        username = PlayerPrefs.GetString("userName", "UserName");
        pv.RPC("UIName", PhotonTargets.All, username);

        InitDistanceArr();

        playerCustomSettings.Add("FlightDistance", flightRecordDistance);
        playerCustomSettings.Add("CurrentRanking", currentRanking);
        PhotonNetwork.player.SetCustomProperties(playerCustomSettings);

    }

    void Update()
    {
        if (!pv.isMine)
            return;

        maxHP = GameObject.Find("userPurigon").GetComponent<NETPurigonCtrl>().maxHP;
        charHP = GameObject.Find("userPurigon").GetComponent<NETPurigonCtrl>().CharHP;
        flightRecordDistance = PlayerPrefs.GetFloat("userFlyDistance", 0.0f);

        playerCustomSettings["FlightDistance"] = flightRecordDistance;
        PhotonNetwork.player.SetCustomProperties(playerCustomSettings);

        pv.RPC("UIHPBar", PhotonTargets.All, charHP, maxHP);
        pv.RPC("UIDistance", PhotonTargets.All, flightRecordDistance);

        //[거리,이름] 업데이트
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            distanceArr[i].FlightDistance = (float)PhotonNetwork.playerList[i].CustomProperties["FlightDistance"];
            distanceArr[i].UserName = PhotonNetwork.playerList[i].NickName;
        }

        //정렬
        SortDistanceArr(distanceArr);

        //UI 위치 이동
        for (int i = 0; i < distanceArr.Length; i++)
        {
            if (distanceArr[i].UserName == username) {
                //this.transform.position = UIPosition[i];
                this.transform.position = Vector3.Lerp(this.transform.position, UIPosition[i], Time.deltaTime * 3.0f);
                playerCustomSettings["CurrentRanking"] = PhotonNetwork.playerList.Length - i;
                PhotonNetwork.player.SetCustomProperties(playerCustomSettings);
                break;
            }
        }
    }

    [PunRPC]
    void UIName(string username) {
        this.nickName.text = username;
    }

    [PunRPC]
    void UIDistance(float flightRecordDistance) {
        this.distanceTxt.text = flightRecordDistance.ToString("00") + "M";
    }

    [PunRPC]
    void UIHPBar(float charHP, float maxHP)
    {
        this.HpBar.GetComponent<Image>().fillAmount = charHP / maxHP;
    }

    [PunRPC]
    void InstantiateTopLeftUI(Vector3 position, string purigonFaceName)
    {
        userPurigonUI.transform.SetParent(GameObject.Find("UICanvas").transform);
        userPurigonUI.transform.position = position;

        foreach (Transform child in userPurigonUI.transform)
        {
            if (child.name == "CharImg")
            {
                child.GetComponent<Image>().sprite = Resources.Load<Sprite>("MyImage/" + purigonFaceName);
                Debug.Log(purigonFaceName);
                break;
            }
        }
    }


    void InitDistanceArr()
    {
        distanceArr = new DistanceName[PhotonNetwork.playerList.Length];

        for(int i =0; i<distanceArr.Length;i++)
            distanceArr[i] = new DistanceName(0.0f, "DefaultName");

    }

    void SortDistanceArr(DistanceName[] distanceArr)
    {
        for (int i = 0; i < distanceArr.Length; i++)
        {
            for (int j = 0; j < distanceArr.Length - 1; j++)
            {
                if (distanceArr[j].FlightDistance > distanceArr[j + 1].FlightDistance)
                {
                    var temp = distanceArr[j + 1];
                    distanceArr[j + 1] = distanceArr[j];
                    distanceArr[j] = temp;
                }
            }
        }
    }


}
