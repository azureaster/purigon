using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public GameObject userID;
    public GameObject password;

    public string inputUserID;
    public string inputUserPW;

    string LoginURL = "http://127.0.0.1/Purigon/UserLogin.php";

    // Use this for initialization
    void Start()
    {

    }

    public void OnClickLogInBtn()
    {
        StartCoroutine(LoginToDB(inputUserID, inputUserPW));
    }

    // Update is called once per frame
    void Update()
    {
        inputUserID = userID.GetComponent<InputField>().text;
        inputUserPW = password.GetComponent<InputField>().text;
    }

    IEnumerator LoginToDB(string userID, string userPW)
    {
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", userID);
        form.AddField("userPWPost", userPW);

        WWW www = new WWW(LoginURL, form);

        yield return www;

        
        Debug.Log(www.text);

        if (www.text == "login success") {
            PlayerPrefs.SetString("LoginID", userID);

            ///마지막 로그인 기록 읽어오고 서버시간과 비교해서 하루 지났으면 해당 유저의 DAILYRECORD 지워지게 해야함

            SceneManager.LoadScene("MainScene");
        }


    }
}
