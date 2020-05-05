using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterState
{
    NULL = 0,
    Alice,
    Beryl
}


public class GirlSel : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation = null;
    public SkeletonDataAsset data_Alice;
    public SkeletonDataAsset data_Beryl;

    public Button aliceBtn;
    public Button BerylBtn;
    public CharacterState characterState = CharacterState.NULL;

    // Update is called once per frame
    void Start()
    {
        aliceBtn.onClick.AddListener(() =>
        {
            characterState = CharacterState.Alice;
            skeletonAnimation.skeletonDataAsset = data_Alice;
        });

        BerylBtn.onClick.AddListener(() =>
        {
            characterState = CharacterState.Beryl;
            skeletonAnimation.skeletonDataAsset = data_Beryl;
        });
    }

    
}

