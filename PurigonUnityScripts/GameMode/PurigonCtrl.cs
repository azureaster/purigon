using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurigonCtrl : MonoBehaviour {

    public enum PurigonState { FlyFoward, FlyGlide, Hitted };
    public PurigonState purigonState = PurigonState.FlyFoward;
    public Animator animator;

    public GameObject speed_purigon;
    public GameObject speed_egg;

    public Transform PurigonTr;

    private int userCHAR;
    string GetCharInfoURL = "https://projectpurigon.000webhostapp.com/unityPhp/GetCharInfo.php";
    public string[] userData;

    public float CharHP;
    public float CharBSPEED;
    public float CharMSPEED;
    public float CharACCEL;
    public float CharWEIGHT;

    public float basicSpeed; //기본 속도. 속도 증가 아이템을 먹을 경우 currentSpeed가 아니라 이 속도가 오름(가속이 끝날 경우 원래 날던 속도로 돌아오기 위한 변수.)
    public float maxbasicSpeed; //아이템으로 증가할 수 있는 최대 기본 속도.
    public float currentSpeed; //현재 날고 있는 속도. 가속버튼이 눌릴 경우 증가된다.
    public float maxSpeed;
    public float upSpeed; 
    public float weight;
    public float accelspeed = 1.7f;
    public float maxHP;
    private float increaseSpeed = 0.05f; //이건 이따가 공식 생기면 캐릭터 가속력에 맞게 GetPurigonStats에서 수정

    bool upClicked = false;
    bool speedClicked = false;
    bool skillClicked = false;

    //공격용 effect
    public GameObject Skill_1;
    public GameObject Skill_3;
    public GameObject Skill_4;
    public GameObject Skill_5;
    public GameObject Collision_Effect;
    public GameObject trace_Effect;
    public GameObject afterImage;

    public bool IsShield = false;
    public bool IsDash = false;
    private bool IsEggMode = false;
    public bool IsCollision = false;

    public static int pllut;

    public Slider hpBar;

    public GameObject shieldClone;
    GameObject afterimage_clone;

    new Rigidbody rigidbody;

    private AudioSource[] soundEffects;
    private AudioSource skillSoundEffect;

    //공식에 따라 수치 변경
    void GetPurigonStats()
    {
        basicSpeed = CharBSPEED / 10.0f;
        currentSpeed = basicSpeed;
        maxSpeed = CharMSPEED / 10.0f;
        maxbasicSpeed = (CharMSPEED - 10) / 10.0f;
        weight =  CharWEIGHT / 23.0f;
        upSpeed = CharWEIGHT / 20.0f;
        maxHP = CharHP;
    }

    void Awake()
    {
        pllut = 0;
        userCHAR = PlayerPrefs.GetInt("CharID", 1);
        soundEffects = GetComponents<AudioSource>(); //0:Skill 1:Collision 2:Item
        skillSoundEffect = soundEffects[0];  
    }


    void Start()    {
        StartCoroutine(GetUserChar());

        //퓨룡의 Transform 할당
        PurigonTr = this.gameObject.GetComponent<Transform>();
        animator = this.gameObject.GetComponent<Animator>();
        hpBar = GameObject.Find("Healthbar").GetComponent<Slider>();

        InvokeRepeating("Collectpllut", 10.0f , 10.0f);  //10초후에 10초마다 한번씩 Collectpllut함수 실행
        InvokeRepeating("HPReduce", 1.0f, 1.0f);

        Vector3 traceTransform = new Vector3(PurigonTr.position.x , PurigonTr.position.y + 0.2f, PurigonTr.position.z + 0.2f);

        GameObject trace_effect = (GameObject)Instantiate(trace_Effect, traceTransform, Quaternion.identity);
        trace_effect.transform.parent = transform;
    }


    void Update()
    {
        //animator.speed = currentSpeed;
        upClicked = GameObject.Find("BtnManager").GetComponent<PurigonBtnCtrl>().upClicked;
        speedClicked = GameObject.Find("BtnManager").GetComponent<PurigonBtnCtrl>().speedClicked;
        skillClicked = GameObject.Find("BtnManager").GetComponent<PurigonBtnCtrl>().skillClicked;

        if (upClicked == true || Input.GetKey(KeyCode.UpArrow)) 
            PurigonTr.Translate(new Vector3(0, upSpeed * Time.deltaTime, currentSpeed * Time.deltaTime), Space.Self);
        else 
            PurigonTr.Translate(new Vector3(0, -weight * Time.deltaTime, currentSpeed * Time.deltaTime), Space.Self);
    
        if (speedClicked == true || Input.GetKey(KeyCode.RightArrow))
            IfSpeedClick();
        else
            IfSpeedUnClick();

        if (skillClicked == true || Input.GetKey(KeyCode.Space))
            IfSkillClicked();
        
       
        hpBar.value = CharHP / maxHP;
        
    }



    //프륫의 개수를 증가시켜주는 함수
    void Collectpllut() {
        pllut += 2;  
        if (pllut > 50) pllut = 50;
    }


    void HPReduce()
    {
        CharHP -= 1;
        if (CharHP <= 0){
            this.GetComponent<PurigonCollisionCtrl>().HP_becameZero();
        }
    }


    public void IfSpeedClick()
    {
        animator.SetBool("IsSpeedBtn", true);
        purigonState = PurigonState.FlyGlide;

        if (currentSpeed < (basicSpeed * accelspeed)){  
            currentSpeed += increaseSpeed;
            if (currentSpeed > maxSpeed) currentSpeed = maxSpeed; //최대속도 초과시 최대속도만큼만.
        }
        

    }
    public void IfSpeedUnClick()
    {
        if (IsDash == true)
        {
            return;
        }
        if (IsCollision == true)
        {
            currentSpeed = 0f;
            return;
        }
        animator.SetBool("IsSpeedBtn", false);
        purigonState = PurigonState.FlyGlide;
        //currentSpeed = basicSpeed;
        if (currentSpeed > basicSpeed) {
            currentSpeed -= increaseSpeed;
        }
    }

    public void IfSkillClicked()
    {
        if (pllut >= 50 && IsShield == false && IsDash == false && IsEggMode == false)    //Level5 skill
        {
            GetAudioClips("Shield");
            GameObject shield = (GameObject)Instantiate(Skill_5, PurigonTr.position, Quaternion.identity);
            shield.transform.parent = transform;
            shieldClone = shield;
            pllut -= 50;
            IsShield = true;

        }

        else if (pllut >= 40 && IsDash == false && IsEggMode == false)   //Level4 skill
        {
            if (gameObject.tag == "BalanceType")
            {
                GetAudioClips("B4");
                GameObject black_hole = (GameObject)Instantiate(Skill_4, PurigonTr.position, Quaternion.identity);
                //일정 시간이 지나면 없앰
                Destroy(black_hole, 2.0f);
            }
            else if (gameObject.tag == "LightType")
            {
                GetAudioClips("L4");
                StartCoroutine(Eagle_Effect());
            }
            else if (gameObject.tag == "PhysicalType")
            {
                GetAudioClips("H4");
                CharHP += 20;
                if (CharHP > maxHP) CharHP = maxHP;

                GameObject recovery_hp = (GameObject)Instantiate(Skill_4, PurigonTr.position, Quaternion.identity);
                recovery_hp.transform.parent = transform;
                Destroy(recovery_hp, 2.0f);
            }
            else if (gameObject.tag == "SpeedType")
            {
                GetAudioClips("S4");
                StartCoroutine(Egg_Transform_Effect());
                IsEggMode = true;
            }
            pllut -= 40;
        }

        else if (pllut >= 30 && IsDash == false && IsEggMode == false)   //Level3 skill
        {
            if (gameObject.tag == "BalanceType")
            {
                GetAudioClips("B3");
                GameObject big_frost_ball = (GameObject)Instantiate(Skill_3, new Vector3(PurigonTr.position.x + 2.0f, PurigonTr.position.y, PurigonTr.position.z), Quaternion.identity);
                Destroy(big_frost_ball, 1.0f);
            }
            else if (gameObject.tag == "LightType")
            {
                GetAudioClips("L3");
                GameObject push_Wave = (GameObject)Instantiate(Skill_3, PurigonTr.position, Quaternion.identity);
                push_Wave.transform.parent = transform;
                Destroy(push_Wave, 1.0f);
            }
            else if (gameObject.tag == "PhysicalType")
            {
                GetAudioClips("H3");
                //자신을 제외한 모든 플레이어들의 무게 5 증가
                GameObject gain_weight = (GameObject)Instantiate(Skill_3, PurigonTr.position, Quaternion.identity);
                //지금은 임시로 해당 플레이어에게 effect를 주지만 다른 플레이어들을 추가한 후 다른 플레이어의 transform으로 수정
                /*
                 * foreach(PhotonPlayer pl in PhotonNetwork.playerList){
                 * if(pl.gameObject.getComponent<InstantiateUserPurigon>().CurrentuserID != PlayerPrefs.GetString("LoginID", "Default ID"))
                        본인 제외 모든 유저
                 * }
                 */
                gain_weight.transform.parent = transform;
                Destroy(gain_weight, 2.0f);
            }
            else if (gameObject.tag == "SpeedType")
            {
                GetAudioClips("S3");
                //다른 플레이어들의 시야 방해
                GameObject fog_attack = (GameObject)Instantiate(Skill_3, PurigonTr.position, Quaternion.identity);
                fog_attack.transform.parent = transform;
                Destroy(fog_attack, 4.0f);
            }
            pllut -= 30;
        }

        else if (pllut >= 20 && IsDash == false && IsEggMode == false)  //Level2 skill
        {
            GetAudioClips("Dash");
            IsDash = true;
            GameObject afterimage = (GameObject)Instantiate(afterImage, PurigonTr.position, Quaternion.identity);
            afterimage.transform.parent = transform;
            afterimage_clone = afterimage;
            StartCoroutine(Dash_Effect());
         
        }

        else if (pllut >= 10 && IsDash == false && IsEggMode == false)   //Level1 skill
        {
            if (gameObject.tag == "BalanceType")
            {
                GetAudioClips("B1");
                GameObject frost_ball = (GameObject)Instantiate(Skill_1, new Vector3(PurigonTr.position.x + 2.0f, PurigonTr.position.y, PurigonTr.position.z), Quaternion.identity);
                Destroy(frost_ball, 1.0f);
            }
            else if (gameObject.tag == "LightType")
            {
                GetAudioClips("L1");
                GameObject life_ball = (GameObject)Instantiate(Skill_1, PurigonTr.position, Quaternion.identity);
                life_ball.transform.parent = transform;
                Destroy(life_ball, 3.0f);
            }
            else if (gameObject.tag == "PhysicalType")
            {
                GetAudioClips("H1");
                //자신을 제외한 근접한 모든 플레이어 데미지 20
                GameObject melee_attack = (GameObject)Instantiate(Skill_1, PurigonTr.position, Quaternion.identity);
                melee_attack.transform.parent = transform;
            }
            else if (gameObject.tag == "SpeedType")
            {
                GetAudioClips("S1");
                //근처에 다른 플레이어가 있을 경우 제일 가까운 유저의 hp를 15흡수

                GameObject absorbed_hp = (GameObject)Instantiate(Skill_1, PurigonTr.position, Quaternion.identity);
                absorbed_hp.transform.parent = transform;
                Destroy(absorbed_hp, 2.0f);
            }
            pllut -= 10;
        }
    }


    public void GetAudioClips(string SkillName) {

        switch (SkillName) {
            case "Dash":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_Dash;
                break;
            case "Shield":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_Shield;
                break;


            case "B1":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_B1;
                break;
            case "B3":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_B3;
                break;
            case "B4":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_B4;
                break;

            case "H1":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_H1;
                break;
            case "H3":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_H3;
                break;
            case "H4":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_H4;
                break;

            case "L1":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_L1;
                break;
            case "L3":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_L3;
                break;
            case "L4":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_L4;
                break;

            case "S1":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_S1;
                break;
            case "S3":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_S3;
                break;
            case "S4":
                skillSoundEffect.clip = this.GetComponent<PurigonSkillSounds>().sound_S4;
                break;

            default:
                break;
        }        
        skillSoundEffect.Play();
    }



    IEnumerator Dash_Effect()
    {
        currentSpeed *= 2.0f;
        pllut -= 20;
        yield return new WaitForSeconds(0.4f);
        currentSpeed = basicSpeed;
        Destroy(afterimage_clone);
        IsDash = false;
    }


    IEnumerator Eagle_Effect()
    {
        GameObject eagle = (GameObject)Instantiate(Skill_4, PurigonTr.position, Quaternion.identity);
        eagle.transform.parent = transform;
        
        //스킬 효과 작성 필요
        yield return new WaitForSeconds(5);
        //스킬 종료(수치 원상복귀)

        Destroy(eagle);
    }

    IEnumerator Egg_Transform_Effect()
    {
        //자신 이외의 모든 유저 변신시키고 속도-30
        GameObject transform_egg = (GameObject)Instantiate(Skill_4, PurigonTr.position, Quaternion.identity);
        transform_egg.transform.parent = transform;
        speed_purigon.SetActive(false);
        speed_egg.SetActive(true);
        yield return new WaitForSeconds(5);
        speed_purigon.SetActive(true);
        speed_egg.SetActive(false);
        IsEggMode = false;
    }

    IEnumerator GetUserChar()
    {
        WWWForm form = new WWWForm();
        form.AddField("charIDPost", userCHAR);

        WWW getCharData = new WWW(GetCharInfoURL, form);
        yield return getCharData;

        string currentCharData = getCharData.text;

        userData = currentCharData.Split(';');
        CharHP = float.Parse(GetDataValue(userData[0], "HP:"));
        CharBSPEED = float.Parse(GetDataValue(userData[0], "BSPEED:"));
        CharMSPEED = float.Parse(GetDataValue(userData[0], "MSPEED:"));
        CharACCEL = float.Parse(GetDataValue(userData[0], "ACCEL:"));
        CharWEIGHT = float.Parse(GetDataValue(userData[0], "WEIGHT:"));

        GetPurigonStats();
    }

    

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }




}
