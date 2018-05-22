using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdatePracticeRecord : MonoBehaviour{

    public int mapID;
    public float thisGameRecord;
    private int newPracticeRecord;

    public string currentuserID;
    public string[] UserData;

    string UpdatePracticeRecordURL = "https://projectpurigon.000webhostapp.com/unityPhp/UpdatePracticeRecord.php";

    public IEnumerator UpdatePracticeRec()
    {
        mapID = PlayerPrefs.GetInt("CurrentMapNo", 00);
        currentuserID = PlayerPrefs.GetString("LoginID", "Default ID");
        thisGameRecord = PlayerPrefs.GetFloat("PracticeRecord", 0);

        WWWForm form = new WWWForm();
        form.AddField("userIDPost", currentuserID);
        form.AddField("mapIDPost", mapID);
        form.AddField("practiceRecordPost", thisGameRecord.ToString("0.00"));

        //Debug.Log(currentuserID + ", " + mapID + ", " + thisGameRecord);

        WWW updateRecordData = new WWW(UpdatePracticeRecordURL, form);
        yield return updateRecordData;

        string updateResult = updateRecordData.text;

        Debug.Log("updateResult: " + updateRecordData.text);

        if (updateResult.Contains("NEW PRACTICE RECORD"))
            PlayerPrefs.SetInt("newPracticeRecord", 1);
        else PlayerPrefs.SetInt("newPracticeRecord", 0);


        SceneManager.LoadScene("GameOverScene_SinglePlayMode");
    }



    }





