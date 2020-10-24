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
        SlotDown = true;
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


    // 클릭한 위치가 맵 안쪽인지 체크, 안쪽이면 번호 
    virtual public bool CheckClickVec(Vector2 CllicVec)
    {
        ClickVec = CllicVec - BaseVec;
        if (ClickVec.x < 0 || ClickVec.y > 0)
        {
            ClickVec.x = 0;
            ClickVec.y = 0;
            m_SelectNum = 0;
            return true;
        }
        X = (int)(ClickVec.x / MatchBase.CellDistance);
        Y = (int)(ClickVec.y / MatchBase.CellDistance);
        m_SelectNum = X -(Y * MatchBase.MaxHorizon);

        ClickVec.x = (MatchBase.CellDistance / 2) + (MatchBase.CellDistance * X);
        ClickVec.y = -(MatchBase.CellDistance /2) + (MatchBase.CellDistance * Y);
        return false;
    }


    // 사용할 맵에 누른 번호를 체크
    virtual public bool CheckSlotNum(MapManager _map)
    {

        if (m_SelectNum % MatchBase.MaxHorizon <= _map.TopRight &&
            m_SelectNum <= _map.BottomRight)
        {
            return false;
        }
        return true;
    }



}
