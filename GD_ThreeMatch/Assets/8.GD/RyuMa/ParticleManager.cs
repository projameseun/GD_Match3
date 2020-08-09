using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    private ParticleSystem particle;
    GameObject parent;
    bool Loop = false;
    float ParticleTime = 0;

    protected ObjectManager theObject;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }



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

    virtual public void ParticleSetting(bool loop, GameObject _Parent = null, float _time = 3f)
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
    virtual public void Resetting()
    {
        parent = null;
        Loop = false;
        ParticleTime = 0;
        var main = particle.main;
        main.loop = false;
        this.gameObject.SetActive(false);
    }

}
