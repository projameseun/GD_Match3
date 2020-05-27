using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    public ParticleSystem particle;
    GameObject parent;
    bool Loop = false;
    float ParticleTime = 0;



    // Update is called once per frame
    void Update()
    {
        if (Loop == false)
        {
            if (ParticleTime > 0)
            {
                ParticleTime -= Time.deltaTime;
            }
            else
            {
                ParticleTime = 0;
                Resetting();
            }
        }
    }

    public void ParticleSetting(bool loop, GameObject _Parent = null, float _time = 3f)
    {
        var main = particle.main;
        ParticleTime = _time;
        if (_Parent != null)
        {
            parent = _Parent;
            this.transform.position = _Parent.transform.position;

        }
        main.loop = loop;
        this.gameObject.SetActive(true);
        particle.Play();
    }
    public void Resetting()
    {
        parent = null;
        Loop = false;
        ParticleTime = 0;
        var main = particle.main;
        main.loop = false;
        this.gameObject.SetActive(false);

    }

}
