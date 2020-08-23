using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static HappyRyuMa.GameMaker;

public class SlotManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{


    public bool SlotDown;
    public int SelectNum;
    int X, Y;

    public Vector2 ClickVec;
    public Vector2 CurrentVec;
    public Vector2 BaseVec;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (SlotDown)
            return;
        if (CheckClickVec(Camera.main.ScreenToWorldPoint(eventData.position)))
            return;

   

        CurrentVec = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PuzzleManager.Instance.SlotDown == true)
        {
            CurrentVec = Camera.main.ScreenToWorldPoint(eventData.position);
        }
            
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if (SlotDown)
        {

            SlotDown = false;
            CurrentVec = Camera.main.ScreenToWorldPoint(eventData.position);
        }

       
    }





    public bool CheckClickVec(Vector2 CllicVec)
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
        SelectNum = X -(Y * GameManager.Instance.MaxHorizon);

        ClickVec.x = 0.3f + 0.6f * X;
        ClickVec.y = -0.3f + 0.6f * Y;




        return false;
    }




}
