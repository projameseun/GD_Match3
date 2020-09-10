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
        PuzzleMaker.Instance.m_blockType = BlockList[SelectNum].GetComponent<Block>().blockType ;
        ChangeBlock();
    }


    // 선택한 아이템을 넣는다
    public void ClickItem(PuzzleSlot _slot)
    {

        // 기본 블럭
        if (SelectType == SlotBaseType.Block)
        {
            _slot.block.blockType = PuzzleMaker.Instance.m_blockType;

            if ((int)PuzzleMaker.Instance.m_NodeColor <= 4)
            {
                _slot.m_Image.sprite = 
                    BlockList[SelectNum].GetComponent<Block>().m_BasicSprite[(int)PuzzleMaker.Instance.m_NodeColor];
                _slot.m_Image.color = new Color(1,1,1, 1);
            }
            else if (PuzzleMaker.Instance.m_NodeColor == NodeColor.NC5_Random)
            {
                _slot.m_Image.sprite = BlockList[SelectNum].GetComponent<Block>().m_BasicSprite[0];
                _slot.m_Image.color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            else
            {
                Debug.Log("여기 들어오면 안됨");
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
            PuzzleMaker.Instance.m_blockType = BlockList[SelectNum].GetComponent<Block>().blockType;

            ChangeBlock();
        }
        else if (SelectType == SlotBaseType.Panel)
        {
            PuzzleMaker.Instance.m_PanelCh[SelectNum] = true;
            PanelImageList[SelectNum % 10].ItemOnOff(true);
            CurrentPanelType = PanelList[SelectNum].GetComponent<Panel>().panelType;
        }
    }

    public void ChangeBlock()
    {
        switch (PuzzleMaker.Instance.m_blockType)
        {
            case BlockType.Cube:
                PuzzleMaker.Instance.m_NodeColor = NodeColor.NC5_Random;

                break;
        }
    }


}
