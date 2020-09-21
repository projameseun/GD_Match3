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





    public void ResetEditorSlot()
    {
        Debug.Log("Reset");
        UpPanel = PanelType.Null;
        MiddlePanel = PanelType.Null;
        DownPanel = PanelType.Null;
        m_blockType = BlockType.Null;
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
