using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NETFollowPurigon : MonoBehaviour {

    public Transform target;

    void Update () {
        transform.position = new Vector3(target.position.x + 2.5f, transform.position.y, transform.position.z);
	}
}
