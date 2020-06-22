using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceSkills : ParticleManager
{


    public override void Resetting()
    {
        base.Resetting();
        theObject.AliceSkills.Enqueue(this.gameObject);
    }
}
