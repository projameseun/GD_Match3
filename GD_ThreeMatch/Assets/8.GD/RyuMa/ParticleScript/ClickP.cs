using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickP : ParticleManager
{
    public override void Resetting()
    {
        base.Resetting();
        theObject.ClickParticles.Enqueue(this.gameObject);
    }
}
