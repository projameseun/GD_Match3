﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum BlockType
{
    Blank = -1,
    Null = 0,
    Cube,
    SpecialCube,
    Player,


}

public enum NodeColor
{
    NC0_Blue = 0,
    NC1_Green,
    NC2_Pink,
    NC3_Red,
    NC4_Yellow,
    NC5_Blank,
    
}




public class Block : MonoBehaviour
{

    public List<SpriteRenderer> m_spriteRen;



    //블럭이 무슨 블럭인지
    public BlockType blockType;

    //블럭의 색
    public NodeColor nodeColor;

    // 연계시 폭발하는지
    public bool Burst;

    // burst중인지 아닌지 체크
    public bool Bursting;

    // 반칸을 채우는지
    public bool Gravity;

    // 이동이 가능한지
    public bool CanMove;







    //Trunk
    public int Num;
    bool OnlyOneEvent;
    Vector2 TargetVec;
    float Speed;

    virtual protected void Awake()
    {
        
    }


    //처음 생성시 연출 및 시작 부분
    virtual public void Init()
    { 
    
    }


    virtual public void ChangeState(int _Num)
    { 
        
    }


    //매치및 이펙트로 터지는 이밴트
    virtual public void BurstEvent(MapManager _map, int _num)
    {

    }

    // 기본적으로 블럭을 움직이게 하는 함수
    virtual public void MoveEvent(Vector2 _vec, float _Speed, bool _Event = false)
    {
        TargetVec = _vec;
        OnlyOneEvent = _Event;
        Speed = _Speed;
        if (this.gameObject.activeSelf == true)
            StartCoroutine(MoveCor());
    }
    IEnumerator MoveCor()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DOMove(TargetVec, Speed)).SetEase(Ease.Linear);

        while (Vector2.Distance(transform.position, TargetVec) >= 0.05f)
        {
            yield return null;
        }
        transform.position = TargetVec;
        if (OnlyOneEvent == true)
        {
            OnlyOneEvent = false;
            PuzzleManager.Instance.CubeEvent = true;
        }
    }


    virtual public void Resetting()
    { 
    
    }

}
