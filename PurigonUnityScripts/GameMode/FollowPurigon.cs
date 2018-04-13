using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPurigon : MonoBehaviour {

    public Transform target;
    public GameObject userPurigon;

    private void Start()
    {
        userPurigon = GameObject.Find("UserPurigonController").GetComponent<InstantiateUserPurigon>().userPurigon;
        target = userPurigon.transform;
    }
    void LateUpdate () {

        transform.position = new Vector3(target.position.x + 2.5f, transform.position.y, transform.position.z);
		
	}
}
