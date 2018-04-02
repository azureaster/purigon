using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfoLoader : MonoBehaviour {

    public string[] items;

	IEnumerator Start () {
        WWW charData = new WWW("http://127.0.0.1/Purigon/CharData.php");
        yield return charData;
        string charDataString = charData.text;
        print(charDataString);
        items = charDataString.Split(';');
        print(GetDataValue(items[0], "CTYPE:"));
	}

    string GetDataValue(string data, string index) {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

}
