using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRankingScoreBoardScene : MonoBehaviour
{

    public Text currentUserDailyBest;
    public Text currentUserTotalBest;
    public Text currentUserPracticeBest;
    public Text currentUserRank;
    public Text currentUserRankName;
    public Text currentUserRankRecord;

    public string currentuserID;
    public int chosenMapID;
    public string whichBest = "DAILYBEST";

    public string[] UserData;

    //USERINFO DB의 NAME, CHARNUM, SKILLNUM 를 ID로 USERRECORD DB와 Join되어있음 명령어 참고: http://kwonsaw.tistory.com/142
    string GetRankingInfoURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetRankingInfo.php";


    public GameObject ItemObject;
    public Transform Content;


    void Start()
    {/*
        Sprite[] faceImage = {
        Resources.Load("Image/Face_Balance") as Sprite,
        Resources.Load("Image/Face_Light") as Sprite,
        Resources.Load("Image/Face_Health") as Sprite,
        Resources.Load("Image/Face_Speed") as Sprite
    };*/

        currentuserID = PlayerPrefs.GetString("LoginID", "Default ID");
        StartCoroutine(GetRankData());
    }

    public void MapChosenChanged()
    {
        StartCoroutine(GetRankData());
    }

    public void DailyBtnClicked() //https://answers.unity.com/questions/314926/make-a-button-behave-like-a-toggle.html
    {
        whichBest = "DAILYBEST";
        StartCoroutine(GetRankData());
    }

    public void TotalBtnClicked()
    {
        whichBest = "TOTALBEST";
        StartCoroutine(GetRankData());
    }


    IEnumerator GetRankData()
    {
        chosenMapID = GameObject.Find("Dropdown").GetComponent<Dropdown>().value + 1;

        WWWForm form = new WWWForm();
        form.AddField("mapIDPost", chosenMapID);
        form.AddField("bestRecordPost", whichBest);

        WWW getRankData = new WWW(GetRankingInfoURL, form);
        yield return getRankData;

        string userDataString = getRankData.text;
        Debug.Log(userDataString);

        UserData = userDataString.Split(';');

        MakeBtninScroll(UserData);

    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    private void MakeBtninScroll(string[] UserData)
    {

        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        GameObject itemTemp;
        RankingBtnItem tempButton;

        for (int i = 0; i < UserData.Length - 1; i++)
        {

            itemTemp = (GameObject)Instantiate(ItemObject);
            itemTemp.transform.SetParent(Content, false);

            tempButton = itemTemp.GetComponent<RankingBtnItem>();

            if (currentuserID == GetDataValue(UserData[i], "UID:"))
            {
                currentUserDailyBest.text = "My Daily Best: " + GetDataValue(UserData[i], "DAILYBEST:") + "m";
                currentUserTotalBest.text = "My Total Best: " + GetDataValue(UserData[i], "TOTALBEST:") + "m";
                currentUserPracticeBest.text = "My Practice Best: " + GetDataValue(UserData[i], "PRACTICEBEST:") + "m";
                currentUserRankName.text = GetDataValue(UserData[i], "NAME:");
                currentUserRank.text = (i+1) + "위";

                if (whichBest == "TOTALBEST")
                    currentUserRankRecord.text = GetDataValue(UserData[i], "TOTALBEST:") + "m";
                
                else
                    currentUserRankRecord.text = GetDataValue(UserData[i], "DAILYBEST:") + "m";
                

            }

            tempButton.UserName.text = GetDataValue(UserData[i], "NAME:");
            if (whichBest == "TOTALBEST")
            {
                tempButton.UserRecord.text = GetDataValue(UserData[i], "TOTALBEST:") + "m";
            }
            else
            {//if (whichBest == "DAILYBEST") {
                tempButton.UserRecord.text = GetDataValue(UserData[i], "DAILYBEST:") + "m";
            }
            //tempButton.UserPurigon = faceImage[(int.Parse(GetDataValue(UserData[i], "CHARNUM:"))) - 1];
            tempButton.Item.onClick.AddListener(() => ItemClick_Result(int.Parse(GetDataValue(UserData[i], "UID:"))));

        }

    }

    public void ItemClick_Result(int i)
    {
        Debug.Log("아이템 클릭: " + i + " - 해당 유저 정보 불러오기");
    }



}
