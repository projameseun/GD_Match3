using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HappyRyuMa.GameMaker;

public enum AttackEffectType
{ 
    ET0_Null = 0,
    ET1_Missile,
    ET2_Gun,
    ET3_

}

public class AttackEffect : MonoBehaviour
{
    public Rigidbody2D RB2D;
    public GameObject FrontObj;
    public AttackEffectType attackEffectType;

    public float DamageValue;
    public int EffectNum;
    public bool AttackEvent;

    public float DestroyCount;


    //Trunk
    GameObject TargetPos; //목표 좌표
    public float CurrentSpeed; // 움직이는 속도
    float AddSpeed = 50000f;
    float MaxSpeed = 50000f;
    float EventTime = 0;
    float AngleZ; // 목표 회전값
    float AngleSpeed; // 회전속도
    public float AngleAddSpeed = 0.3f; //회전속도 증가폭
    bool Move;
    bool MoveEvent;
    bool FindEvnet;

    GameObject EffectObj;

    Vector2 FrontPos;
    Vector3 Rotation = new Vector3(0, 0, 0);


    private BattleManager theBattle;
    private ObjectManager theObject;
    private SoundManager theSound;
    private void Start()
    {
        theSound = FindObjectOfType<SoundManager>();
        theBattle = FindObjectOfType<BattleManager>();
        theObject = FindObjectOfType<ObjectManager>();
    }




    private void FixedUpdate()
    {
        if (Move == true)
        {
            if(MoveEvent == true)
                MoveCube();

            if(FindEvnet == true)
                FindTarget();

            if (DestroyCount > 0)
                DestroyCount -= Time.deltaTime;
            else
                Resetting();

        }
    }


    public void SetCubeEffect(GameObject _TargetVec, int _DamageValue,
        int _EffectNum,
        bool _AttackEvent,
        AttackEffectType AttackType,
        GameObject _EffectObj,
        float _Speed = 2000)
    {
        if (_TargetVec == null)
            return;

        if (theBattle == null)
            theBattle = FindObjectOfType<BattleManager>();

        if (_EffectObj != null)
        {
            EffectObj = _EffectObj;
            EffectObj.transform.position = this.transform.position;
        }


        DestroyCount = 10;
        CurrentSpeed = _Speed;
        AttackEvent = _AttackEvent;
        EffectNum = _EffectNum;
        DamageValue = _DamageValue;
 
        Move = true;
        MoveEvent = true;
        FindEvnet = true;
        TargetPos = _TargetVec;
        attackEffectType = AttackType;
        if (AttackType == AttackEffectType.ET0_Null)
        {
            float RandZ = Random.Range(0F, 360f);
            Rotation.z = RandZ;
            this.transform.eulerAngles = Rotation;
            FrontPos = FrontObj.transform.position - this.transform.position;
            FrontPos.Normalize();
            RB2D.velocity = FrontPos * 100;
            AngleSpeed = 200f;
        }
        else if (AttackType == AttackEffectType.ET1_Missile)
        {
            float RandZ = Random.Range(0F, 120f);
            RandZ -= 60f;
            Rotation.z = RandZ;
            this.transform.eulerAngles = Rotation;
            FrontPos = FrontObj.transform.position - this.transform.position;
            FrontPos.Normalize();
            RB2D.velocity = FrontPos * 5000;
            AngleSpeed = 100f;
        }
        else if (AttackType == AttackEffectType.ET2_Gun)
        {
            AngleZ = GetAngleZ(TargetPos.transform.position, this.transform.position);
            Rotation.z = AngleZ;
            this.transform.eulerAngles = Rotation;
            FrontPos = FrontObj.transform.position - this.transform.position;
            FrontPos.Normalize();
            CurrentSpeed = MaxSpeed;
            AngleSpeed = 2000f;
        }
        else if (attackEffectType == AttackEffectType.ET3_)
        {

            MoveEvent = false;
            float RandZ = Random.Range(0F, 360f);
            Rotation.z = RandZ;
            this.transform.eulerAngles = Rotation;
            EventTime = theBattle.AttackEffectEventTime;
            FrontPos = FrontObj.transform.position - this.transform.position;
            FrontPos.Normalize();
            float Power = Random.Range(5f, 10f);
            RB2D.velocity = FrontPos * Power;
            
        }
    }

    //----------움직이는 기능
    public void MoveCube()
    {
        FrontPos = FrontObj.transform.position - this.transform.position;
        FrontPos.Normalize();
        RB2D.velocity = FrontPos * CurrentSpeed * Time.fixedDeltaTime;
    }


    public void FindTarget()
    {
        if (attackEffectType == AttackEffectType.ET0_Null)
        {
            AngleSpeed = Mathf.Lerp(AngleSpeed, 2000, AngleAddSpeed * Time.deltaTime);
            FindTargetAngleZ();
        }
        else if (attackEffectType == AttackEffectType.ET1_Missile)
        {
            AngleSpeed = Mathf.Lerp(AngleSpeed, 2000, AngleAddSpeed * Time.deltaTime);

            FindTargetAngleZ();
            if (Rotation.z == AngleZ)
            {
                if (CurrentSpeed < MaxSpeed)
                {
                    CurrentSpeed += AddSpeed * Time.deltaTime;
                }
            }
        }
        else if (attackEffectType == AttackEffectType.ET2_Gun)
        {
            FindTargetAngleZ();
        }
        else if (attackEffectType == AttackEffectType.ET3_)
        {
            if (MoveEvent == false)
            {
                if (EventTime > 0)
                {
                    EventTime -= Time.deltaTime;
                }
                else
                {
                    if (theBattle.EnemySkill[theBattle.SkillNum].SkillName == "SlimeAttack")
                    {
                        theSound.PlaySE("SlimeSkillMove");
                    }

                    MoveEvent = true;
                    AngleZ = GetAngleZ(TargetPos.transform.position, this.transform.position);
                    Rotation.z = AngleZ;
                    this.transform.eulerAngles = Rotation;
                    AngleSpeed = 2000;
                    CurrentSpeed = MaxSpeed / 5;
                }
                 
                FindTargetAngleZ();
            }
            else
            {

                FindTargetAngleZ();
            }
          
        }

    }
    //----------움직이는 기능






    // 이펙트가 타겟을 봐라보게 하는 함수
    public void FindTargetAngleZ()
    {
        AngleZ = GetAngleZ(TargetPos.transform.position, this.transform.position);
        if (Rotation.z + 180 < AngleZ)
        {
            Rotation.z -= AngleSpeed * Time.deltaTime;
            if (Rotation.z < 0)
                Rotation.z += 360;
        }
        else
        {
            if (Rotation.z < AngleZ)
            {
                Rotation.z += AngleSpeed * Time.deltaTime;
                if (Rotation.z > AngleZ)
                    Rotation.z = AngleZ;
            }
            else
            {
                Rotation.z -= AngleSpeed * Time.deltaTime;
                if (Rotation.z < AngleZ)
                    Rotation.z = AngleZ;
            }
        }

        this.transform.eulerAngles = Rotation;
    }


    public void Resetting()
    {
        Move = false;
        DamageValue = 0;
        TargetPos = null;
        AngleSpeed = 0;
        AngleZ = 0;
        CurrentSpeed = 0;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        Rotation = new Vector3(0, 0, 0);

        if (EffectObj != null)
        {
            EffectObj.transform.SetParent(null);
            EffectObj.GetComponent<ParticleManager>().Resetting();
            EffectObj = null;
        }


        gameObject.SetActive(false);
        theObject.AttackEffects.Enqueue(this.gameObject);
    }



}
