using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Cube : Block
{


    public override void Init(PuzzleSlot _slot, string[] Data)
    {
        base.Init(_slot, Data);
        if (Data != null)
        {
            NodeColor color = (NodeColor)int.Parse(Data[1]);
            nodeColor = color;
            if (color != NodeColor.NC5_Random)
            {
                SetColor(color);
            }
        }
        else
        {
            SetRandomColor();
        }
      

    }

    // trunk


    public override void BurstEvent(PuzzleSlot _Slot, Action action = null)
    {
        base.BurstEvent(_Slot, action);

        _Slot.m_Block = null;

        EffectManager.Instance.CubeEffect(this.transform.position, (int)nodeColor);

        PuzzleManager.Instance.EventUpdate(0.2f);


        Resetting();
    }






}
