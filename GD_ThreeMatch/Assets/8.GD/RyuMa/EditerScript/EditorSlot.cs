using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorSlot : PuzzleSlot
{
    [Space]
    [Header("에디터")]


    public Image m_BlockImage;
    public Image m_UpImage;
    public Image m_MiddleImage;
    public Image m_DownImage;
    public PanelType UpPanel;
    public PanelType MiddlePanel;
    public PanelType DownPanel;
    public BlockType m_blockType;
    public NodeColor m_Color;
    public int m_UpCount;
    public int m_MiddleCount;
    public int m_DownCount;





    public override void SetSlot(SlotInfo _Info)
    {
        m_blockType = (BlockType)_Info.BlockType;
        UpPanel = (PanelType)_Info.UpPanelType;
        MiddlePanel = (PanelType)_Info.MiddlePanelType;
        DownPanel = (PanelType)_Info.DownPanelType;

        m_Color = (NodeColor)_Info.m_Color;
        m_UpCount = _Info.m_UpCount;
        m_MiddleCount = _Info.m_MiddleCount;
        m_DownCount = _Info.m_DownCount;

    }



    public override void Resetting()
    {

        UpPanel = PanelType.Null;
        MiddlePanel = PanelType.Null;
        DownPanel = PanelType.Null;
        m_blockType = BlockType.Null;

        m_Color = NodeColor.NC6_Null;
        m_UpCount = 0;
        m_MiddleCount = 0;
        m_DownCount = 0;

        m_BlockImage.sprite = null;
        m_BlockImage.color = new Color(1, 1, 1, 0);
        m_UpImage.sprite = null;
        m_UpImage.color = new Color(1, 1, 1, 0);
        m_MiddleImage.sprite = null;
        m_MiddleImage.color = new Color(1, 1, 1, 0);
        m_DownImage.sprite = null;
        m_DownImage.color = new Color(1, 1, 1, 0);
    }


}
