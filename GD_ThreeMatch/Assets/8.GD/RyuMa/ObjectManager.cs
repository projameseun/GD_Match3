
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class ObjectManager : MonoBehaviour
{

    // 오브젝트 타일 이미지
    [Header("Sprite")]
    public Sprite[] SlotPanelSprite;
    public Sprite EnemySlotSprite;
    public Sprite[] ForestSprites;


    [Header("UI")]
    public GameObject WorldCanvasObj;

    //게임오브젝트 리스트
    [HideInInspector] public Queue<GameObject> Cubes = new Queue<GameObject>(); //큐브 리스트
    [HideInInspector] public List<GameObject> CubeList;

    [HideInInspector] public Queue<GameObject> CubeParticles = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> CubeParticleList;

    [HideInInspector] public Queue<GameObject> CubeEfs = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> CubeEfList;

    [HideInInspector] public Queue<GameObject> SpeechBubbles = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> SpeechBubbleList;

    [HideInInspector] public Queue<GameObject> AttackEffects = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> AttackEffectList;

    [HideInInspector] public Queue<GameObject> DamageTexts = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> DamageTextList;

    [HideInInspector] public Queue<GameObject> AliceSkills = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> AliceSkillList;

    [HideInInspector] public Queue<GameObject> AliceAnimEffects = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> AliceAnimEffectList;

    [HideInInspector] public Queue<GameObject> SlimeSkillParticles = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> SlimeSkillParticleList;

    [HideInInspector] public Queue<GameObject> SlimeAttackParticles = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> SlimeAttackParticleList;

    [HideInInspector] public Queue<GameObject> SlotPanels = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> SlotPanelList;

    [HideInInspector] public Queue<GameObject> ClickParticles = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> ClickParticleList;

    [HideInInspector] public Queue<GameObject> Portals = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> PortalList;

    [HideInInspector] public Queue<GameObject> EnemySkulls = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> EnemySkullList;

    [HideInInspector] public Queue<GameObject> CubeEffectBlues = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> CubeEffectBlueList;

    [HideInInspector] public Queue<GameObject> CubeEffectYellows = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> CubeEffectYellowList;

    [HideInInspector] public Queue<GameObject> CubeEffectGreens = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> CubeEffectGreenList;

    [HideInInspector] public Queue<GameObject> CubeEffectPinks = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> CubeEffectPinkList;

    [HideInInspector] public Queue<GameObject> CubeEffectReds = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> CubeEffectRedList;

    [HideInInspector] public Queue<GameObject> BerylSkills = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> BerylSkillList;

    [HideInInspector] public Queue<GameObject> PortalArrows = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> PortalArrowList;

    [HideInInspector] public Queue<GameObject> PoisonSlimeSkills = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> PoisonSlimeSkillList;

    [HideInInspector] public Queue<GameObject> PoisonSlimePs = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> PoisonSlimePList;

    [Header("Prefab")]

    //게임오브젝트 프리팹
    public GameObject CubePrefab; //큐브 프리팹
    public GameObject CubeParticle;
    public GameObject CubeEf;
    public GameObject SpeechObj;
    public GameObject AttackEffect;
    public GameObject DamageText;
    public GameObject AliceSkill;
    public GameObject AliceAnimEffect;
    public GameObject SlimeSkillParticle;
    public GameObject SlimeAttackParticle;
    public GameObject SlotPanel;
    public GameObject ClickParticle;
    public GameObject Portal;
    //public GameObject ObjectSpine;
    public GameObject EnemySkull;
    public GameObject SelectSlotP;

    public GameObject CubeEffectBlue;
    public GameObject CubeEffectGreen;
    public GameObject CubeEffectRed;
    public GameObject CubeEffectPink;
    public GameObject CubeEffectYellow;
    public GameObject BerylSkill;
    public GameObject PortalArrow;
    public GameObject PoisonSlimeSkill;
    public GameObject PoisonSlimeP;

    Queue<GameObject> ObjectQueue = new Queue<GameObject>();
    List<GameObject> ObjectList = new List<GameObject>();
    Vector2 SpawnVec = new Vector2(100, 0);


    private PuzzleManager thePuzzle;
    private SoundManager theSound;
    // Start is called before the first frame update
    void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theSound = FindObjectOfType<SoundManager>();
        Init();
        LoadingInit();
        
    }



    public void Init()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject x = Instantiate(CubePrefab);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            Cubes.Enqueue(x);
            CubeList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(CubeParticle);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            CubeParticles.Enqueue(x);
            CubeParticleList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(CubeEf);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            CubeEfs.Enqueue(x);
            CubeEfList.Add(x);
        }
        for (int i = 0; i < 5; i++)
        {
            GameObject x = Instantiate(SpeechObj);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            SpeechBubbles.Enqueue(x);
            SpeechBubbleList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(AttackEffect);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            AttackEffects.Enqueue(x);
            AttackEffectList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(DamageText);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            DamageTexts.Enqueue(x);
            DamageTextList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(AliceSkill);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            AliceSkills.Enqueue(x);
            AliceSkillList.Add(x);
        }
        for (int i = 0; i < 1; i++)
        {
            GameObject x = Instantiate(AliceAnimEffect);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            AliceAnimEffects.Enqueue(x);
            AliceAnimEffectList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(SlimeSkillParticle);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            SlimeSkillParticles.Enqueue(x);
            SlimeSkillParticleList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(SlimeAttackParticle);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            SlimeAttackParticles.Enqueue(x);
            SlimeAttackParticleList.Add(x);
        }
        for (int i = 0; i < 50; i++)
        {
            GameObject x = Instantiate(SlotPanel);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            SlotPanels.Enqueue(x);
            SlotPanelList.Add(x);
        }
        for (int i = 0; i < 2; i++)
        {
            GameObject x = Instantiate(ClickParticle);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            ClickParticles.Enqueue(x);
            ClickParticleList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(Portal);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            Portals.Enqueue(x);
            PortalList.Add(x);
        }
        //for (int i = 0; i < 10; i++)
        //{
        //    GameObject x = Instantiate(ObjectSpine);
        //    x.transform.position = SpawnVec;
        //    x.SetActive(false);
        //    ObjectSpines.Add(x);
        //}
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(EnemySkull);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            EnemySkulls.Enqueue(x);
            EnemySkullList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(CubeEffectBlue);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            CubeEffectBlues.Enqueue(x);
            CubeEffectBlueList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(CubeEffectGreen);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            CubeEffectGreens.Enqueue(x);
            CubeEffectGreenList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(CubeEffectYellow);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            CubeEffectYellows.Enqueue(x);
            CubeEffectYellowList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(CubeEffectPink);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            CubeEffectPinks.Enqueue(x);
            CubeEffectPinkList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(CubeEffectRed);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            CubeEffectReds.Enqueue(x);
            CubeEffectRedList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(BerylSkill);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            BerylSkills.Enqueue(x);
            BerylSkillList.Add(x);
        }
        //ClickParticles
        //SlotPanel

    }


    // 로딩중에 오브젝트를 한번 잡는다
    public void LoadingInit()
    {
        GameObject Cube = CubeEffectEvent(this.gameObject.transform.position,
            this.gameObject, NodeColor.NC0_Blue, CubeEffectType.GoEnemy, 0, false);
        Cube.GetComponent<CubeEffect>().DestroyCount = 1;

        SpeechEvent(this.transform.position, "test", 1);
      
        
        GameObject AttackEffect = AttackEffectEvent(this.transform.position, this.gameObject, 0, 0, false, AttackEffectType.ET0_Null);
        AttackEffect.GetComponent<AttackEffect>().DestroyCount = 1;

        DamageTextEvent(this.transform.position, "test",true, 1);


        for (int i = 0; i < 10; i++)
        {

            CubeParticleEvent(this.transform.position, NodeColor.NC2_Pink);
        }



        for (int i = 0; i < 10; i++)
        {
            AliceSkillEvent(this.transform.position);
        }

        for (int i = 0; i < 10; i++)
        {
            SlimePEvent(this.transform.position);
        }

    }



    //사용하고싶은 오브젝트를 찾는 함수(오브젝트 이름, 오브젝트 활성화)
    public GameObject FindObj(string _Name, bool _Active = true)
    {
        GameObject Frefab = null;
        switch (_Name)
        {
            case "Cube":
                ObjectQueue = Cubes;
                ObjectList = CubeList;
                Frefab = CubePrefab;
                break;
            case "CubeP":
                ObjectQueue = CubeParticles;
                ObjectList = CubeParticleList;
                Frefab = CubeParticle;
                break;
            case "CubeE":
                ObjectQueue = CubeEfs;
                ObjectList = CubeEfList;
                Frefab = CubeEf;
                break;
            case "Speech":
                ObjectQueue = SpeechBubbles;
                ObjectList = SpeechBubbleList;
                Frefab = SpeechObj;
                break;
            case "AttackEffect":
                ObjectQueue = AttackEffects;
                ObjectList = AttackEffectList;
                Frefab = AttackEffect;
                break;
            case "DamageText":
                ObjectQueue = DamageTexts;
                ObjectList = DamageTextList;
                Frefab = DamageText;
                break;
            case "AliceSkill":
                ObjectQueue = AliceSkills;
                ObjectList = AliceSkillList;
                Frefab = AliceSkill;
                break;
            case "AliceAnimEffect":
                ObjectQueue = AliceAnimEffects;
                ObjectList = AliceAnimEffectList;
                Frefab = AliceAnimEffect;
                break;
            case "SlimeP":
                ObjectQueue = SlimeAttackParticles;
                ObjectList = SlimeAttackParticleList;
                Frefab = SlimeAttackParticle;
                break;
            case "SlimeSkillParticle":
                ObjectQueue = SlimeSkillParticles;
                ObjectList = SlimeSkillParticleList;
                Frefab = SlimeSkillParticle;
                break;
            case "SlotPanel":
                ObjectQueue = SlotPanels;
                ObjectList = SlotPanelList;
                Frefab = SlotPanel;
                break;
            case "ClickParticle":
                ObjectQueue = ClickParticles;
                ObjectList = ClickParticleList;
                Frefab = ClickParticle;
                break;
            case "Portal":
                ObjectQueue = Portals;
                ObjectList = PortalList;
                Frefab = Portal;
                break;
            case "EnemySkull":
                ObjectQueue = EnemySkulls;
                ObjectList = EnemySkullList;
                Frefab = EnemySkull;
                break;
            case "CubeBlue":
                ObjectQueue = CubeEffectBlues;
                ObjectList = CubeEffectBlueList;
                Frefab = CubeEffectBlue;
                break;
            case "CubeRed":
                ObjectQueue = CubeEffectReds;
                ObjectList = CubeEffectRedList;
                Frefab = CubeEffectRed;
                break;
            case "CubeYellow":
                ObjectQueue = CubeEffectYellows;
                ObjectList = CubeEffectYellowList;
                Frefab = CubeEffectYellow;
                break;
            case "CubePink":
                ObjectQueue = CubeEffectPinks;
                ObjectList = CubeEffectPinkList;
                Frefab = CubeEffectPink;
                break;
            case "CubeGreen":
                ObjectQueue = CubeEffectGreens;
                ObjectList = CubeEffectGreenList;
                Frefab = CubeEffectGreen;
                break;
            case "BerylSkill":
                ObjectQueue = BerylSkills;
                ObjectList = BerylSkillList;
                Frefab = BerylSkill;
                break;
            case "PortalArrow":
                ObjectQueue = PortalArrows;
                ObjectList = PortalArrowList;
                Frefab = PortalArrow;
                break;
            case "PoisonSkill":
                ObjectQueue = PoisonSlimeSkills;
                ObjectList = PoisonSlimeSkillList;
                Frefab = PoisonSlimeSkill;
                break;
            case "PoisonP":
                ObjectQueue = PoisonSlimePs;
                ObjectList = PoisonSlimePList;
                Frefab = PoisonSlimeP;
                break;
        }

        if (ObjectQueue.Count > 0)
        {
            GameObject Obj = ObjectQueue.Dequeue();
            if (_Active == true)
                Obj.SetActive(true);
            return Obj;
        }


        //만약 모든 리스트에 오브젝트들이 활성화되어있다면 오브젝트를 추가하고 넣는다
        GameObject X = Instantiate(Frefab);
        X.SetActive(_Active);
        ObjectList.Add(X);
        return X;
    }

    public GameObject SpawnClickP(Vector2 StartPos)
    {
        GameObject ClickP = FindObj("ClickParticle");
        ClickP.transform.position = StartPos;
        ClickP.GetComponent<ParticleManager>().ParticleSetting(true);
        return ClickP;
    }


    public GameObject SpawnCube()
    {
        GameObject Cube = FindObj("Cube");
        Cube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        return Cube;
    }

    public GameObject SpawnSlotPanel(Transform trans ,Vector2 Pos, SlotObjectSheet _Sheet, MapType _mapType, int _SlotNum)
    {
        //if (_Sheet == SlotObjectSheet.S_0_Spin)
        //{
        //    GameObject ObjectSpin = FindObj("ObjectSpine");
        //    ObjectSpin.transform.position = Pos;
        //    ObjectSpin.transform.SetParent(trans);
        //    if (_mapType == MapType.M1_MoveMap)
        //    {
        //        ObjectSpin.GetComponent<ObjectSpineManager>().SetObjectSpine(thePuzzle.theMoveMap.Slots[_SlotNum].SlotSheet.ObjectNum,
        //           thePuzzle.theMoveMap.Slots[_SlotNum].SlotSheet.SkinName);
        //    }
        //    else if (_mapType == MapType.M2_BattleMap)
        //    {
        //        ObjectSpin.GetComponent<ObjectSpineManager>().SetObjectSpine(0, "0");
        //    }

        //    return ObjectSpin;
        //}
        GameObject Slot = FindObj("SlotPanel");
        Slot.transform.position = Pos;
        Slot.GetComponent<SlotObject>().SetSlotObject(_Sheet, _mapType, _SlotNum);
        return Slot;
    }

    public GameObject SpawnPortal(Vector2 StartVec)
    {
        GameObject Portal = FindObj("Portal");
        Portal.transform.position = StartVec;
        Portal.GetComponent<ParticleManager>().ParticleSetting(true);
        return Portal;
    }


    public GameObject SpawnEnemySkull(Vector2 StartVec)
    {
        GameObject Skull = FindObj("EnemySkull");
        Skull.transform.position = StartVec;
        return Skull;
    }

    public GameObject SpawnSelectSlot(Vector2 StartVec)
    {
        SelectSlotP.SetActive(true);
        SelectSlotP.transform.position = StartVec;
        return SelectSlotP;
    }

    public GameObject PortalArrowEvent(Vector2 _PortalVec)
    {
        GameObject Arrow = FindObj("PortalArrow");
        Arrow.GetComponent<PortalArrowManager>().SetPortalArrow(_PortalVec);
        return Arrow;
    }

    // 큐브 이펙트를 사용하는 함수
    public GameObject CubeEffectEvent(Vector2 _StartVec, GameObject _Target,NodeColor _NodeColor,
        CubeEffectType _CubeTarget, int _CubeCount, bool _RandStart,float _Speed = 4000)
    {

        GameObject CubeEffect = FindObj("CubeE");
        CubeEffect.GetComponent<CubeEffect>().SetCubeEffect(_StartVec,
                   _Target,
                   _NodeColor, _CubeTarget, _CubeCount, _RandStart, _Speed
                   );



        return CubeEffect;

    }

    // 말풍선 이밴트를 사용하는 함수
    public GameObject SpeechEvent(Vector2 _StartVec, string _Speech, float _LifeTime)
    {
        GameObject Speech = FindObj("Speech");
        Speech.GetComponent<SpeechBubble>().SetSpeech(
            _StartVec, _Speech, _LifeTime);
        return Speech;

    }

    public GameObject CubeParticleEvent(Vector2 TargetVec,NodeColor _Nod)
    {
        GameObject Paricle = FindObj("CubeP", false);
        Paricle.transform.position = TargetVec;
        Paricle.GetComponent<HitParticle>().SetColor(_Nod);
        Paricle.GetComponent<ParticleManager>().ParticleSetting(false, null, 2);
        Paricle.SetActive(true);
        return Paricle;
    }

    public GameObject AttackEffectEvent(Vector2 StartVec, GameObject _TargetVec, int _DamageValue,
        int _EffectNum, bool _AttackEvent,
        AttackEffectType AttackType,
        float _Speed = 2000)
    {
        GameObject Effect = FindObj("AttackEffect");
        Effect.transform.position = StartVec;

        GameObject EffectP = null;

        Debug.Log(_EffectNum);
        switch (_EffectNum)
        {
            case 0:
                EffectP = FindObj("SlimeSkillParticle");
                break;
            case 1:
                EffectP = FindObj("PoisonSkill");
                break;
        }


        Effect.GetComponent<AttackEffect>()
            .SetCubeEffect(_TargetVec , _DamageValue,
            _EffectNum, _AttackEvent,
            AttackType, EffectP, _Speed);
        EffectP.GetComponent<ParticleManager>().ParticleSetting(true, Effect, 10);



        return Effect;

    }


    public GameObject DamageTextEvent(Vector2 _startPos, string _Value, bool EnemyHit= true,float _Time = 1.5f)
    {
        GameObject TextOBJ = FindObj("DamageText");
        TextOBJ.transform.SetParent(WorldCanvasObj.transform);
        TextOBJ.GetComponent<DamageText>().SetDamageText(_startPos, _Value, EnemyHit, _Time);


        return TextOBJ;
    }

    public GameObject AliceSkillEvent(Vector2 _StartPos)
    {
        if (thePuzzle.Player.GirlEffect == true)
        {
            StartCoroutine(AliceSkillExtra1(_StartPos));
        }
        else
        {
            GameObject AliceObj = FindObj("AliceSkill", false);
            AliceObj.transform.position = _StartPos;
            AliceObj.SetActive(true);
            AliceObj.transform.eulerAngles = new Vector3(0, 0, 0);
            AliceObj.GetComponent<ParticleManager>().ParticleSetting(false,
                null, 1f);
            theSound.PlaySE("AliceSkillHit");
            return AliceObj;
        }
        return null;
    }

    public GameObject BerylSkillEvent(Vector2 _StartPos)
    {
        if (thePuzzle.Player.GirlEffect == true)
        {
            StartCoroutine(BerylSkillExtra1(_StartPos));
        }
        else
        {
            GameObject BerylObj = FindObj("BerylSkill", false);
            BerylObj.transform.position = _StartPos;
            BerylObj.SetActive(true);
            BerylObj.transform.eulerAngles = new Vector3(0, 0, 0);
            BerylObj.GetComponent<ParticleManager>().ParticleSetting(false,
                null, 1f);
            theSound.PlaySE("BerylSkillHit");
            return BerylObj;
        }
        return null;
    }

    IEnumerator AliceSkillExtra1(Vector2 _StartPos)
    {
        int Count = 5;
        while (true)
        {
            GameObject AliceObj = FindObj("AliceSkill", false);
            AliceObj.transform.position = _StartPos;
            AliceObj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0.0f,360.0f));
            AliceObj.SetActive(true);
            AliceObj.GetComponent<ParticleManager>().ParticleSetting(false,
                null, 1f);
            theSound.PlaySE("AliceSkillHit");
            Count--;
           
              
            yield return new WaitForSeconds(0.1f);
            if (Count == 0)
            {
                thePuzzle.CubeEvent = true;
                break;
            }
        }


    }

    IEnumerator BerylSkillExtra1(Vector2 _StartPos)
    {
        int Count = 3;
        while (true)
        {
            GameObject BerylObj = FindObj("BerylSkill", false);
            BerylObj.transform.position = new Vector2(_StartPos.x + Random.Range(-0.1f,0.1f),
                _StartPos.y + Random.Range(-0.1f, 0.1f));
            BerylObj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0.0f, 360.0f));
            BerylObj.SetActive(true);
            BerylObj.GetComponent<ParticleManager>().ParticleSetting(false,
                null, 1f);
            Count--;
            theSound.PlaySE("BerylSkillHit");

            yield return new WaitForSeconds(0.2f);
            if (Count == 0)
            {
                thePuzzle.CubeEvent = true;
                break;
            }
        }


    }


    public GameObject AliceAnimEvent(Vector2 _StartPos, Direction _Dir)
    {
        GameObject AliceAnim = FindObj("AliceAnimEffect", false);

        int Size = 2;
        if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        {
            Size = 4;
        }
        AliceAnim.transform.position = new Vector2(_StartPos.x, _StartPos.y +0.2f);
        if (_Dir == Direction.Left)
            AliceAnim.transform.localScale = new Vector3(-Size, Size, 1);
        else
            AliceAnim.transform.localScale = new Vector3(Size, Size, 1);

        AliceAnim.SetActive(true);
        AliceAnim.GetComponent<ParticleManager>().ParticleSetting(false,
            null, 1f);

        return AliceAnim;
    }




    //SlimeAttackParticle
    public GameObject SlimePEvent(Vector2 TargetVec)
    {
        Debug.Log("Test");
        //SlimeAttackParticle
        GameObject Paricle = FindObj("SlimeP", false);

        Vector2 RandVec = TargetVec;
        float RandX = Random.Range(-0.5f, 0.5f);
        float RandY = Random.Range(-0.3f, 1f);
        RandVec.x += RandX;
        RandVec.y += RandY;
        Paricle.transform.position = RandVec;
        Paricle.GetComponent<ParticleManager>().ParticleSetting(false, null, 2);
        Paricle.SetActive(true);
        return Paricle;
    }
    public GameObject PosionSlimePEvent(Vector2 TargetVec)
    {
        //SlimeAttackParticle
        GameObject Paricle = FindObj("PoisonP", false);

        Vector2 RandVec = TargetVec;
        float RandX = Random.Range(-0.5f, 0.5f);
        float RandY = Random.Range(-0.3f, 1f);
        RandVec.x += RandX;
        RandVec.y += RandY;
        Paricle.transform.position = RandVec;
        Paricle.GetComponent<ParticleManager>().ParticleSetting(false, null, 2);
        Paricle.SetActive(true);
        return Paricle;
    }

    public void ResettingAllObj()
    {
        for (int i = 0; i < CubeList.Count; i++)
        {
            if(CubeList[i].activeSelf)
            {
                CubeList[i].GetComponent<Cube>().Resetting();
            }
        }

        for(int i = 0; i < CubeParticleList.Count; i++)
        {
            if(CubeParticleList[i].activeSelf)
            {
                CubeParticleList[i].GetComponent<ParticleManager>().Resetting();
            }
        }

        for (int i = 0; i < CubeEfList.Count; i++)
        {
            if (CubeEfList[i].activeSelf)
            {
                CubeEfList[i].GetComponent<CubeEffect>().Resetting();
            }
        }

        for (int i = 0; i < SpeechBubbleList.Count; i++)
        {
            if (SpeechBubbleList[i].activeSelf)
            {
                SpeechBubbleList[i].GetComponent<SpeechBubble>().Resetting();
            }
        }

        for (int i = 0; i < AttackEffectList.Count; i++)
        {
            if (AttackEffectList[i].activeSelf)
            {
                AttackEffectList[i].GetComponent<AttackEffect>().Resetting();
            }


        }
        for (int i = 0; i < DamageTextList.Count; i++)
        {
            if (DamageTextList[i].activeSelf)
            {
                DamageTextList[i].GetComponent<DamageText>().Resetting();
            }


        }
        for (int i = 0; i < AliceSkillList.Count; i++)
        {
            if (AliceSkillList[i].activeSelf)
            {
                AliceSkillList[i].GetComponent<ParticleManager>().Resetting();
            }
        }

        for (int i = 0; i < AliceAnimEffectList.Count; i++)
        {
            if (AliceAnimEffectList[i].activeSelf)
            {
                AliceAnimEffectList[i].GetComponent<ParticleManager>().Resetting();
            }
        }
        for (int i = 0; i < SlimeSkillParticleList.Count; i++)
        {
            if (SlimeSkillParticleList[i].activeSelf)
            {
                SlimeSkillParticleList[i].GetComponent<ParticleManager>().Resetting();
            }
        }
        
        for (int i = 0; i < SlimeAttackParticleList.Count; i++)
        {
            if (SlimeAttackParticleList[i].activeSelf)
            {
                SlimeAttackParticleList[i].GetComponent<ParticleManager>().Resetting();
            }
        }

        for (int i = 0; i < SlotPanelList.Count; i++)
        {
            if (SlotPanelList[i].activeSelf == true)
            {
                SlotPanelList[i].GetComponent<SlotObject>().Resetting();
            }
        }


        for (int i = 0; i < ClickParticleList.Count; i++)
        {
            if (ClickParticleList[i].activeSelf == true)
            {
                ClickParticleList[i].GetComponent<ParticleManager>().Resetting();
            }
        }

        for (int i = 0; i < PortalList.Count; i++)
        {
            if (PortalList[i].activeSelf == true)
            {
                PortalList[i].GetComponent<ParticleManager>().Resetting();
            }
        }

        for (int i = 0; i < CubeEffectBlueList.Count; i++)
        {
            if (CubeEffectBlueList[i].activeSelf == true)
            {
                CubeEffectBlueList[i].GetComponent<ParticleManager>().Resetting();
            }
        }
        for (int i = 0; i < CubeEffectRedList.Count; i++)
        {
            if (CubeEffectRedList[i].activeSelf == true)
            {
                CubeEffectRedList[i].GetComponent<ParticleManager>().Resetting();
            }
        }
        for (int i = 0; i < CubeEffectGreenList.Count; i++)
        {
            if (CubeEffectGreenList[i].activeSelf == true)
            {
                CubeEffectGreenList[i].GetComponent<ParticleManager>().Resetting();
            }
        }
        for (int i = 0; i < CubeEffectYellowList.Count; i++)
        {
            if (CubeEffectYellowList[i].activeSelf == true)
            {
                CubeEffectYellowList[i].GetComponent<ParticleManager>().Resetting();
            }
        }
        for (int i = 0; i < CubeEffectRedList.Count; i++)
        {
            if (CubeEffectRedList[i].activeSelf == true)
            {
                CubeEffectRedList[i].GetComponent<ParticleManager>().Resetting();
            }
        }

        //베릴 이펙트 없음

    }





}
