﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static HappyRyuMa.GameMaker;


public enum NodeColor
{
    Black = 0,
    Blue,
    Orange,
    Pink,
    Red,
    Yellow,
    Blank,
    Player,
    Special,
    Null
}

[System.Serializable]
public class MonsterSheet
{
    //몬스터 시트
    [HideInInspector] public bool OnlyOneEnemy = false;     //true일 경우 한번 처치후 더이상 나오지 않음
    [HideInInspector] public int addEnemyMeet;              //적과 조우할 확률 증가량
    [HideInInspector] public int[] EnemyIndex = null;       //몬스터 인덱스 번호
    [HideInInspector] public int[] EnemyChance = null;      //몬스터별 확률
    [HideInInspector] public int OnlyOneNum;                //데이터 시트에 저장할 번호
}

[System.Serializable]
public class PortalSheet
{
    [HideInInspector] public string MapName = null;
    [HideInInspector] public int NextPosNum;
}


public class PuzzleSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public enum NodeType
    {
        Null = 0,
        Normal,
        Enemy,
        Portal,
        Object,
    

    }



    public NodeType nodeType;
    public NodeColor nodeColor;
    public int SlotNum;

    public Text TestText;

    //DB
    public bool Down;
    public Cube cube;


    ////몬스터 시트

    public MonsterSheet monsterSheet = null;

    // 포탈 시트
    public PortalSheet portalSheet = null;

    //[HideInInspector] public int[] EnemyIndex = null;
    //[HideInInspector] public int[] EnemyChance = null;
    //[HideInInspector] public bool OnlyOneEnemy = false;
    //[HideInInspector] public int OnlyOneNum;

    //포탈 시트





    //trunk

    bool[] DoubleClick = new bool[2];
    float[] DownTime = new float[2];
    Vector2 FirstVec;
    Vector2 CurrentVec;
    private PuzzleManager thePuzzle;
    private FindMatches theMatch;
    // Start is called before the first frame update
    void Start()
    {
        theMatch = FindObjectOfType<FindMatches>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }
    private void Update()
    {
        CheckDoubleClick();
    }



    public void OnPointerDown(PointerEventData eventData)
    {
      

        if (thePuzzle.SlotDown == false&& thePuzzle.state == PuzzleManager.State.Ready &&
            nodeType != NodeType.Null)
        {
            if (DownTime[0] <= 0)
            {
                DoubleClick[0] = true;
            }
            else
            {

                DoubleClick[1] = true;
            }
            Down = true;
            thePuzzle.SlotDown = true;
            FirstVec = this.gameObject.transform.position;
            CurrentVec = this.gameObject.transform.position;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (thePuzzle.SlotDown == true && Down == true)
        {
            CurrentVec = eventData.position;
            CurrentVec = Camera.main.ScreenToWorldPoint(CurrentVec);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
       
     

        if (thePuzzle.SlotDown == true && Down == true)
        {
            if (DoubleClick[0] == true && Vector2.Distance(CurrentVec, FirstVec) < 0.3f)
            {
                DoubleClick[0] = false;
                if (DownTime[0] >= 0.4f)
                {
                    DownTime[0] = 0;
                }
                else
                {

                    DownTime[0] = 0.4f;
                }
            }
            else if (DownTime[0] > 0 && DownTime[1] <= 0.4f && Vector2.Distance(CurrentVec, FirstVec) < 0.3f)
            {
                //Debug.Log("더블클릭 성공");
                SpecialCubeEvent();
                DownTime[0] = 0;
                DownTime[1] = 0;
                DoubleClick[1] = false;


            }

            if (DoubleClick[1] == true)
            {
                DoubleClick[1] = false;
            }


            if (Vector2.Distance(CurrentVec, FirstVec) < 0.3f)
            {
                thePuzzle.SlotDown = false;
                Down = false;
                return;
            }
            float AngleZ = GetAngleZ(CurrentVec, FirstVec);
            Direction direction = Direction.Down;
            if (AngleZ <= 45 || AngleZ >= 315) // 위
            {
                direction = Direction.Up;

            }
            else if (AngleZ > 45 && AngleZ < 135) // 왼쪽
            {
                direction = Direction.Left;

            }
            else if (AngleZ >= 135 && AngleZ <= 225) // 아래
            {
                direction = Direction.Down;

            }
            else if (AngleZ > 225 && AngleZ < 315) // 오른쪽
            {
                direction = Direction.Right;

            }
            if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
            {
                thePuzzle.CheckMoveCube(thePuzzle.theMoveMap, direction, SlotNum);
            } 
            else if(thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
            {
                thePuzzle.CheckMoveCube(thePuzzle.theBattleMap, direction, SlotNum);
            }
            //findMatches.FindAllMatches();
            thePuzzle.SlotDown = false;
            Down = false;
        }
    }



    public void CheckDoubleClick()
    {
        if (DoubleClick[0] == true)
        {
            DownTime[0] += Time.deltaTime;
            if (DownTime[0] > 0.4f)
            {
                DownTime[0] = 0;
                DoubleClick[0] = false;
            }
                
        }
        else if (DownTime[0] > 0 && DoubleClick[0] == false)
        {
            DownTime[0] -= Time.deltaTime;
            if (DownTime[0] < 0)
            {
                DownTime[0] = 0;
            }
        }

        if (DoubleClick[1] == true)
        {
            DownTime[1] += Time.deltaTime;
            if (DownTime[1] > 0.4f)
            {
                DownTime[1] = 0;
                DoubleClick[1] = false;
            }
               
        }
        else if (DoubleClick[1] == false && DownTime[1] > 0)
        {
            DownTime[1] -= Time.deltaTime;
            if (DownTime[1] < 0)
                DownTime[1] = 0;
        }


    }


    public void SpecialCubeEvent()
    {
        if (cube.specialCubeType == SpecialCubeType.Null)
            return;

        MapManager _Map = null;
        thePuzzle.state = PuzzleManager.State.SpecialCubeEvent;
        thePuzzle.SetMoveCount(-1);
        if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
        {
            _Map = thePuzzle.theMoveMap;
        }
        else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        {
            _Map = thePuzzle.theBattleMap;
        }


        switch (cube.specialCubeType)
        {
            case SpecialCubeType.Horizon:
                theMatch.FindHorizonCube(_Map, SlotNum);
                break;

            case SpecialCubeType.Vertical:
                theMatch.FindVerticalCube(_Map, SlotNum);
                break;

            case SpecialCubeType.Hanoi:
                theMatch.FindHanoiCube(_Map, SlotNum);
                break;
        }

    }


    public void Resetting()
    {
        if (nodeType != NodeType.Null)
        {

            if (nodeType == NodeType.Enemy)
            {
                monsterSheet = null;
            }
            else if (nodeType == NodeType.Portal)
            {
                portalSheet = null;
            }


            nodeType = NodeType.Normal;
            nodeColor = NodeColor.Null;
            cube.Resetting();
            cube = null;
        }
    }




}
