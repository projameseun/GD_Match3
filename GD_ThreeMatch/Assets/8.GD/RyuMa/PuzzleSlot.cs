using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static HappyRyuMa.GameMaker;





//[System.Serializable]
//public class MonsterSheet
//{
//    //몬스터 시트
//    public int SlotImageIndex;
//    public bool OnlyOneEnemy = false;     //true일 경우 한번 처치후 더이상 나오지 않음
//    public int addEnemyMeet;              //적과 조우할 확률 증가량
//    public int[] EnemyIndex = null;       //몬스터 인덱스 번호
//    public int[] EnemyChance = null;      //몬스터별 확률
//    public int OnlyOneNum;                //데이터 시트에 저장할 번호


//}

//[System.Serializable]
//public class PortalSheet
//{
//    public string MapName = null;
//    public int NextPosNum;
//}

//[System.Serializable]
//public class SlotObjectSheets
//{
//    public SlotObjectSheet SlotSheet;
//}



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
                return false;
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


    public void CreatBlock(BlockType _blockType, string[] Data)
    {
        GameObject block = BlockManager.Instance.CreatBlock(_blockType);
        if (block != null)
        {
            m_Block = block.GetComponent<Block>();
            m_Block.Init(this, Data);
        }
        else
           m_Block = null;
    }

    public void CreatPanel(PanelPos _PanelPos, PanelType _panelType, string[] Data)
    {
        GameObject panel = PanelManager.Instance.CreatePanel(_panelType);
        if (panel != null)
        {
            switch (_PanelPos)
            {
                case PanelPos.Up:
                    m_UpPanel = panel.GetComponent<Panel>();
                    m_UpPanel.Init(this, Data);
                    break;
                case PanelPos.Middle:
                    m_MiddlePanel = panel.GetComponent<Panel>();
                    m_MiddlePanel.Init(this, Data);
                    break;
                case PanelPos.Down:
                    m_DownPanel = panel.GetComponent<Panel>();
                    m_DownPanel.Init(this, Data);
                    break;
            }
        }
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
                if (SlotNum % MatchBase.MaxHorizon > thisMap.TopRight)
                    return null;
                return thisMap.Slots[SlotNum + 1];
        }
        return null;
    }

    public PuzzleSlot GetSlot(int _Num)
    {
        int Num = SlotNum + _Num;

        if (Num < 0)
            return null;

        if (Num >= thisMap.Slots.Length)
            return null;


        return thisMap.Slots[Num];

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

    public void SpecialCubeEvent()
    {
        //thePuzzle.SetMoveCount(-1);
        //if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
        //{
        //    thePuzzle.state = PuzzleManager.State.SpecialCubeEvent;
        //    thePuzzle.Player.ChangeAnim("Attack");
        //    thePuzzle.Player.Map = thePuzzle.theMoveMap;
        //    thePuzzle.Player.SlotNum = SlotNum;
        //    thePuzzle.Player.Type = cube.specialCubeType;
            
        //}
        //else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        //{
        //    thePuzzle.state = PuzzleManager.State.SpecialCubeEvent;


        //    Direction dir = Direction.Right;
        //    Vector2 StartVec = new Vector2(this.transform.position.x, this.transform.position.y);
        //    // 슬롯이 오른쪽
        //    if (SlotNum % thePuzzle.theBattleMap.Horizontal > 5)
        //    {
        //        StartVec.x -= 1.8f;
        //        dir = Direction.Right;
        //    }
        //    else
        //    {
        //        StartVec.x += 1.8f;
        //        dir = Direction.Left;
        //    }

        //    if (SlotNum / thePuzzle.theBattleMap.Horizontal <= 4)
        //    {
        //        StartVec.y -= 1f;
        //    }
        //    else
        //    {
        //        StartVec.y += 1f;
        //    }
        //    thePuzzle.Player.BattleEvent(StartVec,dir,SkillType.ST0_SpecialCube, cube.specialCubeType,thePuzzle.theBattleMap,SlotNum);

        //}

    }






    public void BurstEvent(float _Delay = 0f)
    {
        bool BurstEnd = false;
        if (m_UpPanel != null)
        {
            if (m_UpPanel.m_BlockBurst == true)
            {
                BurstEnd = m_UpPanel.m_BlockBurst;
                m_UpPanel.BurstEvent();
                if(BurstEnd)
                    return;
            }
        }

        if (m_MiddlePanel != null)
        {
            if (m_MiddlePanel.m_BlockBurst == true)
            {
                BurstEnd = m_MiddlePanel.m_BlockBurst;
                m_MiddlePanel.BurstEvent();
                if (BurstEnd)
                    return;
            }
        }

        if (m_DownPanel != null)
        {
            if (m_DownPanel.m_BlockBurst == true)
            {
                BurstEnd = m_DownPanel.m_BlockBurst;
                m_DownPanel.BurstEvent();
                if (BurstEnd)
                    return;
            }
        }

        if (m_Block != null)
        {
            m_Block.BurstEvent(thisMap,SlotNum);
        }
    }

    //블럭과 블럭을 서로 교환
    public void SwitchBlock(PuzzleSlot OtherSlot)
    {
        //자신의 슬롯 복사
        Block CopyBlock = this.m_Block != null? this.m_Block : null;

        if(m_Block != null)
            m_Block.MoveEvent(OtherSlot, MatchBase.BlockSpeed);

        if(OtherSlot.m_Block != null)
            OtherSlot.m_Block.MoveEvent(this, MatchBase.BlockSpeed);


        m_Block = OtherSlot.m_Block != null? OtherSlot.m_Block : null;

        OtherSlot.m_Block = CopyBlock;




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
