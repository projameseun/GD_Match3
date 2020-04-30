using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlSel : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public SkeletonDataAsset data_Alice;
    public SkeletonDataAsset data_Beryl;

    // Update is called once per frame
    void Start()
    {
        if (skeletonAnimation.skeletonDataAsset != null)
        {
            if (skeletonAnimation.skeletonDataAsset == data_Alice)
            {
                Debug.Log("앨리스 출력");
            }
        }
        if (skeletonAnimation.skeletonDataAsset != null)
        {
            if (skeletonAnimation.skeletonDataAsset == data_Beryl)
            {
                Debug.Log("배릴 출력");
            }
        }

    }
}
