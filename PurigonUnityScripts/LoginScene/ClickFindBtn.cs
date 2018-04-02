using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickFindBtn : MonoBehaviour {

    public GameObject FindIdPanel;
    public GameObject ShowIdPanel;

    public InputField[] inputFieldRef;

    public void CancleFindIdPanel()
    {
        for (int i = 0; i < 3; i++)
        { //입력받은 data 삭제
            inputFieldRef[i].text = "";
        }

        if (FindIdPanel.gameObject.activeSelf == true)
        {
            FindIdPanel.gameObject.SetActive(false);
        }

        if (ShowIdPanel.gameObject.activeSelf == false)
        {
            ShowIdPanel.gameObject.SetActive(true);
        }
    }
}
