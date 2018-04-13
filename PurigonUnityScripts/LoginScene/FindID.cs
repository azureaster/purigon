using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindID : MonoBehaviour {

    public Text foundOutput;
    public Toggle IDToggle;

    public GameObject inputUserEmail;
    public GameObject inputUserID;

    public GameObject FindIdPanel;
    public GameObject ShowIdPanel;
    

    public string userInputEmail;
    public string userInputID;
    public string[] UserData;

    string FindIdPwURL = "https://projectpurigon.000webhostapp.com/unityPhp/UserFindIdPw.php";

    public void Update()
    {
        
    }


    public void OnClickFindBtn()
    {
        userInputEmail = inputUserEmail.GetComponent<InputField>().text;
        userInputID = inputUserID.GetComponent<InputField>().text;
        StartCoroutine(FindIDPW());
        CancleFindIdPanel();
    }


    IEnumerator FindIDPW()
    {
        WWWForm form = new WWWForm();
        form.AddField("userEmailPost", userInputEmail);

        WWW getuserData = new WWW(FindIdPwURL, form);
        yield return getuserData;

        string userDataString = getuserData.text;
        print(userDataString);

        if (userDataString == "INVALID")
        {
            foundOutput.text = "Invaild email";
        }
        else {
            UserData = userDataString.Split(';');

            if (IDToggle.isOn){
                foundOutput.text = GetDataValue(UserData[0], "ID:");
            }
            else {
                if (userInputID == GetDataValue(UserData[0], "ID:")) {
                    foundOutput.text = GetDataValue(UserData[0], "PW:");
                }
                else foundOutput.text = "Invaild ID";

            }
        }
        
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    void CancleFindIdPanel()
    {
        if (FindIdPanel.gameObject.activeSelf == true)
        {
            FindIdPanel.gameObject.SetActive(false);
        }

        if (ShowIdPanel.gameObject.activeSelf == false)
        {
            ShowIdPanel.gameObject.SetActive(true);
        }
    }

}
