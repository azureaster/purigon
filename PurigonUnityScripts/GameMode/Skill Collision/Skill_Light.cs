using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Light : Photon.MonoBehaviour
{
    GameObject coll_Player;
    int num;
    public bool damaged = false;
    private PhotonView pv = null;

    void Awake()
    {
        pv = this.GetComponent<PhotonView>();
    }

    [PunRPC]
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "BalanceType" || coll.gameObject.tag == "LightType" || coll.gameObject.tag == "PhysicalType" || coll.gameObject.tag == "SpeedType")
            Debug.Log(coll.gameObject.tag);

        //     if (coll.gameObject == GameObject.Find("userPurigon").GetComponent<NETInstantiateUserPurigon>().userPurigon)
        //       return;

        if (this.gameObject.tag == "Skill1")
        {//닿은 상대에게 초당 15의 데미지
            if (coll.gameObject.layer == LayerMask.NameToLayer("Purigon"))
            {
                if (coll.GetComponent<NETPurigonCtrl>().IsDamaged_L1 == true)
                    return;
                Debug.Log("start");
                coll_Player = coll.gameObject;
                coll.GetComponent<NETPurigonCtrl>().IsDamaged_L1 = true;
                num = 0;
                InvokeRepeating("Skill1_Damage", 0, 1f);
            }
        }

        if (this.gameObject.tag == "Skill3")
        {// 반경 1m이내의 모든 플레이어 0.5m 밀쳐냄

            if (coll.gameObject.layer == LayerMask.NameToLayer("Purigon"))
            {
                coll_Player = coll.gameObject;
                coll_Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                coll.gameObject.GetComponent<NETPurigonCtrl>().IsLightTypeSkill3 = true;
                // StartCoroutine(MoveLeftFunction());
                // pv.RPC("MoveLeftFunction", PhotonTargets.All, null);
                MoveLeftFunction();

            }
        }
    }

    [PunRPC]
    void Skill1_Damage()
    {
        num += 1;
        if (num < 3)
        {
           // Debug.Log("damage***" + num);
            coll_Player.gameObject.GetComponent<NETPurigonCtrl>().CharHP -= 15;
            if (coll_Player.gameObject.GetComponent<NETPurigonCtrl>().CharHP < 0)
                coll_Player.gameObject.GetComponent<NETPurigonCtrl>().CharHP = 0;
            InvokeRepeating("Skill1_DamageEffect", 0.1f, 0.2f);
        }
        else
        {
            damaged = true;
            CancelInvoke("Skill1_Damage");
            //Debug.Log("damage off");
        }
    }

    void Skill1_DamageEffect()
    {
        Debug.Log("Effect");
        if (coll_Player.GetComponentInChildren<SkinnedMeshRenderer>().material.GetColor("_Color") == Color.white)
            coll_Player.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", Color.red);
        else if (coll_Player.GetComponentInChildren<SkinnedMeshRenderer>().material.GetColor("_Color") == Color.red)
            coll_Player.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);

        if (damaged == true)
        {
            //Debug.Log("End!!!!!");
            Invoke("DamageMode_Off", 0.1f);
            CancelInvoke("Skill1_DamageEffect");
        }
    }

    void DamageMode_Off()
    {
        Debug.Log("Damage_Mode_OFF");
        coll_Player.GetComponent<NETPurigonCtrl>().IsDamaged_L1 = false;
        damaged = false;
    }

    void MoveLeftFunction()
    {
        float startTimer = 0.0f;
        while (startTimer < 2.0f)
        {

            coll_Player.GetComponent<NETPurigonCtrl>().currentSpeed -= 0.05f;
            coll_Player.GetComponent<NETPurigonCtrl>().PurigonTr.position += Vector3.left * Time.deltaTime * 1f;

            startTimer += Time.deltaTime;
        }
        coll_Player.GetComponent<NETPurigonCtrl>().IsLightTypeSkill3 = false;
        coll_Player.GetComponent<NETPurigonCtrl>().currentSpeed = coll_Player.GetComponent<NETPurigonCtrl>().basicSpeed;
        coll_Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    }
}
