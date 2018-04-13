using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetUserInfo : MonoBehaviour {

    public Text userName;
    public Text userLV;
    public Slider userEXP;
    public int userCHAR;
    public int userSKILL;

    string CurrentuserID;
    public string[] UserData;

    string GetUserInfoURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetUserInfo.php";

    IEnumerator Start()
    {
        CurrentuserID = PlayerPrefs.GetString("LoginID", "Default ID");

        WWWForm form = new WWWForm();
        form.AddField("userIDPost", CurrentuserID);

        WWW getuserData = new WWW(GetUserInfoURL, form);
        yield return getuserData;

        string userDataString = getuserData.text;
        //print("CurrentUser: " + userDataString);

        UserData = userDataString.Split(';');
        userName.text = GetDataValue(UserData[0], "NAME:");
        userLV.text = GetDataValue(UserData[0], "LV:");
        userEXP.value = int.Parse(GetDataValue(UserData[0], "EXP:"));
        userCHAR = int.Parse(GetDataValue(UserData[0], "CHARNUM:"));
        userSKILL = int.Parse(GetDataValue(UserData[0], "SKILLNUM:"));
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

}
