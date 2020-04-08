using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Direction
{ 
    Up = 0,
    Down,
    Left,
    Right
}


public class PuzzleManager : MonoBehaviour
{
    public enum State
    { 
        Ready = 0,
        ChangeMatch,
        ChangeMatchRetrun,
        FillBlank
    }

    public State state;
    public Direction direction;


    public Sprite[] CubeSprites;
    public PuzzleSlot[] Slots;

    public bool isMatched = false;
    private FindMatches findMatches;

    //DB
    public bool SlotDown = false;
    public bool CubeEvent = false;
    int SelectNum = 0;
    int OtherNum = 0;
    public bool Test = false; // true면 FillBlank실행
    public GameObject Player;

    //DB
    public int Horizontal;
    public int Vertical;
    public int TopLeft;
    public int TopRight;
    public int BottomLeft;
    public int BottomRight;


    private ObjectManager theObject;
    private PlayerCube thePlayer;
    private void Start()
    {
        theObject = FindObjectOfType<ObjectManager>();
        thePlayer = FindObjectOfType<PlayerCube>();

        for (int i = 0; i < Slots.Length; i++)
        {
            if (i <= TopRight || i >= BottomLeft || i % Horizontal <= TopLeft || i % Horizontal >= TopRight)
            {
                Slots[i].nodeType = PuzzleSlot.NodeType.Null;
                Slots[i].Mask.enabled = false;
            }

            Slots[i].SlotNum = i;
        }

        findMatches = FindObjectOfType<FindMatches>();
    }
    private void Update()
    {
        if (Test == true)
        {
            Test = false;
            BT_FillBlank();
        }

        if (state == State.ChangeMatch)
        { 
            if(CubeEvent == true)
            {
                CubeEvent = false;

                //매치 조건이 맞는지 확인한다
                findMatches.FindAllMatches();
                if (isMatched)
                    Debug.Log("Matched!");

                //매치가 안될경우
                if (!isMatched)
                {
                    ChangeCube(SelectNum, OtherNum, true);
                    Debug.Log("Not Natched");
                }
                state = State.ChangeMatchRetrun;
            }
        }

        if (state == State.ChangeMatchRetrun)
        { 
            if(CubeEvent == true)
            {
                CubeEvent = false;
                state = State.Ready;

            }
        }
        if (state == State.FillBlank)
        {
            if (CubeEvent == true)
            {
                CubeEvent = false;
                BT_FillBlank();

            }
        }


    }




    public void SetSlot()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].nodeType != PuzzleSlot.NodeType.Null)
            {
                Slots[i].nodeType = PuzzleSlot.NodeType.Normal;
                if (Slots[i].cube != null)
                {
                    Slots[i].cube.gameObject.SetActive(false);
                    Slots[i].cube = null;
                }
                Slots[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
                Slots[i].TestText.text = i.ToString();

            }
            else
            {
                Slots[i].GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
                Slots[i].TestText.text = i.ToString();
                Slots[i].TestText.color = new Color(1, 1, 1);
            }
        }


        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].nodeType != PuzzleSlot.NodeType.Null)
            {
                GameObject Cube = theObject.FindObj("Cube");

                SetCube(Cube, Slots[i]);
                Slots[i].TestText.text = i.ToString();
                Slots[i].cube.Num = i;
            }

        }

        while (true)
        {
            int rand = Random.Range(0, 144);

            if (Slots[rand].nodeType != PuzzleSlot.NodeType.Null)
            {
                Slots[rand].nodeColor = PuzzleSlot.NodeColor.Player;
                Player.transform.position = Slots[rand].transform.position;

                Transform Parent = Slots[rand].cube.transform;

                Player.transform.parent = Parent;
                thePlayer.ChangeSprite(direction);
                break;
            }
        }
        while (true)
        {
            int rand = Random.Range(0, 144);

            if (Slots[rand].nodeType != PuzzleSlot.NodeType.Null &&
                Slots[rand].nodeColor != PuzzleSlot.NodeColor.Player)
            {
                Slots[rand].nodeType = PuzzleSlot.NodeType.Enemy;
                Slots[rand].GetComponent<Image>().color = new Color(1f, 0, 0, 0.8f);
                Slots[rand].TestText.text = "적";
                Slots[rand].TestText.enabled = true;
                break;
            }
        }
        while (true)
        {
            int rand = Random.Range(0, 144);

            if (Slots[rand].nodeType != PuzzleSlot.NodeType.Null &&
                Slots[rand].nodeColor != PuzzleSlot.NodeColor.Player &&
                Slots[rand].nodeType != PuzzleSlot.NodeType.Enemy)
            {
                Slots[rand].nodeType = PuzzleSlot.NodeType.Goal;
                Slots[rand].GetComponent<Image>().color = new Color(0, 1f, 0, 0.8f);
                Slots[rand].TestText.text = "골";
                Slots[rand].TestText.enabled = true;
                break;
            }
        }

    }


    // 최초 한번만 실행해서 NULL이 아닌 슬롯에 큐브를 설치
    public void SetCube(GameObject _Cube, PuzzleSlot _Slot)
    {
        int rand = Random.Range(0, CubeSprites.Length);

        _Cube.GetComponent<SpriteRenderer>().sprite = CubeSprites[rand];
        _Slot.nodeColor = (PuzzleSlot.NodeColor)rand;

        _Cube.transform.position = _Slot.transform.position;
        _Slot.cube = _Cube.GetComponent<Cube>();
    }


    //큐브가 움직일 수 있는지 확인
    public void CheckMoveCube(int _Num, Direction _direction)
    {

        int ChangeNum = 0;

        if (_direction == Direction.Up) 
        {
            if (Slots[_Num - 12].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num - 12;
        }
        else if (_direction == Direction.Down)
        {
            if (Slots[_Num + 12].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num + 12;
        }
        else if (_direction == Direction.Left)
        {
            if (Slots[_Num -1].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num - 1;
        }
        else if (_direction == Direction.Right)
        {
            if (Slots[_Num +1].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num +1;
        }

        state = State.ChangeMatch;
        ChangeCube(_Num, ChangeNum,true);
        SelectNum = _Num;
        OtherNum = ChangeNum;
    }


    public void ChangeCube(int _Num, int _OtherNum,bool _Event = false, float Speed = 0.005f)
    {
        //_Num의 큐브정보 복제
        Vector2 Vec = Slots[_Num].cube.transform.position;
        GameObject Cube = Slots[_Num].cube.gameObject;
        PuzzleSlot.NodeColor nodeColor = Slots[_Num].nodeColor;

        //_Num의 정보를 _OtherNum의 정보로 덮어쓰기
        Slots[_Num].cube.MoveCube(Slots[_OtherNum].transform.position, false, Speed);

        Slots[_Num].cube = Slots[_OtherNum].cube;
        Slots[_Num].nodeColor = Slots[_OtherNum].nodeColor;

        //_OtherNum을 복제정보로 덮어쓰기
        Slots[_OtherNum].cube.MoveCube(Slots[_Num].transform.position, _Event, Speed);
        Slots[_OtherNum].cube = Cube.GetComponent<Cube>();
        Slots[_OtherNum].nodeColor = nodeColor;

        Slots[_Num].cube.Num = Slots[_Num].SlotNum;
        Slots[_OtherNum].cube.Num = Slots[_OtherNum].SlotNum;

        
    }

    public void BT_ShowSlotText()
    {
        bool Active = !Slots[0].GetComponentInChildren<Text>().enabled;


        if (Active == true)
            GetComponentInParent<Canvas>().sortingOrder = 5;
        else
            GetComponentInParent<Canvas>().sortingOrder = 0;

        for (int i = 0; i < 144; i++)
        {
            Slots[i].GetComponentInChildren<Text>().enabled = Active;
        }
    
    }

    public void BT_RandomBlank()
    {

        for (int i = 0; i < 144; i++)
        {
            if (Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
                Slots[i].nodeColor != PuzzleSlot.NodeColor.Blank &&
                Slots[i].nodeColor != PuzzleSlot.NodeColor.Player)
            {
                break;
            }

            if (i == 143)
                return;
        }

        while (true)
        {
            int rand = Random.Range(0, 144);
            if (Slots[rand].nodeType != PuzzleSlot.NodeType.Null &&
                Slots[rand].nodeColor != PuzzleSlot.NodeColor.Player &&
                Slots[rand].nodeColor != PuzzleSlot.NodeColor.Blank)
            {
                Slots[rand].nodeColor = PuzzleSlot.NodeColor.Blank;
                Slots[rand].cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
                return;
            }
        }
      

    }

    public void BT_FillBlank()
    {
        state = State.FillBlank;
        bool FirstEvent = true;
        float Speed = 0.015f;
        if (direction == Direction.Down)
        {
            for (int i = 0; i < TopRight - TopLeft; i++)
            {
                for (int Num = BottomLeft -Horizontal; Num > TopRight; Num -= Horizontal)
                {
                    if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        Slots[Num + i].cube.gameObject.SetActive(false);
                        
                        if (Slots[Num + i - Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                        {
                            
                            ChangeCube(Num + i, Num + i - Horizontal, FirstEvent, Speed);
                            if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,1);
                            SetCube(NewCube, Slots[Num + i]);
                            NewCube.transform.position = Slots[Num + i - Horizontal].transform.position;
                            Slots[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(Slots[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }

        }
        else if (direction == Direction.Up)
        {
            for (int i = 0; i < TopRight - TopLeft; i++)
            {
                for (int Num = TopLeft + Horizontal; Num < BottomLeft; Num += Horizontal)
                {
                    if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        Slots[Num + i].cube.gameObject.SetActive(false);

                        if (Slots[Num + i + Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                        {

                            ChangeCube(Num + i, Num + i + Horizontal, FirstEvent, Speed);
                            if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            SetCube(NewCube, Slots[Num + i]);
                            NewCube.transform.position = Slots[Num + i + 12].transform.position;
                            Slots[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(Slots[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }
        }
        else if (direction == Direction.Left)
        {
            for (int i = TopLeft + Horizontal; i < BottomLeft; i+=Horizontal)
            {
                for (int Num = 0; Num <TopRight - TopLeft; Num++)
                {
                    if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        Slots[Num + i].cube.gameObject.SetActive(false);

                        if (Slots[Num + i +1].nodeType != PuzzleSlot.NodeType.Null)
                        {

                            ChangeCube(Num + i, Num + i + 1, FirstEvent, Speed);
                            if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            SetCube(NewCube, Slots[Num + i]);
                            NewCube.transform.position = Slots[Num + i +1].transform.position;
                            Slots[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(Slots[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }
        }
        else if (direction == Direction.Right)
        {
            for (int i = TopRight + Horizontal; i < BottomLeft; i += Horizontal)
            {
                for (int Num = 0; Num > -(TopRight - TopLeft); Num--)
                {
                    if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        Slots[Num + i].cube.gameObject.SetActive(false);

                        if (Slots[Num + i - 1].nodeType != PuzzleSlot.NodeType.Null)
                        {

                            ChangeCube(Num + i, Num + i - 1, FirstEvent, Speed);
                            if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            SetCube(NewCube, Slots[Num + i]);
                            NewCube.transform.position = Slots[Num + i - 1].transform.position;
                            Slots[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(Slots[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }
        }



        if (FirstEvent == true)
        {
            isMatched = false;
            state = State.Ready;
        }

    }

    public void BT_ChangeDirection(int _Num)
    {
        direction = (Direction)_Num;
        thePlayer.ChangeSprite(direction);
    }


}
