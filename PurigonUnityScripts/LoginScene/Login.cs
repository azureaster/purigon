using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
	public GameObject loginCanvas;
	
    public GameObject userID;
    public GameObject password;

    public string inputUserID;
    public string inputUserPW;

    string LoginURL = "https://projectpurigon.000webhostapp.com/unityPhp/UserLogin.php";

	public bool IsLoadingFin = false;
	
    public void OnClickLogInBtn()
    {
        loginCanvas.SetActive(true);
        StartCoroutine(LoginToDB(inputUserID, inputUserPW));
        IsLoadingFin = false;
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
            IsLoadingFin = true;
            PlayerPrefs.SetString("LoginID", userID);
            SceneManager.LoadScene("MainScene");
        }

        else if(www.text=="user not found")
        {
            IsLoadingFin = true;
            loginCanvas.SetActive(false);
        }


    }
}
