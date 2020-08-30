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
    public Block block;
    public List<Panel> panel;


    [HideInInspector]
    public Image m_Image;
    [HideInInspector]
    public Text m_Text;

    ////몬스터 시트



    // 포탈 시트



    //몬스터일 경우 넣는다






    //trunk
    Vector2 FirstVec;
    Vector2 CurrentVec;
    bool CheckCor;


    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_Text = GetComponentInChildren<Text>();
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

    public void SkillEvent()
    {
       
        //Direction dir = Direction.Right;
        //thePuzzle.SetMoveCount(-1);
        //Vector2 StartVec = new Vector2(this.transform.position.x, this.transform.position.y);
        //thePuzzle.state = PuzzleManager.State.SpecialCubeEvent;
        //// 슬롯이 오른쪽
        //if (SlotNum % thePuzzle.theBattleMap.Horizontal > 5)
        //{
        //    StartVec.x -= 1.8f;
        //    dir = Direction.Right;
        //}
        //else
        //{
        //    StartVec.x += 1.8f;
        //    dir = Direction.Left;
        //}
        //if (SlotNum / thePuzzle.theBattleMap.Horizontal <= 4)
        //{
        //    StartVec.y -= 1f;
        //}
        //else
        //{
        //    StartVec.y += 1f;
        //}

        ////수정 확인
        ////thePuzzle.Player.BattleEvent(StartVec, dir, SkillType.ST1_GirlSkill, block.specialCubeType,
        ////    thePuzzle.theBattleMap, SlotNum);

      
        //thePuzzle.playerUIs[(int)theBattle.CurrentSkillUI].ResetSkillGauge();
        //theBattle.SkillEventOnOff = true;
    }




    public void BlockBurst(float _Delay = 0f)
    {
        if (block == null)
            return;
        if (block.Burst == false || block.Bursting == true)
            return;
    }








    public void SetSlot(int _Num)
    {
        Vec = this.transform.position;
        SlotNum = _Num;
        m_Text.text = _Num.ToString();
    }

    public void Resetting()
    { 
    }


}
