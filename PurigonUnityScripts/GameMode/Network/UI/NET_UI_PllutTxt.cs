using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NET_UI_PllutTxt : MonoBehaviour {

    Text pllutTxt;

    // Use this for initialization
    void Start()
    {
        pllutTxt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //pllutTxt.text = PurigonCtrl.pllut.ToString();
        pllutTxt.text = "Pllut : " + NETPurigonCtrl.pllut.ToString();

    }

}
