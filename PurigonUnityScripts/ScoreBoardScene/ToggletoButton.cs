using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggletoButton : MonoBehaviour {

    public Toggle DailyTotal;
    public bool pressed = true;

    public void OnGUI()
    {
        pressed = GUILayout.Toggle(pressed, "TOTALBEST", "Button");
    }



}
