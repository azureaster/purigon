using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurigonBtnCtrl : MonoBehaviour {

    public bool upClicked = false;
    public bool speedClicked = false;
    public bool skillClicked = false;


    public void OnclickUpPressed() {
        upClicked = true;
    }

    public void OnclickUpReleased() {
        upClicked = false;
    }

    public void OnclickSpeedupPressed()
    {
        speedClicked = true;
    }

    public void OnclickSpeedupReleased()
    {
        speedClicked = false;
    }

    public void OnclickSkillClicked()
    {
        skillClicked = true;
    }
    public void OnclickSkillReleased()
    {
        skillClicked = false;
    }




}
