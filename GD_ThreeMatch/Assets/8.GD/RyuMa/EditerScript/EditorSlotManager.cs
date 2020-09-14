using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorSlotManager : SlotManager
{
    public GameObject BaseObj;
    [HideInInspector]
    public MapManager EditMap;

    private void Awake()
    {
        EditMap = BaseObj.GetComponent<MapManager>();
    }

    protected override void Start()
    {
        GameManager.Instance.gameMode = GameMode.Editor;
        BaseVec = BaseObj.transform.position;
    }



    public override void DownAction()
    {
        base.DownAction();

        SlotEditorBase.Instance.ClickItem(EditMap.Slots[m_SelectNum].GetComponent<EditorSlot>());


    }

    public override void DragAction()
    {
        if (CheckClickVec(CurrentVec))
            return;
        if (CheckSlotNum(EditMap))
            return;

        SlotEditorBase.Instance.ClickItem(EditMap.Slots[m_SelectNum].GetComponent<EditorSlot>());
    }


    public override void UpAction()
    {
        base.UpAction();
    }





}
