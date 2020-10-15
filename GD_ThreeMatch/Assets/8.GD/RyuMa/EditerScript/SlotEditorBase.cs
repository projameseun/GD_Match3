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


        ChangeBlockImage(_slot, PuzzleMaker.Instance.m_blockType, PuzzleMaker.Instance.m_NodeColor);



    }


    public void PanelItem(EditorSlot _slot)
    {

        
        ChangePanelImage(_slot, PuzzleMaker.Instance.m_PanelType, PuzzleMaker.Instance.m_Count);

    }

    public void ChangeBlockImage(EditorSlot _slot, BlockType _type, NodeColor _Color)
    {
        _slot.m_Image.enabled = false;
        switch (_type)
        {
            case BlockType.Cube:


                _slot.m_blockType = _type;
                _slot.m_Color = _Color;
                if ((int)_slot.m_Color <= 4)
                {

                    _slot.m_BlockImage.sprite =
                        BlockList[SelectNum].GetComponent<Block>().m_BasicSprite[(int)_slot.m_Color];
                    _slot.m_BlockImage.color = new Color(1, 1, 1, 1);
                    _slot.m_BlockImage.enabled = true;
                }
                else if (_slot.m_Color == NodeColor.NC5_Random)
                {

                    _slot.m_BlockImage.sprite = BlockList[SelectNum].GetComponent<Block>().m_BasicSprite[0];
                    _slot.m_BlockImage.color = new Color(0.5f, 0.5f, 0.5f, 1);
                    _slot.m_BlockImage.enabled = true;
                }
                else
                {
                    Debug.Log("여기 들어오면 안됨");
                }
                break;
        }





    }

    public void ChangePanelImage(EditorSlot _slot,PanelType _Type ,int _Count)
    {
        _slot.m_Image.enabled = false;
        switch (_Type)
        {
            //0 백판넬
            case PanelType.BackPanel:
                _slot.Resetting();
                _slot.MiddlePanel = _Type;
                _slot.m_MiddleCount = _Count;

                _slot.m_MiddleImage.sprite =
                PanelList[0].GetComponent<Panel>().m_sprite[_Count];

               
                _slot.m_MiddleImage.enabled = true;
                if (_Count == 0)
                {
                    _slot.m_MiddleImage.color = new Color(0, 0, 1, 1);
                }else
                    _slot.m_MiddleImage.color = new Color(1, 1, 1, 1);
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
