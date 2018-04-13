using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TimeTxtScript : MonoBehaviour {

    Text timeTxt;
    private float timeCnt;
    string timeStr;
    void Start()
    {
        timeTxt = GetComponent<Text>();
    }


    void Update () {
        timeCnt += Time.deltaTime;
    }

    void OnGUI()
    {
        if (timeCnt > 60f)
        {
            timeStr = "" + timeCnt.ToString("00:00");
        }
        else
        {
            //00:00으로 하면 1초단위로 업데이트, 00.00으로 하면 0.01초 단위로 업데이트
            timeStr = "" + timeCnt.ToString("00.00");
            //게임 화면에서 .을 :로 보이게 해줌
            timeStr = timeStr.Replace(".", ":");
        }
        timeTxt.text = "Time : " + timeStr;
    }

}
