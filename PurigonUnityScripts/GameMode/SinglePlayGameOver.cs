using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayGameOver : MonoBehaviour
{
    public int mapID = 1;

    public Text userFuenta;
    public Text thisGameRecord;


    public Text userPracticeBest;
    public Text userDailyBest;
    public Text userTotalBest;
    
    public Text totalDailyBest;
    public Text totalTotalBest;


    public string currentuserID;
    public string[] UserData;
   
    string GetRankingInfoURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetRankingInfo.php";
    string GetMyRankingURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetMyRanking.php";

    void Start()
    {
        currentuserID = PlayerPrefs.GetString("LoginID", "Default ID");

        StartCoroutine(GetDailyRankData());
        StartCoroutine(GetTotalRankData());
        StartCoroutine(FindMyRecord());
    }

    IEnumerator GetDailyRankData()
    {
        WWWForm form = new WWWForm();
        form.AddField("mapIDPost", mapID);
        form.AddField("bestRecordPost", "DAILYBEST");

        WWW getRankData = new WWW(GetRankingInfoURL, form);
        yield return getRankData;

        string userDataString = getRankData.text;

        UserData = userDataString.Split(';');

        totalDailyBest.text = GetDataValue(UserData[0], "DAILYBEST:") + "m";
    }

    IEnumerator GetTotalRankData()
    {
        WWWForm form = new WWWForm();
        form.AddField("mapIDPost", mapID);
        form.AddField("bestRecordPost", "TOTALBEST");

        WWW getRankData = new WWW(GetRankingInfoURL, form);
        yield return getRankData;

        string userDataString = getRankData.text;

        UserData = userDataString.Split(';');

        totalTotalBest.text = GetDataValue(UserData[0], "TOTALBEST:") + "m";
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    IEnumerator FindMyRecord()
    {
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", currentuserID);
        form.AddField("mapIDPost", mapID);

        WWW getRankData = new WWW(GetMyRankingURL, form);
        yield return getRankData;

        string userDataString = getRankData.text;
        Debug.Log(userDataString);

        UserData = userDataString.Split(';');

        userDailyBest.text = GetDataValue(UserData[0], "DAILYBEST:") + "m";
        userTotalBest.text = GetDataValue(UserData[0], "TOTALBEST:") + "m";
        userPracticeBest.text = GetDataValue(UserData[0], "PRACTICEBEST:") + "m";
          

    }







}
