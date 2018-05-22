using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Healthy : MonoBehaviour {
    [PunRPC]
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "BalanceType" || coll.gameObject.tag == "LightType" || coll.gameObject.tag == "PhysicalType" || coll.gameObject.tag == "SpeedType")
            Debug.Log(coll.tag);

        if (coll.gameObject == GameObject.Find("userPurigon").GetComponent<NETInstantiateUserPurigon>().userPurigon)
               return;

        if (this.gameObject.tag == "Skill1")
        {
            Debug.Log("skill1");
            if (coll.gameObject.tag == "BalanceType" || coll.gameObject.tag == "LightType" || coll.gameObject.tag == "PhysicalType" || coll.gameObject.tag == "SpeedType")
            {
                Debug.Log(coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP);
                coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP -= 20;
                if (coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP < 0)
                    coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP = 0;
                Debug.Log(coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP);
            }
        }
        else if (this.gameObject.tag == "Skill3")
        {
            Debug.Log("skill3");
            if (coll.gameObject.tag == "BalanceType" || coll.gameObject.tag == "LightType" || coll.gameObject.tag == "PhysicalType" || coll.gameObject.tag == "SpeedType")
            {
                GameObject gain_weight = (GameObject)PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/H3_Gain_Weight", coll.gameObject.GetComponent<NETPurigonCtrl>().PurigonTr.position, Quaternion.identity,0);
                gain_weight.transform.parent = coll.transform;
                Destroy(gain_weight, 2.0f);
                coll.gameObject.GetComponent<NETPurigonCtrl>().weight += 5;
            }
        }
    }
}
