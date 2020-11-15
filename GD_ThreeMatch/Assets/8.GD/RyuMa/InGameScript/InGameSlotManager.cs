using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static HappyRyuMa.GameMaker;
public class InGameSlotManager : SlotManager
{



    PuzzleManager thePuzzle;


    protected override void Start()
    {
        base.Start();
        thePuzzle = PuzzleManager.Instance;
    }




    public override void OnPointerDown(PointerEventData eventData)
    {
        if (PuzzleManager.Instance.state != PuzzleManager.State.Ready)
            return;

        base.OnPointerDown(eventData);
    }
    public override void DownAction()
    {
        if (CheckSlotNum(thePuzzle.GetMap()))
        {
            return;
        }
        thePuzzle.SlotDown = true;
        thePuzzle.SelectNum = m_SelectNum;


    }



    public override void OnDrag(PointerEventData eventData)
    {
        if (thePuzzle.state != PuzzleManager.State.Ready || thePuzzle.SlotDown == false)
            return;
        base.OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (thePuzzle.state != PuzzleManager.State.Ready || thePuzzle.SlotDown == false)
            return;

        base.OnPointerUp(eventData);
    }

    public override void UpAction()
    {

        // 스위치 이밴트
        SwitchEvent();
    }


    #region 스위치 이밴트
    public void SwitchEvent()
    {
        thePuzzle.SlotDown = false;
        float Dis = Vector2.Distance(ClickVec, CurrentVec);

        if (Dis < 0.3f)
            return;

        float AngleZ = GetAngleZ(CurrentVec, ClickVec);
        Direction CurrentDir = Direction.Up;
        if (AngleZ <= 45 || AngleZ >= 315)      //위
        {
            CurrentDir = Direction.Up;
        }
        else if (AngleZ > 45 && AngleZ < 135)   //왼쪽
        {
            CurrentDir = Direction.Left;
        }
        else if (AngleZ >= 135 && AngleZ <= 225)//아래
        {
            CurrentDir = Direction.Down;
        }
        else                                    //오른쪽
        {
            CurrentDir = Direction.Right;
        }
        PuzzleSlot thisSlot = thePuzzle.GetMap().Slots[m_SelectNum];
        PuzzleSlot otherSlot = thePuzzle.GetMap().Slots[m_SelectNum].GetSlot(CurrentDir);
        thePuzzle.OtherNum = otherSlot.SlotNum;
        if (otherSlot == null)
            return;

        if (otherSlot.CheckSwitch() == false)
            return;
        thePuzzle.state = PuzzleManager.State.Switching;
        thePuzzle.EventUpdate(MatchBase.BlockSpeed);
        thisSlot.SwitchBlock(otherSlot);

    }

    #endregion

}
