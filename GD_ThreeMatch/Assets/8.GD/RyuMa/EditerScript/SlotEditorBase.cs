using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SlotBaseType
{ 
    Block,
    Panel,
    Null
}


public class SlotEditorBase : A_Singleton<SlotEditorBase>
{

    public ItemBase[] BlockImageList;
    public Image[] BlockSetList;
    public ItemBase[] PanelImageList;
    public Image[] PanelSetList;


    public SlotBaseType SelectType;
    public int SelectNum;


    public List<GameObject> BlockList;
    public List<GameObject> PanelList;

    Block CopyBlock;
    Panel CopyPanel;


    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i < BlockList.Count)
            {
                CopyBlock = BlockList[i].GetComponent<Block>();
                BlockImageList[i].SetItem(CopyBlock.m_spriteRen[0].sprite, i,
                   SlotBaseType.Block);
            }
            else
            {
                BlockImageList[i].SetItem(null, i, SlotBaseType.Null);
            }

            if (i < PanelList.Count)
            {
                CopyPanel = PanelList[i].GetComponent<Panel>();
                PanelImageList[i].SetItem(CopyBlock.m_spriteRen[0].sprite, i,
                   SlotBaseType.Panel);
            }
            else
            {
                PanelImageList[i].SetItem(null, i, SlotBaseType.Null);
            }

        }
        PuzzleMaker.Instance.m_Block[SelectNum] = true;
        BlockImageList[0].ItemOnOff(true);
    }




    public void SelectItem(int _Num, SlotBaseType _Type)
    {
        if (SelectNum == _Num && _Type == SelectType)
            return;

        if (SelectType == SlotBaseType.Block)
        {
            PuzzleMaker.Instance.m_Block[SelectNum] = false;
            BlockImageList[SelectNum % 10].ItemOnOff(false);
        }
        else if (SelectType == SlotBaseType.Panel)
        {
            PuzzleMaker.Instance.m_Panel[SelectNum] = false;
            PanelImageList[SelectNum % 10].ItemOnOff(false);
        }
        SelectNum = _Num;
        SelectType = _Type;

        if (SelectType == SlotBaseType.Block)
        {
            PuzzleMaker.Instance.m_Block[SelectNum] = true;
            BlockImageList[SelectNum % 10].ItemOnOff(true);
        }
        else if (SelectType == SlotBaseType.Panel)
        {
            PuzzleMaker.Instance.m_Panel[SelectNum] = true;
            PanelImageList[SelectNum % 10].ItemOnOff(true);
        }
    }
}
