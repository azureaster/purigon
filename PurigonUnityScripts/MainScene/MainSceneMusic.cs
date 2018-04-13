using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneMusic : MonoBehaviour {

    static bool AudioBegin = false;
    public AudioClip mainMusic;
    private AudioSource mainMusicSource;

    void Awake()
    {
        mainMusicSource = GetComponent<AudioSource>();
        mainMusicSource.clip = mainMusic;

        if (!AudioBegin)
        {
            mainMusicSource.Play();
            DontDestroyOnLoad(gameObject);
            AudioBegin = true;
        }
    }
    void Update()
    {
        if ((SceneManager.GetActiveScene().name).Contains("Single0") || (SceneManager.GetActiveScene().name).Contains("Multi0"))
        {
            mainMusicSource.Stop();
            AudioBegin = false;
        }
    }
}
