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



    public override void DownAction()
    {
        base.DownAction();
    }

    public override void DragAction()
    {
        base.DragAction();
    }


    public override void UpAction()
    {
        base.UpAction();
    }



    public void CheckSlot()
    { 
        



    }


}
