using System.Collections;
using System.Collections.Generic;
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

    private PuzzleManager thePuzzle;
    private BattleManager theBattle;
    private void Start()
    {
        theBattle = FindObjectOfType<BattleManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }



    private void FixedUpdate()
    {

        if (Move == true)
        {
            MoveCube();
            FindTarget();
        }

       

    }


    public void SetCubeEffect(Vector2 StartVec,GameObject _TargetVec,
        NodeColor _nodeColor,
        CubeEffectType _Type,
        int _CubeCount,
        bool RandomStart)
    {

        if (_TargetVec == null)
            return;


        if ((int)_nodeColor == 0) //검은색
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0.42f, 0.42f, 0.42f);

        }
        else if ((int)_nodeColor == 1) //파란색
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0.56f, 0.78f, 0.9f);
        }
        else if ((int)_nodeColor == 2) // 주황색
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.38f, 0.01f);
        }
        else if ((int)_nodeColor == 3) // 핑크
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0.95f, 0.3f, 0.57f);
        }
        else if ((int)_nodeColor == 4) // 빨간색
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0.94f, 0.11f, 0.01f);
        }
        else if ((int)_nodeColor == 5) // 노란색
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.89f, 0.51f);
        }


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

    //----------움직이는 기능
    public void MoveCube()
    {
        FrontPos = FrontObj.transform.position - this.transform.position;
        FrontPos.Normalize();
        RB2D.velocity = FrontPos * Speed * Time.deltaTime;



    }
    public void FindTarget()
    {

        AngleSpeed = Mathf.Lerp(AngleSpeed, 2000, AngleAddSpeed*Time.deltaTime); ;

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



    public void UiSet(CubeUI _UI, int _UiNum)
    {
        if (_UiNum < 2)
            thePuzzle.playerUIs[_UiNum].AddSkillGauge(CubeCount);


        _UI.CubeCount += CubeCount;

        if (_UI.uIType == CubeUI.UIType.EnemyUI)
        {
            if (_UI.CubeCount > -1)
            {
                theBattle.TakeDamage();
            }
        }



        if (_UI.CubeCount < 0)
            _UI.CubeCount = 0;


        _UI.CubeCountText.text = _UI.CubeCount.ToString();
        Resetting();
    }



    public void Resetting()
    {
        Move = false;
        TargetPos = null;
        AngleSpeed = 0;
        AngleZ = 0;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        Rotation = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }


}
