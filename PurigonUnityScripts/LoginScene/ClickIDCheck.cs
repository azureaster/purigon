using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class ClickIDCheck : MonoBehaviour {
    public GameObject userID;
    public string inputCheckID;

    public bool validID = false;

    string IDCheckURL = "https://projectpurigon.000webhostapp.com/unityPhp/IDCheck.php";
    string returnString;

    public void OnClickIDCheckBtn()
    {
        StartCoroutine(CheckIDinDB(inputCheckID));
    }



    IEnumerator CheckIDinDB(string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", userID);
        
        WWW IDvalidCheckReturn = new WWW(IDCheckURL, form);
        yield return IDvalidCheckReturn;
        returnString = IDvalidCheckReturn.text;

        if (returnString == "VALID")
        {
            validID = true;
        }
        else validID = false;

        print("ID "+userID+" is " + returnString);
    }

    void Start() {

    }

	void Update () {
        inputCheckID = userID.GetComponent<InputField>().text;
    }
}
