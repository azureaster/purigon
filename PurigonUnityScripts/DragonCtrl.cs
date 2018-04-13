using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DragonCtrl : MonoBehaviour
{
    public enum PurigonState { FlyFoward, FlyGlide };
    public PurigonState purigonState = PurigonState.FlyFoward;
    private Animator animator;

    public GameObject speed_purigon;
    public GameObject speed_egg;
    public GameObject speed_cg;

    private Transform PurigonTr;
    public float moveSpeed = 2.0f;
    public float UpSpeed = 0.05f;
    public float Weight = 0.05f;


    new Rigidbody rigidbody;

    bool IsJumped = false;

    //공격용 effect
    public GameObject Skill_1;
    public GameObject Skill_3;
    public GameObject Skill_4;
    public GameObject Skill_5;
    public GameObject Collision_Effect;

    bool IfShield = false;
    //무적 스킬 사용중에는 중력 0, 다른 스킬 사용 불가
    bool IfDash = false;
    bool IfEggMode = false;

    bool IsHitted = false;

    public static int pllut;
    public GameObject pllutTxt;

    GameObject shieldClone;

    void Start()
    {
        //퓨룡의 Transform 할당
        PurigonTr = this.gameObject.GetComponent<Transform>();
        animator = this.gameObject.GetComponent<Animator>();

        pllutTxt = GameObject.Find("pllutTxt");
        //2초후에 2초마다 한번씩 Collectpllut함수 실행
        InvokeRepeating("Collectpllut", 1, 1);
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        pllut = 0;
    }
    void Update()
    {
        animator.speed = moveSpeed;

        if (IsHitted == true)
        {

            return;
        }
        if (IsJumped == true || Input.GetKey(KeyCode.UpArrow))
        {
            PurigonTr.Translate(new Vector3(0, UpSpeed, UpSpeed), Space.Self);
        }
        else
        {
            PurigonTr.Translate(new Vector3(0, -Weight, Weight), Space.Self);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            OnMouseUp();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            IfSpeedClick();
        }
        else
        {
            IfSpeedUnClick();
        }

    }

    //푸륫의 개수를 증가시켜주는 함수
    void Collectpllut()
    {
        pllut += 5;
    }

    public void IfUpClick()
    {
        IsJumped = true;
    }

    public void IfUpUnClick()
    {
        IsJumped = false;
    }

    public void IfSpeedClick()
    {
        animator.SetBool("IsSpeedBtn", true);
        purigonState = PurigonState.FlyGlide;
        moveSpeed = 10.0f;
    }
    public void IfSpeedUnClick()
    {
        animator.SetBool("IsSpeedBtn", false);
        purigonState = PurigonState.FlyGlide;
        moveSpeed = 2.0f;
    }

    public void OnMouseUp()
    {
        if (pllut >= 50 && IfShield == false && IfDash == false && IfEggMode == false)    //Level5 skill
        {
            //shield 방어막 effect
            GameObject shield = (GameObject)Instantiate(Skill_5, PurigonTr.position, Quaternion.identity);
            shield.transform.parent = transform;
            shieldClone = shield;
            pllut -= 50;
            IfShield = true;

        }

        else if (pllut >= 40 && IfDash == false && IfEggMode == false)   //Level4 skill
        {
            if (gameObject.tag == "BalanceType")
            {
                //Black hole 공격 effect
                //animator는 사용하지 않음
                GameObject black_hole = (GameObject)Instantiate(Skill_4, PurigonTr.position, Quaternion.identity);
                //일정 시간이 지나면 없앰
                Destroy(black_hole, 2.0f);
            }
            else if (gameObject.tag == "LightType")
            {
                StartCoroutine(Eagle_Effect());
            }
            else if (gameObject.tag == "PhysicalType")
            {
                //자신의 체력 20 회복
                GameObject recovery_hp = (GameObject)Instantiate(Skill_4, PurigonTr.position, Quaternion.identity);
                recovery_hp.transform.parent = transform;
                Destroy(recovery_hp, 2.0f);
            }
            else if (gameObject.tag == "SpeedType")
            {
                StartCoroutine(Egg_Transform_Effect());
                IfEggMode = true;
            }
            pllut -= 40;
        }

        else if (pllut >= 30 && IfDash == false && IfEggMode == false)   //Level3 skill
        {
            if (gameObject.tag == "BalanceType")
            {
                //Frost Ball 공격 effect 
                GameObject big_frost_ball = (GameObject)Instantiate(Skill_3, new Vector3(PurigonTr.position.x + 2.0f, PurigonTr.position.y, PurigonTr.position.z), Quaternion.identity);
                Destroy(big_frost_ball, 1.0f);
            }
            else if (gameObject.tag == "LightType")
            {
                GameObject push_Wave = (GameObject)Instantiate(Skill_3, PurigonTr.position, Quaternion.identity);
                push_Wave.transform.parent = transform;
                Destroy(push_Wave, 1.0f);
            }
            else if (gameObject.tag == "PhysicalType")
            {
                //자신을 제외한 모든 플레이어들의 무게 5 증가
                GameObject gain_weight = (GameObject)Instantiate(Skill_3, PurigonTr.position, Quaternion.identity);
                //지금은 임시로 해당 플레이어에게 effect를 주지만 다른 플레이어들을 추가한 후 다른 플레이어의 transform으로 수정
                gain_weight.transform.parent = transform;
                Destroy(gain_weight, 2.0f);
            }
            else if (gameObject.tag == "SpeedType")
            {
                GameObject mainCamera = GameObject.Find("MainCamera");
                //다른 플레이어들의 시야 방해
                GameObject fog_attack = (GameObject)Instantiate(Skill_3, mainCamera.transform.position, Skill_3.transform.rotation);
                fog_attack.transform.parent = mainCamera.transform;
                Destroy(fog_attack, 4.0f);
            }
            pllut -= 30;
        }

        else if (pllut >= 20 && IfDash == false && IfEggMode == false)  //Level2 skill
        {
            StartCoroutine(Advance_Effect());
            IfDash = true;
        }

        else if (pllut >= 10 && IfDash == false && IfEggMode == false)   //Level1 skill
        {
            if (gameObject.tag == "BalanceType")
            {
                GameObject frost_ball = (GameObject)Instantiate(Skill_1, new Vector3(PurigonTr.position.x + 2.0f, PurigonTr.position.y, PurigonTr.position.z), Quaternion.identity);
                Destroy(frost_ball, 1.0f);
            }
            else if (gameObject.tag == "LightType")
            {
                //3초간 불기둥 공격 Life Ball effect 

                GameObject life_ball = (GameObject)Instantiate(Skill_1, PurigonTr.position, Quaternion.identity);
                life_ball.transform.parent = transform;
                Destroy(life_ball, 3.0f);
            }
            else if (gameObject.tag == "PhysicalType")
            {
                //전방 근접 공격 effect 
                //자신을 제외한 모든 플레이어 데미지 20
                GameObject melee_attack = (GameObject)Instantiate(Skill_1, PurigonTr.position, Quaternion.identity);
                melee_attack.transform.parent = transform;
            }
            else if (gameObject.tag == "SpeedType")
            {
                //근처에 있는 다른 플레이어들의 hp를 15흡수

                GameObject absorbed_hp = (GameObject)Instantiate(Skill_1, PurigonTr.position, Quaternion.identity);
                absorbed_hp.transform.parent = transform;
                Destroy(absorbed_hp, 2.0f);
            }
            pllut -= 10;
        }
    }

    //만약 부딪히면 방어막 없어짐 , IfShield=false 함수 추가

    IEnumerator Eagle_Effect()
    {
        GameObject eagle = (GameObject)Instantiate(Skill_4, PurigonTr.position, Quaternion.identity);
        eagle.transform.parent = transform;
        IfSpeedClick();
        yield return new WaitForSeconds(6);
        IfSpeedUnClick();
        Destroy(eagle);
    }

    IEnumerator Advance_Effect()
    {
        rigidbody.AddForce(Vector3.right * 30.0f, ForceMode.Impulse);
        pllut -= 20;
        yield return new WaitForSeconds(1);
        rigidbody.AddForce(Vector3.right * 0.0f, ForceMode.Impulse);
        IfDash = false;
    }

    IEnumerator Egg_Transform_Effect()
    {
        //다른 플레이어들을 양 모드로 변환
        GameObject transform_egg = (GameObject)Instantiate(Skill_4, PurigonTr.position, Quaternion.identity);
        //속도-30
        transform_egg.transform.parent = transform;
        speed_purigon.SetActive(false);
        speed_egg.SetActive(true);
        pllut -= 40;
        yield return new WaitForSeconds(5);
        speed_purigon.SetActive(true);
        speed_egg.SetActive(false);
        IfEggMode = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (IfShield == true)
        {
            Destroy(shieldClone);
            IfShield = false;
            return;
        }
        if (col.gameObject.tag == "Wall")
        {
            if (IfDash == true)
            //스킬 2 dash상태에서 장애물에 부딪히면
            {
                Destroy(col.gameObject);
                return;
            }
            IsHitted = true;
            PurigonTr.Translate(new Vector3(0, 0, 0), Space.Self);
            moveSpeed = 0.0f;
            GameObject collision_effect = (GameObject)Instantiate(Collision_Effect, PurigonTr.position, Quaternion.identity);
            Invoke("Collision_Mode", 0.2f);
            Destroy(collision_effect, 0.2f);
        }
        else if (col.gameObject.tag == "Obstacle")
        {
            if (IfDash == true)
            //스킬 2 dash상태에서 장애물에 부딪히면
            {
                Destroy(col.gameObject);
                return;
            }
            IsHitted = true;
            PurigonTr.Translate(new Vector3(0, 0, 0), Space.Self);
            moveSpeed = 0.0f;
            GameObject collision_effect = (GameObject)Instantiate(Collision_Effect, PurigonTr.position, Quaternion.identity);
            Invoke("Collision_Mode", 0.2f);
            Destroy(col.gameObject);
            Destroy(collision_effect, 0.2f);
        }
    }
    void Collision_Mode()
    {
        IsHitted = false;
        moveSpeed = 2.0f;

    }


}
