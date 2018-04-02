using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickJoinBtn : MonoBehaviour
{
    public GameObject Panel;

    public void JoinPanel()
    {
        if (Panel.gameObject.activeSelf == false)
        {
            Panel.gameObject.SetActive(true);
        }
    }
}
