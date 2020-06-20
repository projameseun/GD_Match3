using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;


public enum SkillType
{ 
    ST0_SpecialCube,
    ST1_GirlSkill
}


public class PlayerCube : MonoBehaviour
{
    public MeshRenderer SpinMesh;
    public SkeletonAnimation anim;
    public SelectGirl selectGirl;
    public GameObject SdPlayer;
    public GameObject PlayerDirObj;



    
    public string AnimName; // 현재 에니메이션 상태를 저장
    public Direction direction;

    //특수블럭 사용 변수
    public MapManager Map;
    public int SlotNum;
    public SpecialCubeType Type;

    //전투 이밴트 변수
    SkillType skillType;
    Vector2 VisitVec;
    public bool GirlEffect = false; // 스킬 효과
    Direction Lastdir;



    // 몬스터시트
    public int CurrentEnemyMeetChance = 0; //현재 적과 조주할 확률

    private PuzzleManager thePuzzle;
    private FindMatches theMatch;
    private BattleManager theBattle;
    private ObjectManager theObject;
    private GirlManager theGirl;
    private void Start()
    {
        theGirl = FindObjectOfType<GirlManager>();
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
            if (selectGirl == SelectGirl.G1_Alice)
            {
                theObject.AliceAnimEvent(SdPlayer.transform.position, direction);
            }
            if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
            {
                theMatch.SpecialCubeEvent(Map, SlotNum, Type);
            }
            else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
            {
                theBattle.AddComboValue();
                
                if (skillType == SkillType.ST0_SpecialCube)
                {
                    theMatch.SpecialCubeEvent(Map, SlotNum, Type);
                }
                else if (skillType == SkillType.ST1_GirlSkill)
                {
                    GirlEffect = true;
                    theMatch.GirlSkill(
                        selectGirl,
                        Map, 
                        SlotNum);
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
                float Resize = theGirl.Girls[(int)thePuzzle.selectGirl].SdSize;
                SdPlayer.transform.localScale = new Vector3(Resize, Resize, 1);
                ChangeAnim("Idle", true);
                SdPlayer.transform.position = VisitVec;
                theBattle.SkillEventOnOff = false;

                anim.GetComponent<MeshRenderer>().sortingOrder = 50;
                ChangeDirection(Lastdir);
            }


           
        }
    }

    public void SetSpine(int _SelNum, string _SkinName)
    {
        thePuzzle.selectGirl = (SelectGirl)_SelNum;
        anim.skeletonDataAsset = theGirl.Girls[_SelNum].SdDatae;
        if (_SkinName != "")
            anim.initialSkinName = _SkinName;
        anim.Initialize(true);
        SdPlayer.transform.localScale = new Vector3(theGirl.Girls[_SelNum].SdSize,
            theGirl.Girls[_SelNum].SdSize, 1);
        selectGirl = theGirl.Girls[_SelNum].selectGirl;
        anim.state.Event += HandleEvent;
    }


    public void ChangeDirection(Direction _direction)
    {
        if (direction == _direction)
            return;

        direction = _direction;

        if (direction == Direction.Up)
        {
            PlayerDirObj.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (direction == Direction.Down)
        {
            PlayerDirObj.transform.eulerAngles = new Vector3(0, 0, 270);
        }
        else if (direction == Direction.Left)
        {
            PlayerDirObj.transform.eulerAngles = new Vector3(0, 0, 180);
            SdPlayer.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction == Direction.Right)
        {
            PlayerDirObj.transform.eulerAngles = new Vector3(0, 0, 0);
            SdPlayer.transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }

    public void ChangeAnim(string _state,bool _Loop = false)
    {
        if (_state == AnimName)
            return;
        if (_state == "Attack" && thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        {
            theBattle.SkillEventOnOff = true;
            float Resize = theGirl.Girls[(int)thePuzzle.selectGirl].SdSize;
            Resize *= 2.5f;
            SdPlayer.transform.localScale = new Vector3(Resize, Resize, 1);
            anim.GetComponent<MeshRenderer>().sortingOrder = 202;
        }
            
        anim.AnimationState.SetAnimation(0, _state, _Loop);
        AnimName = _state;
    }
    public void BattleEvent(Vector2 TargetVec, 
        Direction _Dir, 
        SkillType _Type,
        SpecialCubeType _CubeType,
        MapManager _Map, 
        int _SlotNum)
    {
        Map = _Map;
        skillType = _Type;
        SlotNum = _SlotNum;
        Type = _CubeType;
        VisitVec = this.transform.position;
        ChangeAnim("Attack");
        SdPlayer.transform.position = TargetVec;
        Lastdir = direction;
        ChangeDirection(_Dir);
    }


}
