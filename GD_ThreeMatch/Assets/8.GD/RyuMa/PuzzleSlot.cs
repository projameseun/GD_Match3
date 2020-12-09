using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static HappyRyuMa.GameMaker;





public enum PanelPos
{ 
    Up,
    Middle,
    Down
}



public class PuzzleSlot : MonoBehaviour
{
    public int SlotNum;

 
    //DB
    public Vector2 Vec;

    public bool Down;
    public Block m_Block = null;
    public Panel m_UpPanel = null;
    public Panel m_MiddlePanel = null;
    public Panel m_DownPanel = null;
    public List<Panel> m_PanelList = new List<Panel>();



    public bool m_isBursting = false; 






    [HideInInspector]
    public Image m_Image;
    [HideInInspector]
    public Text m_Text;


    //trunk
    Vector2 FirstVec;
    Vector2 CurrentVec;
    bool CheckCor;
    MapManager thisMap;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_Text = GetComponentInChildren<Text>();
    }



    //슬롯을 한번에 정리해준다
    public void SetSlot(MapManager _map)
    {
        thisMap = _map;

        if (m_UpPanel != null)
            m_PanelList.Add(m_UpPanel);

        if (m_MiddlePanel != null)
            m_PanelList.Add(m_MiddlePanel);

        if (m_DownPanel != null)
            m_PanelList.Add(m_DownPanel);


        for (int i = 0; i < m_PanelList.Count; i++)
        {

        }
        FindMatches.Instance.CheckSetBlock(this);
    }



    #region 체크

    //스위치가 가능하면 true
    public bool CheckSwitch()
    {
        if (m_Block == null)
            return false;

        if (m_Block.Switch == false)
            return false;
        if (m_PanelList.Find(obj => obj.m_Switch == false) != null)
        {
            return false;
        }

        return true;
    }

    //해당 슬롯을 중력에 의해 바꿀 수 있는지 최초에 확인
    public bool CheckGravityStart()
    {
        if (m_Block != null)
            return false;

        if (m_PanelList.Find(obj => obj.m_Gravity == false) != null)
        {
            return false;
        }

        return true;

    }

    //중력 확인후 서로 교환이 가능한지 확인
    public bool CheckGravityBlock(bool CheckPass)
    {
        if (m_Block == null)
            return false;
        if (m_Block.Gravity == false)
            return false;

        if (CheckPass == true)
        {
            if (m_Block.GravityJump == false)
            {
                Debug.Log("점프 가능 :" + SlotNum);
                return false;
            }
             
        }

        if (m_PanelList.Find(obj => obj.m_Gravity == false) != null)
        {
            return false;
        }
        return true;
    }

    // 해당 슬롯에 BackPanel이면 새로운 큐브를 생성
    public bool CheckBackPanel()
    {
        if (m_MiddlePanel != null)
        {
            if (m_MiddlePanel.panelType == PanelType.PT0_BackPanel)
            {
                return true;
            }
        }

        return false;
    }

    // 매치가 가능하면 true
    public bool CheckMatch()
    {
        //블럭이 없다
        if (m_Block == null)
            return false;

        //블럭이 매치를 안한다
        if (m_Block.Match == false)
            return false;

        //판넬이 블럭을 매치 못하게 한다
        if (m_PanelList.Find(obj => obj.m_Match == false) != null)
        {
            return false;
        }
        return true;
    }

    // 블럭의 색과 유무 확인
    public bool CheckBlockColor()
    {
        if (m_Block == null)
            return false;
        if ((int)m_Block.nodeColor > 4)
            return false;
        return true;
    }

    public bool CheckBlockCanBurst()
    {
        for (int i = 0; i < m_PanelList.Count; i++)
        {
            if (m_PanelList[i].m_BlockBurst == false)
                return false;
        }

        if (m_Block == null)
            return false;

        if (m_Block.Burst == false)
            return false;

        return true;
    }


    // 자기 자신은 매칭이 가능한지 먼저 채크한 후 사용한다
    public bool CheckThreeMatch(PuzzleSlot slot1, PuzzleSlot slot2)
    {
        if (slot1 == null || slot2 == null)
        {
            return false;
        }

        //먼저 두개가 매칭이 가능한지
        if (slot1.CheckMatch() == false || slot2.CheckMatch() == false)
            return false;

        //모든 블럭의 색이 매칭이 가능한지
        if (slot1.m_Block.nodeColor != m_Block.nodeColor || slot2.m_Block.nodeColor != m_Block.nodeColor)
            return false;

        return true;

    }

    public void CheckAroundBurst(Action action = null)
    {
        if (m_isBursting)
            return;
        bool CheckBlock = true;


        for (int i = 0; i < m_PanelList.Count; i++)
        {
            if (m_PanelList[i].m_BlockBurst == false)
                CheckBlock = false;
            if (m_PanelList[i].m_AroundBurst == true)
            {
                CheckBlock = m_PanelList[i].m_BlockBurst;
                
                m_PanelList[i].BurstEvent(this);
                return;
            }
        }
       
        if (m_Block != null)
        {
            if (m_Block.AroundBurst == true)
            {
                m_Block.BurstEvent(this);
            }
             
        }



    }

    #endregion
    public void CreatBlock(BlockType _blockType, string[] Data)
    {
        Block block = BlockManager.Instance.CreatBlock(_blockType);
        if (block != null)
        {
            m_Block = block;
            m_Block.Init(this, Data);
        }
        else
           m_Block = null;
    }

    public void CreatPanel(PanelPos _PanelPos, PanelType _panelType, string[] Data)
    {
        Panel panel = PanelManager.Instance.CreatePanel(_panelType);
        if (panel != null)
        {
            switch (_PanelPos)
            {
                case PanelPos.Up:
                    m_UpPanel = panel;
                    m_UpPanel.Init(this, Data);
                    break;
                case PanelPos.Middle:
                    m_MiddlePanel = panel;
                    m_MiddlePanel.Init(this, Data);
                    break;
                case PanelPos.Down:
                    m_DownPanel = panel;
                    m_DownPanel.Init(this, Data);
                    break;
            }
        }
    }






    //자신의 방향에 있는 슬롯을 가져온다
    public PuzzleSlot GetSlot(Direction _dir)
    {
        switch (_dir)
        {
            case Direction.Up:
                if (SlotNum - MatchBase.MaxHorizon < 0)
                    return null;
                return thisMap.Slots[SlotNum - MatchBase.MaxHorizon];
            case Direction.Left:
                if (SlotNum % MatchBase.MaxHorizon == 0)
                    return null;
                return thisMap.Slots[SlotNum - 1];
            case Direction.Down:
                if (SlotNum + MatchBase.MaxHorizon > thisMap.BottomRight)
                    return null;
                return thisMap.Slots[SlotNum + MatchBase.MaxHorizon];
            case Direction.Right:
                if (SlotNum % MatchBase.MaxHorizon >= thisMap.TopRight)
                    return null;
                return thisMap.Slots[SlotNum + 1];
            case Direction.TopLeft:
                if (SlotNum % MatchBase.MaxHorizon == 0 || SlotNum - MatchBase.MaxHorizon < 0)
                    return null;
                return thisMap.Slots[SlotNum - MatchBase.MaxHorizon - 1];
            case Direction.TopRight:
                if (SlotNum % MatchBase.MaxHorizon >= thisMap.TopRight || SlotNum - MatchBase.MaxHorizon < 0)
                    return null;
                return thisMap.Slots[SlotNum - MatchBase.MaxHorizon + 1];
            case Direction.BottomLeft:
                if (SlotNum % MatchBase.MaxHorizon >= thisMap.TopRight || SlotNum + MatchBase.MaxHorizon > thisMap.BottomRight)
                    return null;
                return thisMap.Slots[SlotNum + MatchBase.MaxHorizon - 1];
            case Direction.BottomRight:
                if (SlotNum % MatchBase.MaxHorizon >= thisMap.TopRight || SlotNum + MatchBase.MaxHorizon > thisMap.BottomRight)
                    return null;
                return thisMap.Slots[SlotNum + MatchBase.MaxHorizon - 1];
        }
        return null;
    }

    public PuzzleSlot GetSlot(int _Num)
    {
        int Num = SlotNum + _Num;

        if (Num < 0)
            return null;

        if (Num > thisMap.BottomRight)
            return null;


        return thisMap.Slots[Num];

    }




    public void DestroyPanel(Panel _panel)
    {

        if (m_UpPanel != null && m_UpPanel == _panel)
        {
            m_UpPanel = null;
        }
        else if (m_MiddlePanel != null && m_MiddlePanel == _panel)
        {
            m_MiddlePanel = null;
        }
        else if (m_DownPanel != null && m_DownPanel == _panel)
        {
            m_DownPanel = null;
        }
        if (m_PanelList.Contains(_panel) == true)
        {
            m_PanelList.Remove(_panel);
        }
    }









    //매치 버스트
    public void BurstEvent(float _Delay = 0f, Action action = null)
    {

        if (PuzzleManager.Instance.AroundSlot.Contains(this) == true)
            PuzzleManager.Instance.AroundSlot.Remove(this);
        m_isBursting = true;
        PuzzleManager.Instance.BurstList.Add(this);
        bool CheckBlock = true;
        bool Around = false;

        for (int i = 0; i < m_PanelList.Count; i++)
        {
            if (m_UpPanel.m_PanelBurst == true)
            {
                CheckBlock = m_PanelList[i].m_BlockBurst;
                Around = true;
            
                m_PanelList[i].BurstEvent(this, action);

                break;
            }
        }

        if (m_Block != null && CheckBlock)
        {
            if (CheckBlockCanBurst() == true)
            {
                Around = true;
                m_Block.BurstEvent(this, action);
            }
                
        }
        if (Around == true)
            AroundBurstEvent();


    }

    //블럭과 블럭을 서로 교환
    public void SwitchBlock(PuzzleSlot OtherSlot)
    {
        //자신의 슬롯 복사
        Block CopyBlock = this.m_Block != null? this.m_Block : null;

        if(m_Block != null)
            m_Block.MoveEvent(OtherSlot,MatchBase.BlockSpeed);

        if(OtherSlot.m_Block != null)
            OtherSlot.m_Block.MoveEvent(this, MatchBase.BlockSpeed);


        m_Block = OtherSlot.m_Block != null? OtherSlot.m_Block : null;

        OtherSlot.m_Block = CopyBlock != null ? CopyBlock : null;




    }



    public void AroundBurstEvent()
    {
        PuzzleSlot slot = GetSlot(Direction.Up);
        if (slot != null)
        {
            if (slot.m_isBursting == false && PuzzleManager.Instance.AroundSlot.Contains(slot) == false)
                PuzzleManager.Instance.AroundSlot.Add(slot);
        }
        slot = GetSlot(Direction.Left);
        if (slot != null)
        {
            if (slot.m_isBursting == false && PuzzleManager.Instance.AroundSlot.Contains(slot) == false)
                PuzzleManager.Instance.AroundSlot.Add(slot);
        }
        slot = GetSlot(Direction.Down);
        if (slot != null)
        {
            if (slot.m_isBursting == false && PuzzleManager.Instance.AroundSlot.Contains(slot) == false)
                PuzzleManager.Instance.AroundSlot.Add(slot);
        }
        slot = GetSlot(Direction.Right);
        if (slot != null)
        {
            if (slot.m_isBursting == false && PuzzleManager.Instance.AroundSlot.Contains(slot) == false)
                PuzzleManager.Instance.AroundSlot.Add(slot);
        }
    }








    public void SetSlot(int _Num)
    {
        Vec = this.transform.position;
        SlotNum = _Num;
        m_Text.text = _Num.ToString();
    }

    virtual public void Resetting()
    {
        if (m_Block != null)
        {
            m_Block.Resetting();
            m_Block = null;
        }
        for (int i = 0; i < m_PanelList.Count; i++)
        {
            m_PanelList[i].Resetting();
        }
        m_PanelList.Clear();
        m_UpPanel = null;
        m_MiddlePanel = null;
        m_DownPanel = null;

    }

    virtual public void SetSlot(SlotInfo _Info)
    { 
        
    }



}
