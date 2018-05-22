using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting_Btn_Position : MonoBehaviour {
    public Button upButton;
    public Button speedButton;
    public Button skillButton;

    public int changed_btnPosition;
    // Use this for initialization
    void Start () {
        changed_btnPosition = PlayerPrefs.GetInt("btnPosition");

        if (changed_btnPosition==1)        // 버튼 좌우 반전
        {
            speedButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
            speedButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            speedButton.GetComponent<RectTransform>().anchoredPosition =new Vector2(-80,15);

            upButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            upButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            upButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(80, 15);

            skillButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
            skillButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
            skillButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 15);
        }
        else
        {
            speedButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            speedButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            speedButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(80, 15);

            upButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
            upButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            upButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80, 15);

            skillButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
            skillButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
            skillButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 15);
        }
    }
}
