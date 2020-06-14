using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static HappyRyuMa.GameMaker;




public enum NodeColor
{
    NC0_Blue = 0,
    NC1_Green,
    NC2_Pink,
    NC3_Red,
    NC4_Yellow,
    NC5_Blank,
    NC6_Player,
    NC7_Special,
    NC8_Null
}

[System.Serializable]
public class MonsterSheet
{

    //몬스터 시트
    public int SlotImageIndex;
    public bool OnlyOneEnemy = false;     //true일 경우 한번 처치후 더이상 나오지 않음
    public int addEnemyMeet;              //적과 조우할 확률 증가량
    public int[] EnemyIndex = null;       //몬스터 인덱스 번호
    public int[] EnemyChance = null;      //몬스터별 확률
    public int OnlyOneNum;                //데이터 시트에 저장할 번호


}

[System.Serializable]
public class PortalSheet
{
    public string MapName = null;
    public int NextPosNum;
}

[System.Serializable]
public class SlotObjectSheets
{
    public SlotObjectSheet SlotSheet;
    public int ObjectNum;
    public string SkinName;
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

    public SlotObjectSheets SlotSheet;




    //trunk

    bool[] DoubleClick;
    float[] DownTime;
    Vector2 FirstVec;
    Vector2 CurrentVec;
    bool CheckCor;



    private PuzzleManager thePuzzle;
    private FindMatches theMatch;
    private PuzzleMaker theMaker;
    private BattleManager theBattle;
    // Start is called before the first frame update
    void Start()
    {
        DoubleClick = new bool[2] { false, false };
        DownTime = new float[2] { 0, 0 };
        theBattle = FindObjectOfType<BattleManager>();
        theMaker = FindObjectOfType<PuzzleMaker>();
        theMatch = FindObjectOfType<FindMatches>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        monsterSheet = null;


    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (theMaker.PuzzleMakerStart == true || Down == true)
        {
            return;
        }

        if (thePuzzle.SlotDown == false && thePuzzle.state == PuzzleManager.State.Ready &&
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
            if (CheckCor == false)
            {
                CheckCor = true;
                DoubleClickCor();
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
        if (theMaker.PuzzleMakerStart == true)
        {
            theMaker.BT_PuzzleMaker(this, SlotNum);
            return;
        }


        if (thePuzzle.SlotDown == true && Down == true)
        {
            if (DoubleClick[0] == true && Vector2.Distance(CurrentVec, FirstVec) < 0.3f)
            {
                DoubleClick[0] = false;
                if (DownTime[0] >= 0.4f)
                {
                    DownTime[0] = 0;
                    CheckCor = false;
                }
                else
                {
                    DownTime[0] = 0.4f;
                }
            }
            else if (DownTime[0] > 0 && DownTime[1] <= 0.4f && Vector2.Distance(CurrentVec, FirstVec) < 0.3f)
            {
                //Debug.Log("더블클릭 성공");

                // 스킬을 사용할 수 있는지
                CheckCor = false;
                if (theBattle.SkillEventOnOff == false)
                {
                    if (theBattle.CurrentSkillUI != SkillUI.UI2_Null)
                    {
                        SkillEvent();
                    }
                    // 특수블럭인지
                    else if (cube.specialCubeType != SpecialCubeType.Null)
                    {
                        SpecialCubeEvent();
                    }
                }
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



    //public void CheckDoubleClick2()
    //{
    //    if (DoubleClick[0] == true)
    //    {
    //        DownTime[0] += Time.deltaTime;
    //        if (DownTime[0] > 0.4f)
    //        {
    //            DownTime[0] = 0;
    //            DoubleClick[0] = false;
    //        }
                
    //    }
    //    else if (DownTime[0] > 0 && DoubleClick[0] == false)
    //    {
    //        DownTime[0] -= Time.deltaTime;
    //        if (DownTime[0] < 0)
    //        {
    //            DownTime[0] = 0;
    //        }
    //    }

    //    if (DoubleClick[1] == true)
    //    {
    //        DownTime[1] += Time.deltaTime;
    //        if (DownTime[1] > 0.4f)
    //        {
    //            DownTime[1] = 0;
    //            DoubleClick[1] = false;
    //        }
               
    //    }
    //    else if (DoubleClick[1] == false && DownTime[1] > 0)
    //    {
    //        DownTime[1] -= Time.deltaTime;
    //        if (DownTime[1] < 0)
    //            DownTime[1] = 0;
    //    }


    //}


    public void DoubleClickCor()
    {
        StartCoroutine(CheckDoubleClick());
    }

    IEnumerator CheckDoubleClick()
    {
        while (true)
        {
            if (CheckCor == false)
                break;
            if (DoubleClick[0] == true)
            {
                DownTime[0] += Time.deltaTime;
                if (DownTime[0] > 0.4f)
                {
                    DownTime[0] = 0;
                    DoubleClick[0] = false;
                    CheckCor = false;

                    Debug.Log("더블클릭 실패");
                }

            }
            else if (DownTime[0] > 0 && DoubleClick[0] == false)
            {
                DownTime[0] -= Time.deltaTime;
                if (DownTime[0] < 0)
                {
                    DownTime[0] = 0;
                    CheckCor = false;
                    Debug.Log("더블클릭 실패");
                }
            }

            if (DoubleClick[1] == true)
            {
                DownTime[1] += Time.deltaTime;
                if (DownTime[1] > 0.4f)
                {
                    DownTime[1] = 0;
                    DoubleClick[1] = false;
                    CheckCor = false;
                    Debug.Log("더블클릭 실패");
                }

            }
            else if (DoubleClick[1] == false && DownTime[1] > 0)
            {
                DownTime[1] -= Time.deltaTime;
                if (DownTime[1] < 0)
                {
                    DownTime[1] = 0;
                    CheckCor = false;
                    Debug.Log("더블클릭 실패");
                }
                    
            }
            Debug.Log("코르틴 확인");
            yield return new WaitForEndOfFrame();

            

        }
    }



    public void SpecialCubeEvent()
    {
        thePuzzle.SetMoveCount(-1);
        if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
        {
            thePuzzle.state = PuzzleManager.State.SpecialCubeEvent;
            thePuzzle.Player.ChangeAnim("Attack");
            thePuzzle.Player.Map = thePuzzle.theMoveMap;
            thePuzzle.Player.SlotNum = SlotNum;
            thePuzzle.Player.Type = cube.specialCubeType;
            
        }
        else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        {
            thePuzzle.state = PuzzleManager.State.SpecialCubeEvent;


            Direction dir = Direction.Right;
            Vector2 StartVec = new Vector2(this.transform.position.x, this.transform.position.y);
            // 슬롯이 오른쪽
            if (SlotNum % thePuzzle.theBattleMap.Horizontal > 5)
            {
                StartVec.x -= 1.8f;
                dir = Direction.Right;
            }
            else
            {
                StartVec.x += 1.8f;
                dir = Direction.Left;
            }

            Debug.Log(SlotNum / thePuzzle.theBattleMap.Horizontal);
            if (SlotNum / thePuzzle.theBattleMap.Horizontal <= 4)
            {
                StartVec.y -= 1f;
            }
            else
            {
                StartVec.y += 1f;
            }
            thePuzzle.Player.BattleEvent(StartVec,dir,SkillType.ST0_SpecialCube, cube.specialCubeType,thePuzzle.theBattleMap,SlotNum);

        }


        //switch (cube.specialCubeType)
        //{
        //    case SpecialCubeType.Horizon:
        //        theMatch.FindHorizonCube(_Map, SlotNum);
        //        break;

        //    case SpecialCubeType.Vertical:
        //        theMatch.FindVerticalCube(_Map, SlotNum);
        //        break;

        //    case SpecialCubeType.Hanoi:
        //        theMatch.FindHanoiCube(_Map, SlotNum);
        //        break;
        //}

    }

    public void SkillEvent()
    {
       
        Direction dir = Direction.Right;
        thePuzzle.SetMoveCount(-1);
        Vector2 StartVec = new Vector2(this.transform.position.x, this.transform.position.y);
        thePuzzle.state = PuzzleManager.State.SpecialCubeEvent;
        // 슬롯이 오른쪽
        if (SlotNum % thePuzzle.theBattleMap.Horizontal > 5)
        {
            StartVec.x -= 1.8f;
            dir = Direction.Right;
        }
        else
        {
            StartVec.x += 1.8f;
            dir = Direction.Left;
        }

        Debug.Log(SlotNum / thePuzzle.theBattleMap.Horizontal);
        if (SlotNum / thePuzzle.theBattleMap.Horizontal <= 4)
        {
            StartVec.y -= 1f;
        }
        else
        {
            StartVec.y += 1f;
        }

        thePuzzle.Player.BattleEvent(StartVec, dir, SkillType.ST1_GirlSkill, cube.specialCubeType,
            thePuzzle.theBattleMap, SlotNum);

      
        thePuzzle.playerUIs[(int)theBattle.CurrentSkillUI].ResetSkillGauge();
        theBattle.SkillEventOnOff = true;
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
            nodeColor = NodeColor.NC6_Player;
            cube.Resetting();
            cube = null;
        }
    }




}
