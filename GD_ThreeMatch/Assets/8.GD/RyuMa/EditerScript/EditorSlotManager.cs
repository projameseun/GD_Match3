using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorSlotManager : SlotManager
{
    public GameObject BaseObj;


    protected override void Start()
    {
        GameManager.Instance.gameMode = GameMode.Editor;
        BaseVec = BaseObj.transform.position;
    }



    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        

    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }




    public void CheckSlot()
    { 
        
    }


}
