using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : ParticleManager
{

    public ParticleSystem[] Particles;


    public override void ParticleSetting(bool loop = false, float _time = 2, int Index = 0, GameObject _Parent = null)
    {

        SetColor((NodeColor)Index);

        base.ParticleSetting(loop, _time, Index, _Parent);
    }



    public void SetColor(NodeColor _Nod)
    {

        if (_Nod == NodeColor.NC0_Blue)
        {
            var Par1 = Particles[0].main;
            Par1.startColor = new Color(0.25f, 0.37f, 1, 0.611f);

            var Par2 = Particles[1].main;
            Par2.startColor = new Color(0.36f, 0.33f, 1f, 1);

            var Par3 = Particles[2].main;
            Par3.startColor = new Color(0.6f, 0.65f, 1, 0.61f);

            var Par4 = Particles[3].main;
            Par4.startColor = new Color(0.17f, 0.15f, 1, 1);

            var Par5 = Particles[4].main;
            Par5.startColor = new Color(0.1f, 0, 0.31f, 0.56f);
        }
        else if (_Nod == NodeColor.NC1_Green)
        {
            var Par1 = Particles[0].main;
            Par1.startColor = new Color(0.4f, 1, 0.22f, 0.6f);

            var Par2 = Particles[1].main;
            Par2.startColor = new Color(0.6f, 1, 0.6f, 1);

            var Par3 = Particles[2].main;
            Par3.startColor = new Color(0.62f, 1, 0.607f, 0.6f);

            var Par4 = Particles[3].main;
            Par4.startColor = new Color(0.15f, 1, 0.1f,1);

            var Par5 = Particles[4].main;
            Par5.startColor = new Color(0, 0.31f, 0, 0.56f);
        }
        else if (_Nod == NodeColor.NC2_Pink)
        {
            var Par1 = Particles[0].main;
            Par1.startColor = new Color(1, 0.29f, 0.82f, 0.6f);

            var Par2 = Particles[1].main;
            Par2.startColor = new Color(1, 0.58f, 0.9f, 1);

            var Par3 = Particles[2].main;
            Par3.startColor = new Color(1, 0.6f, 0.92f, 0.6f);

            var Par4 = Particles[3].main;
            Par4.startColor = new Color(1, 0.3f, 0.8f, 0.6f);

            var Par5 = Particles[4].main;
            Par5.startColor = new Color(0.3f, 0, 0.2f, 0.48f);

        }
        else if (_Nod == NodeColor.NC3_Red)
        {
            var Par1 = Particles[0].main;
            Par1.startColor = new Color(1, 0.15f, 0.18f, 0.6f);

            var Par2 = Particles[1].main;
            Par2.startColor = new Color(1, 0.3f, 0.31f, 1);

            var Par3 = Particles[2].main;
            Par3.startColor = new Color(1, 0.6f, 0.6f, 0.6f);

            var Par4 = Particles[3].main;
            Par4.startColor = new Color(1, 0.12f, 0.18f, 1);

            var Par5 = Particles[4].main;
            Par5.startColor = new Color(0.3f, 0, 0, 0.56f);
        }
        else if (_Nod == NodeColor.NC4_Yellow)
        {
            var Par1 = Particles[0].main;
            Par1.startColor = new Color(1,1, 0.12f, 0.61f);

            var Par2 = Particles[1].main;
            Par2.startColor = new Color(1, 1, 0.3f, 1);

            var Par3 = Particles[2].main;
            Par3.startColor = new Color(1, 1, 0.6f, 0.6f);

            var Par4 = Particles[3].main;
            Par4.startColor = new Color(1, 0.97f, 0.11f, 1);

            var Par5 = Particles[4].main;
            Par5.startColor = new Color(0.33f, 0.27f, 0, 0.56f);
        }
    }



}
