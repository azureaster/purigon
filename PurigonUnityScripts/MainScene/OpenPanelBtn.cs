using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPanelBtn : MonoBehaviour {

    public GameObject Panel;

    public void SettingPannel()
    {
        if (Panel.gameObject.activeSelf == false)
        {
            Panel.gameObject.SetActive(true);
        }
    }
}
