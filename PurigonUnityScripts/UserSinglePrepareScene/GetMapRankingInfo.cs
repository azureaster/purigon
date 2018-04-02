using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMapRankingInfo : MonoBehaviour {
   
    public Text userPracticeBest;
    public Text userDailyBest;
    public Text userTotalBest;

    public Text totalDailyBest;
    public Text totalTotalBest;

    public int chosenMapID;
    public string currentuserID;
    public string[] UserData;

    string GetRankingInfoURL = "http://127.0.0.1/Purigon/GetRankingInfo.php";
    string GetMyRankingURL = "http://127.0.0.1/Purigon/GetMyRanking.php";

    void Start()
    {
        currentuserID = PlayerPrefs.GetString("LoginID", "Default ID");
        chosenMapID = GameObject.Find("Dropdown").GetComponent<Dropdown>().value + 1;

        StartCoroutine(GetDailyRankData());
        StartCoroutine(GetTotalRankData());
        StartCoroutine(FindMyRecord());
    }

    public void MapChosenChanged()
    {
        chosenMapID = GameObject.Find("Dropdown").GetComponent<Dropdown>().value + 1;
        StartCoroutine(GetDailyRankData());
        StartCoroutine(GetTotalRankData());
        StartCoroutine(FindMyRecord());
    }


    IEnumerator GetDailyRankData()
    {
        WWWForm form = new WWWForm();
        form.AddField("mapIDPost", chosenMapID);
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
        form.AddField("mapIDPost", chosenMapID);
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
        form.AddField("mapIDPost", chosenMapID);

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
