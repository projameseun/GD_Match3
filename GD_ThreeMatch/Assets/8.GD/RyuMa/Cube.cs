using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpecialCubeType
{ 
    Vertical = 0,
    Horizon,
    Hanoi,
    Null
}

public class Cube : MonoBehaviour
{
    public SpecialCubeType specialCubeType = SpecialCubeType.Null;
    public NodeColor nodeColor;



    public int Num;
    public float Size;
    //public SpriteRenderer MinimapSprite;
    public bool SpecialCube = false; // 특수블럭으로 바꾸기 위해 사용하는 값


    // trunk
    Vector2 TargetVec;
    float Speed;
    bool Move;
    bool OnlyOneEvent = false;
    bool DestroyEvent = false;
    float DestoryTime = 1.0f;
    Color color = new Color(1f, 1f, 1f, 1f);
    SpriteRenderer SpriteRen;
    float SkillDamage = 0;



    private PuzzleManager thePuzzle;
    private ObjectManager theObject;
    private BattleManager theBattle;
    private BattleResultManager theBattleResult;
    private FindMatches theMatch;
    private void Start()
    {
        theMatch = FindObjectOfType<FindMatches>();
        theBattleResult = FindObjectOfType<BattleResultManager>();
        theBattle = FindObjectOfType<BattleManager>();
        theObject = FindObjectOfType<ObjectManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        SpriteRen = GetComponent<SpriteRenderer>();
    }



    private void FixedUpdate()
    {
        if (Move == true)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, TargetVec, Speed);

    
            if(Vector2.Distance(this.transform.position, TargetVec) <= Speed/2)
            {
                this.transform.position = TargetVec;
                Move = false;

                if(OnlyOneEvent == true)
                {
                    thePuzzle.CubeEvent = true;
                    OnlyOneEvent = false;
                }

            }
                
        }

        if (DestroyEvent == true)
        {
            DestoryTime -= Time.deltaTime*1.5f;

            if (DestoryTime < 0.5f)
            {
                if (OnlyOneEvent == true)
                {
                    OnlyOneEvent = false;
                    thePuzzle.CubeEvent = true;
                }

                if (SpecialCube == true)
                {
                    color.a = 1;
                    DestroyEvent = false;
                    SpriteRen.color = color;
                    SpriteRen.sprite = thePuzzle.SpecialSprites[(int)specialCubeType];
                    SpecialCube = false;
                    return;

                }
                else
                {
                    DestroyCubeEvent();
                }

               
            }
            color.a = DestoryTime;
            SpriteRen.color = color;
        }

    }


    public void MoveCube(Vector2 _vec, bool _Event = false, float _Speed = 0.5f)
    {
        Move = true;
        TargetVec = _vec;
        OnlyOneEvent = _Event;
        Speed = _Speed;
    }

    // 큐브 파괴이밴트, 한번만 true로 할것
    public void DestroyCube(bool _Event, bool SpecialEffect = false, float Skill = 0)
    {
        OnlyOneEvent = _Event;
        DestroyEvent = true;
        DestoryTime = 1f;
        if (Skill >0)
        {
            SkillDamage = Skill;
        }
        if (SpecialCube == true)
        {
            if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
            {
                thePuzzle.theMoveMap.Slots[Num].nodeColor = nodeColor;
            }
            else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
            {
                thePuzzle.theBattleMap.Slots[Num].nodeColor = nodeColor;
            }
        }
        else
        {
            if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
            {
                thePuzzle.theMoveMap.Slots[Num].nodeColor = NodeColor.NC5_Blank;
            }
            else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
            {
                thePuzzle.theBattleMap.Slots[Num].nodeColor = NodeColor.NC5_Blank;
            }
        }

        if (SpecialEffect == true)
        {
            SkillEffectEvent(thePuzzle.selectGirl);
            //if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
            //{
            //    SkillEffectEvent(thePuzzle.selectGirl);
            //}
            //else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
            //{
            //    if (theBattle.CurrentSkillUI == SkillUI.UI2_Null)
            //    {
            //        SkillEffectEvent(thePuzzle.selectGirl);
            //    }
            //    else
            //    {
            //        SkillEffectEvent((SelectGirl)thePuzzle.playerUIs[(int)theBattle.CurrentSkillUI].nodeColor);
            //    }
            //}
        }
    }



    public void SkillEffectEvent(SelectGirl _Girl)
    {

        if (_Girl == SelectGirl.G1_Alice)
        {
            
            theObject.AliceSkillEvent(this.transform.position);
        }
        else if (_Girl == SelectGirl.G3_Beryl)
        { 
            // 베릴 이펙트넣기   
        }
    }



    //큐브가 터진 후 이밴트
    public void DestroyCubeEvent()
    {
        Debug.Log("Test");
        for (int i = 0; i < 2; i++)
        {
            if (thePuzzle.playerUIs[i].nodeColor == nodeColor)
            {
                thePuzzle.playerUIs[i].AddSkillGauge(1);
            }
        }

        int CubeNum = 1;   // 플레이어면 1, 적이면 -1
        int CubeTarget = 0;  // 플레이어면 0, 적이면 1
        GameObject Target = null;
        if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
        {
            for (int i = 0; i < thePuzzle.CubeSprites.Length; i++)
            {
                for (int x = 0; x < thePuzzle.CubeSprites.Length; x++)
                {
                    if ((int)nodeColor == (int)thePuzzle.PlayerCubeUI[x].cubeColor)
                    {

                        Target = thePuzzle.PlayerCubeUI[x].gameObject;
                    }
                }

            }
        }
        else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        {
            bool CheckEnemyColor = false;

            theBattleResult.DestroyCubeCount++;

            //전투 이동 구하기
            for (int i = 0; i < theBattle.EnemyCubeUi.Length; i++)
            {
                if (theBattle.EnemyCubeUi[i].gameObject.activeSelf == true)
                {
                    for (int x = 0; x < thePuzzle.CubeSprites.Length; x++)
                    {
                        if ((int)nodeColor == (int)theBattle.EnemyCubeUi[x].cubeColor)
                        {
                            if (theBattle.EnemyCubeUi[x].CubeCount > 0)
                            {
                                Target = theBattle.EnemyCubeUi[x].gameObject;
                                if (SkillDamage > 0)
                                {
                                    CubeNum = (int)(-1 * theBattle.ComboStack*SkillDamage);
                                }
                                else
                                {
                                    CubeNum = (int)(-1 * theBattle.ComboStack);
                                }
                                
                                CubeTarget = 1;
                                i = 6;
                                x = 6;
                                CheckEnemyColor = true;
                            }
                        }
                    }
                }
            }
            if (CheckEnemyColor == false)
            {
                for (int i = 0; i < thePuzzle.CubeSprites.Length; i++)
                {
                    for (int x = 0; x < thePuzzle.CubeSprites.Length; x++)
                    {
                        if ((int)nodeColor == (int)thePuzzle.PlayerCubeUI[x].cubeColor)
                        {
                            CubeNum = 1;
                            Target = thePuzzle.PlayerCubeUI[x].gameObject;
                        }
                    }

                }
            }

        }

        if ((int)nodeColor < 6)
        {
            GameObject CubeEffect = theObject.CubeEffectEvent(this.transform.position, Target, nodeColor,
            (CubeEffectType)CubeTarget, CubeNum, true);
            if (theBattle.CurrentEnemyCount == 0 && CubeTarget == 1)
            {
                theBattle.PlayerAttackEffectList.Add(CubeEffect);
            }
            theObject.CubeParticleEvent(this.transform.position);

        }


        SpecialCubeEvent();

        DestoryTime = 0;
        DestroyEvent = false;
    }


    public void SpecialCubeEvent()
    {
        if (specialCubeType == SpecialCubeType.Null)
            return;

        theBattle.AddComboValue();
      
        MapManager _Map = null;
        thePuzzle.state = PuzzleManager.State.SpecialCubeEvent;

        if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
        {
            _Map = thePuzzle.theMoveMap;
        }
        else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        {
            _Map = thePuzzle.theBattleMap;
        }

        theMatch.SpecialCubeEvent(_Map, Num, specialCubeType);
    }


    public void Resetting()
    {
        specialCubeType = SpecialCubeType.Null;
        Move = false;
        SpecialCube = false;
        OnlyOneEvent = false;
        DestroyEvent = false;
        DestoryTime = 1f;
        SkillDamage = 0;
        SpriteRen.color = new Color(1, 1, 1, 1);
        this.gameObject.SetActive(false);
    }






}
