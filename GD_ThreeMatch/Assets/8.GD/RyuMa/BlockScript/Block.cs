using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;








public class Block : MonoBehaviour
{
    public BlockType blockType;
    public NodeColor nodeColor;

    public int Num;
    bool OnlyOneEvent;
    Vector2 TargetVec;
    float Speed;

    virtual protected void Awake()
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


    // 블럭이 파괴하는 
    virtual public void DestroyCube()
    {


    }


    virtual public void Resetting()
    { 
    
    }












}
