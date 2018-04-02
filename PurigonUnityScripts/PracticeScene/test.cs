using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    public string inputUserID;
    public string inputUserPW;
    public string inputUserEmail;
    public string inputUserName;


    string CreatUserURL = "http://127.0.0.1/Purigon/test.php";

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) CreatUser(inputUserID, inputUserPW, inputUserEmail, inputUserName);
    }

    IEnumerator CreatUser(string userID, string userPW, string userEmail, string userName)
    {
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", userID);
        form.AddField("userPWPost", userPW);
        form.AddField("userEmailPost", userEmail);
        form.AddField("userNamePost", userName);

        WWW www = new WWW(CreatUserURL, form);
        yield return www;
        Debug.Log(www.text);
    }


}
