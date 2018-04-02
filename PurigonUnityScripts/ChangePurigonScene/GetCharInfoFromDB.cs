using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetCharInfoFromDB : MonoBehaviour
{

    public Slider CharHP;
    public Slider CharBSPEED;
    public Slider CharMSPEED;
    public Slider CharACCEL;
    public Slider CharWEIGHT;
    public Text CharName;

    public int currentCharNum;
    public string currentuserID;
    public string[] userData;

    string GetUserCharInfoURL = "http://127.0.0.1/Purigon/GetCharinfoInitial.php";
    string GetCharInfoURL = "http://127.0.0.1/Purigon/GetCharinfo.php";
    string SaveCharInfoURL = "http://127.0.0.1/Purigon/UpdateUserChar.php";

    private void Start()
    {
        currentCharNum = PlayerPrefs.GetInt("CharID", 1);
        StartCoroutine(GetUserCharInfo());
    }


    IEnumerator GetUserCharInfo()
    {
        currentuserID = PlayerPrefs.GetString("LoginID", "Default ID");

        WWWForm form = new WWWForm();
        form.AddField("userIDPost", currentuserID);

        WWW getCharData = new WWW(GetUserCharInfoURL, form);
        yield return getCharData;

        string userDataString = getCharData.text;
        print(currentuserID + "   " + userDataString);

        userData = userDataString.Split(';');
        

        CharName.text = "퓨룡 유형: " +  GetDataValue(userData[0], "CTYPE:");
        CharHP.value = int.Parse(GetDataValue(userData[0], "HP:"));
        CharBSPEED.value = int.Parse(GetDataValue(userData[0], "BSPEED:"));
        CharMSPEED.value = int.Parse(GetDataValue(userData[0], "MSPEED:"));
        CharACCEL.value = int.Parse(GetDataValue(userData[0], "ACCEL:"));
        CharWEIGHT.value = int.Parse(GetDataValue(userData[0], "WEIGHT:"));

    }


    IEnumerator GetCharInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("charIDPost", currentCharNum);

        WWW getCharData = new WWW(GetCharInfoURL, form);
        yield return getCharData;

        string currentCharData = getCharData.text;
        //print(userDataString);

        userData = currentCharData.Split(';');

        CharName.text = "퓨룡 유형: " + GetDataValue(userData[0], "CTYPE:");
        CharHP.value = int.Parse(GetDataValue(userData[0], "HP:"));
        CharBSPEED.value = int.Parse(GetDataValue(userData[0], "BSPEED:"));
        CharMSPEED.value = int.Parse(GetDataValue(userData[0], "MSPEED:"));
        CharACCEL.value = int.Parse(GetDataValue(userData[0], "ACCEL:"));
        CharWEIGHT.value = int.Parse(GetDataValue(userData[0], "WEIGHT:"));

    }

    public void OnClickNextCharClicked()
    {
        if (currentCharNum < 4)
            currentCharNum += 1;
        else currentCharNum = 1;
        StartCoroutine(GetCharInfo());
    }

    public void OnClickPrevCharClicked()
    {
        if (currentCharNum > 1)
            currentCharNum -= 1;
        else currentCharNum = 4;
        StartCoroutine(GetCharInfo());
    }

    public void OnClickSaveClicked() {
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", currentuserID);
        form.AddField("charIDPost", currentCharNum);
        form.AddField("skillIDPost", currentCharNum);

        WWW getuserData = new WWW(SaveCharInfoURL, form);

        Debug.Log("UserChar Updated");

        PlayerPrefs.SetInt("CharID", currentCharNum);
        SceneManager.LoadScene("MainScene");
    }




    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }




}
