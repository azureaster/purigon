using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NET_UI_DistanceTxt : MonoBehaviour {

    public Text distanceTxt;
    private float flightDistance;
    public float flightRecordDistance;

    public GameObject purigonPosition;

    void Start()
    {
        distanceTxt = this.GetComponent<Text>();
    }


    void Update()
    {
        purigonPosition = GameObject.Find("UserPurigonController").GetComponent<NETInstantiateUserPurigon>().userPurigon;
        flightDistance = purigonPosition.transform.position.x - GameObject.Find("UserPurigonController").GetComponent<NETInstantiateUserPurigon>().purigonLocation.x;
        flightRecordDistance = flightDistance * 10f;

        PlayerPrefs.SetFloat("userFlyDistance", flightRecordDistance);
        distanceTxt.text = flightRecordDistance.ToString("00.00") + "M";
    }

    

}
