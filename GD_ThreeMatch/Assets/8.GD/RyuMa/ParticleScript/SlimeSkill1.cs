using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSkill1 : ParticleManager
{
    public override void Resetting()
    {
        base.Resetting();
        theObject.SlimeAttackParticles.Enqueue(this.gameObject);
    }
}
