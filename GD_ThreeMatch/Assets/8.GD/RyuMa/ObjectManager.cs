﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;



public class ObjectPool
{
    public GameObject Ori;
    public Queue<GameObject> OirQueue;
    public List<GameObject> OriList;
}



public class ObjectManager : G_Singleton<ObjectManager>
{






    [Header("UI")]
    public GameObject WorldCanvasObj;


    public List<ObjectPool> ObjectList;




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

    [HideInInspector] public Queue<GameObject> SlimeSkillAttacks = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> SlimeSkillAttackList;

    [HideInInspector] public Queue<GameObject> SlimeSkillHits = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> SlimeSkillHitList;

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

    //[HideInInspector] public Queue<GameObject> PortalArrows = new Queue<GameObject>();
    //[HideInInspector] public List<GameObject> PortalArrowList;

    [HideInInspector] public Queue<GameObject> PoisonSlimeSkills = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> PoisonSlimeSkillList;

    [HideInInspector] public Queue<GameObject> PoisonSlimePs = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> PoisonSlimePList;

    [HideInInspector] public Queue<GameObject> BlockBreaks = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> BlockBreakList;

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
    public GameObject SlimeSkillAttack;
    public GameObject SlimeSkillHit;
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

    public GameObject BlockBreak;

    Queue<GameObject> ObjectQueue = new Queue<GameObject>();
    Vector2 SpawnVec = new Vector2(100, 0);


    private PuzzleManager thePuzzle;
    private SoundManager theSound;
    // Start is called before the first frame update
    void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theSound = FindObjectOfType<SoundManager>();
        ObjectPool();
        LoadingInit();
        
    }



    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < 200; i++)
            {
                Vector2 vec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                AliceSkillEvent(vec);
            }
        }
    }


    public void ObjectPool()
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
            GameObject x = Instantiate(SlimeSkillHit);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            SlimeSkillHits.Enqueue(x);
            SlimeSkillHitList.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(SlimeSkillHit);
            x.transform.position = SpawnVec;
            x.SetActive(false);
            SlimeSkillHits.Enqueue(x);
            SlimeSkillHitList.Add(x);
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

        DamageTextEvent(this.transform.position, 999,true, 1);


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
 

        if (Frefab == null)
        {
            Debug.LogError(_Name + " 은 없습니다");
        }

        GameObject Obj = null;

        while (ObjectQueue.Count > 0)
        {

            Obj = ObjectQueue.Dequeue();
            if (Obj != null)
            {
                if (_Active == true)
                    Obj.SetActive(true);
                return Obj;
            }
           
        }


        //만약 모든 리스트에 오브젝트들이 활성화되어있다면 오브젝트를 추가하고 넣는다
        Obj = Instantiate(Frefab);
        Obj.SetActive(_Active);

        return Obj;
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

    public GameObject SpawnBlockBreak(Vector2 _Vec)
    {
        GameObject Break = FindObj("BlockBreak");
        Break.transform.position = _Vec;
        Break.GetComponent<ParticleManager>().ParticleSetting(false,null,1f);
        return Break;
    }


    public GameObject SpawnPortal(Vector2 StartVec)
    {
        GameObject Portal = FindObj("Portal");
        Portal.transform.position = StartVec;
        Portal.GetComponent<ParticleManager>().ParticleSetting(true);
        return Portal;
    }




    public GameObject SpawnSelectSlot(Vector2 StartVec)
    {
        SelectSlotP.SetActive(true);
        SelectSlotP.transform.position = StartVec;
        return SelectSlotP;
    }

    //public GameObject PortalArrowEvent(Vector2 _PortalVec)
    //{
    //    GameObject Arrow = FindObj("PortalArrow");
    //    Arrow.GetComponent<PortalArrowManager>().SetPortalArrow(_PortalVec);
    //    return Arrow;
    //}

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


    public GameObject DamageTextEvent(Vector2 _startPos, int _Value, bool EnemyHit= true,float _Time = 1.5f)
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
            AliceObj.transform.position = new Vector2(_StartPos.x + Random.Range(-0.1f, 0.1f),
                _StartPos.y + Random.Range(-0.1f, 0.1f));
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
            BerylObj.transform.position = new Vector2(_StartPos.x + Random.Range(-0.15f,0.15f),
                _StartPos.y + Random.Range(-0.15f, 0.15f));
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
        for (int i = 0; i < SlimeSkillAttackList.Count; i++)
        {
            if (SlimeSkillAttackList[i].activeSelf)
            {
                SlimeSkillAttackList[i].GetComponent<ParticleManager>().Resetting();
            }
        }
        
        for (int i = 0; i < SlimeSkillHitList.Count; i++)
        {
            if (SlimeSkillHitList[i].activeSelf)
            {
                SlimeSkillHitList[i].GetComponent<ParticleManager>().Resetting();
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
