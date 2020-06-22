using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSkill2 : ParticleManager
{


    public override void Resetting()
    {
        base.Resetting();
        theObject.SlimeSkillParticles.Enqueue(this.gameObject);
    }

}
