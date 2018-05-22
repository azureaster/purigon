using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NETPurigonCtrl : Photon.MonoBehaviour
{

    public enum PurigonState { FlyFoward, FlyGlide, Hitted, Dead };
    public PurigonState purigonState = PurigonState.FlyFoward;
    public Animator animator;

    public GameObject speed_purigon;
    public GameObject speed_egg;
    public GameObject purigonEye;
    public GameObject pllutFill;

    public Transform PurigonTr;

    private SkinnedMeshRenderer smr;
    public Sprite ghostSprite;
    public Sprite purigonSprite;
    public Material material;

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
    public GameObject Collision_Effect;
    public GameObject trace_Effect;
    public GameObject afterImage;
    public GameObject transform_Ghost;
    public GameObject ghost_Skill;

    //Skills
    public GameObject skill1;
    public GameObject skill2;
    public GameObject skill3;
    public GameObject skill4;
    public GameObject skill5;


    public bool IsShield = false;
    public bool IsDash = false;
    public bool IsEggMode = false;
    public bool IsCollision = false;
    public bool IsDead = false;
    public bool IsGhost = false;
    public bool IsSkilled = false;
    public bool IsCollisionWithPlayer = false;
    public bool IsLightTypeSkill3 = false;
    public bool IsDamaged_L1 = false;

    public static float pllut;

    public RectTransform followHealthBar;

    private Collision anotherPlayer;
    public GameObject shieldClone;
    public GameObject frostBallClone;

    GameObject afterimage_clone;
    GameObject ghost_skill_Clone;

    private AudioSource[] soundEffects;
    private AudioSource skillSoundEffect;

    private PhotonView pv = null;
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;
    Hashtable playerCustomSettings = new Hashtable();


    //공식에 따라 수치 변경
    void GetPurigonStats()
    {
        basicSpeed = CharBSPEED / 10.0f;
        currentSpeed = basicSpeed;
        maxSpeed = CharMSPEED / 10.0f;
        maxbasicSpeed = (CharMSPEED - 10) / 10.0f;
        weight = CharWEIGHT / 23.0f;
        upSpeed = CharWEIGHT / 20.0f;
        maxHP = CharHP;
    }

    void Awake()
    {
        IsDamaged_L1 = false;

        pllut = 0;
        userCHAR = PlayerPrefs.GetInt("CharID", 1);
        soundEffects = GetComponents<AudioSource>(); //0:Skill 1:Collision 2:Item
        pllutFill = GameObject.Find("PllutFill");
        skillSoundEffect = soundEffects[0];

        pv = this.GetComponent<PhotonView>();
        Debug.Log("pv.isMine: " + pv.isMine);
        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        pv.ObservedComponents[0] = this;

        if (pv.isMine)
        {
            GameObject.Find("MainCamera").GetComponent<NETFollowPurigon>().target = PurigonTr;
            GameObject.Find("Background_Forest").GetComponent<NETFollowPurigon>().target = PurigonTr;
        }

        currPos = PurigonTr.position;
        currRot = PurigonTr.rotation;
        smr = GetComponentInChildren<SkinnedMeshRenderer>();

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(PurigonTr.position);
            stream.SendNext(PurigonTr.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }



    void Start()
    {
        StartCoroutine(GetUserChar());

        //퓨룡의 Transform 할당
        PurigonTr = this.gameObject.GetComponent<Transform>();
        animator = this.gameObject.GetComponent<Animator>();

        smr.material.SetTexture("_MainTex", purigonSprite.texture);

        if (pv.isMine)
        {
            Vector3 traceTransform = new Vector3(PurigonTr.position.x, PurigonTr.position.y + 0.2f, PurigonTr.position.z + 0.2f);
            GameObject trace_effect = (GameObject)Instantiate(trace_Effect, traceTransform, Quaternion.identity);
            trace_effect.transform.parent = PurigonTr;
            InvokeRepeating("Collectpllut", 1.0f, 1.0f);  //10초후에 10초마다 한번씩 Collectpllut함수 실행
            pv.RPC("HPReduceRPC", PhotonTargets.All);
        }
    }


    void Update()
    {

        if (!pv.isMine)
        {
            PurigonTr.position = Vector3.Lerp(PurigonTr.position, currPos, Time.deltaTime * 3.0f);
            return;
        }
        if (IsDead)
        {
            followHealthBar.sizeDelta = new Vector2(CharHP / maxHP * 100, followHealthBar.sizeDelta.y);
            currentSpeed = 0;
            PurigonTr.Translate(new Vector3(0, -weight * Time.deltaTime, currentSpeed * Time.deltaTime), Space.Self);
            //transform.LookAt(Camera.main.transform);
            playerCustomSettings["IsDead"] = true;
            PhotonNetwork.player.SetCustomProperties(playerCustomSettings);
            return;
        }
        //animator.speed = currentSpeed;
        upClicked = GameObject.Find("BtnManager").GetComponent<PurigonBtnCtrl>().upClicked;
        speedClicked = GameObject.Find("BtnManager").GetComponent<PurigonBtnCtrl>().speedClicked;
        skillClicked = GameObject.Find("BtnManager").GetComponent<PurigonBtnCtrl>().skillClicked;

        if (upClicked == true || Input.GetKey(KeyCode.UpArrow))
            PurigonTr.Translate(new Vector3(0, upSpeed * Time.deltaTime, currentSpeed * Time.deltaTime), Space.Self);
        /*else
            PurigonTr.Translate(new Vector3(0, -weight * Time.deltaTime, currentSpeed * Time.deltaTime), Space.Self);*/

        //for testing purpose
        if (Input.GetKey(KeyCode.DownArrow))
            PurigonTr.Translate(new Vector3(0, -weight * Time.deltaTime, currentSpeed * Time.deltaTime), Space.Self);
        if (Input.GetKey(KeyCode.RightArrow))
            PurigonTr.Translate(new Vector3(0, 0, currentSpeed * Time.deltaTime), Space.Self);


        if (speedClicked == true || Input.GetKey(KeyCode.RightArrow))
            IfSpeedClick();
        else
            IfSpeedUnClick();

        if (skillClicked == true || Input.GetKey(KeyCode.Space))
            IfSkillClicked();

        //followHealthBar.sizeDelta = new Vector2(CharHP / maxHP * 100, followHealthBar.sizeDelta.y);
        pv.RPC("FollowHPBar", PhotonTargets.All);
        //transform.LookAt(Camera.main.transform);
        pllutFill.GetComponent<Image>().fillAmount = pllut / 50;
    }

    [PunRPC]
    void FollowHPBar()
    {
        followHealthBar.sizeDelta = new Vector2(CharHP / maxHP * 100, followHealthBar.sizeDelta.y);
    }


    public void IfSpeedClick()
    {
        if (IsDead) return;
        animator.SetBool("IsSpeedBtn", true);
        purigonState = PurigonState.FlyGlide;

        if (currentSpeed < (basicSpeed * accelspeed))
        {
            currentSpeed += increaseSpeed;
            if (currentSpeed > maxSpeed) currentSpeed = maxSpeed; //최대속도 초과시 최대속도만큼만.
        }


    }
    public void IfSpeedUnClick()
    {
        if (IsDead) return;
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
        if (currentSpeed > basicSpeed)
        {
            currentSpeed -= increaseSpeed;
        }
    }

    //프륫의 개수를 증가시켜주는 함수
    void Collectpllut()
    {
        if (IsDead) return;
        pllut += 5;  //원래 2 여야함
        if (pllut > 50) pllut = 50;
    }

    [PunRPC]
    void HPReduceRPC()
    {
        InvokeRepeating("HPReduce", 1.0f, 1.0f);
    }
    void HPReduce()
    {
        if (IsDead) return;
        CharHP -= 1;
        if (CharHP <= 0)
        {
            this.GetComponent<NETPurigonCollisionCtrl>().HP_becameZero();
        }
    }



    /*
    [PunRPC]
    void InstantiateSkill2(string skillName)
    {
        if (skillName.Contains("Shield"))
        {
            Instantiate(skill5, PurigonTr);
            Debug.Log("Skill instantiated");// 's Parent: " + skillMade.transform.parent.name);
            shieldClone = skill5;
            pllut -= 50;
            IsShield = true;
        }
    }
    */

    [PunRPC]
    void InstantiateSkill(string skillName, Vector3 position)
    {
        GameObject skillMade = PhotonNetwork.Instantiate(skillName, position, Quaternion.identity, 0);

        if (skillName.Contains("Shield"))
        {
            skillMade.transform.parent = PurigonTr;
            shieldClone = skillMade;
            pllut -= 50;
            IsShield = true;
        }
        else if (skillName.Contains("AfterImage"))
        {
            skillMade.transform.parent = transform;
            afterimage_clone = skillMade;
            StartCoroutine(Dash_Effect());
        }


        else if (skillName.Contains("B4_Black_hole"))
        {
            Destroy(skillMade, 2.0f);
        }
        else if (skillName.Contains("H4_Recovery_Hp"))
        {
            skillMade.transform.parent = transform;
            Destroy(skillMade, 2.0f);
        }
        else if (skillName.Contains("Collider_S4_Egg"))
        {
            skillMade.transform.parent = transform;
            Destroy(skillMade, 5.0f);
        }




        else if (skillName.Contains("B3_Big_Frost_Ball"))
        {
            Destroy(skillMade, 1.0f);
        }
        else if (skillName.Contains("L3_Push"))
        {
            skillMade.transform.parent = transform;
            Destroy(skillMade, 1.0f);
        }
        else if (skillName.Contains("Collider_H3_Weight"))
        {
            /*
           * foreach(PhotonPlayer pl in PhotonNetwork.playerList){
           * if(pl.gameObject.getComponent<InstantiateUserPurigon>().CurrentuserID != PlayerPrefs.GetString("LoginID", "Default ID"))
                   본인 제외 모든 유저
           * }
           */
            //지금은 임시로 해당 플레이어에게 effect를 주지만 다른 플레이어들을 추가한 후 다른 플레이어의 transform으로 수정
            skillMade.transform.parent = transform;
            Destroy(skillMade, 2.0f);
        }
        else if (skillName.Contains("Collider_S3_Fog"))
        {
            skillMade.transform.parent = transform;
            Destroy(skillMade, 4.0f);
        }



        else if (skillName.Contains("B1_Frost_Ball"))
        {
            frostBallClone = skillMade;
            Destroy(skillMade, 1.0f);
        }
        else if (skillName.Contains("L1_Life_Ball"))
        {
            skillMade.transform.parent = transform;
            Invoke("Delete_L1", 3.0f);
            Destroy(skillMade, 2.5f);
        }
        else if (skillName.Contains("H1_Forward_Attack"))
        {
            skillMade.transform.parent = transform;
        }
        else if (skillName.Contains("S1_Absorbed_Hp"))
        {
            skillMade.transform.parent = transform;
            Destroy(skillMade, 2.0f);
        }


        else if (skillName.Contains("Ghost_Skill"))
        {

            ghost_skill_Clone = skillMade;
            ghost_skill_Clone.transform.parent = transform;
        }
        else if (skillName.Contains("Transform_Ghost"))
        {
            skillMade.transform.parent = PurigonTr;
            Destroy(skillMade, 2.0f);
        }


    }
    void Delete_L1()
    {
        IsDamaged_L1 = false;
        this.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
    }


    public void IfSkillClicked()
    {
        if (IsDead) return;
        if (IsSkilled) return;
        if (IsGhost)
        {
            //4초간 겹쳐져 있는 타 플레이어의 체력 20%를 빼앗음
            //타 플레이어가 부딪히면 스킬 사용 취소
            IsSkilled = true;
            StartCoroutine(Ghost_Skill());
            return;
        }
        if (pllut >= 50 && IsShield == false && IsDash == false && IsEggMode == false)    //Level5 skill
        {
            GetAudioClips("Shield");
            //GameObject shield = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/Shield", PurigonTr.position, Quaternion.identity, 0);
            pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/Shield", PurigonTr.position);
            //pv.RPC("InstantiateSkill2", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/Shield");
        }

        else if (pllut >= 40 && IsDash == false && IsEggMode == false)   //Level4 skill
        {
            if (IsDead) return;
            if (gameObject.tag == "BalanceType")
            {
                IsSkilled = true;
                GetAudioClips("B4");
                //GameObject black_hole = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/B4_Black_hole", PurigonTr.position, Quaternion.identity, 0);
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/B4_Black_hole", PurigonTr.position);
            }
            else if (gameObject.tag == "LightType")
            {
                IsSkilled = true;
                GetAudioClips("L4");
                StartCoroutine(Eagle_Effect());
            }
            else if (gameObject.tag == "PhysicalType")
            {
                IsSkilled = true;
                GetAudioClips("H4");
                CharHP += 20;
                if (CharHP > maxHP) CharHP = maxHP;

                //GameObject recovery_hp = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/H4_Recovery_Hp", PurigonTr.position, Quaternion.identity, 0);
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/H4_Recovery_Hp", PurigonTr.position);
            }
            else if (gameObject.tag == "SpeedType")
            {
                IsSkilled = true;
                GetAudioClips("S4");
                Vector3 t = new Vector3(PurigonTr.position.x + 5, PurigonTr.position.y, PurigonTr.position.z);
                //GameObject collider_helper4 = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/Collider_S4_Egg", t, Quaternion.identity, 0);
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/Collider_S4_Egg", t);

            }
            pllut -= 40;
        }

        else if (pllut >= 30 && IsDash == false && IsEggMode == false)   //Level3 skill
        {
            IsSkilled = true;
            if (gameObject.tag == "BalanceType")
            {

                GetAudioClips("B3");
                //GameObject big_frost_ball = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/B3_Big_Frost_Ball", new Vector3(PurigonTr.position.x + 2.0f, PurigonTr.position.y, PurigonTr.position.z), Quaternion.identity, 0);
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/B3_Big_Frost_Ball", new Vector3(PurigonTr.position.x + 2.0f, PurigonTr.position.y, PurigonTr.position.z));

            }
            else if (gameObject.tag == "LightType")
            {
                GetAudioClips("L3");
                //GameObject push_Wave = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/L3_Push", PurigonTr.position, Quaternion.identity, 0);
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/L3_Push", PurigonTr.position);

            }
            else if (gameObject.tag == "PhysicalType")
            {
                GetAudioClips("H3");
                //자신을 제외한 모든 플레이어들의 무게 5 증가
                //GameObject gain_weight = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/Collider_H3_Weight", PurigonTr.position, Quaternion.identity, 0);
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/Collider_H3_Weight", PurigonTr.position);

            }
            else if (gameObject.tag == "SpeedType")
            {
                GetAudioClips("S3");
                //다른 플레이어들의 시야 방해
                //GameObject fog_attack = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/Collider_S3_Fog", PurigonTr.position, Quaternion.identity, 0);
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/Collider_S3_Fog", PurigonTr.position);
            }
            pllut -= 30;
        }

        else if (pllut >= 20 && IsDash == false && IsEggMode == false)  //Level2 skill
        {
            IsSkilled = true;
            GetAudioClips("Dash");
            IsDash = true;
            //GameObject afterimage = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/AfterImage", PurigonTr.position, Quaternion.identity, 0);
            pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/AfterImage", PurigonTr.position);


        }

        else if (pllut >= 10 && IsDash == false && IsEggMode == false)   //Level1 skill
        {
            if (gameObject.tag == "BalanceType")
            {
                GetAudioClips("B1");
                //GameObject frost_ball = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/B1_Frost_Ball", new Vector3(PurigonTr.position.x + 1.0f, PurigonTr.position.y, PurigonTr.position.z), Quaternion.identity, 0);
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/B1_Frost_Ball", new Vector3(PurigonTr.position.x + 1.0f, PurigonTr.position.y, PurigonTr.position.z));

            }
            else if (gameObject.tag == "LightType")
            {
                GetAudioClips("L1");
                //GameObject life_ball = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/L1_Life_Ball", PurigonTr.position, Quaternion.identity, 0);
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/L1_Life_Ball", PurigonTr.position);
            }
            else if (gameObject.tag == "PhysicalType")
            {
                GetAudioClips("H1");
                //자신을 제외한 근접한 모든 플레이어 데미지 20
                //GameObject melee_attack = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/H1_Forward_Attack", PurigonTr.position, Quaternion.identity, 0);
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/H1_Forward_Attack", PurigonTr.position);
            }
            else if (gameObject.tag == "SpeedType")
            {
                GetAudioClips("S1");
                //근처에 다른 플레이어가 있을 경우 제일 가까운 유저의 hp를 15흡수
                //GameObject absorbed_hp = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/S1_Absorbed_Hp", PurigonTr.position, Quaternion.identity, 0)
                pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/S1_Absorbed_Hp", PurigonTr.position);
            }
            pllut -= 10;
        }
        else return;
        IsSkilled = false;
    }


    public void GetAudioClips(string SkillName)
    {

        switch (SkillName)
        {
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
        GameObject eagle = PhotonNetwork.Instantiate("MyPrefabs/Effect/Skill_Effect/L4_Eagle", PurigonTr.position, Quaternion.identity, 0);
        eagle.transform.parent = transform;
        weight -= 1;
        yield return new WaitForSeconds(6);
        weight += 1;
        Destroy(eagle);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Purigon"))
            IsCollisionWithPlayer = true;
        else
            return;
        anotherPlayer = coll;
        InvokeRepeating("CheckCollision(coll)", 1, 1);

    }
    void CheckCollision(Collision coll)
    {
        //상대방이 부딪힌 경우
        if (coll.collider.GetComponent<NETPurigonCtrl>().IsCollision == true)
        {
            //스킬 작동 중단
            Destroy(ghost_skill_Clone);
            anotherPlayer = null;
            CancelInvoke("CheckCollision(coll)");
        }
    }
    IEnumerator Ghost_Skill()
    {
        //상대 플레이어와 붙어있지 않다면 break;
        //if (IsCollisionWithPlayer == false)
        //    yield break;

        pv.RPC("InstantiateSkill", PhotonTargets.All, "MyPrefabs/Effect/Skill_Effect/Ghost_Skill", PurigonTr.position);
        yield return new WaitForSeconds(4);
        Destroy(ghost_skill_Clone);
        smr.material.SetTexture("_MainTex", purigonSprite.texture);
        //CharHP += anotherPlayer.collider.GetComponent<NETPurigonCtrl>().CharHP/5;//상대방의 체력의 20%    
        IsGhost = false;
        Debug.Log(IsGhost);
        IsSkilled = false;
        anotherPlayer = null;
        CancelInvoke("CheckCollision(coll)");
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