using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_CountDown : MonoBehaviour {

    public Text timeTxt;
    int time = 10;

    void Awake()
    {
        InvokeRepeating("CountDown", 1, 1);
    }

    void CountDown()
    {
        time -= 1;
    }
    void OnGUI()
    {
        timeTxt.text = time.ToString();
    }
    void Update()
    {
        if (time <= 0)
        {
            PhotonNetwork.LoadLevel("GameOverScene_MultiPlayMode");
            //SceneManager.LoadScene("UserMatchScene");
        }
    }

}
