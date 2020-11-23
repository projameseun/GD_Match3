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

    public SlotInfo slotInfo;





    public override void SetSlot(SlotInfo _Info)
    {
        slotInfo = _Info;
    }



    public override void Resetting()
    {
        slotInfo.BlockData = new string[] { "-1" };
        slotInfo.UpPanelData = new string[] { "-1" };
        slotInfo.MiddlePanelData = new string[] { "-1" };
        slotInfo.DownPanelData = new string[] { "-1" };

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

    public void BlockResetting()
    {
        slotInfo.BlockData = new string[] { "-1" };
        m_BlockImage.sprite = null;
        m_BlockImage.enabled = false;
        m_BlockImage.color = new Color(1, 1, 1, 1);
    }



}
