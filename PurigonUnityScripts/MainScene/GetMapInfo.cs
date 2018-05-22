using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMapInfo : MonoBehaviour {

    public Text currentMapName;
    public int chosenMapID;

    string GetMapInfoURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetMapInfo.php";

    void Start () {
        StartCoroutine(GetMapName());
	}

    public void GetMapNameFromServer() {
        StartCoroutine(GetMapName());
    }

    
    IEnumerator GetMapName () {
        chosenMapID = GameObject.Find("Dropdown").GetComponent<Dropdown>().value + 1;
        WWWForm form = new WWWForm();
        form.AddField("MapIDPost", chosenMapID);

        WWW getMapData = new WWW(GetMapInfoURL, form);
        yield return getMapData;

        currentMapName.text =  getMapData.text;
    }

    


	
	
}
