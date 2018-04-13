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

    string LoginURL = "https://projectpurigon.000webhostapp.com/unityPhp/UserLogin.php";

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
            SceneManager.LoadScene("MainScene");
        }


    }
}
