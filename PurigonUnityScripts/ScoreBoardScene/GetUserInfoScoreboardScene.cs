using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetUserInfoScoreboardScene : MonoBehaviour
{

    public Text userName;
    public Text userLV;
    public Slider userEXP;
    public Text userCHAR;
    public Text userSKILL;

    string CurrentuserID;
    public string[] UserData;

    string GetUserInfoURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetUserInfo.php";
    private string[] UserType = {"BALANCE","LIGHT","HEALTH","SPEED"};


    IEnumerator Start()
    {
        CurrentuserID = PlayerPrefs.GetString("LoginID", "Default ID");

        WWWForm form = new WWWForm();
        form.AddField("userIDPost", CurrentuserID);

        WWW getuserData = new WWW(GetUserInfoURL, form);
        yield return getuserData;

        string userDataString = getuserData.text;
        //print(userDataString);

        UserData = userDataString.Split(';');
        userName.text = GetDataValue(UserData[0], "NAME:");
        userLV.text = "LV." + GetDataValue(UserData[0], "LV:");
        userEXP.value = int.Parse(GetDataValue(UserData[0], "EXP:"));
        userCHAR.text = "퓨룡 유형: " + UserType[int.Parse(GetDataValue(UserData[0], "CHARNUM:"))-1];
        userSKILL.text = "스킬 유형: " + UserType[int.Parse(GetDataValue(UserData[0], "SKILLNUM:"))-1];
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

}
