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

    public BlockType m_blockType;
    public NodeColor m_BlockColor;
    public int m_BlockCount;


    public PanelType UpPanel;
    public NodeColor m_UpColor;
    public int m_UpCount;

    public PanelType MiddlePanel;
    public NodeColor m_MiddleColor;
    public int m_MiddleCount;


    public PanelType DownPanel;
    public NodeColor m_DownColor;
    public int m_DownCount;





    public override void SetSlot(SlotInfo _Info)
    {
        m_blockType = (BlockType)_Info.BlockType;
        UpPanel = (PanelType)_Info.UpPanelType;
        MiddlePanel = (PanelType)_Info.MiddlePanelType;
        DownPanel = (PanelType)_Info.DownPanelType;

        m_BlockColor = (NodeColor)_Info.m_BlockColor;
        m_UpColor = (NodeColor)_Info.m_UpColor;
        m_MiddleColor = (NodeColor)_Info.m_MiddleColor;
        m_DownColor = (NodeColor)_Info.m_DownColor;


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

        m_BlockColor = NodeColor.NC6_Null;
        m_UpColor = NodeColor.NC6_Null;
        m_MiddleColor = NodeColor.NC6_Null;
        m_DownColor = NodeColor.NC6_Null;

        m_UpCount = 0;
        m_MiddleCount = 0;
        m_DownCount = 0;

        m_BlockImage.sprite = null;
        m_BlockImage.enabled = false;
        m_BlockImage.color = new Color(1, 1, 1, 1);

        m_UpImage.sprite = null;
        m_UpImage.enabled = false;
        m_UpImage.color = new Color(1, 1, 1, 1);

        m_MiddleImage.sprite = null;
        m_MiddleImage.enabled = false;
        m_MiddleImage.color = new Color(1, 1, 1, 1);

        m_DownImage.sprite = null;
        m_DownImage.enabled = false;
        m_DownImage.color = new Color(1, 1, 1, 1);
    }


}
