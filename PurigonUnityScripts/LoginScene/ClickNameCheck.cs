using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class ClickNameCheck : MonoBehaviour
{
    public GameObject userName;
    public string inputCheckName;

    public bool validName = false;

    string NameCheckURL = "https://projectpurigon.000webhostapp.com/unityPhp/NameCheck.php";
    string returnString;

    public void OnClickNameCheckBtn()
    {
        StartCoroutine(CheckNameinDB(inputCheckName));
    }


    IEnumerator CheckNameinDB(string userName)
    {
        WWWForm form = new WWWForm();
        form.AddField("userNamePost", userName);

        WWW NamevalidCheckReturn = new WWW(NameCheckURL, form);
        yield return NamevalidCheckReturn;
        returnString = NamevalidCheckReturn.text;

        if (returnString == "VALID")
        {
            validName = true;
        }
        else validName = false;

        print("Name "+userName+" is "+returnString);
    }

    void Start()
    {

    }

    void Update()
    {
        inputCheckName = userName.GetComponent<InputField>().text;
    }
}
