using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{ 
    Up = 0,
    Down,
    Left,
    Right
}


public class PuzzleManager : MonoBehaviour
{
    public Sprite[] CubeSprites;




    public bool SlotDown = false;


    public PuzzleSlot[] Slots;



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





    public void SetSlot()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].nodeType != PuzzleSlot.NodeType.Null)
            {
                GameObject Cube = theObject.FindObj("Cube");

                SetCube(Cube, Slots[i]);
                Slots[i].TestText.text = i.ToString();
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

        Debug.Log("_Num = " + _Num + "  ChangeNum = " + ChangeNum);
        ChangeCube(_Num, ChangeNum);

    }


    public void ChangeCube(int _Num, int _OtherNum)
    {
        //_Num의 큐브정보 복제
        Vector2 Vec = Slots[_Num].cube.transform.position;
        GameObject Cube = Slots[_Num].cube.gameObject;
        PuzzleSlot.NodeColor nodeColor = Slots[_Num].nodeColor;

        //_Num의 정보를 _OtherNum의 정보로 덮어쓰기
        Slots[_Num].cube.transform.position = Slots[_OtherNum].cube.transform.position;
        Slots[_Num].cube = Slots[_OtherNum].cube;
        Slots[_Num].nodeColor = Slots[_OtherNum].nodeColor;

        //_OtherNum을 복제정보로 덮어쓰기
        Slots[_OtherNum].cube.transform.position = Vec;
        Slots[_OtherNum].cube = Cube.GetComponent<Cube>();
        Slots[_OtherNum].nodeColor = nodeColor;


        
    }


}
