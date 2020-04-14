using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class SpineManager : MonoBehaviour
{
    public SkeletonAnimation animation;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animation.timeScale = 10;
            animation.loop = true;
            animation.AnimationName = "Idle_Clip";


        }


    }
}
