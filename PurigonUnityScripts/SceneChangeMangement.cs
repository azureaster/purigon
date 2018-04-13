using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeMangement : MonoBehaviour {

    public void GoToScoreBoardScene() {
        SceneManager.LoadScene("ScoreBoardScene");
    }

    public void GoToChangePurigonScene(){
        SceneManager.LoadScene("ChangePurigonScene");
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void GoToUserSinglePrepareScene()
    {
        SceneManager.LoadScene("UserSinglePrepareScene");
    }


    public void PlaySingleMode() {
        int mapID = GameObject.Find("MapDropdown").GetComponent<Dropdown>().value + 1;

        if(mapID == 1) SceneManager.LoadScene("Single01_Forest");
        else if (mapID == 2) SceneManager.LoadScene("Single02_Jungle");
        else if (mapID == 3) SceneManager.LoadScene("Single03_Underwater");


    }


}
