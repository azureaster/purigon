using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NET_UI_TimeTxt : MonoBehaviour {
    Text timeTxt;
    float maxTime = 90f;
    private float timeCnt;
    float countDown;
    string timeStr;

    void Start()
    {
        timeTxt = GetComponent<Text>();
    }

    void Update()
    {
        timeCnt += Time.deltaTime;
        countDown = maxTime - timeCnt;
    }

    void OnGUI()
    {
        if (GameObject.Find("userPurigon").GetComponent<NETPurigonCtrl>().IsDead == true)
        {
            return;
        }
        //00:00으로 하면 1초단위로 업데이트, 00.00으로 하면 0.01초 단위로 업데이트
        timeStr = "" + countDown.ToString("00.00");
        //게임 화면에서 .을 :로 보이게 해줌
        timeStr = timeStr.Replace(".", ":");

        timeTxt.text = "Time : " + timeStr;
    }

}
