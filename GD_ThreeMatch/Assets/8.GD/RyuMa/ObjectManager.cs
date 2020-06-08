
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    // 오브젝트 타일 이미지
    public Sprite[] SlotPanelSprite;
    public Sprite EnemySlotSprite;
    public Sprite PortalSlotSprite;
    public Sprite[] ForestSprites;



    //게임오브젝트 리스트
    [HideInInspector] public List<GameObject> Cubes; //큐브 리스트
    [HideInInspector] public List<GameObject> CubeParticles;
    [HideInInspector] public List<GameObject> CubeEfs;
    [HideInInspector] public List<GameObject> SpeechBubbles;
    [HideInInspector] public List<GameObject> AttackEffects;
    [HideInInspector] public List<GameObject> DamageTexts;
    [HideInInspector] public List<GameObject> AliceSkills;
    [HideInInspector] public List<GameObject> AliceAnimEffects;
    [HideInInspector] public List<GameObject> SlimeSkillParticles;
    [HideInInspector] public List<GameObject> SlimeAttackParticles;
    [HideInInspector] public List<GameObject> SlotPanels;
    [HideInInspector] public List<GameObject> ClickParticles;


    //게임오브젝트 프리팹
    public GameObject Cube; //큐브 프리팹
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

    private PuzzleManager thePuzzle;
    // Start is called before the first frame update
    void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();

        Init();
        LoadingInit();


    }



    public void Init()
    {
        for (int i = 0; i < 400; i++)
        {
            GameObject x = Instantiate(Cube);
            x.SetActive(false);
            Cubes.Add(x);
        }
        for (int i = 0; i < 50; i++)
        {
            GameObject x = Instantiate(CubeParticle);
            x.SetActive(false);
            CubeParticles.Add(x);
        }
        for (int i = 0; i < 100; i++)
        {
            GameObject x = Instantiate(CubeEf);
            x.SetActive(false);
            CubeEfs.Add(x);
        }
        for (int i = 0; i < 5; i++)
        {
            GameObject x = Instantiate(SpeechObj);
            x.SetActive(false);
            SpeechBubbles.Add(x);
        }
        for (int i = 0; i < 15; i++)
        {
            GameObject x = Instantiate(AttackEffect);
            x.SetActive(false);
            AttackEffects.Add(x);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject x = Instantiate(DamageText);
            x.SetActive(false);
            DamageTexts.Add(x);
        }
        for (int i = 0; i < 30; i++)
        {
            GameObject x = Instantiate(AliceSkill);
            x.SetActive(false);
            AliceSkills.Add(x);
        }
        for (int i = 0; i < 2; i++)
        {
            GameObject x = Instantiate(AliceAnimEffect);
            x.SetActive(false);
            AliceAnimEffects.Add(x);
        }
        for (int i = 0; i < 20; i++)
        {
            GameObject x = Instantiate(SlimeSkillParticle);
            x.SetActive(false);
            SlimeSkillParticles.Add(x);
        }
        for (int i = 0; i < 20; i++)
        {
            GameObject x = Instantiate(SlimeAttackParticle);
            x.SetActive(false);
            SlimeAttackParticles.Add(x);
        }
        for (int i = 0; i < 200; i++)
        {
            GameObject x = Instantiate(SlotPanel);
            x.SetActive(false);
            SlotPanels.Add(x);
        }
        for (int i = 0; i < 2; i++)
        {
            GameObject x = Instantiate(ClickParticle);
            x.SetActive(false);
            ClickParticles.Add(x);
        }

        //ClickParticles
        //SlotPanel

    }


    // 로딩중에 오브젝트를 한번 잡는다
    public void LoadingInit()
    {
        GameObject Cube = CubeEffectEvent(this.gameObject.transform.position,
            this.gameObject, NodeColor.NC0_Blue, CubeEffectType.GoEnemy, 0, false, 2000);
        Cube.GetComponent<CubeEffect>().DestroyCount = 1;

        SpeechEvent(this.transform.position, "test", 1);
        CubeParticleEvent(this.transform.position);
        GameObject AttackEffect = AttackEffectEvent(this.transform.position, this.gameObject, 0, 0, false, AttackEffectType.ET0_Null);
        AttackEffect.GetComponent<AttackEffect>().DestroyCount = 1;

        DamageTextEvent(this.transform.position, "test", 1);


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
        List<GameObject> List = null;
        GameObject Frefab = null;
        switch (_Name)
        {
            case "Cube":
                List = Cubes;
                Frefab = Cube;
                break;
            case "CubeP":
                List = CubeParticles;
                Frefab = CubeParticle;
                break;
            case "CubeE":
                List = CubeEfs;
                Frefab = CubeEf;
                break;
            case "Speech":
                List = SpeechBubbles;
                Frefab = SpeechObj;
                break;
            case "AttackEffect":
                List = AttackEffects;
                Frefab = AttackEffect;
                break;
            case "DamageText":
                List = DamageTexts;
                Frefab = DamageText;
                break;
            case "AliceSkill":
                List = AliceSkills;
                Frefab = AliceSkill;
                break;
            case "AliceAnimEffect":
                List = AliceAnimEffects;
                Frefab = AliceAnimEffect;
                break;
            case "SlimeP":
                List = SlimeAttackParticles;
                Frefab = SlimeAttackParticle;
                break;
            case "SlimeSkillParticle":
                List = SlimeSkillParticles;
                Frefab = SlimeSkillParticle;
                break;
            case "SlotPanel":
                List = SlotPanels;
                Frefab = SlotPanel;
                break;
            case "ClickParticle":
                List = ClickParticles;
                Frefab = ClickParticle;
                break;

        }


        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].activeSelf == false)
            {
                List[i].SetActive(_Active);
                return List[i];
            }
        }

        //만약 모든 리스트에 오브젝트들이 활성화되어있다면 오브젝트를 추가하고 넣는다
        GameObject X = Instantiate(Frefab);
        X.SetActive(_Active);
        List.Add(X);

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

    public GameObject SpawnSlotPanel(Vector2 Pos,SlotObjectSheet _Sheet, MapType _mapType, int _SlotNum)
    {
        GameObject Slot = FindObj("SlotPanel");
        Slot.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        Slot.GetComponent<SlotObject>().SetSlotObject(Pos,_Sheet, _mapType, _SlotNum);
        return Slot;
    }


    // 큐브 이펙트를 사용하는 함수
    public GameObject CubeEffectEvent(Vector2 _StartVec, GameObject _Target,NodeColor _NodeColor,
        CubeEffectType _CubeTarget, int _CubeCount, bool _RandStart,float _Speed = 2000)
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

    public GameObject CubeParticleEvent(Vector2 TargetVec)
    {
        GameObject Paricle = FindObj("CubeP", false);
        Paricle.transform.position = TargetVec;
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

        Effect.GetComponent<AttackEffect>()
            .SetCubeEffect(StartVec, _TargetVec , _DamageValue,
            _EffectNum, _AttackEvent,
            AttackType, _Speed);

        GameObject EffectP = null;

        switch (_EffectNum)
        {
            case 0:
                EffectP = FindObj("SlimeSkillParticle");
                break;
        }
        EffectP.GetComponent<ParticleManager>().ParticleSetting(true, Effect, 10);


        return Effect;

    }


    public GameObject DamageTextEvent(Vector2 _startPos, string _Value,float _Time = 1.5f)
    {
        GameObject TextOBJ = FindObj("DamageText");

        TextOBJ.GetComponent<DamageText>().SetDamageText(_startPos, _Value, _Time);


        return TextOBJ;
    }

    public GameObject AliceSkillEvent(Vector2 _StartPos)
    {
        GameObject AliceObj = FindObj("AliceSkill",false);
        AliceObj.transform.position = _StartPos;
        AliceObj.transform.localScale = new Vector3(1, 1, 1);
        AliceObj.SetActive(true);
        AliceObj.GetComponent<ParticleManager>().ParticleSetting(false,
            null, 1f);

        if (thePuzzle.Player.GirlEffect == true)
        {
            GameObject AliceObj2 = FindObj("AliceSkill", false);
            AliceObj2.transform.position = _StartPos;
            AliceObj2.transform.localScale = new Vector3(-1, 1, 1);
            AliceObj2.SetActive(true);
            AliceObj2.GetComponent<ParticleManager>().ParticleSetting(false,
                null, 1f);
        }

        return AliceObj;
    }

    public GameObject AliceAnimEvent(Vector2 _StartPos, Direction _Dir)
    {
        GameObject AliceAnim = FindObj("AliceAnimEffect", false);

        AliceAnim.transform.position = new Vector2(_StartPos.x, _StartPos.y +0.1f);
        if (_Dir == Direction.Left)
            AliceAnim.transform.localScale = new Vector3(-2f, 2f, 1);
        else
            AliceAnim.transform.localScale = new Vector3(2f, 2f, 1);

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


    public void ResettingObj()
    {
        for(int i = 0; i < Cubes.Count; i++)
        {
            if(Cubes[i].activeSelf)
            {
                Cubes[i].GetComponent<Cube>().Resetting();
            }
        }

        for(int i = 0; i < CubeParticles.Count; i++)
        {
            if(CubeParticles[i].activeSelf)
            {
                CubeParticles[i].GetComponent<ParticleManager>().Resetting();
            }
        }

        for (int i = 0; i < CubeEfs.Count; i++)
        {
            if (CubeEfs[i].activeSelf)
            {
                CubeEfs[i].GetComponent<CubeEffect>().Resetting();
            }
        }

        for (int i = 0; i < SpeechBubbles.Count; i++)
        {
            if (SpeechBubbles[i].activeSelf)
            {
                SpeechBubbles[i].GetComponent<SpeechBubble>().Resetting();
            }
        }

        for (int i = 0; i < AttackEffects.Count; i++)
        {
            if (AttackEffects[i].activeSelf)
            {
                AttackEffects[i].GetComponent<AttackEffect>().Resetting();
            }


        }
        for (int i = 0; i < DamageTexts.Count; i++)
        {
            if (DamageTexts[i].activeSelf)
            {
                DamageTexts[i].GetComponent<DamageText>().Resetting();
            }


        }
        for (int i = 0; i < AliceSkills.Count; i++)
        {
            if (AliceSkills[i].activeSelf)
            {
                AliceSkills[i].GetComponent<ParticleManager>().Resetting();
            }
        }

        for (int i = 0; i < AliceAnimEffects.Count; i++)
        {
            if (AliceAnimEffects[i].activeSelf)
            {
                AliceAnimEffects[i].GetComponent<ParticleManager>().Resetting();
            }
        }
        for (int i = 0; i < SlimeSkillParticles.Count; i++)
        {
            if (SlimeSkillParticles[i].activeSelf)
            {
                SlimeSkillParticles[i].GetComponent<ParticleManager>().Resetting();
            }
        }
        
        for (int i = 0; i < SlimeAttackParticles.Count; i++)
        {
            if (SlimeAttackParticles[i].activeSelf)
            {
                SlimeAttackParticles[i].GetComponent<ParticleManager>().Resetting();
            }
        }

        for (int i = 0; i < SlotPanels.Count; i++)
        {
            if (SlotPanels[i].activeSelf == true)
            {
                SlotPanels[i].GetComponent<SlotObject>().Resetting();
            }
        }


        for (int i = 0; i < ClickParticles.Count; i++)
        {
            if (ClickParticles[i].activeSelf == true)
            {
                ClickParticles[i].GetComponent<ParticleManager>().Resetting();
            }
        }


    }


}
