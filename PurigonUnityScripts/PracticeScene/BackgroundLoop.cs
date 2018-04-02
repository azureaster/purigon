using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour {

    public float speed = 0.5f;
    private new Renderer renderer;


	// Use this for initialization
	public void Start () {
        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	public void Update () {
        Vector2 offset = new Vector2(Time.time * speed, 0);

        renderer.material.mainTextureOffset = offset;
	}
}
