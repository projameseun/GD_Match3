using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using static HappyRyuMa.GameMaker;

public enum CubeEffectType
{ 
    GoPlayer = 0,
    GoEnemy

}
public class CubeEffect : MonoBehaviour
{

    public CubeEffectType cubeEffectType;
    public NodeColor nodeColor;
    public int CubeCount;
    CubeEffectP CubeP;


    public GameObject TargetPos; //목표 좌표
    public float Speed; // 움직이는 속도
    public float AngleZ; // 목표 회전값
    public float AngleSpeed; // 회전속도
    public float AngleAddSpeed; //회전속도 증가폭
    bool Move;

    public Rigidbody2D RB2D;
    public GameObject FrontObj;
    Vector2 FrontPos;
    Vector3 Rotation = new Vector3(0,0,0);
    public float DestroyCount;

    private PuzzleManager thePuzzle;
    private BattleManager theBattle;
    private ObjectManager theObject;


    private void FixedUpdate()
    {

        if (Move == true)
        {
            MoveCube();
            FindTarget();
        }

        if (DestroyCount > 0)
            DestroyCount -= Time.fixedDeltaTime;
        else
            Resetting();



    }


    //public void SetCubeEffect(Vector2 StartVec, GameObject _TargetVec,
    //    NodeColor _nodeColor,
    //    CubeEffectType _Type,
    //    int _CubeCount,
    //    bool RandomStart,
    //    float _Speed)
    //{
    //    if (theObject == null)
    //        theObject = FindObjectOfType<ObjectManager>();
    //    if (theBattle == null)
    //        theBattle = FindObjectOfType<BattleManager>();
    //    if (thePuzzle == null)
    //        thePuzzle = FindObjectOfType<PuzzleManager>();
    //    RandomStart = false;


    //    if (_TargetVec == null)
    //        return;


    //    //if (_nodeColor == NodeColor.NC0_Blue)
    //    //{
    //    //    CubeP = theObject.FindObj("CubeBlue", false).GetComponent<CubeEffectP>();
    //    //}
    //    //else if (_nodeColor == NodeColor.NC1_Green)
    //    //{
    //    //    CubeP = theObject.FindObj("CubeGreen", false).GetComponent<CubeEffectP>();
    //    //}
    //    //else if (_nodeColor == NodeColor.NC2_Pink) // 핑크
    //    //{
    //    //    CubeP = theObject.FindObj("CubePink", false).GetComponent<CubeEffectP>();
    //    //}
    //    //else if (_nodeColor == NodeColor.NC3_Red) // 빨간색
    //    //{
    //    //    CubeP = theObject.FindObj("CubeRed", false).GetComponent<CubeEffectP>();
    //    //}
    //    //else if (_nodeColor == NodeColor.NC4_Yellow) // 노란색
    //    //{
    //    //    CubeP = theObject.FindObj("CubeYellow", false).GetComponent<CubeEffectP>();
    //    //}
    //    CubeP.ParticleSetting(true, this.gameObject);
    //    CubeP.gameObject.SetActive(true);


    //    Speed = _Speed;

    //    this.transform.position = StartVec;
    //    nodeColor = _nodeColor;
    //    cubeEffectType = _Type;
    //    Move = true;
    //    TargetPos = _TargetVec;
    //    CubeCount = _CubeCount;
    //    DestroyCount = 10f; // 오브젝트 생존시간
    //    if (RandomStart == true)
    //    {
    //        float RandZ = Random.Range(0F, 360f);
    //        Rotation.z = RandZ;
    //        this.transform.eulerAngles = Rotation;
    //        FrontPos = FrontObj.transform.position - this.transform.position;
    //        FrontPos.Normalize();
    //        RB2D.velocity = FrontPos * 100;
    //        AngleSpeed = 200f;
    //    }
    //    else
    //    {
    //        AngleSpeed = 2000f;
    //    }
    //}

    //----------움직이는 기능
    public void MoveCube()
    {
        FrontPos = FrontObj.transform.position - this.transform.position;
        FrontPos.Normalize();
        RB2D.velocity = FrontPos * Speed * Time.deltaTime;



    }
    public void FindTarget()
    {

        AngleSpeed = Mathf.Lerp(AngleSpeed, 2000, AngleAddSpeed*Time.deltaTime); 
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



    public void UiSet(CubeUI _UI)
    {


      

        if (_UI.uIType == CubeUI.UIType.EnemyUI)
        {
            if (_UI.CubeCount + CubeCount >= 0)
            {
                if (CubeCount < 0)
                    theObject.DamageTextEvent(this.transform.position, (int)(-CubeCount));
                theBattle.TakeDamage(CubeCount);
            }
            else
            {

                if (CubeCount < 0)
                {
                    if (_UI.CubeCount != 0)
                    {
                        theObject.DamageTextEvent(this.transform.position, (int)(_UI.CubeCount));
                        theBattle.TakeDamage(-_UI.CubeCount);
                    }
                }
       
            }
        }

        _UI.CubeCount += CubeCount;




        if (_UI.CubeCount < 0)
        {
            _UI.CubeCount = 0;
            _UI.CubeCountText.text = _UI.CubeCount.ToString();
            CubeCount = 1;
            DestroyCount = 10f;
            
            cubeEffectType = CubeEffectType.GoPlayer;
            for (int i = 0; i < thePuzzle.CubeSprites.Length; i++)
            {
                if (thePuzzle.PlayerCubeUI[i].cubeColor == nodeColor)
                    TargetPos = thePuzzle.PlayerCubeUI[i].gameObject;
            }

        }
        else
        {
            _UI.CubeCountText.text = _UI.CubeCount.ToString();
            Resetting();
        }
           

    }



    public void Resetting()
    {
        Move = false;
        TargetPos = null;
        AngleSpeed = 0;
        AngleZ = 0;
        Speed = 2000;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        Rotation = new Vector3(0, 0, 0);
        gameObject.SetActive(false);

        if (CubeP != null)
        {
            CubeP.transform.SetParent(null);
            CubeP.Resetting();
            CubeP = null;
        }
           
    }


}
