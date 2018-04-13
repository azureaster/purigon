using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour {

    public GameObject countDownSprite;

	IEnumerator Start () {
        Time.timeScale = 0;
        float pauseTime = Time.realtimeSinceStartup + 3.5f;
        while (Time.realtimeSinceStartup < pauseTime)
            yield return null;
        countDownSprite.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
