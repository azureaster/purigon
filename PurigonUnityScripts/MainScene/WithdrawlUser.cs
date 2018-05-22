using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WithdrawlUser : MonoBehaviour {

    string userWithdrawlURL = "https://projectpurigon.000webhostapp.com/unityPhp/UserWithdrawl.php";
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
        }
        else Debug.Log("ID Incorrect");
    }

    IEnumerator TryWithdral()
    {
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", IDInput.text);
        form.AddField("userPWPost", PasswordInput.text);

        Debug.Log(IDInput.text + ", " + PasswordInput.text);

        WWW Withdrawl = new WWW(userWithdrawlURL, form);
        yield return Withdrawl;

        if(Withdrawl.text == "Record deleted successfully"){
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("LoginScene");
        }
        else Debug.Log(Withdrawl.text);
    }
}
