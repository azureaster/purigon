using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NETGameOver : MonoBehaviour {

    public Text userDailyBest;
    public Text userTotalBest;

    public Text totalDailyBest;
    public Text totalTotalBest;

    public int chosenMapID;
    public string currentuserID;
    public string[] UserData;

    string GetRankingInfoURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetRankingInfo.php";
    string GetMyRankingURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetMyRanking.php";

    public Text[] NameTxt = new Text[6];
    public Text[] DistanceTxt = new Text[6];


	// Use this for initialization
	void Start () {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            Debug.Log(PhotonNetwork.playerList[i].NickName + " CurrentRanking: " + PhotonNetwork.playerList[i].CustomProperties["CurrentRanking"]);
            NameTxt[(int)PhotonNetwork.playerList[i].CustomProperties["CurrentRanking"]-1].text = PhotonNetwork.playerList[i].NickName;
            float flightDistance = (float)PhotonNetwork.playerList[i].CustomProperties["FlightDistance"];
            DistanceTxt[(int)PhotonNetwork.playerList[i].CustomProperties["CurrentRanking"] - 1].text = flightDistance.ToString("00") + "M"; ;
        }

        currentuserID = PlayerPrefs.GetString("LoginID", "Default ID");
        chosenMapID = PlayerPrefs.GetInt("CurrentMapNo", 00);

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
        if (userDataString.Contains("error"))
        {
            userDailyBest.text = "No Record Exists";
            userTotalBest.text = "No Record Exists";

        }
        else
        {
            UserData = userDataString.Split(';');

            userDailyBest.text = GetDataValue(UserData[0], "DAILYBEST:") + "m";
            userTotalBest.text = GetDataValue(UserData[0], "TOTALBEST:") + "m";
        }


    }

}
