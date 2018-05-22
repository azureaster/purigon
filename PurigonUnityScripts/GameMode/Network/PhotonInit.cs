using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviour {

    public string version = "v1.0";
    public string roomName;
    public string userName;
    private int userCHAR;
    
    void Awake(){

        if (!PhotonNetwork.connected) {
            PhotonNetwork.ConnectUsingSettings(version);
        }
        
        roomName = "ROOM_" + Random.Range(0, 999).ToString("000");
        
        userName = PlayerPrefs.GetString("userName", "Default NAME");
        userCHAR = PlayerPrefs.GetInt("CharID", 1);
    }
    private void Start()
    {
        PhotonNetwork.automaticallySyncScene = false;
    }

    void OnJoinedLobby() {
        Debug.Log("Entered Lobby !");
    }

    void OnPhotonRandomJoinFailed() {
        Debug.Log("No rooms !");
        OnClickCreateRoom();
    }

    void OnJoinedRoom() {
        Debug.Log("Enter Room");
        
        StartCoroutine(this.LoadLobbyScene());
    }
    
    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.player.NickName = userName;
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnClickCreateRoom() {

        string _roomName = roomName;
        PhotonNetwork.player.NickName = userName;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 6;

        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
        Debug.Log("Create Room Failed = " + codeAndMsg[1]);
    }


    IEnumerator LoadLobbyScene()
    {
        PhotonNetwork.isMessageQueueRunning = false;
        AsyncOperation ao = SceneManager.LoadSceneAsync("UserMatchScene");
        yield return ao;
    }


    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());    
    }
}
