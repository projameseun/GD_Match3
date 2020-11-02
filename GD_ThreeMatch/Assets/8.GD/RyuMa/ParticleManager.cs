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

    virtual public void ParticleSetting(bool loop = false, float _time = 2f,int Index = 0, GameObject _Parent = null)
    {
        if (theObject == null)
            theObject = ObjectManager.Instance;
        if(particle == null)
            particle = GetComponent<ParticleSystem>();

        var main = particle.main;
        Loop = loop;
        main.loop = loop;
        ParticleTime = _time;

        if (_Parent != null)
        {
            parent = _Parent;
            this.transform.parent = _Parent.transform;
            this.transform.position = _Parent.transform.position;

        }

        this.gameObject.SetActive(true);
        particle.Play();

        if (loop == false)
        {
            StartCoroutine(NotLoop());
        }

    }



    virtual public IEnumerator NotLoop()
    {
        while (ParticleTime > 0)
        {
            ParticleTime -= Time.deltaTime;
            yield return null;
        }
        ParticleTime = 0;
        Resetting();
    }


    virtual public void Resetting()
    {
      

        parent = null;
        Loop = false;
        ParticleTime = 0;
        var main = particle.main;
        main.loop = false;
        ObjectManager.Instance.ResetObj(this.gameObject);
    }

}
