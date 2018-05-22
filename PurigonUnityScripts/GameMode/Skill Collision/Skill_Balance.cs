using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Balance : MonoBehaviour 
{
    int num;

    private PhotonView pv = null;

    void Awake()
    {
        pv = this.GetComponent<PhotonView>();
    }


    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
    }
    void OnCollisionEnter(Collision coll)
    {
     
        if (this.tag == "Skill1")
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Purigon"))
            {
                coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP -= 30;
                if (coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP < 0)
                    coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP = 0;
                //데미지 30
            }
            Destroy(coll.gameObject.GetComponent<NETPurigonCtrl>().frostBallClone);
           
        }

        else if (this.gameObject.tag == "Skill3"){

            if (coll.gameObject == GameObject.Find("userPurigon").GetComponent<NETPurigonCtrl>().speed_purigon)
                return;
            if (coll.gameObject.layer == LayerMask.NameToLayer("Purigon"))
            {
                coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP -= 60;
                if (coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP < 0)
                    coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP = 0;
                //데미지 60
            }
        }
   }  
    void OnTriggerEnter(Collider coll)
    { 
        if (this.gameObject.tag == "Skill4")
        {
            if (coll.gameObject == GameObject.Find("userPurigon").GetComponent<NETPurigonCtrl>().speed_purigon)
                return;
            if (coll.gameObject.layer == LayerMask.NameToLayer("Purigon"))
            {
                //2초간 화상상태
                pv.RPC("Skill4(coll)", PhotonTargets.All);
                
            }
        }
    }

    [PunRPC]
    void Skill4(Collider coll)
    {
        num = 0;
        InvokeRepeating("Skill4_Damage(coll)", 0, 1);
    }

    void Skill4_Damage(Collider coll)
    {
        num += 1;
        coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP -= 30;
        if (coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP < 0)
            coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP = 0;
        if (num==2)
            CancelInvoke("Skill4_Damage");
    }
    
}
