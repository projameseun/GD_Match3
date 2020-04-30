using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerCube : MonoBehaviour
{
    public SkeletonAnimation anim;
    AnimationState state;
    public string AnimName;

    public Direction direction;


    public int TrakNum;


    //색 조정

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        anim.skeleton.SetColor(new Color(0.5f, 0.5f, 0.5f));
    //    }
    //}



    public void ChangeDirection(Direction _direction)
    {

        direction = _direction;

        if (direction == Direction.Up)
        { 
            
        }
        else if (direction == Direction.Down)
        {

        }
        else if (direction == Direction.Left)
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction == Direction.Right)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }

    public void ChangeAnim(string _state,bool _Loop = false)
    {
        if (_state == AnimName)
            return;
        anim.AnimationState.SetAnimation(TrakNum, _state, _Loop);
        AnimName = _state;
    }



}
