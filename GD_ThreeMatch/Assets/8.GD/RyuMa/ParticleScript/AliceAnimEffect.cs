using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceAnimEffect : ParticleManager
{



    public override void Resetting()
    {
        base.Resetting();
        theObject.AliceAnimEffects.Enqueue(this.gameObject);
    }


}
