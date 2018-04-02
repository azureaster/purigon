using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


}
