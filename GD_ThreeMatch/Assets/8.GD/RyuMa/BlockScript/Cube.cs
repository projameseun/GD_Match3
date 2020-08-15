using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Cube : Block
{
    public SpecialCubeType specialCubeType = SpecialCubeType.Null;
    public SpriteRenderer SpriteRen;

    //public SpriteRenderer MinimapSprite;
    public bool SpecialCube = false; // 특수블럭으로 바꾸기 위해 사용하는 값


    // trunk
    Vector2 TargetVec;
    float Speed;
    bool OnlyOneEvent = false;
    bool DestroyEvent = false;
    float DestoryTime = 1.0f;
    Color color = new Color(1f, 1f, 1f, 1f);
    public float SkillDamage = 0;




    
    //private void Start()
    //{

    //}

    //IEnumerator MoveCor()
    //{
    //    Sequence seq = DOTween.Sequence();


    //    seq.Append(this.transform.DOMove(TargetVec, Speed)).SetEase(Ease.Linear);

    //    while (Vector2.Distance(transform.position, TargetVec) >= 0.05f)
    //    {
    //        yield return null;
    //    }
    //    transform.position = TargetVec;
    //    if (OnlyOneEvent == true)
    //    {

    //        thePuzzle.CubeEvent = true;
    //        OnlyOneEvent = false;
    //    }

    //}
    //IEnumerator DestroyCor()
    //{
        
    //    while (DestroyEvent)
    //    {
    //        DestoryTime -= Time.deltaTime * 1.5f;

    //        if (DestoryTime < 0.5f)
    //        {
    //            if (OnlyOneEvent == true)
    //            {
    //                OnlyOneEvent = false;
    //                thePuzzle.CubeEvent = true;
    //            }

    //            if (SpecialCube == true)
    //            {
    //                color.a = 1;
    //                DestroyEvent = false;
    //                SpriteRen.color = color;
    //                SpriteRen.sprite = thePuzzle.SpecialSprites[(int)specialCubeType];
    //                SpecialCube = false;
    //                break;

    //            }
    //            else
    //            {
    //                DestroyCubeEvent();
    //            }


    //        }
    //        //color.a = DestoryTime;
    //        //SpriteRen.color = color;

    //        yield return null;
    //    }
    //}

    //public void MoveCube(Vector2 _vec, float _Speed, bool _Event = false)
    //{
    //    TargetVec = _vec;
    //    OnlyOneEvent = _Event;
    //    Speed = _Speed;
    //    if(this.gameObject.activeSelf == true)
    //        StartCoroutine(MoveCor());
    //}

    //// 큐브 파괴이밴트, 한번만 true로 할것
    //public void DestroyCube(bool _Event, bool SpecialEffect = false, float Skill = 0, float _Invoke = 0)
    //{
    //    OnlyOneEvent = _Event;
    //    DestroyEvent = true;
    //    DestoryTime = 1f;
    //    if (Skill >0)
    //    {
    //        SkillDamage = Skill;
    //    }
    //    if (SpecialCube == true)
    //    {
    //        if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
    //        {
    //            thePuzzle.theMoveMap.Slots[Num].nodeColor = nodeColor;
    //        }
    //        else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
    //        {
    //            thePuzzle.theBattleMap.Slots[Num].nodeColor = nodeColor;
    //        }
    //    }
    //    else
    //    {
    //        if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
    //        {
    //            thePuzzle.theMoveMap.Slots[Num].nodeColor = NodeColor.NC5_Blank;
    //        }
    //        else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
    //        {
    //            thePuzzle.theBattleMap.Slots[Num].nodeColor = NodeColor.NC5_Blank;
    //        }
    //    }

    //    if (SpecialEffect == true)
    //    {
    //        SkillEffectEvent(thePuzzle.selectGirl);

    //    }
    //    if (_Invoke > 0)
    //    {
    //        Invoke("DestroyCorStart", _Invoke);
    //    }
    //    else
    //    {
    //        DestroyCorStart();
    //    }

        


    //}



    //public void SkillEffectEvent(SelectGirl _Girl)
    //{

    //    if (_Girl == SelectGirl.G1_Alice)
    //    {
    //        theObject.AliceSkillEvent(this.transform.position);
    //    }
    //    else if (_Girl == SelectGirl.G3_Beryl)
    //    {
    //        theObject.BerylSkillEvent(this.transform.position);
    //        // 베릴 이펙트넣기   
    //    }
    //}



    ////큐브가 터진 후 이밴트
    //public void DestroyCubeEvent()
    //{
    //    theSound.PlaySE("BlockHit");
    //    color.a = 0;
    //    SpriteRen.color = color;
    //    for (int i = 0; i < 2; i++)
    //    {
    //        if (thePuzzle.playerUIs[i].selectGirl == (SelectGirl)nodeColor)
    //        {
    //            thePuzzle.playerUIs[i].AddSkillGauge(1);
    //        }
    //    }

    //    int CubeNum = 1;   // 플레이어면 1, 적이면 -1
    //    int CubeTarget = 0;  // 플레이어면 0, 적이면 1
    //    GameObject Target = null;
    //    if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
    //    {
    //        for (int i = 0; i < thePuzzle.CubeSprites.Length; i++)
    //        {
    //            for (int x = 0; x < thePuzzle.CubeSprites.Length; x++)
    //            {
    //                if ((int)nodeColor == (int)thePuzzle.PlayerCubeUI[x].cubeColor)
    //                {

    //                    Target = thePuzzle.PlayerCubeUI[x].gameObject;
    //                }
    //            }

    //        }
    //    }
    //    else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
    //    {
    //        bool CheckEnemyColor = false;

    //        theBattleResult.DestroyCubeCount++;

    //        //전투 이동 구하기
    //        for (int i = 0; i < theBattle.EnemyCubeUi.Length; i++)
    //        {
    //            if (theBattle.EnemyCubeUi[i].gameObject.activeSelf == true)
    //            {
    //                for (int x = 0; x < thePuzzle.CubeSprites.Length; x++)
    //                {
    //                    if ((int)nodeColor == (int)theBattle.EnemyCubeUi[x].cubeColor)
    //                    {
    //                        if (theBattle.EnemyCubeUi[x].CubeCount > 0)
    //                        {
    //                            Target = theBattle.EnemyCubeUi[x].gameObject;
    //                            if (SkillDamage > 0)
    //                            {
    //                                CubeNum = (int)(-1 * theBattle.ComboStack*SkillDamage);
    //                            }
    //                            else
    //                            {
    //                                CubeNum = (int)(-1 * theBattle.ComboStack);
    //                            }
    //                            CubeTarget = 1;
    //                            i = 6;
    //                            x = 6;
    //                            CheckEnemyColor = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        if (CheckEnemyColor == false)
    //        {
    //            for (int i = 0; i < thePuzzle.CubeSprites.Length; i++)
    //            {
    //                for (int x = 0; x < thePuzzle.CubeSprites.Length; x++)
    //                {
    //                    if ((int)nodeColor == (int)thePuzzle.PlayerCubeUI[x].cubeColor)
    //                    {
    //                        CubeNum = 1;
    //                        Target = thePuzzle.PlayerCubeUI[x].gameObject;
    //                    }
    //                }

    //            }
    //        }

    //    }

    //    if ((int)nodeColor < 6)
    //    {
    //        GameObject CubeEffect = theObject.CubeEffectEvent(this.transform.position, Target, nodeColor,
    //        (CubeEffectType)CubeTarget, CubeNum, true);
    //        if (theBattle.CurrentEnemyCount == 0 && CubeTarget == 1)
    //        {
    //            theBattle.PlayerAttackEffectList.Add(CubeEffect);
    //        }
    //        theObject.CubeParticleEvent(this.transform.position, nodeColor);

    //    }


    //    SpecialCubeEvent();
    //    SkillDamage = 0;
    //    DestoryTime = 0;
    //    DestroyEvent = false;
    //}


    //public void SpecialCubeEvent()
    //{
    //    if (specialCubeType == SpecialCubeType.Null)
    //        return;
      
    //    MapManager _Map = null;
    //    thePuzzle.state = PuzzleManager.State.SpecialCubeEvent;

    //    if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
    //    {
    //        _Map = thePuzzle.theMoveMap;
    //    }
    //    else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
    //    {
    //        _Map = thePuzzle.theBattleMap;
    //    }

    //    theMatch.SpecialCubeEvent(_Map, Num, specialCubeType);
    //}

    //void DestroyCorStart()
    //{
    //    if (this.gameObject.activeSelf == true)
    //    {
    //        ObjectManager.Instance.SpawnBlockBreak(this.transform.position);
    //        StartCoroutine(DestroyCor());
    //    }
           
    //}


    //public void Resetting()
    //{
    //    StopAllCoroutines();
    //    specialCubeType = SpecialCubeType.Null;
    //    SpecialCube = false;
    //    OnlyOneEvent = false;
    //    DestroyEvent = false;
    //    DestoryTime = 1f;
    //    SkillDamage = 0;
    //    SpriteRen.color = new Color(1, 1, 1, 1);
    //    this.gameObject.SetActive(false);
    //    theObject.Cubes.Enqueue(this.gameObject);
    //}






}
