using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotationScript : MonoBehaviour
{
    public Vector3 localRotationVelocityEuler = Vector3.zero;

	void Update()
    {
        transform.localRotation *= Quaternion.Euler(localRotationVelocityEuler * Time.deltaTime);
	}
}
