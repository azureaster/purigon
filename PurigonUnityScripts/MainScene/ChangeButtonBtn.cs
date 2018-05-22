using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeButtonBtn : MonoBehaviour
{
    //0일때 기본상태, 1이면 좌우반전 클릭
    public int changeButton_RL = 0;

    public void Change_RightLeft()
    {
        if (changeButton_RL==0)
            changeButton_RL = 1;
        else
            changeButton_RL = 0 ;

        PlayerPrefs.SetInt("btnPosition", changeButton_RL);
        PlayerPrefs.Save();
    }
}
