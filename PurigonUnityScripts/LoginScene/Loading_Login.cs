using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading_Login : MonoBehaviour
{
    public float endTimer;
    public Image loadBar;
    public GameObject loadingCanvas;
    public float loadValue = 0;

    public void OnclickLoginButton()
    {
        loadingCanvas.SetActive(true);
        endTimer = 0;
    }

    void Update()
    {
        if (endTimer >= 5f)
        {
            loadingCanvas.SetActive(false);
            Debug.Log("Login Error");
        }
        if (GameObject.Find("OnClickLogin").GetComponent<Login>().IsLoadingFin == true)
            loadingCanvas.SetActive(false);
        loadValue += Time.deltaTime;
        endTimer += Time.deltaTime;
        if (loadValue > 1)
            loadValue = 0;
        Loading(loadValue);
    }

    void Loading(float loadValue)
    {
        loadBar.fillAmount = loadValue;

    }


}