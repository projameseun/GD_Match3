using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static HappyRyuMa.GameMaker;

public enum CubeType
{
    Null = 0,
    NormalCube,
    SpecialCube,
    GirlCube,
}
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


public class PuzzleSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public enum NodeType
    {
        Null = 0,
        Normal,
        Enemy,
        Goal,
        Object,

    }



    public NodeType nodeType;
    public NodeColor nodeColor;
    public CubeType cubeType;
    public int SlotNum;
    

    public Text TestText;

    //DB
    public bool Down;
    public Cube cube;



    //trunk

    bool[] DoubleClick = new bool[2];
    float[] DownTime = new float[2];
    Vector2 FirstVec;
    Vector2 CurrentVec;
    private PuzzleManager thePuzzle;
    // Start is called before the first frame update
    void Start()
    {
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
            if (DoubleClick[0] == true)
            {
                DoubleClick[0] = false;
                if (DownTime[0] > 0.2f)
                {
                    DownTime[0] = 0;
                }
                else
                {

                    DownTime[0] = 0.3f;
                }
            }
            else if (DownTime[0] > 0 && DownTime[1] < 0.2f)
            {
                Debug.Log("더블클릭 성공");
                DownTime[0] = 0;
                DownTime[1] = 0;
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

        }
        else if (DoubleClick[1] == false && DownTime[1] > 0)
        {
            DownTime[1] -= Time.deltaTime;
            if (DownTime[1] < 0)
                DownTime[1] = 0;
        }


    }


}
