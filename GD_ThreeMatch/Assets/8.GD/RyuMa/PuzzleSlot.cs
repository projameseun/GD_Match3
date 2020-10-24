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


    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_Text = GetComponentInChildren<Text>();
    }



    //슬롯을 한번에 정리해준다
    public void SetSlot()
    {
        if (m_UpPanel != null)
            m_PanelList.Add(m_UpPanel);

        if (m_MiddlePanel != null)
            m_PanelList.Add(m_MiddlePanel);

        if (m_DownPanel != null)
            m_PanelList.Add(m_DownPanel);


        for (int i = 0; i < m_PanelList.Count; i++)
        {

        }

    }



    //해당 슬롯이 스위치가 가능하지 체크한다
    public bool CheckSwitch()
    {
        if (m_Block == null)
            return true;

        if (m_PanelList.Find(obj => obj.m_Switch == false) == null)
        {
            return true;
        }

        return false;

    }

    public bool CheckMatch()
    {
        if (m_Block == null)
            return true;

        if (m_PanelList.Find(obj => obj.m_Match == false) == null)
        {
            return true;
        }

        return false;
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
