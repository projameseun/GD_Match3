﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.UIElements;


public enum SkillType
{ 
    ST0_SpecialCube,
    ST1_GirlSkill
}
public class PlayerCube : MonoBehaviour
{
    public SkeletonAnimation anim;
    [SpineSlot]


    
    public string AnimName; // 현재 에니메이션 상태를 저장
    public int TrakNum;
    public Direction direction;

    //특수블럭 사용 변수
    public MapManager Map;
    public int SlotNum;
    public SpecialCubeType Type;

    //전투 이밴트 변수
    SkillType skillType;
    Vector2 VisitVec;
    public bool GirlEffect = false; // 스킬 효과
    



    // 몬스터시트
    public int CurrentEnemyMeetChance = 0; //현재 적과 조주할 확률

    private PuzzleManager thePuzzle;
    private FindMatches theMatch;
    private BattleManager theBattle;
    private ObjectManager theObject;
    private void Start()
    {
        theObject = FindObjectOfType<ObjectManager>();
        theBattle = FindObjectOfType<BattleManager>();
        theMatch = FindObjectOfType<FindMatches>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        anim.state.Event += HandleEvent;
    }
    //색 조정

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        anim.skeleton.SetColor(new Color(0.5f, 0.5f, 0.5f));
    //    }
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        anim.skeleton.Data.FindSlot("Hand_L_1").
    //    }


    //}

    public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "Attack")
        {
            
            theObject.AliceAnimEvent(this.transform.position, direction);

            if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
            {
                theMatch.SpecialCubeEvent(Map, SlotNum, Type);
            }
            else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
            {
                if (skillType == SkillType.ST0_SpecialCube)
                {
                    theMatch.SpecialCubeEvent(Map, SlotNum, Type);
                }
                else if (skillType == SkillType.ST1_GirlSkill)
                {
                    GirlEffect = true;
                    theMatch.GirlSkill(
                          (SelectGirl)thePuzzle.playerUIs[(int)theBattle.CurrentSkillUI].nodeColor,
                    Map, SlotNum);
                    theBattle.ReadySkill(SkillUI.UI2_Null);
                    GirlEffect = false;
                }
               

            }
        }

        //공격 끝나고
        if (e.Data.Name == "Attack_End")
        {
            if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
            {
               
            }
            else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
            {

                ChangeAnim("Idle", true);
                this.transform.position = VisitVec;

            }


           
        }
    }




    public void ChangeDirection(Direction _direction)
    {
        direction = _direction;

        if (direction == Direction.Up)
        { 
            
        }
        else if (direction == Direction.Down)
        {

        }
        else if (direction == Direction.Left)
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction == Direction.Right)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }

    public void ChangeAnim(string _state,bool _Loop = false)
    {
        if (_state == AnimName)
            return;
        anim.AnimationState.SetAnimation(TrakNum, _state, _Loop);
        AnimName = _state;
    }
    public void BattleEvent(Vector2 TargetVec, Direction _Dir, SkillType _Type,SpecialCubeType _CubeType,MapManager _Map, int _SlotNum)
    {
        Map = _Map;
        skillType = _Type;
        SlotNum = _SlotNum;
        Type = _CubeType;
        VisitVec = this.transform.position;
        ChangeAnim("Attack");
        this.transform.position = TargetVec;
        ChangeDirection(_Dir);
    }


}
