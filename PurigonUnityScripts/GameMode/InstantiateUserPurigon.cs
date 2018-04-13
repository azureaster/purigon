using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateUserPurigon : MonoBehaviour {

    public GameObject[] purigonPrefabs;
    public GameObject userPurigon;

    public Vector3 purigonScale;
    public Vector3 purigonLocation;

    public string CurrentuserID;
    private int userCHAR;

    private void Awake()
    {
        CurrentuserID = PlayerPrefs.GetString("LoginID", "Default ID");
        userCHAR = PlayerPrefs.GetInt("CharID", 1);
        ShowPurigonPrefab(userCHAR);
    }

    
   
    void ShowPurigonPrefab(int currentCharNum)
    {
        userPurigon = (GameObject)Instantiate(purigonPrefabs[currentCharNum - 1]);
        userPurigon.name = "userPurigon";

        userPurigon.transform.localScale = purigonScale;
        userPurigon.transform.localPosition = purigonLocation;
    }



}
