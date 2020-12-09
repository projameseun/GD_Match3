using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cage : Panel
{




    public override void BurstEvent(PuzzleSlot slot, Action action = null)
    {
          Resetting();
    }



    public override void Resetting()
    {
        base.Resetting();
    }




}
