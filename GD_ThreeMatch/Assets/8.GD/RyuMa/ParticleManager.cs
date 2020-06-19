using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleName particleName;

    public ParticleSystem particle;
    GameObject parent;
    bool Loop = false;
    float ParticleTime = 0;

    private ObjectManager theObject;

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
        if (theObject == null)
            theObject = FindObjectOfType<ObjectManager>();
        var main = particle.main;
        ParticleTime = _time;
        if (_Parent != null)
        {
            parent = _Parent;
            this.transform.parent = _Parent.transform;
            this.transform.position = _Parent.transform.position;

        }
        Loop = loop;
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

        switch (particleName)
        {
            case ParticleName.PN0_CubeP:
                theObject.CubeParticles.Enqueue(this.gameObject);
                break;
            case ParticleName.PN1_AliceSkill:
                theObject.AliceSkills.Enqueue(this.gameObject);
                break;
            case ParticleName.PN2_AliceAnimEffect:
                theObject.AliceAnimEffects.Enqueue(this.gameObject);
                break;
            case ParticleName.PN3_SlimeSkill:
                theObject.SlimeSkillParticles.Enqueue(this.gameObject);
                break;
            case ParticleName.PN3_SlimeSkill2:
                theObject.SlimeAttackParticles.Enqueue(this.gameObject);
                break;
            case ParticleName.PN4_ClickP:
                theObject.ClickParticles.Enqueue(this.gameObject);
                break;
            case ParticleName.PN5_Portal:
                theObject.Portals.Enqueue(this.gameObject);
                break;
        }
    }

}
