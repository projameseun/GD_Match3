using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : A_Singleton<EffectManager>
{
    public GameObject PF_CubeEffect;




    ObjectManager theObject;

    private void Start()
    {
        theObject = ObjectManager.Instance;
    }




    public void CubeEffect(Vector2 vec, int index)
    {
        ParticleManager Effect = theObject.FindObj<ParticleManager>(PF_CubeEffect);
        Effect.gameObject.transform.position = vec;

        Effect.ParticleSetting(false, 1, index, null);
    }



}
