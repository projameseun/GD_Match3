using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using static HappyRyuMa.GameMaker;



public class SlotManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{




    public bool SlotDown;
    public int m_SelectNum;
    int X, Y;

    public Vector2 ClickVec;
    public Vector2 CurrentVec;
    public Vector2 BaseVec; // 매니저의 위치값을 빼서 0, 0 좌표 기준으로 잡는다


    virtual protected void Start()
    {
        
    }




    virtual public void OnPointerDown(PointerEventData eventData)
    {
        if (SlotDown)
            return;
        if (CheckClickVec(Camera.main.ScreenToWorldPoint(eventData.position)))
            return;
        CurrentVec = Camera.main.ScreenToWorldPoint(eventData.position);

        DownAction();
    }

    virtual public void DownAction()
    { 
    
    }


    virtual public void OnDrag(PointerEventData eventData)
    {
        if (SlotDown == false)
            return;

        CurrentVec = Camera.main.ScreenToWorldPoint(eventData.position);
        DragAction();
    }

    virtual public void DragAction()
    {

    }
    virtual public void OnPointerUp(PointerEventData eventData)
    {
        if (SlotDown)
        {

            SlotDown = false;
            CurrentVec = Camera.main.ScreenToWorldPoint(eventData.position);
        }

        UpAction();

    }

    virtual public void UpAction()
    { 
    
    }


    
    virtual public bool CheckClickVec(Vector2 CllicVec)
    {
        ClickVec = CllicVec - BaseVec;
        if (ClickVec.x < 0 || ClickVec.y > 0)
        {
            ClickVec.x = 0;
            ClickVec.y = 0;
            return true;
        }
        X = (int)(ClickVec.x / 0.6f);
        Y = (int)(ClickVec.y / 0.6f);
        m_SelectNum = X -(Y * MatchBase.MaxHorizon);

        ClickVec.x = 0.3f + 0.6f * X;
        ClickVec.y = -0.3f + 0.6f * Y;
        return false;
    }




}
