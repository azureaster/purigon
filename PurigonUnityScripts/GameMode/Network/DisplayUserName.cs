using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUserName : MonoBehaviour {

    public Text userName;
    private PhotonView pv = null;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        userName.text = pv.owner.NickName;
    }


}
