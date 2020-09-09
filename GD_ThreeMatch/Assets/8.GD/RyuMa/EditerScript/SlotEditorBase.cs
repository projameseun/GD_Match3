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
    public BlockType CurrentBlockType;
    public PanelType CurrentPanelType;
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
                PanelImageList[i].SetItem(CopyPanel.m_spriteRen[0].sprite, i,
                   SlotBaseType.Panel);
            }
            else
            {
                PanelImageList[i].SetItem(null, i, SlotBaseType.Null);
            }

        }
        PuzzleMaker.Instance.m_BlockCh[SelectNum] = true;
        BlockImageList[0].ItemOnOff(true);
        CurrentBlockType = BlockList[SelectNum].GetComponent<Block>().blockType;
        PuzzleMaker.Instance.m_Block = BlockList[SelectNum].GetComponent<Block>();
    }


    // 선택한 아이템을 넣는다
    public void ClickItem(PuzzleSlot _slot)
    {

        if (SelectType == SlotBaseType.Block)
        {
            _slot.block = PuzzleMaker.Instance.m_Block;

            if ((int)_slot.block.nodeColor <= 4)
            {
                _slot.m_Image.sprite = _slot.block.m_BasicSprite[(int)_slot.block.nodeColor];
            }
            else if (_slot.block.nodeColor == NodeColor.NC5_Random)
            {
                _slot.m_Image.sprite = _slot.block.m_BasicSprite[Random.Range(0, 5)];
            }
            else
            {
                _slot.m_Image.sprite = _slot.block.m_BasicSprite[0];
            }





        } 
        
        else if (SelectType == SlotBaseType.Panel)
        { 
        
        }

       


    }







    public void SelectItem(int _Num, SlotBaseType _Type)
    {
        if (SelectNum == _Num && _Type == SelectType)
            return;

        if (SelectType == SlotBaseType.Block)
        {
            PuzzleMaker.Instance.m_BlockCh[SelectNum] = false;
            BlockImageList[SelectNum % 10].ItemOnOff(false);
        }
        else if (SelectType == SlotBaseType.Panel)
        {
            PuzzleMaker.Instance.m_PanelCh[SelectNum] = false;
            PanelImageList[SelectNum % 10].ItemOnOff(false);
        }
        SelectNum = _Num;
        SelectType = _Type;

        if (SelectType == SlotBaseType.Block)
        {
            PuzzleMaker.Instance.m_BlockCh[SelectNum] = true;
            BlockImageList[SelectNum % 10].ItemOnOff(true);
            CurrentBlockType = BlockList[SelectNum].GetComponent<Block>().blockType;
            PuzzleMaker.Instance.m_Block = BlockList[SelectNum].GetComponent<Block>();
        }
        else if (SelectType == SlotBaseType.Panel)
        {
            PuzzleMaker.Instance.m_PanelCh[SelectNum] = true;
            PanelImageList[SelectNum % 10].ItemOnOff(true);
            CurrentPanelType = PanelList[SelectNum].GetComponent<Panel>().panelType;
        }
    }
}
