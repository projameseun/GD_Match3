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
        for (int i = 0; i < BlockManager.Instance.blockLists.Count; i++)
        {
            BlockList.Add(BlockManager.Instance.blockLists[i].Block);
        }
        for (int i = 0; i < PanelManager.Instance.panelLists.Count; i++)
        {
            PanelList.Add(PanelManager.Instance.panelLists[i].Panel);
        }


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
        if (Input.GetMouseButton(1))
        {
            _slot.Resetting();

            string[] RanCube = { "0", "5" };

            ChangeBlockImage(_slot, RanCube);
        }
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
        if (int.Parse(_slot.slotInfo.MiddlePanelData[0]) == (int)PanelType.PT0_BackPanel)
            return;

        ChangeBlockImage(_slot, PuzzleMaker.Instance.SlotData);



    }

    



    public void PanelItem(EditorSlot _slot)
    {

        
        ChangePanelImage(_slot, PuzzleMaker.Instance.SlotData);

    }

    public void ChangeBlockImage(EditorSlot _slot,string[] _Data)
    {
        _slot.m_Image.enabled = false;
        switch ((BlockType)(int.Parse(_Data[0])))
        {
            case BlockType.Null:
                _slot.slotInfo.BlockData = new string[] { "-1" };
                break;


            case BlockType.BT0_Cube:

                _slot.slotInfo.BlockData = _Data;
                if (int.Parse(_slot.slotInfo.BlockData[1]) <= 4)
                {

                    _slot.m_BlockImage.sprite =
                        BlockList[SelectNum].GetComponent<Block>().m_sprite[int.Parse(_slot.slotInfo.BlockData[1])];
                    _slot.m_BlockImage.color = new Color(1, 1, 1, 1);
                    _slot.m_BlockImage.enabled = true;
                }
                else if (int.Parse(_slot.slotInfo.BlockData[1]) == (int)NodeColor.NC5_Random)
                {

                    _slot.m_BlockImage.sprite = BlockList[SelectNum].GetComponent<Block>().m_sprite[0];
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

    public void ChangePanelImage(EditorSlot _slot,string[] Data)
    {
        _slot.m_Image.enabled = false;
        switch ((PanelType)int.Parse(Data[0]))
        {
            case PanelType.Null:

                break;

            //0 백판넬
            case PanelType.PT0_BackPanel:
                _slot.Resetting();
                _slot.slotInfo.MiddlePanelData = Data;
                _slot.m_MiddleImage.sprite =
                PanelList[0].GetComponent<Panel>().m_sprite[int.Parse(Data[1])];

               
                _slot.m_MiddleImage.enabled = true;
                if (int.Parse(Data[1]) == 0)
                {
                    _slot.m_MiddleImage.color = new Color(0, 0, 1, 1);
                }else
                    _slot.m_MiddleImage.color = new Color(1, 1, 1, 1);
                break;

            case PanelType.PT1_Portal:
                _slot.slotInfo.UpPanelData = Data;
                _slot.m_MiddleImage.sprite =
                PanelList[0].GetComponent<Panel>().m_sprite[0];
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

    }



    //베이스를 바꾼다
    public void ChangeBlock()
    {
        switch (PuzzleMaker.Instance.m_blockType)
        {
            case BlockType.BT0_Cube:
                PuzzleMaker.Instance.m_CubeCh = true;
                for (int i = 0; i < BlockImageList.Length; i++)
                {
                    if (i < 5)
                    {
                        BlockImageList[i].m_ItemImage.sprite = BlockList[SelectNum].GetComponent<Block>().m_sprite[i];
                    }
                    else if (i == 5)
                    {
                        BlockImageList[i].m_ItemImage.sprite = BlockList[SelectNum].GetComponent<Block>().m_sprite[0];
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
            case PanelType.PT0_BackPanel:
                PuzzleMaker.Instance.m_BackPanelCh = true;
                PuzzleMaker.Instance.m_Count = 0;
                break;
            case PanelType.PT1_Portal:
                PuzzleMaker.Instance.m_PortalCh = true;
                PuzzleMaker.Instance.m_Count = 0;
                break;
        }
    }




}
