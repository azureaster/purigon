using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PurigonCollisionCtrl : MonoBehaviour {

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

    private void Awake()
    {
        soundEffect = GetComponents<AudioSource>(); ////0:Skill 1:Collision 2:Items
        collisionSoundEffect = soundEffect[1];
        itemnSoundEffect = soundEffect[2];
    }
    
    void OnCollisionEnter(Collision coll)
    {
        if (this.GetComponent<PurigonCtrl>().IsShield == true && (coll.collider.tag == "HardObstacle" || coll.collider.tag == "SoftObstacle" || coll.collider.tag == "Wall"))
        {
            collisionSoundEffect.clip = sound_HitShield;
            collisionSoundEffect.Play();

            Destroy(this.GetComponent<PurigonCtrl>().shieldClone);
            this.GetComponent<PurigonCtrl>().IsShield = false;
            return;
        }
        else if (coll.collider.tag == "HardObstacle")
        {
            this.GetComponent<PurigonCtrl>().CharHP -= 50;
            Collision_Effect(coll);
            if (this.GetComponent<PurigonCtrl>().CharHP < 0)
                HP_becameZero();

        }
        else if (coll.collider.tag == "SoftObstacle")
        {
            this.GetComponent<PurigonCtrl>().CharHP -= 10;
            Collision_Effect(coll);
            if (this.GetComponent<PurigonCtrl>().CharHP < 0)
                HP_becameZero();
        }
        else if (coll.collider.tag == "Wall")
        {
            this.GetComponent<PurigonCtrl>().CharHP -= 50;
            Collision_Effect(coll);
            if (this.GetComponent<PurigonCtrl>().CharHP < 0)
                HP_becameZero();
        }

        else if (coll.collider.tag == "Item_SmallP")
        {
            itemnSoundEffect.clip = sound_Pllut;
            itemnSoundEffect.Play();

            PurigonCtrl.pllut += 10;
            if (PurigonCtrl.pllut > 50)
                PurigonCtrl.pllut = 50;
            Destroy(coll.gameObject);
            GameObject pickItem_pllut = (GameObject)Instantiate(effect_Pllut, this.GetComponent<PurigonCtrl>().PurigonTr.position, Quaternion.identity);
            pickItem_pllut.transform.parent = transform;
            Destroy(pickItem_pllut, 2);
        }

        else if (coll.collider.tag == "Item_LargeP")
        {
            itemnSoundEffect.clip = sound_Pllut;
            itemnSoundEffect.Play();

            PurigonCtrl.pllut = 50;
            Destroy(coll.gameObject);
            GameObject pickItem_pllut = (GameObject)Instantiate(effect_Pllut, this.GetComponent<PurigonCtrl>().PurigonTr.position, Quaternion.identity);
            pickItem_pllut.transform.parent = transform;
            Destroy(pickItem_pllut, 2);
        }

        else if (coll.collider.tag == "Item_Recover")
        {
            itemnSoundEffect.clip = sound_Recovery;
            itemnSoundEffect.Play();

            this.GetComponent<PurigonCtrl>().CharHP += 20;
            if (this.GetComponent<PurigonCtrl>().CharHP > this.GetComponent<PurigonCtrl>().maxHP)
                this.GetComponent<PurigonCtrl>().CharHP = this.GetComponent<PurigonCtrl>().maxHP;

            GameObject pickItem_recovery = (GameObject)Instantiate(effect_Recovery, this.GetComponent<PurigonCtrl>().PurigonTr.position, Quaternion.identity);
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

            this.GetComponent<PurigonCtrl>().basicSpeed += speedupItemVal;
            Destroy(coll.gameObject);
            GameObject pickitem_speed = (GameObject)Instantiate(effect_Speedup, this.GetComponent<PurigonCtrl>().PurigonTr.position, Quaternion.identity);
            pickitem_speed.transform.parent = transform;
            Destroy(pickitem_speed, 2);
        }
    }
    void Create_RecoveryItem()
    {
        GameObject recovery_item = (GameObject)Instantiate(item_Recovery, recoverItemPosition, Quaternion.identity);
    }

    void Collision_Effect(Collision coll) {
  
        if (this.GetComponent<PurigonCtrl>().IsDash == true){
            Physics.IgnoreCollision(coll.collider, this.GetComponent<Collider>(),true);
            this.GetComponent<PurigonCtrl>().IsDash = false;
            return;
        }
        else{
            collisionSoundEffect.clip = sound_Hit;
            collisionSoundEffect.Play();

            this.GetComponent<PurigonCtrl>().IsCollision = true;
            this.GetComponent<PurigonCtrl>().animator.SetBool("IsHitted", true);
            this.GetComponent<PurigonCtrl>().purigonState = PurigonCtrl.PurigonState.Hitted;


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
            this.GetComponent<PurigonCtrl>().currentSpeed -= 0.03f;
            this.GetComponent<PurigonCtrl>().PurigonTr.position += Vector3.left * Time.deltaTime * 5f;
            startTimer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SetInvincible(Collision coll){
        startTimer = 0.0f;

        Vector3 invincibleEffectPosition = new Vector3(this.GetComponent<PurigonCtrl>().PurigonTr.position.x, this.GetComponent<PurigonCtrl>().PurigonTr.position.y+0.2f, this.GetComponent<PurigonCtrl>().PurigonTr.position.z);
        GameObject invincible_effect = (GameObject)Instantiate(effect_Invincible, invincibleEffectPosition, Quaternion.identity);
        invincible_effect.transform.parent = transform;

        while (startTimer < invincibleTimeToRun){
            Physics.IgnoreLayerCollision(8, 10, true);  //layer8 = Purigon, layer10 = Obstacles
            startTimer += Time.deltaTime;
            yield return null;
        }
        Physics.IgnoreLayerCollision(8, 10, false);
        Destroy(invincible_effect);
        this.GetComponent<PurigonCtrl>().animator.SetBool("IsHitted", false);
        this.GetComponent<PurigonCtrl>().purigonState = PurigonCtrl.PurigonState.Hitted;
    }



    void CollisionPositionEffect()
    {
        collPos = CollisionDetect.pos;
        GameObject collision_effect = (GameObject)Instantiate(this.GetComponent<PurigonCtrl>().Collision_Effect, collPos, Quaternion.identity); 
        collision_effect.transform.parent = transform;
        Destroy(collision_effect, 0.2f);
    }

    void CurrentSpeed_Zero()
    {
        this.GetComponent<PurigonCtrl>().PurigonTr.Translate(new Vector3(0, 0, 0), Space.Self);
        this.GetComponent<PurigonCtrl>().currentSpeed = 0;
    }
    void Collision_Mode()
    {
        this.GetComponent<PurigonCtrl>().currentSpeed = this.GetComponent<PurigonCtrl>().basicSpeed;
        this.GetComponent<PurigonCtrl>().IsCollision = false;
    }
   



    public void HP_becameZero() {
        if (this.GetComponent<PurigonCtrl>().CharHP < 0) this.GetComponent<PurigonCtrl>().CharHP = 0;
        //만약 영혼이며 죽는효과나 에니메이션



        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene.Contains("Single")) {
            //SceneManager.LoadScene("GameOverScene_SinglePlayMode");
            Debug.Log("GameOver");
            PlayerPrefs.SetFloat("PracticeRecord",0);
        }
        else {


        }
    }


}
