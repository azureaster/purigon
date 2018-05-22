using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NETPurigonCollisionCtrl : MonoBehaviour {

    float speedupItemVal = (2.0f / 10.0f); //스피드업 아이템을 먹을 경우 증가할 기본속도 수치

    public GameObject effect_Speedup;
    public GameObject effect_Pllut;
    public GameObject effect_Recovery;
    public GameObject effect_Invincible;
    
    private AudioSource[] soundEffect;
    private AudioSource collisionSoundEffect;
    private AudioSource itemnSoundEffect;

    public AudioClip sound_Speedup;
    public AudioClip sound_Pllut;
    public AudioClip sound_Recovery;

    public AudioClip sound_Hit;
    public AudioClip sound_HitShield;

    public GameObject item_Recovery;

    Vector3 recoverItemPosition;
    Vector3 collPos;

    float startTimer = 0.0f;
    float invincibleTimeToRun = 2.0f;
    float moveLeftTimeToRun = 1.0f;
    private SkinnedMeshRenderer smr;

    private PhotonView pv = null;
    Hashtable playerCustomSettings = new Hashtable();
    bool allPlayerDead;

    private void Awake()
    {
        playerCustomSettings.Add("IsDead", false);
        PhotonNetwork.player.SetCustomProperties(playerCustomSettings);

        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        soundEffect = GetComponents<AudioSource>(); ////0:Skill 1:Collision 2:Items
        collisionSoundEffect = soundEffect[1];
        itemnSoundEffect = soundEffect[2];

        pv = this.GetComponent<PhotonView>();
    }
    
    void OnCollisionEnter(Collision coll)
    {
        if (this.GetComponent<NETPurigonCtrl>().IsDead)
        {
            this.GetComponent<NETPurigonCtrl>().weight = 0;
            return;
        }

        if (this.GetComponent<NETPurigonCtrl>().IsShield == true && (coll.collider.tag == "HardObstacle" || coll.collider.tag == "SoftObstacle" || coll.collider.tag == "Wall"))
        {
            collisionSoundEffect.clip = sound_HitShield;
            collisionSoundEffect.Play();

            Destroy(this.GetComponent<NETPurigonCtrl>().shieldClone);
            this.GetComponent<NETPurigonCtrl>().IsShield = false;
            return;
        }
        else if (coll.collider.tag == "HardObstacle")
        {
            this.GetComponent<NETPurigonCtrl>().CharHP -= 50;
            if (this.GetComponent<NETPurigonCtrl>().CharHP <= 0)
                HP_becameZero();
            Collision_Effect(coll);

        }
        else if (coll.collider.tag == "SoftObstacle")
        {
            this.GetComponent<NETPurigonCtrl>().CharHP -= 10;
            if (this.GetComponent<NETPurigonCtrl>().CharHP <= 0)
                HP_becameZero();
            Collision_Effect(coll);
        }
        else if (coll.collider.tag == "Wall")
        {
            this.GetComponent<NETPurigonCtrl>().CharHP -= 50;
            if (this.GetComponent<NETPurigonCtrl>().CharHP <= 0)
                HP_becameZero();
            Collision_Effect(coll);
        }

        else if (coll.collider.tag == "Item_SmallP")
        {
            itemnSoundEffect.clip = sound_Pllut;
            itemnSoundEffect.Play();

            NETPurigonCtrl.pllut += 10;
            if (NETPurigonCtrl.pllut > 50)
                NETPurigonCtrl.pllut = 50;
            Destroy(coll.gameObject);
            GameObject pickItem_pllut = (GameObject)Instantiate(effect_Pllut, this.GetComponent<NETPurigonCtrl>().PurigonTr.position, Quaternion.identity);
            pickItem_pllut.transform.parent = transform;
            Destroy(pickItem_pllut, 2);
        }

        else if (coll.collider.tag == "Item_LargeP")
        {
            itemnSoundEffect.clip = sound_Pllut;
            itemnSoundEffect.Play();

            NETPurigonCtrl.pllut = 50;
            Destroy(coll.gameObject);
            GameObject pickItem_pllut = (GameObject)Instantiate(effect_Pllut, this.GetComponent<NETPurigonCtrl>().PurigonTr.position, Quaternion.identity);
            pickItem_pllut.transform.parent = transform;
            Destroy(pickItem_pllut, 2);
        }

        else if (coll.collider.tag == "Item_Recover")
        {
            itemnSoundEffect.clip = sound_Recovery;
            itemnSoundEffect.Play();

            this.GetComponent<NETPurigonCtrl>().CharHP += 20;
            if (this.GetComponent<NETPurigonCtrl>().CharHP > this.GetComponent<NETPurigonCtrl>().maxHP)
                this.GetComponent<NETPurigonCtrl>().CharHP = this.GetComponent<NETPurigonCtrl>().maxHP;

            GameObject pickItem_recovery = (GameObject)Instantiate(effect_Recovery, this.GetComponent<NETPurigonCtrl>().PurigonTr.position, Quaternion.identity);
            pickItem_recovery.transform.parent = transform;
            Destroy(pickItem_recovery, 2);

            recoverItemPosition = coll.transform.position;
            Destroy(coll.gameObject);
            Invoke("Create_RecoveryItem", 2);

        }

        else if (coll.collider.tag == "Item_Speedup")
        {
            itemnSoundEffect.clip = sound_Speedup;
            itemnSoundEffect.Play();

            this.GetComponent<NETPurigonCtrl>().basicSpeed += speedupItemVal;
            Destroy(coll.gameObject);
            GameObject pickitem_speed = (GameObject)Instantiate(effect_Speedup, this.GetComponent<NETPurigonCtrl>().PurigonTr.position, Quaternion.identity);
            pickitem_speed.transform.parent = transform;
            Destroy(pickitem_speed, 2);
        }
    }
    void Create_RecoveryItem()
    {
        GameObject recovery_item = (GameObject)Instantiate(item_Recovery, recoverItemPosition, Quaternion.identity);
    }

    void Collision_Effect(Collision coll) {
  
        if (this.GetComponent<NETPurigonCtrl>().IsDash == true){
            Physics.IgnoreCollision(coll.collider, this.GetComponent<Collider>(),true);
            this.GetComponent<NETPurigonCtrl>().IsDash = false;
            return;
        }
        else{
            collisionSoundEffect.clip = sound_Hit;
            collisionSoundEffect.Play();

            this.GetComponent<NETPurigonCtrl>().IsCollision = true;
            this.GetComponent<NETPurigonCtrl>().animator.SetBool("IsHitted", true);
            this.GetComponent<NETPurigonCtrl>().purigonState = NETPurigonCtrl.PurigonState.Hitted;


            Invoke("CollisionPositionEffect", 0.001f);
            Invoke("CurrentSpeed_Zero", 0.6f);
            Invoke("Collision_Mode", 0.7f);
            

            if (coll.collider.tag == "HardObstacle")
            {
                StartCoroutine(MoveLeftFunction());
                coll.transform.localScale *= 0.95f;
            }
            else if (coll.collider.tag == "SoftObstacle") {
                StartCoroutine(MoveLeftFunction());
                Physics.IgnoreCollision(coll.collider, this.GetComponent<Collider>(), true);
            }
            
            StartCoroutine(SetInvincible(coll));
        }

    }

    IEnumerator MoveLeftFunction() {
        startTimer = 0.0f;
        while (startTimer < moveLeftTimeToRun){
            this.GetComponent<NETPurigonCtrl>().currentSpeed -= 0.03f;
            this.GetComponent<NETPurigonCtrl>().PurigonTr.position += Vector3.left * Time.deltaTime * 5f;
            startTimer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SetInvincible(Collision coll){
        startTimer = 0.0f;

        Vector3 invincibleEffectPosition = new Vector3(this.GetComponent<NETPurigonCtrl>().PurigonTr.position.x, this.GetComponent<NETPurigonCtrl>().PurigonTr.position.y+0.2f, this.GetComponent<NETPurigonCtrl>().PurigonTr.position.z);
        GameObject invincible_effect = (GameObject)Instantiate(effect_Invincible, invincibleEffectPosition, Quaternion.identity);
        invincible_effect.transform.parent = transform;

        while (startTimer < invincibleTimeToRun){
            Physics.IgnoreLayerCollision(8, 10, true);  //layer8 = Purigon, layer10 = Obstacles
            startTimer += Time.deltaTime;
            yield return null;
        }
        Physics.IgnoreLayerCollision(8, 10, false);
        Destroy(invincible_effect);
        this.GetComponent<NETPurigonCtrl>().animator.SetBool("IsHitted", false);
        this.GetComponent<NETPurigonCtrl>().purigonState = NETPurigonCtrl.PurigonState.Hitted;
    }



    void CollisionPositionEffect()
    {
        collPos = CollisionDetect.pos;
        GameObject collision_effect = (GameObject)Instantiate(this.GetComponent<NETPurigonCtrl>().Collision_Effect, collPos, Quaternion.identity); 
        collision_effect.transform.parent = transform;
        Destroy(collision_effect, 0.2f);
    }

    void CurrentSpeed_Zero()
    {
        this.GetComponent<NETPurigonCtrl>().PurigonTr.Translate(new Vector3(0, 0, 0), Space.Self);
        this.GetComponent<NETPurigonCtrl>().currentSpeed = 0;
    }
    void Collision_Mode()
    {
        this.GetComponent<NETPurigonCtrl>().currentSpeed = this.GetComponent<NETPurigonCtrl>().basicSpeed;
        this.GetComponent<NETPurigonCtrl>().IsCollision = false;
    }
   

    public void HP_becameZero() {
        //멀티플레이어모드: 상태확인 - 일반상태이면 영혼부활 / 영혼상태이면 완전 죽음
        this.GetComponent<NETPurigonCtrl>().CharHP = 0;
        string currentScene = SceneManager.GetActiveScene().name;

        if (this.GetComponent<NETPurigonCtrl>().IsGhost)
        {
            //만약 영혼상태=true
            // this.GetComponent<NETPurigonCtrl>().CharHP = 0;
            this.GetComponent<NETPurigonCtrl>().animator.SetTrigger("IsDead");
            this.GetComponent<NETPurigonCtrl>().IsDead = true;
            this.GetComponent<NETPurigonCtrl>().material.SetTexture("_MainTex", this.GetComponent<NETPurigonCtrl>().purigonSprite.texture);
            this.GetComponent<NETPurigonCtrl>().purigonState = NETPurigonCtrl.PurigonState.Dead;
            
            playerCustomSettings["IsDead"] = true;
            PhotonNetwork.player.SetCustomProperties(playerCustomSettings);
            //관전모드로 다른 유저 보기 or 모두 죽었으면 화면 전환

        }
        else
        {
            //영혼상태=false 
            //영혼상태로 부활
            this.GetComponent<NETPurigonCtrl>().IsGhost = true;
            StartCoroutine(TransformToGhost());
        }


        //모두 죽었는지 확인
        allPlayerDead = true;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            Debug.Log(PhotonNetwork.playerList[i].NickName + " IsDead: " + PhotonNetwork.playerList[i].CustomProperties["IsDead"]);
            if ((bool)PhotonNetwork.playerList[i].CustomProperties["IsDead"] == false)
                allPlayerDead = false;
                break;
        }
        
        //만약 모두 죽었음 화면 전환
        if(allPlayerDead)
            Invoke("Multi_GameOverScene", 3);
        
    }
    
    [PunRPC]
    void ChangeTexture() {
        smr.material.SetTexture("_MainTex", this.GetComponent<NETPurigonCtrl>().ghostSprite.texture);
    }

    IEnumerator TransformToGhost(){
        this.GetComponent<NETPurigonCtrl>().CharHP = 100;
        //GameObject transform_ghost = (GameObject)PhotonView.Instantiate(this.GetComponent<NETPurigonCtrl>().transform_Ghost, this.GetComponent<NETPurigonCtrl>().PurigonTr.position, Quaternion.identity);
        pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Transform_Ghost", this.GetComponent<NETPurigonCtrl>().PurigonTr.position);
        yield return new WaitForSeconds(0.5f);
        pv.RPC("ChangeTexture", PhotonTargets.All);

        Debug.Log("changeToGhostDone");
        //GameObject.Find("userPurigonUI").GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("MyImage/" + purigonFaceName);
    }
    

    void Multi_GameOverScene()
    {
        PhotonNetwork.LoadLevel("GameOverScene_MultiPlayMode");
        //SceneManager.LoadScene("GameOverScene_MultiPlayMode");
        Debug.Log("MultiMode GameOver");
        PlayerPrefs.SetFloat("CurrentMapRecord", 0); //기록(현재 맵/기록/(당시사용퓨룡?)) DB에 저장하게 해야함
    }

}
