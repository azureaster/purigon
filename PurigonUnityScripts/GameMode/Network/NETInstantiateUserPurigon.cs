using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NETInstantiateUserPurigon : MonoBehaviour {

    public GameObject userPurigon;
    public GameObject userPurigonUI;

    public Vector3 purigonLocation;
    public Vector3[] purigonPosition = new Vector3[6];
    public Vector3[] UILocation = new Vector3[6];

    public string currentUserName;
    public string purigonPrefName;
    public string purigonFaceName;

    private int userCHAR;
    private int userPosition;
    private int cnt = 0;


    private PhotonView pv = null;


    private void Awake()
    {
        currentUserName = PlayerPrefs.GetString("LoginID", "Default ID");
        userCHAR = PlayerPrefs.GetInt("CharID", 1);
        userPosition = PlayerPrefs.GetInt("userPositionNum", 0);
        PhotonNetwork.isMessageQueueRunning = true;

        ShowPurigonPrefab(userCHAR, userPosition);
    }
   
    void ShowPurigonPrefab(int currentCharNum, int userPosition)
    {
        switch (currentCharNum)
        {
            case 1:
                purigonPrefName = "NETBalanceType";
                purigonFaceName = "Face_Balance";
                break;
            case 2:
                purigonFaceName = "Face_Light";
                purigonPrefName = "NETLightType";
                break;
            case 3:
                purigonPrefName = "NETHealthyType";
                purigonFaceName = "Face_Health";
                break;
            case 4:
                purigonFaceName = "Face_Speed";
                purigonPrefName = "NETSpeedType";
                break;
            default:
                purigonFaceName = "Face_Balance";
                purigonPrefName = "NETBalanceType";
                break;
        }

        userPurigon = PhotonNetwork.Instantiate("MyPrefabs/Purigon_Network/" + purigonPrefName, purigonPosition[userPosition], Quaternion.identity, 0);
        userPurigon.name = ("userPurigon");
        userPurigon.transform.Rotate(new Vector3(0,90,0));
        purigonLocation = purigonPosition[userPosition];
        
        
        userPurigonUI = PhotonNetwork.Instantiate("MyPrefabs/UI/TopLeftUI", UILocation[userPosition], Quaternion.identity, 0);
        userPurigonUI.name = ("userPurigonUI");

        pv = userPurigonUI.GetComponent<PhotonView>();
        if(pv.isMine)
            pv.RPC("InstantiateTopLeftUI", PhotonTargets.All, UILocation[userPosition], purigonFaceName);
       
    }


 }
