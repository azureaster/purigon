using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetPurigonPosition : Photon.MonoBehaviour
{

    public GameObject positionBar;
    public GameObject positionArrow;
    static Vector3 prevUserPosition;
    Vector3 nowUserPosition;

    private PhotonView pv = null;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {

        prevUserPosition = this.transform.position;
        positionArrow = PhotonNetwork.Instantiate("MyPrefabs/UI/PositionArrow", positionBar.transform.position, positionArrow.transform.rotation, 0);
        positionArrow.transform.parent = GameObject.Find("Position").transform;

    }

    void Update()
    {
        if (!pv.isMine)
            return;
        nowUserPosition = this.transform.position;
        pv.RPC("MoveArrow", PhotonTargets.All, nowUserPosition, prevUserPosition);
        prevUserPosition = nowUserPosition;
    }

    [PunRPC]
    public void MoveArrow(Vector3 now, Vector3 prev)
    {
        positionArrow.transform.position = new Vector3(positionArrow.transform.position.x + (now.x - prev.x) * 1.5f, positionArrow.transform.position.y, positionArrow.transform.position.z);
    }
}