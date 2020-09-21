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
    public int SelectNum;  //선택한 번호


    public int BlockListCount;
    public List<GameObject> BlockList;
    public int PanelListCount;
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
        PuzzleMaker.Instance.m_CubeCh = true;
        BlockImageList[0].ItemOnOff(true);
        CurrentBlockType = BlockList[SelectNum].GetComponent<Block>().blockType;
        PuzzleMaker.Instance.m_blockType = BlockList[SelectNum].GetComponent<Block>().blockType ;
        ChangeBlock();
    }


    // 선택한 아이템을 넣는다
    public void ClickItem(EditorSlot _slot)
    {
        _slot.m_Image.enabled = false;
        // 기본 블럭
        if (SelectType == SlotBaseType.Block)
        {

            BlockItem(_slot);
        } 
        else if (SelectType == SlotBaseType.Panel)
        {

            PanelItem(_slot);
        }

       


    }


    public void BlockItem(EditorSlot _slot)
    {
        _slot.block.blockType = PuzzleMaker.Instance.m_blockType;
        
        switch (PuzzleMaker.Instance.m_blockType)
        {
            case BlockType.Cube:

                if ((int)PuzzleMaker.Instance.m_NodeColor <= 4)
                {

                    _slot.m_BlockImage.sprite =
                        BlockList[SelectNum].GetComponent<Block>().m_BasicSprite[(int)PuzzleMaker.Instance.m_NodeColor];
                    _slot.m_BlockImage.color = new Color(1, 1, 1, 1);
                }
                else if (PuzzleMaker.Instance.m_NodeColor == NodeColor.NC5_Random)
                {

                    _slot.m_BlockImage.sprite = BlockList[SelectNum].GetComponent<Block>().m_BasicSprite[0];
                    _slot.m_BlockImage.color = new Color(0.5f, 0.5f, 0.5f, 1);
                }
                else
                {
                    Debug.Log("여기 들어오면 안됨");
                }
                break;
        }


       
    }


    public void PanelItem(EditorSlot _slot)
    {
        switch (PuzzleMaker.Instance.m_PanelType)
        {
            case PanelType.BackPanel:
                PuzzleMaker.Instance.m_BackPanelCh = true;
                _slot.ResetEditorSlot();
                _slot.MiddlePanel = PanelType.BackPanel;
                _slot.m_MiddleImage.sprite =
                PanelList[SelectNum].GetComponent<Panel>().m_sprite[PuzzleMaker.Instance.m_Count];
                break;


        }

    }



    public void SelectItem(int _Num, SlotBaseType _Type)
    {
        if (SelectNum == _Num && _Type == SelectType)
            return;

        PuzzleMaker.Instance.AllCheckOff();

        if (SelectType == SlotBaseType.Block)
        {
            BlockImageList[SelectNum % 10].ItemOnOff(false);
        }
        else if (SelectType == SlotBaseType.Panel)
        {
            PanelImageList[SelectNum % 10].ItemOnOff(false);
        }
        SelectNum = _Num;
        SelectType = _Type;
        
        if (SelectType == SlotBaseType.Block)
        {

            BlockImageList[SelectNum % 10].ItemOnOff(true);
            CurrentBlockType = BlockList[SelectNum].GetComponent<Block>().blockType;
            PuzzleMaker.Instance.m_blockType = CurrentBlockType;

            ChangeBlock();
        }
        else if (SelectType == SlotBaseType.Panel)
        {
            
           PanelImageList[SelectNum % 10].ItemOnOff(true);
            CurrentPanelType = PanelList[SelectNum].GetComponent<Panel>().panelType;
            PuzzleMaker.Instance.m_PanelType = CurrentPanelType;

            ChangePanel();
        }

        Debug.Log("타입은 " + CurrentPanelType.ToString());
    }



    //베이스를 바꾼다
    public void ChangeBlock()
    {
        switch (PuzzleMaker.Instance.m_blockType)
        {
            case BlockType.Cube:
                PuzzleMaker.Instance.m_CubeCh = true;
                for (int i = 0; i < BlockImageList.Length; i++)
                {
                    if (i < 5)
                    {
                        BlockImageList[i].m_ItemImage.sprite = BlockList[SelectNum].GetComponent<Block>().m_BasicSprite[i];
                    }
                    else if (i == 5)
                    {
                        BlockImageList[i].m_ItemImage.sprite = BlockList[SelectNum].GetComponent<Block>().m_BasicSprite[0];
                    }
                    else
                    {
                        BlockImageList[i].m_ItemImage.enabled = false;
                    }
                }

                PuzzleMaker.Instance.m_NodeColor = NodeColor.NC5_Random;

                break;
        }
    }


    //베이스를 바꾼다
    public void ChangePanel()
    {
        switch (PuzzleMaker.Instance.m_PanelType)
        {
            case PanelType.BackPanel:
                PuzzleMaker.Instance.m_BackPanelCh = true;

                PuzzleMaker.Instance.m_Count = 0;
                break;
        }
    }




}
