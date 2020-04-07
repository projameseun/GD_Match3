using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static HappyRyuMa.GameMaker;


public class PuzzleSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public enum NodeType
    {
        Null = 0,
        Normal,
        Enemy,
        Goal,
        Player
    }

    public enum NodeColor
    { 
        Red = 0,
        Yellow,
        Orange,
        Blue,
        Pink,
        White,
        Blank
    }



    public NodeType nodeType;
    public NodeColor nodeColor;
    public int SlotNum;
    

    public Text TestText;




    //DB
    public bool Down;
    public Cube cube;


    public Vector2 FirstVec;
    public Vector2 CurrentVec;
    public float Distance = 0;
    private PuzzleManager thePuzzle;
    // Start is called before the first frame update
    void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();

        //if (nodeType == NodeType.Null)
        //{
        //    this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        //}

    }


    private void Update()
    {
        //if (thePuzzle.SlotDown == true && Down == true)
        //{


        //    if (Distance > 140)
        //    {
        //        float AngleZ = GetAngleZ(CurrentVec, FirstVec);

        //        if (AngleZ <= 45 || AngleZ >= 315) // 위
        //        {
        //            Debug.Log("위");
        //        }
        //        else if (AngleZ > 45 && AngleZ < 135) // 왼쪽
        //        {
        //            Debug.Log("왼쪽");
        //        }
        //        else if (AngleZ >= 135 && AngleZ <= 225) // 아래
        //        {
        //            Debug.Log("아래");
        //        }
        //        else if (AngleZ > 225 && AngleZ < 315) // 오른쪽
        //        {
        //            Debug.Log("오른쪽");
        //        }
        //        Debug.Log("Distance = " + Distance);
        //        thePuzzle.SlotDown = false;
        //        Down = false;
        //    }
        //}
    }



   

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Right)
        //    return;


        if (thePuzzle.SlotDown == false)
        {
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
            if (Vector2.Distance(CurrentVec, FirstVec) < 0.5f)
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
            thePuzzle.CheckMoveCube(SlotNum, direction);

            thePuzzle.SlotDown = false;
            Down = false;
        }
    }
}
