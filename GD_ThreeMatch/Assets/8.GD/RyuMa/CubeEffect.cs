using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HappyRyuMa.GameMaker;

public enum CubeEffectType
{ 
    GoPlayer,
    GoEnemy

}
public class CubeEffect : MonoBehaviour
{

    public CubeEffectType cubeEffectType;
    public NodeColor nodeColor;
    public int CubeCount;



    public Vector2 TargetPos; //목표 좌표
    public float Speed; // 움직이는 속도
    public float AngleZ; // 목표 회전값
    public float AngleSpeed; // 회전속도
    public float AngleAddSpeed; //회전속도 증가폭
    bool Move;

    public Rigidbody2D RB2D;
    public GameObject FrontObj;
    Vector2 FrontPos;
    Vector3 Rotation = new Vector3(0,0,0);





    private void FixedUpdate()
    {

        if (Move == true)
        {
            MoveCube();
            FindTarget();
        }

       

    }


    public void SetCubeEffect(Vector2 StartVec,Vector2 _TargetVec,
        NodeColor _nodeColor,
        CubeEffectType _Type,
        int _CubeCount,
        bool RandomStart)
    {
        this.transform.position = StartVec;
        nodeColor = _nodeColor;
        cubeEffectType = _Type;
        Move = true;
        TargetPos = _TargetVec;
        CubeCount = _CubeCount;
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


    public void MoveCube()
    {
        FrontPos = FrontObj.transform.position - this.transform.position;
        FrontPos.Normalize();
        RB2D.velocity = FrontPos * Speed * Time.deltaTime;



    }
    public void FindTarget()
    {

        AngleSpeed = Mathf.Lerp(AngleSpeed, 2000, AngleAddSpeed*Time.deltaTime); ;

        AngleZ = GetAngleZ(TargetPos, this.transform.position);
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

    public void UiSet(CubeUI _UI)
    {
        _UI.CubeCount += CubeCount;
        _UI.CubeCountText.text = _UI.CubeCount.ToString();
        Resetting();
    }



    public void Resetting()
    {
        Move = false;
        AngleSpeed = 0;
        AngleZ = 0;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        Rotation = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }


}
