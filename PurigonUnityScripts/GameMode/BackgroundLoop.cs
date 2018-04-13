using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour {

    private new Renderer renderer;
    public float speed;
    public float currentspeed;


	// Use this for initialization
	public void Start () {
        renderer = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	public void Update () {
        SpeedCtrl();
        Vector2 offset = new Vector2(Time.time * speed, 0);

        renderer.material.mainTextureOffset = offset;
	}

    void SpeedCtrl() {
        currentspeed = GameObject.Find("userPurigon").GetComponent<PurigonCtrl>().currentSpeed;
        //speed는 퓨룡 속도에 맞춰서 변화해야한다(퓨룡은 부딪혀서 멈춰있는데 배경은 계속 움직이면 안되니까. 
        //가감속 기능 추가 후 수정해야함.

        if (gameObject.tag == "Background_first") speed = 0.14f;
        if (gameObject.tag == "Background_second") speed = 0.10f;
        if (gameObject.tag == "Background_third") speed = 0.05f;
        if (gameObject.tag == "Background_fourth") speed = 0.03f;


    }



}
