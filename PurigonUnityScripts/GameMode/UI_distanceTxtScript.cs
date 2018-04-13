using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_distanceTxtScript : MonoBehaviour {
    
    Text distanceTxt;
    private float flightDistance;
    public float flightRecordDistance;
    
    public GameObject purigonPosition; 

    void Start()
    {
        distanceTxt = GameObject.Find("DistanceTxt").GetComponent<Text>();
    }


    void Update()
    {
        purigonPosition = GameObject.Find("UserPurigonController").GetComponent<InstantiateUserPurigon>().userPurigon;
        flightDistance = purigonPosition.transform.position.x - GameObject.Find("UserPurigonController").GetComponent<InstantiateUserPurigon>().purigonLocation.x;
        flightRecordDistance = flightDistance * 10f;
        
        distanceTxt.text = flightRecordDistance.ToString("00.00") + "M";
    }
}
