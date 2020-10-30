using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Cube : Block
{


    public override void Init(PuzzleSlot _slot, string[] Data)
    {
        base.Init(_slot, Data);
        if (Data != null)
        {
            NodeColor color = (NodeColor)int.Parse(Data[1]);
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




    public override void BurstEvent(MapManager _map, int _num)
    {
        base.BurstEvent(_map, _num);


        PuzzleManager.Instance.EventUpdate(0.5f);


        Resetting();

    }




}
