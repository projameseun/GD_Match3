using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HappyRyuMa.GameMaker;

public enum AttackEffectType
{ 
    Null = 0,
    Missile,

}

public class AttackEffect : MonoBehaviour
{
    public Rigidbody2D RB2D;
    public GameObject FrontObj;
    public AttackEffectType attackEffectType;

    public float DamageValue;
    public int EffectNum;
    public bool AttackEvent;




    //Trunk
    GameObject TargetPos; //목표 좌표
    public float CurrentSpeed; // 움직이는 속도
    float AddSpeed = 50000f;
    float MaxSpeed = 50000f;
    float AngleZ; // 목표 회전값
    float AngleSpeed; // 회전속도
    public float AngleAddSpeed = 0.3f; //회전속도 증가폭
    bool Move;


    Vector2 FrontPos;
    Vector3 Rotation = new Vector3(0, 0, 0);


    private void FixedUpdate()
    {
        if (Move == true)
        {
            MoveCube();
            FindTarget();
        }
    }


    public void SetCubeEffect(Vector2 StartVec, GameObject _TargetVec, int _DamageValue,
        int _EffectNum,
        bool _AttackEvent,
        AttackEffectType AttackType,
        float _Speed = 2000)
    {
        if (_TargetVec == null)
            return;


        CurrentSpeed = _Speed;
        AttackEvent = _AttackEvent;
        EffectNum = _EffectNum;
        DamageValue = _DamageValue;
        this.transform.position = StartVec;
 
        Move = true;
        TargetPos = _TargetVec;
        attackEffectType = AttackType;
        if (AttackType == AttackEffectType.Null)
        {
            AngleSpeed = 2000f;
        }
        else if(AttackType == AttackEffectType.Missile)
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
    }

    //----------움직이는 기능
    public void MoveCube()
    {
        FrontPos = FrontObj.transform.position - this.transform.position;
        FrontPos.Normalize();
        RB2D.velocity = FrontPos * CurrentSpeed * Time.deltaTime;
    }


    public void FindTarget()
    {
        if (attackEffectType == AttackEffectType.Missile)
        {
            AngleSpeed = Mathf.Lerp(AngleSpeed, 2000, AngleAddSpeed * Time.deltaTime);

            AngleZ = GetAngleZ(TargetPos.transform.position, this.transform.position);
            if (Rotation.z == AngleZ)
            {
                if (CurrentSpeed < MaxSpeed)
                {
                    CurrentSpeed += AddSpeed * Time.deltaTime;
                }
            }


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
    }
    //----------움직이는 기능

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
        gameObject.SetActive(false);
    }



}
