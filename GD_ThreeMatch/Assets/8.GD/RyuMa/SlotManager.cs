using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static HappyRyuMa.GameMaker;

public class SlotManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Vector2 ClickVec;
    public Vector2 CurrentVec;



    public void OnPointerDown(PointerEventData eventData)
    {
        if (PuzzleManager.Instance.SlotDown == true)
            return;
        ClickVec = Camera.main.ScreenToWorldPoint(eventData.position);
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
        if (PuzzleManager.Instance.SlotDown == true)
        {

            PuzzleManager.Instance.SlotDown = false;
            CurrentVec = Camera.main.ScreenToWorldPoint(eventData.position);
        }

       
    }




}
