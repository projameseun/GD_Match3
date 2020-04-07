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

    //DB
    public bool SlotDown = false;
    public bool CubeEvent = false;
    public int SelectNum = 0;
    public int OtherNum = 0;




    private ObjectManager theObject;
    private void Start()
    {
        theObject = FindObjectOfType<ObjectManager>();


        for (int i = 0; i < Slots.Length; i++)
        {
            if (i < 12 || i >= 132 || i % 12 == 0 || i %12 == 11)
            {
                Slots[i].nodeType = PuzzleSlot.NodeType.Null;
            }

            Slots[i].SlotNum = i;
        }
        
    }
    private void Update()
    {
        if (state == State.ChangeMatch)
        { 
            if(CubeEvent == true)
            {
                CubeEvent = false;

                //매치 조건이 맞는지 확인한다


                //매치가 안될경우
                ChangeCube(SelectNum, OtherNum, true);
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
                GameObject Cube = theObject.FindObj("Cube");

                SetCube(Cube, Slots[i]);
                Slots[i].TestText.text = i.ToString();
                Slots[i].cube.Num = i;
            }
        }
    }



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


    public void ChangeCube(int _Num, int _OtherNum,bool _Event = false, float Speed = 0.05f)
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
 

        for (int i = 0; i < 144; i++)
        {
            Slots[i].GetComponentInChildren<Text>().enabled = Active;
        }
    
    }

    public void BT_RandomBlank()
    {
        int rand = Random.Range(0, 144);
        if (Slots[rand].nodeType != PuzzleSlot.NodeType.Null)
        {
            Slots[rand].nodeColor = PuzzleSlot.NodeColor.Blank;
            Slots[rand].cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }

    }

    public void BT_FillBlank()
    {
        state = State.FillBlank;
        bool FirstEvent = true;
        float Speed = 0.15f;
        if (direction == Direction.Down)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int Num = 121; Num > 12; Num -= 12)
                {
                    if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        Slots[Num + i].cube.gameObject.SetActive(false);

                        if (Slots[Num + i - 12].nodeType != PuzzleSlot.NodeType.Null)
                        {
                            
                            ChangeCube(Num + i, Num + i - 12, FirstEvent, Speed);
                            if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,1);
                            SetCube(NewCube, Slots[Num + i]);
                            NewCube.transform.position = Slots[Num + i - 12].transform.position;
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
            for (int i = 0; i < 10; i++)
            {
                for (int Num = 13; Num < 131; Num += 12)
                {
                    if (Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        Slots[Num + i].cube.gameObject.SetActive(false);

                        if (Slots[Num + i + 12].nodeType != PuzzleSlot.NodeType.Null)
                        {

                            ChangeCube(Num + i, Num + i + 12, FirstEvent, Speed);
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
            for (int i = 13; i < 131; i+=12)
            {
                for (int Num = 0; Num <10; Num++)
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
            for (int i = 22; i < 131; i += 12)
            {
                for (int Num = 0; Num > -10; Num--)
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
            state = State.Ready;
        }

    }


}
