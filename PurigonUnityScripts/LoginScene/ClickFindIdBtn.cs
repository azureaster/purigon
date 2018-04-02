using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClickFindIdBtn : MonoBehaviour {

    public GameObject Panel;

    public void FindIdPannel()
    {
        if (Panel.gameObject.activeSelf == false)
        {
            Panel.gameObject.SetActive(true);
        }
    }
}
