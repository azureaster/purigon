using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour {

    public static Vector3 pos;

    void OnCollisionEnter(Collision coll)
    {
        pos = coll.transform.localPosition;
        
        //Debug.Log(pos);
    }
}
