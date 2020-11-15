using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rock : Block
{


    public override void Init(PuzzleSlot _slot, string[] Data)
    {
        base.Init(_slot, Data);



        m_Value = int.Parse(Data[1]);



    }


    public override void BurstEvent(PuzzleSlot _Slot, Action action = null)
    {
        base.BurstEvent(_Slot, action);

        m_Value--;

        if (m_Value <= 0)
            Resetting();



    }




    public override void Resetting()
    {
        m_Slot.m_Block = null;

        base.Resetting();


    }


}
