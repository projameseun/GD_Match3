using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
public class Character : Block
{
    //character
    public MeshRenderer SpinMesh;
    public SkeletonAnimation anim;

    public int Hp;

    Direction direction;

    string CurrentAnim = "";


    public override void Init(PuzzleSlot _slot, string[] Data)
    {
        base.Init(_slot, Data);

        anim.Initialize(true);
        anim.state.Event += HandleEvent;

    }




    virtual public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        //if (e.Data.Name == "Attack_Sound")
        //{



        //}
    }


    public void ChangeDirection(Direction _direction)
    {
    }



    public void ChangeAnim(string _state, bool _Loop = false)
    {
        if (_state == CurrentAnim)
            return;


        anim.AnimationState.SetAnimation(0, _state, _Loop);
        CurrentAnim = _state;
    }




    public override void Resetting()
    {
        




    }


}
