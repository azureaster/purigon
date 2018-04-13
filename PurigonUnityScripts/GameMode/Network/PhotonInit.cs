using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInit : MonoBehaviour {

    public string version = "v1.0";

	void Awake () {
        PhotonNetwork.ConnectUsingSettings(version);
	}

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());    
    }
}
