using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour{
    public GameObject userid;
    public GameObject username;
    public GameObject email;
    public GameObject password;
    public GameObject confpassword;
    public GameObject JoinPanel;

    public string inputUserID;
    public string inputUserName;
    public string inputUserEmail;
    public string inputUserPW;
    private string ConfPassword;

    string CreatUserURL = "https://projectpurigon.000webhostapp.com/unityPhp/UserRegister.php";
    
    private ClickIDCheck IDCheckBtn;
    private ClickNameCheck NameCheckBtn;


    void Awake()
    {
        IDCheckBtn = GameObject.Find("OnClickIDCheck").GetComponent<ClickIDCheck>();
        NameCheckBtn = GameObject.Find("OnClickNameCheck").GetComponent<ClickNameCheck>();
    }
     
    void Start()
    {

    }

    public void OnClickRegisterBtn()
    {
        RegisterButton(inputUserID, inputUserName, inputUserEmail, inputUserPW, ConfPassword);
    }

    public void RegisterButton(string userID, string userName, string Uemail, string Upassword, string Uconfpassword){
        if (IDCheckBtn.validID == true && NameCheckBtn.validName == true) {
            if (Upassword.Length < 20 && Upassword == Uconfpassword) {
                if (Uemail.Contains("@") && Uemail.Contains(".")) {
                    StartCoroutine(CreatUser(inputUserID, inputUserPW, inputUserEmail, inputUserName));

                    
                    if (JoinPanel.gameObject.activeSelf == true)
                    {
                        userid.GetComponent<InputField>().text = "";
                        username.GetComponent<InputField>().text = "";
                        email.GetComponent<InputField>().text = "";
                        password.GetComponent<InputField>().text = "";
                        confpassword.GetComponent<InputField>().text = "";

                        JoinPanel.gameObject.SetActive(false);
                    }


                }
                else Debug.Log("Invalid Email");
            }
            else Debug.Log("Invalid Password");
        }
        else Debug.Log("Invalid ID or NickName");
    }


    void Update()
    {
        inputUserID = userid.GetComponent<InputField>().text;
        inputUserName = username.GetComponent<InputField>().text;
        inputUserEmail = email.GetComponent<InputField>().text;
        inputUserPW = password.GetComponent<InputField>().text;
        ConfPassword = confpassword.GetComponent<InputField>().text;
    }

    IEnumerator CreatUser(string userID, string userPW, string userEmail, string userName)
    {
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", userID);
        form.AddField("userPWPost", userPW);
        form.AddField("userEmailPost", userEmail);
        form.AddField("userNamePost", userName);

        PlayerPrefs.SetInt("CharID", 1);

        WWW Registeration = new WWW(CreatUserURL, form);
        yield return Registeration;
        Debug.Log(Registeration.text);
    }
}
