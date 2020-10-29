
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


[System.Serializable]
public class ObjectPool
{
    public GameObject Ori;
    public Queue<GameObject> OirQueue = new Queue<GameObject>();
    public List<GameObject> OriList = new List<GameObject>();

    public ObjectPool(GameObject _Ori)
    {
        Ori = _Ori;
    }

    public GameObject CreatePool()
    {
        GameObject Create = GameObject.Instantiate(Ori, Vector3.zero, Quaternion.identity);
        Create.name = Ori.name;
        OriList.Add(Create);
        return Create;
    }

}



public class ObjectManager : G_Singleton<ObjectManager>
{






    [Header("UI")]
    public GameObject WorldCanvasObj;


    public Dictionary<string,ObjectPool> PoolList = new Dictionary<string,ObjectPool>();




    //게임오브젝트 리스트
   

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
    public GameObject FindObj(GameObject _Obj, bool _Active = true)
    {
        GameObject Prefab = null;

        if (PoolList.ContainsKey(_Obj.name))
        {
            ObjectPool Pool = PoolList[_Obj.name];
            if (Pool.OirQueue.Count > 0)
            {
                Prefab = Pool.OirQueue.Dequeue();
                Prefab.SetActive(_Active);
                return Prefab;
            }
            else
            {
                Prefab = Pool.CreatePool();
                Prefab.SetActive(_Active);
                return Prefab;
            }
        }
        else
        {
            ObjectPool NewPool = new ObjectPool(_Obj);
            PoolList.Add(_Obj.name, NewPool);
            Prefab = NewPool.CreatePool();
            Prefab.SetActive(_Active);
            return Prefab;

        }


    }


    //오브젝트를 비활성화
    public void ResetObj(GameObject _Obj)
    {
        _Obj.transform.SetParent(this.transform);
        _Obj.SetActive(false);

        PoolList[_Obj.name].OirQueue.Enqueue(_Obj);

    }



    public GameObject SpawnClickP(Vector2 StartPos)
    {
        //GameObject ClickP = FindObj("ClickParticle");
        //ClickP.transform.position = StartPos;
        //ClickP.GetComponent<ParticleManager>().ParticleSetting(true);
        return null;
    }


    public GameObject SpawnCube()
    {
        //GameObject Cube = FindObj("Cube");
        //Cube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        return null;
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

        //GameObject CubeEffect = FindObj("CubeE");
        //CubeEffect.GetComponent<CubeEffect>().SetCubeEffect(_StartVec,
        //           _Target,
        //           _NodeColor, _CubeTarget, _CubeCount, _RandStart, _Speed
        //           );



        return null;

    }

    // 말풍선 이밴트를 사용하는 함수
    public GameObject SpeechEvent(Vector2 _StartVec, string _Speech, float _LifeTime)
    {
        //GameObject Speech = FindObj("Speech");
        //Speech.GetComponent<SpeechBubble>().SetSpeech(
        //    _StartVec, _Speech, _LifeTime);
        return null;

    }

    public GameObject CubeParticleEvent(Vector2 TargetVec,NodeColor _Nod)
    {
        //GameObject Paricle = FindObj("CubeP", false);
        //Paricle.transform.position = TargetVec;
        //Paricle.GetComponent<HitParticle>().SetColor(_Nod);
        //Paricle.GetComponent<ParticleManager>().ParticleSetting(false, null, 2);
        //Paricle.SetActive(true);
        return null;
    }

    public GameObject AttackEffectEvent(Vector2 StartVec, GameObject _TargetVec, int _DamageValue,
        int _EffectNum, bool _AttackEvent,
        AttackEffectType AttackType,
        float _Speed = 2000)
    {
        //GameObject Effect = FindObj("AttackEffect");
        //Effect.transform.position = StartVec;

        //GameObject EffectP = null;

        //switch (_EffectNum)
        //{
        //    case 0:
        //        EffectP = FindObj("SlimeSkillParticle");
        //        break;
        //    case 1:
        //        EffectP = FindObj("PoisonSkill");
        //        break;
        //}


        //Effect.GetComponent<AttackEffect>()
        //    .SetCubeEffect(_TargetVec , _DamageValue,
        //    _EffectNum, _AttackEvent,
        //    AttackType, EffectP, _Speed);
        //EffectP.GetComponent<ParticleManager>().ParticleSetting(true, Effect, 10);



        return null;

    }


    public GameObject DamageTextEvent(Vector2 _startPos, int _Value, bool EnemyHit= true,float _Time = 1.5f)
    {
        //GameObject TextOBJ = FindObj("DamageText");
        //TextOBJ.transform.SetParent(WorldCanvasObj.transform);
        //TextOBJ.GetComponent<DamageText>().SetDamageText(_startPos, _Value, EnemyHit, _Time);


        return null;
    }

    public GameObject AliceSkillEvent(Vector2 _StartPos)
    {
        //if (thePuzzle.Player.GirlEffect == true)
        //{
        //    StartCoroutine(AliceSkillExtra1(_StartPos));
        //}
        //else
        //{
        //    GameObject AliceObj = FindObj("AliceSkill", false);
        //    AliceObj.transform.position = _StartPos;
        //    AliceObj.SetActive(true);
        //    AliceObj.transform.eulerAngles = new Vector3(0, 0, 0);
        //    AliceObj.GetComponent<ParticleManager>().ParticleSetting(false,
        //        null, 1f);
        //    theSound.PlaySE("AliceSkillHit");
        //    return AliceObj;
        //}
        return null;
    }

    public GameObject BerylSkillEvent(Vector2 _StartPos)
    {
        //if (thePuzzle.Player.GirlEffect == true)
        //{
        //    StartCoroutine(BerylSkillExtra1(_StartPos));
        //}
        //else
        //{
        //    GameObject BerylObj = FindObj("BerylSkill", false);
        //    BerylObj.transform.position = _StartPos;
        //    BerylObj.SetActive(true);
        //    BerylObj.transform.eulerAngles = new Vector3(0, 0, 0);
        //    BerylObj.GetComponent<ParticleManager>().ParticleSetting(false,
        //        null, 1f);
        //    theSound.PlaySE("BerylSkillHit");
        //    return BerylObj;
        //}
        return null;
    }

    //IEnumerator AliceSkillExtra1(Vector2 _StartPos)
    //{
    //    int Count = 5;
    //    while (true)
    //    {
    //        GameObject AliceObj = FindObj("AliceSkill", false);
    //        AliceObj.transform.position = new Vector2(_StartPos.x + Random.Range(-0.1f, 0.1f),
    //            _StartPos.y + Random.Range(-0.1f, 0.1f));
    //        AliceObj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0.0f,360.0f));
    //        AliceObj.SetActive(true);
    //        AliceObj.GetComponent<ParticleManager>().ParticleSetting(false,
    //            null, 1f);
    //        theSound.PlaySE("AliceSkillHit");
    //        Count--;
           
              
    //        yield return new WaitForSeconds(0.1f);
    //        if (Count == 0)
    //        {
    //            thePuzzle.CubeEvent = true;
    //            break;
    //        }
    //    }


    //}

    //IEnumerator BerylSkillExtra1(Vector2 _StartPos)
    //{
    //    int Count = 3;
    //    while (true)
    //    {
    //        GameObject BerylObj = FindObj("BerylSkill", false);
    //        BerylObj.transform.position = new Vector2(_StartPos.x + Random.Range(-0.15f,0.15f),
    //            _StartPos.y + Random.Range(-0.15f, 0.15f));
    //        BerylObj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0.0f, 360.0f));
    //        BerylObj.SetActive(true);
    //        BerylObj.GetComponent<ParticleManager>().ParticleSetting(false,
    //            null, 1f);
    //        Count--;
    //        theSound.PlaySE("BerylSkillHit");

    //        yield return new WaitForSeconds(0.2f);
    //        if (Count == 0)
    //        {
    //            thePuzzle.CubeEvent = true;
    //            break;
    //        }
    //    }


    //}


    public GameObject AliceAnimEvent(Vector2 _StartPos, Direction _Dir)
    {
        //GameObject AliceAnim = FindObj("AliceAnimEffect", false);

        //int Size = 2;
        //if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        //{
        //    Size = 4;
        //}
        //AliceAnim.transform.position = new Vector2(_StartPos.x, _StartPos.y +0.2f);
        //if (_Dir == Direction.Left)
        //    AliceAnim.transform.localScale = new Vector3(-Size, Size, 1);
        //else
        //    AliceAnim.transform.localScale = new Vector3(Size, Size, 1);

        //AliceAnim.SetActive(true);
        //AliceAnim.GetComponent<ParticleManager>().ParticleSetting(false,
        //    null, 1f);

        return null;
    }




    //SlimeAttackParticle
    public GameObject SlimePEvent(Vector2 TargetVec)
    {
        //SlimeAttackParticle
        //GameObject Paricle = FindObj("SlimeP", false);

        //Vector2 RandVec = TargetVec;
        //float RandX = Random.Range(-0.5f, 0.5f);
        //float RandY = Random.Range(-0.3f, 1f);
        //RandVec.x += RandX;
        //RandVec.y += RandY;
        //Paricle.transform.position = RandVec;
        //Paricle.GetComponent<ParticleManager>().ParticleSetting(false, null, 2);
        //Paricle.SetActive(true);
        return null;
    }
    public GameObject PosionSlimePEvent(Vector2 TargetVec)
    {
        ////SlimeAttackParticle
        //GameObject Paricle = FindObj("PoisonP", false);

        //Vector2 RandVec = TargetVec;
        //float RandX = Random.Range(-0.5f, 0.5f);
        //float RandY = Random.Range(-0.3f, 1f);
        //RandVec.x += RandX;
        //RandVec.y += RandY;
        //Paricle.transform.position = RandVec;
        //Paricle.GetComponent<ParticleManager>().ParticleSetting(false, null, 2);
        //Paricle.SetActive(true);
        return null;
    }



}
