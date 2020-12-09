using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Panel
{



    public override void Init(PuzzleSlot _slot, string[] Data)
    {
        base.Init(_slot, Data);

        m_Count = int.Parse(Data[1]);

    }


    public override void BurstEvent(PuzzleSlot slot, Action action = null)
    {
        m_Count--;
        if (m_Count <= 0)
            Resetting();

    }



    public override void Resetting()
    {
        base.Resetting();
    }



}
