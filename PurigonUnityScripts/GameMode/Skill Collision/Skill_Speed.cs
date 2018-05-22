using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Speed : MonoBehaviour
{

    GameObject userPurigon;
    [PunRPC]
    void Start()
    {
        userPurigon = GameObject.Find("userPurigon").GetComponent<NETInstantiateUserPurigon>().userPurigon;

    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Purigon"))
            Debug.Log(coll.gameObject.tag);
        if (coll.gameObject == GameObject.Find("userPurigon").GetComponent<NETInstantiateUserPurigon>().userPurigon)
            return;

        if (this.gameObject.tag == "Skill1")
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Purigon"))
            {
                coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP -= 15;
                if (coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP < 0)
                    coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP = 0;
                //데미지 30
            }
            userPurigon.GetComponent<NETPurigonCtrl>().CharHP += 15;
            if (userPurigon.gameObject.GetComponent<NETPurigonCtrl>().CharHP > userPurigon.gameObject.GetComponent<NETPurigonCtrl>().maxHP)
                coll.gameObject.GetComponent<NETPurigonCtrl>().CharHP = userPurigon.gameObject.GetComponent<NETPurigonCtrl>().maxHP;
        }


        if (this.gameObject.tag == "Skill3")
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Purigon"))
            {
                GameObject fog_attack = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/S3_Fog", coll.transform.position, Quaternion.identity, 0);

                fog_attack.transform.parent = coll.transform;
                Destroy(fog_attack, 4.0f);
            }
        }
        else if (this.gameObject.tag == "Skill4")
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Purigon"))
            {
                coll.gameObject.GetComponent<NETPurigonCtrl>().IsEggMode = true;
                GameObject transform_egg = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/Shield/S4_Transform_Egg", coll.transform.position, Quaternion.identity, 0);
               // transform_egg.transform.parent = coll.transform;
                Destroy(transform_egg, 1f);
                StartCoroutine(Egg_Transform_Effect(coll));
            }
        }
    }

    [PunRPC]
    IEnumerator Egg_Transform_Effect(Collider coll)
{
        coll.gameObject.GetComponent<NETPurigonCtrl>().speed_purigon.SetActive(false);
        coll.gameObject.GetComponent<NETPurigonCtrl>().speed_egg.SetActive(true);
        coll.gameObject.GetComponent<NETPurigonCtrl>().purigonEye.SetActive(false);
        coll.gameObject.GetComponent<NETPurigonCtrl>().currentSpeed -= 5;
        yield return new WaitForSeconds(4);
        coll.gameObject.GetComponent<NETPurigonCtrl>().speed_purigon.SetActive(true);
        coll.gameObject.GetComponent<NETPurigonCtrl>().speed_egg.SetActive(false);
        coll.gameObject.GetComponent<NETPurigonCtrl>().purigonEye.SetActive(true);
        coll.gameObject.GetComponent<NETPurigonCtrl>().IsEggMode = false;
        coll.gameObject.GetComponent<NETPurigonCtrl>().currentSpeed = coll.gameObject.GetComponent<NETPurigonCtrl>().basicSpeed;
    }
}