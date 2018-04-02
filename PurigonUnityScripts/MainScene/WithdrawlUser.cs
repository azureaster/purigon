using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WithdrawlUser : MonoBehaviour {

    string userWithdrawlURL = "http://127.0.0.1/Purigon/userWithdrawl.php";
    public string currentuserID;

    public Text IDInput;
    public Text PasswordInput;


    private void Start()
    {
        currentuserID = PlayerPrefs.GetString("LoginID", "Default ID");
    }


    public void OnClickWithdrawl() {
        if (currentuserID == IDInput.text) {
            StartCoroutine(TryWithdral());
            SceneManager.LoadScene("LoginScene");
        }
        else Debug.Log("ID Incorrect");
    }

    IEnumerator TryWithdral()
    {
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", IDInput.text);
        form.AddField("userPWPost", PasswordInput.text);

        WWW Withdrawl = new WWW(userWithdrawlURL, form);
        yield return Withdrawl;

        Debug.Log(Withdrawl.text);
        
    }
}
