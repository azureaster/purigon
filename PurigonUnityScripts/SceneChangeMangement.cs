using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeMangement : MonoBehaviour {

    public void Awake()
    {
        PhotonNetwork.isMessageQueueRunning = true;
    }

    public void GoToScoreBoardScene() {
        PhotonNetwork.isMessageQueueRunning = false;
        SceneManager.LoadScene("ScoreBoardScene");
    }

    public void GoToChangePurigonScene(){
        PhotonNetwork.isMessageQueueRunning = false;
        SceneManager.LoadScene("ChangePurigonScene");
    }

    public void GoToMainScene()
    {
        PhotonNetwork.isMessageQueueRunning = false;
        SceneManager.LoadScene("MainScene");
    }

    public void GoToUserSinglePrepareScene()
    {
        PhotonNetwork.isMessageQueueRunning = false;
        SceneManager.LoadScene("UserSinglePrepareScene");
    }


    public void PlaySingleMode() {
        int mapID = GameObject.Find("MapDropdown").GetComponent<Dropdown>().value + 1;
        PlayerPrefs.SetInt("CurrentMapNo", mapID);

        PhotonNetwork.isMessageQueueRunning = false;
        if (mapID == 1) SceneManager.LoadScene("Single01_Forest");
        else if (mapID == 2) SceneManager.LoadScene("Single02_Jungle");
        else if (mapID == 3) SceneManager.LoadScene("Single03_Underwater");

    }

    public void PlaySingleAgain() {
        int mapID =PlayerPrefs.GetInt("CurrentMapNo", 00);

        PhotonNetwork.isMessageQueueRunning = false;
        if (mapID == 1) SceneManager.LoadScene("Single01_Forest");
        else if (mapID == 2) SceneManager.LoadScene("Single02_Jungle");
        else if (mapID == 3) SceneManager.LoadScene("Single03_Underwater");
        else Debug.Log("Error Occured");

    }
    


}
