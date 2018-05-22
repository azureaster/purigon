using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetCharInfoFromDB : MonoBehaviour
{

    public Slider CharHP;
    public Slider CharBSPEED;
    public Slider CharMSPEED;
    public Slider CharACCEL;
    public Slider CharWEIGHT;
    public Text CharName;

    public int currentCharNum;
    public string currentuserID;
    public string[] userData;

    string GetUserCharInfoURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetCharInfoInitial.php";
    string GetCharInfoURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetCharInfo.php";
    string SaveCharInfoURL = "https://projectpurigon.000webhostapp.com/unityPhp/UpdateUserChar.php";


    //purigon prefab
    public GameObject[] purigonPrefabs;
    public GameObject[] purigons;
    public Transform PurigonModelPanel;
    public Vector3 purigonScale;
    public Vector3 purigonLocation;
    public Vector3 purigonRotation;

 
     private void Start()
    {
        currentCharNum = PlayerPrefs.GetInt("CharID", 1);
        ShowPurigonPrefab(currentCharNum);
        StartCoroutine(GetUserCharInfo());
    }


    IEnumerator GetUserCharInfo()
    {
        currentuserID = PlayerPrefs.GetString("LoginID", "Default ID");

        WWWForm form = new WWWForm();
        form.AddField("userIDPost", currentuserID);

        WWW getCharData = new WWW(GetUserCharInfoURL, form);
        yield return getCharData;

        string userDataString = getCharData.text;

        userData = userDataString.Split(';');
        

        CharName.text = "퓨룡 유형: " +  GetDataValue(userData[0], "CTYPE:");
        CharHP.value = int.Parse(GetDataValue(userData[0], "HP:"));
        CharBSPEED.value = int.Parse(GetDataValue(userData[0], "BSPEED:"));
        CharMSPEED.value = int.Parse(GetDataValue(userData[0], "MSPEED:"));
        CharACCEL.value = int.Parse(GetDataValue(userData[0], "ACCEL:"));
        CharWEIGHT.value = int.Parse(GetDataValue(userData[0], "WEIGHT:"));

    }


    IEnumerator GetCharInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("charIDPost", currentCharNum);

        WWW getCharData = new WWW(GetCharInfoURL, form);
        yield return getCharData;

        string currentCharData = getCharData.text;
        //print(userDataString);

        userData = currentCharData.Split(';');

        CharName.text = "퓨룡 유형: " + GetDataValue(userData[0], "CTYPE:");
        CharHP.value = int.Parse(GetDataValue(userData[0], "HP:"));
        CharBSPEED.value = int.Parse(GetDataValue(userData[0], "BSPEED:"));
        CharMSPEED.value = int.Parse(GetDataValue(userData[0], "MSPEED:"));
        CharACCEL.value = int.Parse(GetDataValue(userData[0], "ACCEL:"));
        CharWEIGHT.value = int.Parse(GetDataValue(userData[0], "WEIGHT:"));

    }

    public void OnClickNextCharClicked()
    {
        if (currentCharNum < 4)
            currentCharNum += 1;
        else currentCharNum = 1;

        ShowPurigonPrefab(currentCharNum);
        StartCoroutine(GetCharInfo());
    }

    public void OnClickPrevCharClicked()
    {
        if (currentCharNum > 1)
            currentCharNum -= 1;
        else currentCharNum = 4;
        
        ShowPurigonPrefab(currentCharNum);
        StartCoroutine(GetCharInfo());
    }

    public void OnClickSaveClicked() {
        StartCoroutine(SaveChange());
    }

    IEnumerator SaveChange() {
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", currentuserID);
        form.AddField("charIDPost", currentCharNum);
        form.AddField("skillIDPost", currentCharNum);

        WWW getuserData = new WWW(SaveCharInfoURL, form);
        yield return getuserData;

        Debug.Log("UserChar Updated: " + getuserData.text);
        PlayerPrefs.SetInt("CharID", currentCharNum);

        PhotonNetwork.isMessageQueueRunning = false;
        SceneManager.LoadScene("MainScene");
    }


    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    void ShowPurigonPrefab(int currentCharNum) {

        foreach (Transform child in PurigonModelPanel){
            Destroy(child.gameObject);
        }

        purigons = new GameObject[1];
        purigons[0] = Instantiate(purigonPrefabs[currentCharNum-1]) as GameObject;

        purigons[0].transform.Rotate(purigonRotation);
        purigons[0].transform.localScale = purigonScale;
        purigons[0].transform.localPosition = purigonLocation;
        purigons[0].transform.SetParent(PurigonModelPanel);
        
    }


}
