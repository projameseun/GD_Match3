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
        
    }

    public enum NodeColor
    { 
        Red = 0,
        Yellow,
        Orange,
        Blue,
        Pink,
        White,
        Blank,
        Player
    }



    public NodeType nodeType;
    public NodeColor nodeColor;
    public int SlotNum;
    

    public Text TestText;

    public PuzzleSlot[] pSlots;
    FindMatches findMatches;

    //DB
    public bool Down;
    public Cube cube;
    public SpriteMask Mask;


    public Vector2 FirstVec;
    public Vector2 CurrentVec;
    public float Distance = 0;
    private PuzzleManager thePuzzle;
    // Start is called before the first frame update
    void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
        findMatches = FindObjectOfType<FindMatches>();
        pSlots = new PuzzleSlot[SlotNum];
    }
    
    void Update()
    {
        SetTag();
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Right)
        //    return;


        if (thePuzzle.SlotDown == false&& thePuzzle.state == PuzzleManager.State.Ready)
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
            thePuzzle.CheckMoveCube(SlotNum, direction);
            findMatches.FindAllMatches();
            thePuzzle.SlotDown = false;
            Down = false;
        }
    }

    void SetTag()
    {
        if (this.nodeColor == NodeColor.Red)
            this.tag = "RED";
        if (this.nodeColor == NodeColor.Yellow)
            this.tag = "YELLOW";
        if (this.nodeColor == NodeColor.Orange)
            this.tag = "ORANGE";
        if (this.nodeColor == NodeColor.Blue)
            this.tag = "BLUE";
        if (this.nodeColor == NodeColor.Pink)
            this.tag = "PINK";
        if (this.nodeColor == NodeColor.White)
            this.tag = "WHITE";
        if (this.nodeColor == NodeColor.Blank)
            this.tag = "BLANK";
        if (this.nodeColor == NodeColor.Player)
            this.tag = "PLAYER";
    }
}
