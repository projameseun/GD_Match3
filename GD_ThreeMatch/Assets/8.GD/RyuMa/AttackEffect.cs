using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HappyRyuMa.GameMaker;

public class AttackEffect : MonoBehaviour
{
    public Rigidbody2D RB2D;
    public GameObject FrontObj;


    public float DamageValue;
    public int EffectNum;
    public bool AttackEvent;




    //Trunk
    GameObject TargetPos; //목표 좌표
    float Speed; // 움직이는 속도
    float AngleZ; // 목표 회전값
    float AngleSpeed; // 회전속도
    float AngleAddSpeed; //회전속도 증가폭
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
        bool RandomStart,
        float _Speed = 2000)
    {
        if (_TargetVec == null)
            return;

        Speed = _Speed;
        AttackEvent = _AttackEvent;
        EffectNum = _EffectNum;
        DamageValue = _DamageValue;
        this.transform.position = StartVec;
 
        Move = true;
        TargetPos = _TargetVec;
        if (RandomStart == true)
        {
            float RandZ = Random.Range(0F, 360f);
            Rotation.z = RandZ;
            this.transform.eulerAngles = Rotation;
            FrontPos = FrontObj.transform.position - this.transform.position;
            FrontPos.Normalize();
            RB2D.velocity = FrontPos * 100;
            AngleSpeed = 200f;
        }
        else
        {
            AngleSpeed = 2000f;
        }
    }

    //----------움직이는 기능
    public void MoveCube()
    {
        FrontPos = FrontObj.transform.position - this.transform.position;
        FrontPos.Normalize();
        RB2D.velocity = FrontPos * Speed * Time.deltaTime;
    }


    public void FindTarget()
    {

        AngleSpeed = Mathf.Lerp(AngleSpeed, 2000, AngleAddSpeed * Time.deltaTime); ;

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
    //----------움직이는 기능

    public void Resetting()
    {
        Move = false;
        DamageValue = 0;
        TargetPos = null;
        AngleSpeed = 0;
        AngleZ = 0;
        Speed = 2000;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        Rotation = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }



}
