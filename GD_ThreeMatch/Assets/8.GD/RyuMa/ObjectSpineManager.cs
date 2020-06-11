using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ObjectSpineManager : MonoBehaviour
{
    public MeshRenderer mesh;
    public SkeletonAnimation anim;

    public string AnimName;

    private PuzzleMaker theMaker;
    private ObjectManager theObject;

    private void Start()
    {
        theObject = FindObjectOfType<ObjectManager>();
        theMaker = FindObjectOfType<PuzzleMaker>();
    }
    public void SetObjectSpine(int _ObjectNum , string _SkinName)
    {
        if (theObject == null)
            theObject = FindObjectOfType<ObjectManager>();

        if (theMaker == null)
            theMaker = FindObjectOfType<PuzzleMaker>();

        AnimName = "";
        if (_SkinName != "")
        {
            anim.initialSkinName = _SkinName;
        }

        switch (theMaker.mapMainType)
        {
            case MapMainType.M0_Forest:
                mesh.material = theObject.ForestMaterial[_ObjectNum];
                anim.skeletonDataAsset = theObject.ForestData[_ObjectNum];
                break;
        }
        anim.Initialize(true);
        ChangeAnim("Idle", true);

    }

    public void ChangeAnim(string _state, bool _Loop = false)
    {
        if (_state == AnimName)
            return;
        anim.AnimationState.SetAnimation(0, _state, _Loop);
        AnimName = _state;
    }


    public void Resetting()
    {
        AnimName = "";
        
    }

}
