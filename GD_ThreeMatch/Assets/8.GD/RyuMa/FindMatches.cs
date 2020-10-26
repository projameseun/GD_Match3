using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpecialData
{
    public int Num;
    SpecialCubeType Type;

    public SpecialData(int _Num, SpecialCubeType _Type)
    {
        Num = _Num;
        Type = _Type;
    }


}




public class FindMatches : A_Singleton<FindMatches>
{

    // 큐브를 터트릴 경우 조건을 통합적으로 표시해주는 구간
    //_Map.Slots[i].nodeColor != NodeColor.Player &&
    //_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
    //_Map.Slots[i].nodeColor != NodeColor.Blank &&





    public List<PuzzleSlot> currentMathces = new List<PuzzleSlot>();
    List<SpecialData> SpecialList = new List<SpecialData>();
    public bool CheckBoom;
    public float CurrentCheckBoomTime;
    public float MaxCheckBoomTime;


    private GirlManager theGirl;
    private BattleManager theBattle;
// Start is called before the first frame update

    void Start()
    {
        theBattle = FindObjectOfType<BattleManager>();
        CurrentCheckBoomTime = MaxCheckBoomTime;
        theGirl = FindObjectOfType<GirlManager>();

    }
    #region FindMatch
    //매치가 가능한 조건이 있는지 확인
    public bool FindAllMatches(MapManager _Map,bool _ChangeBlank = true)
    {

        for (int y = 0; y < _Map.BottomRight; y += MatchBase.MaxHorizon)
        {
            for (int x = 1; x < _Map.TopRight; x++)
            {

                // 현재 슬롯이 매치가 가능한가
                if (_Map.Slots[x + y].CheckMatch())
                {

                    // 좌우로 서로 매치가 가능한가
                    if (_Map.Slots[x + y - 1].CheckMatch() && _Map.Slots[x + y + 1].CheckMatch())
                    {
                        if (_Map.Slots[x + y].m_Block.nodeColor == _Map.Slots[x + y - 1].m_Block.nodeColor && _Map.Slots[x + y].m_Block.nodeColor == _Map.Slots[x + y + 1].m_Block.nodeColor)
                        {
                            if (!currentMathces.Contains(_Map.Slots[x + y - 1]))
                            {
                                currentMathces.Add(_Map.Slots[x + y - 1]);
                            }
                            if (!currentMathces.Contains(_Map.Slots[x + y + 1]))
                            {
                                currentMathces.Add(_Map.Slots[x + y + 1]);
                            }
                            if (!currentMathces.Contains(_Map.Slots[x + y]))
                            {
                                currentMathces.Add(_Map.Slots[x + y]);
                            }
                        }
                    }


                    // 상하로 서로 매치가 가능한가
                    if (_Map.Slots[x + y - MatchBase.MaxHorizon].CheckMatch() && _Map.Slots[x + y + MatchBase.MaxHorizon].CheckMatch())
                    {
                        if (_Map.Slots[x + y].m_Block.nodeColor == _Map.Slots[x + y - MatchBase.MaxHorizon].m_Block.nodeColor && _Map.Slots[x + y].m_Block.nodeColor == _Map.Slots[x + y + MatchBase.MaxHorizon].m_Block.nodeColor)
                        {
                            if (!currentMathces.Contains(_Map.Slots[x + y - MatchBase.MaxHorizon]))
                            {
                                currentMathces.Add(_Map.Slots[x + y - MatchBase.MaxHorizon]);
                            }
                            if (!currentMathces.Contains(_Map.Slots[x + y + MatchBase.MaxHorizon]))
                            {
                                currentMathces.Add(_Map.Slots[x + y + MatchBase.MaxHorizon]);
                            }
                            if (!currentMathces.Contains(_Map.Slots[x + y]))
                            {
                                currentMathces.Add(_Map.Slots[x + y]);
                            }
                        }
                    }
                }
            }
        }

        if (currentMathces.Count > 0)
        {
            if (_ChangeBlank == false)
                currentMathces.Clear();

            return true;
        }
        else
            return false;

    }
    #endregion
    //특수 큐브를 만들 수 있는지 확인
    public void FindSpecialCube(MapManager _Map)
    {


        for (int i = 1; i < _Map.TopRight; i++)
        {
            for (int Num = _Map.TopLeft + MatchBase.MaxHorizon; Num < _Map.BottomLeft;)
            {
                if (_Map.Slots[Num + i].CheckMatch())
                {
                    int Count = 1;


                    while (true)
                    {
                        if (_Map.Slots[Num + i + (MatchBase.MaxHorizon * Count)].CheckMatch())
                        {
                            if (_Map.Slots[Num + i].m_Block.nodeColor == 
                                _Map.Slots[Num + i + (MatchBase.MaxHorizon * Count)].m_Block.nodeColor)
                            {
                                Count++;
                            }
                            else
                            {
                                if (Count < 4)
                                {
                                    Count = 1;
                                    break;
                                }
                                else
                                {
                                    // 스페셜 발동
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (Count < 4)
                            {
                                Count = 1;
                                break;
                            }
                            else
                            {


                                break;
                            }
                        }
                    }
                    Num += (Count * MatchBase.MaxHorizon);
                }
                else
                {
                    Num += MatchBase.MaxHorizon;
                }
            }

        }


        for (int i = MatchBase.MaxHorizon; i < _Map.BottomLeft; i += MatchBase.MaxHorizon)
        {
            for (int Num = 1; Num < _Map.TopRight;)
            {
                if (_Map.Slots[Num + i].CheckMatch())
                {
                    int Count = 1;

                    while (true)
                    {
                        if (_Map.Slots[Num + i + Count].CheckMatch())
                        {

                            if (_Map.Slots[Num + i].m_Block.nodeColor == _Map.Slots[Num + i + Count].m_Block.nodeColor)
                            {
                                Count++;
                            }
                            else
                            {
                                if (Count < 4)
                                {
                                    Count = 1;
                                    break;
                                }
                                else
                                {
                                    // 스페셜 발동
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (Count < 4)
                            {
                                Count = 1;
                                break;
                            }
                            else
                            {
                                // 스페셜 발동
                                break;
                            }
                        }


                    }

                    Num += Count;

                }
                else
                {
                    Num += 1;
                }
            }
        }

    }

    //가로 특수큐브
    public void FindHorizonCube(MapManager _Map,int _SlotNum)
    {

        //int HorizonNum =0;


        //_Map.Slots[_SlotNum].m_Block.GetComponent<SpecialCube>().specialCubeType = SpecialCubeType.Null;

        //for (int i = 0; i < _Map.Vertical; i++)
        //{
        //    if (_SlotNum < i * MatchBase.MaxHorizon)
        //    {
        //        HorizonNum = (i - 1) * MatchBase.MaxHorizon;
        //        break;
        //    }
        //}
        //for (int i = HorizonNum; i < HorizonNum + MatchBase.MaxHorizon; i++)
        //{
        //    if (_Map.Slots[i].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[i].nodeColor != NodeColor.NC6_Null)
        //    {
        //        if (i == _SlotNum)
        //            continue;

        //        if (_Map.Slots[i].cube.specialCubeType != SpecialCubeType.Null &&
        //            _Map.Slots[i].cube.specialCubeType != SpecialCubeType.Horizon)
        //        {
        //            CheckBoom = true;
        //        }
                    

        //    }
        //}

        //for (int i = HorizonNum; i < HorizonNum + MatchBase.MaxHorizon; i++)
        //{
        //    if (_Map.Slots[i].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[i].nodeColor != NodeColor.NC6_Null)
        //    {
        //        if (_Map.Slots[i].cube.specialCubeType == SpecialCubeType.Horizon)
        //            _Map.Slots[i].cube.specialCubeType = SpecialCubeType.Null;
        //        _Map.Slots[i].cube.DestroyCube(true,true);

        //    }
        //}

 
    }

    //세로 특수큐브
    public void FindVerticalCube(MapManager _Map, int _SlotNum)
    {
        //int Vertical = _SlotNum % MatchBase.MaxHorizon;
        //_Map.Slots[_SlotNum].cube.specialCubeType = SpecialCubeType.Null;


        //for (int i = Vertical; i < _Map.BottomLeft; i += MatchBase.MaxHorizon)
        //{
        //    if (_Map.Slots[i].nodeColor != NodeColor.NC6_Player &&
        //         _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
        //         _Map.Slots[i].nodeColor != NodeColor.NC6_Null)
        //    {
        //        if (i == _SlotNum)
        //            continue;
        //        if (_Map.Slots[i].cube.specialCubeType != SpecialCubeType.Null &&
        //            _Map.Slots[i].cube.specialCubeType != SpecialCubeType.Vertical)
        //        {
        //            CheckBoom = true;
        //        }

        //    }
        //}

        //for (int i = Vertical; i < _Map.BottomLeft; i += MatchBase.MaxHorizon)
        //{
        //    if (_Map.Slots[i].nodeColor != NodeColor.NC6_Player &&
        //         _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
        //         _Map.Slots[i].nodeColor != NodeColor.NC6_Null)
        //    {

        //        if (_Map.Slots[i].cube.specialCubeType == SpecialCubeType.Vertical)
        //            _Map.Slots[i].cube.specialCubeType = SpecialCubeType.Null;

        //        _Map.Slots[i].cube.DestroyCube(true,true);

        //    }
        //}

       

    }

    //대각선 특수큐브
    public void FindDiagonalCube(MapManager _Map, int _SlotNum)
    {
       
        //int CheckCount = 1;


        //_Map.Slots[_SlotNum].cube.specialCubeType = SpecialCubeType.Null;



        //// 11시 방향 확인
        //while (true && CheckBoom == false)
        //{
        //    int Count = _SlotNum - ((MatchBase.MaxHorizon + 1) * CheckCount);

        //    if (Count < _Map.TopRight ||
        //        Count % MatchBase.MaxHorizon == 0)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC6_Null)
        //    {
        //        if (_Map.Slots[Count].cube.specialCubeType != SpecialCubeType.Null &&
        //            _Map.Slots[Count].cube.specialCubeType != SpecialCubeType.Diagonal)
        //        {
        //            CheckBoom = true;
        //            break;
        //        }

        //    }

        //    CheckCount++;

        //}


        //CheckCount = 1;
        //// 1시 방향 확인
        //while (true && CheckBoom == false)
        //{

        //    int Count = _SlotNum - ((MatchBase.MaxHorizon - 1) * CheckCount);

        //    if (Count <= _Map.TopRight ||
        //        Count % MatchBase.MaxHorizon == MatchBase.MaxHorizon - 1)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC6_Null)
        //    {
        //        if (_Map.Slots[Count].cube.specialCubeType != SpecialCubeType.Null &&
        //            _Map.Slots[Count].cube.specialCubeType != SpecialCubeType.Diagonal)
        //        {
        //            CheckBoom = true;

        //            break;
        //        }
        //    }

        //    CheckCount++;
        //}

        //CheckCount = 1;

        //// 7시 방향 확인
        //while (true && CheckBoom == false)
        //{
        //    int Count = _SlotNum + ((MatchBase.MaxHorizon - 1) * CheckCount);

        //    if (Count >= _Map.BottomLeft ||
        //        Count % MatchBase.MaxHorizon == 0)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC6_Null)
        //    {
        //        if (_Map.Slots[Count].cube.specialCubeType != SpecialCubeType.Null &&
        //            _Map.Slots[Count].cube.specialCubeType != SpecialCubeType.Diagonal)
        //        {
        //            CheckBoom = true;
        //            break;
        //        }
        //    }

        //    CheckCount++;
        //}


        //CheckCount = 1;

        //// 5시 방향
        //while (true && CheckBoom == false)
        //{
        //    int Count = _SlotNum + ((MatchBase.MaxHorizon + 1) * CheckCount);

        //    if (Count >= _Map.BottomLeft ||
        //        Count % MatchBase.MaxHorizon == MatchBase.MaxHorizon - 1)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC6_Null)
        //    {
        //        if (_Map.Slots[Count].cube.specialCubeType != SpecialCubeType.Null &&
        //            _Map.Slots[Count].cube.specialCubeType != SpecialCubeType.Diagonal)
        //        {
        //            CheckBoom = true;
        //            break;
        //        }
        //    }

        //    CheckCount++;
        //}




        //CheckCount = 1;



        //// 11시 방향
        //while (true)
        //{
        //    int Count = _SlotNum - ((MatchBase.MaxHorizon + 1) * CheckCount);

        //    if (Count < _Map.TopRight ||
        //        Count % MatchBase.MaxHorizon == 0)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC6_Null)
        //    {
        //        if (Count == _SlotNum)
        //            continue;


        //        //if (_Map.Slots[Count].cube.specialCubeType == SpecialCubeType.Diagonal)
        //        //    _Map.Slots[Count].cube.specialCubeType = SpecialCubeType.Null;


        //        _Map.Slots[Count].cube.DestroyCube(true,true);
        //    }

        //    CheckCount++;

        //}


        //CheckCount = 1;
        //// 1시 방향
        //while (true)
        //{
        //    int Count = _SlotNum - ((MatchBase.MaxHorizon - 1) * CheckCount);

        //    if (Count <= _Map.TopRight ||
        //        Count % MatchBase.MaxHorizon == MatchBase.MaxHorizon - 1)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC6_Null)
        //    {

        //        //if (_Map.Slots[Count].cube.specialCubeType == SpecialCubeType.Diagonal)
        //        //    _Map.Slots[Count].cube.specialCubeType = SpecialCubeType.Null;


        //        _Map.Slots[Count].cube.DestroyCube(true,true);
        //    }

        //    CheckCount++;
        //}

        //CheckCount = 1;

        //// 7시 방향
        //while (true)
        //{
        //    int Count = _SlotNum + ((MatchBase.MaxHorizon - 1) * CheckCount);

        //    if (Count >= _Map.BottomLeft ||
        //        Count % MatchBase.MaxHorizon == 0)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC6_Null)
        //    {


        //        //if (_Map.Slots[Count].cube.specialCubeType == SpecialCubeType.Diagonal)
        //        //    _Map.Slots[Count].cube.specialCubeType = SpecialCubeType.Null;

        //        _Map.Slots[Count].cube.DestroyCube(true,true);
        //    }

        //    CheckCount++;
        //}


        //CheckCount = 1;

        //// 5시 방향
        //while (true)
        //{
        //    int Count = _SlotNum + ((MatchBase.MaxHorizon + 1) * CheckCount);

        //    if (Count >= _Map.BottomLeft ||
        //        Count % MatchBase.MaxHorizon == MatchBase.MaxHorizon -1)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC6_Null)
        //    {
        //        //if (_Map.Slots[Count].cube.specialCubeType == SpecialCubeType.Diagonal)
        //        //    _Map.Slots[Count].cube.specialCubeType = SpecialCubeType.Null;

        //        _Map.Slots[Count].cube.DestroyCube(true,true);
        //    }

        //    CheckCount++;
        //}

        //_Map.Slots[_SlotNum].cube.DestroyCube(true,true);
    }

    public void SpecialCubeEvent(MapManager _Map, int _SlotNum, SpecialCubeType _Type)
    {
        if (CurrentCheckBoomTime < MaxCheckBoomTime /2f)
            theBattle.AddComboValue();

        CurrentCheckBoomTime = MaxCheckBoomTime;
        switch (_Type)
        {
            case SpecialCubeType.Horizon:
                FindHorizonCube(_Map, _SlotNum);
                break;

            case SpecialCubeType.Vertical:
                FindVerticalCube(_Map, _SlotNum);
                break;

            case SpecialCubeType.Diagonal:
                FindDiagonalCube(_Map, _SlotNum);
                break;
        }

    }






    public void GirlSkill(SelectGirl _Girl, MapManager _Map, int _SlotNum)
    {
        switch (_Girl)
        {
            case SelectGirl.G1_Alice:
                SkillAilce(_Map, _SlotNum);

                break;
            case SelectGirl.G2a222:
                break;
            case SelectGirl.G3_Beryl:
                SkillBeryl(_Map, _SlotNum);
                break;
            case SelectGirl.G4a444:
                break;
            case SelectGirl.G5a555:
                break;
        }
    }


    //앨리스 스킬
    public void SkillAilce(MapManager _Map, int _SlotNum)
    {
        //float InvokeTime = theGirl.Girls[(int)PuzzleManager.Instance.selectGirl].SkillTime;
        //float Damage = theGirl.Girls[(int)SelectGirl.G1_Alice].SkillDamage;
        //if (_Map.Slots[_SlotNum - MatchBase.MaxHorizon].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    //if (_Map.Slots[_SlotNum - MatchBase.MaxHorizon].nodeColor == NodeColor.NC7_Special)
        //    //{
        //    //    Special = false;
        //    //}

        //    _Map.Slots[_SlotNum - MatchBase.MaxHorizon].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}

        //if (_Map.Slots[_SlotNum + MatchBase.MaxHorizon].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    //if (_Map.Slots[_SlotNum + MatchBase.MaxHorizon].nodeColor == NodeColor.NC7_Special)
        //    //{
        //    //    Special = false;
        //    //}

        //    _Map.Slots[_SlotNum + MatchBase.MaxHorizon].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}

        //if (_Map.Slots[_SlotNum - 1].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    //if (_Map.Slots[_SlotNum - 1].nodeColor == NodeColor.NC7_Special)
        //    //{
        //    //    Special = false;
        //    //}

        //    _Map.Slots[_SlotNum - 1].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}
        //if (_Map.Slots[_SlotNum + 1].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    //if (_Map.Slots[_SlotNum + 1].nodeColor == NodeColor.NC7_Special)
        //    //{
        //    //    Special = false;
        //    //}

        //    _Map.Slots[_SlotNum + 1].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}
        ////if (_Map.Slots[_SlotNum].nodeColor == NodeColor.NC7_Special)
        ////{
        ////    Special = false;
        ////}

        //_Map.Slots[_SlotNum].cube.DestroyCube(false, true, Damage, InvokeTime);


    }

    public void SkillBeryl(MapManager _Map, int _SlotNum)
    {
        //float Damage = theGirl.Girls[(int)SelectGirl.G3_Beryl].SkillDamage;
        //float InvokeTime = theGirl.Girls[(int)PuzzleManager.Instance.selectGirl].SkillTime;
        //// 11시
        //if (_Map.Slots[_SlotNum - MatchBase.MaxHorizon -1].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    _Map.Slots[_SlotNum - MatchBase.MaxHorizon -1].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}


        ////1시

        //if (_Map.Slots[_SlotNum - MatchBase.MaxHorizon + 1].nodeType != PuzzleSlot.NodeType.Null)
        //{

        //    _Map.Slots[_SlotNum - MatchBase.MaxHorizon + 1].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}

        //// 7시
        //if (_Map.Slots[_SlotNum + MatchBase.MaxHorizon - 1].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    _Map.Slots[_SlotNum + MatchBase.MaxHorizon - 1].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}


        //// 5시

        //if (_Map.Slots[_SlotNum + MatchBase.MaxHorizon + 1].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    _Map.Slots[_SlotNum + MatchBase.MaxHorizon + 1].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}
        //_Map.Slots[_SlotNum].cube.DestroyCube(false, true, Damage, InvokeTime);



    }





}
