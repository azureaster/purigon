using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCancleBtn : MonoBehaviour {
    public GameObject Panel;

    public InputField[] inputFieldRef;

    public void CanclePanel()
    {
        if (Panel.tag == "FindIDPanel")
        {
            for (int i = 0; i < 3; i++)
            { //입력받은 data 삭제
                inputFieldRef[i].text = "";
            }
        }
        else if (Panel.tag == "JoinPanel")
        {
            for (int i = 0; i < 5; i++)
            { //입력받은 data 삭제
                inputFieldRef[i].text = "";
            }
        }
        
        Panel.gameObject.SetActive(false);
    }
}
