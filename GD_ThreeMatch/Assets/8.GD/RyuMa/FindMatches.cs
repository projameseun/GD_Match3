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





    public List<Block> currentMathces = new List<Block>();
    List<int> SpecialCubeList = new List<int>();
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


    //매치가 가능한 조건이 있는지 확인
    public bool FindAllMatches(MapManager _Map,bool _ChangeBlank = true)
    {

        for (int Hor = 0; Hor < _Map.BottomRight; Hor += GameManager.Instance.MaxHorizon)
        {
            for (int i = 1; i <= _Map.TopRight; i++)
            {
                if (_Map.Slots[i+Hor].block.nodeColor != NodeColor.NC5_Blank)
                {

                    if (i + Hor > _Map.TopRight && i + Hor < _Map.BottomLeft)
                    {

                        if (_Map.Slots[i+Hor - 1].block.nodeColor != NodeColor.NC5_Blank &&
                            _Map.Slots[i+Hor + 1].block.nodeColor != NodeColor.NC5_Blank)
                        {

                            if (_Map.Slots[i+Hor - 1].block.nodeColor == _Map.Slots[i+Hor].block.nodeColor &&
                                _Map.Slots[i+Hor + 1].block.nodeColor == _Map.Slots[i+Hor].block.nodeColor)
                            {
                                if (!currentMathces.Contains(_Map.Slots[i+Hor - 1].block))
                                {
                                    currentMathces.Add(_Map.Slots[i+Hor - 1].block);
                                }
                                if (!currentMathces.Contains(_Map.Slots[i+Hor + 1].block))
                                {
                                    currentMathces.Add(_Map.Slots[i+Hor + 1].block);
                                }
                                if (!currentMathces.Contains(_Map.Slots[i+Hor].block))
                                {
                                    currentMathces.Add(_Map.Slots[i+Hor].block);
                                }



                            }
                        }

                        if (_Map.Slots[i+Hor + _Map.Horizontal].block.nodeColor != NodeColor.NC5_Blank &&
                            _Map.Slots[i+Hor - _Map.Horizontal].block.nodeColor != NodeColor.NC5_Blank)
                        {
                            if (_Map.Slots[i+Hor + _Map.Horizontal].block.nodeColor == _Map.Slots[i+Hor].block.nodeColor &&
                                _Map.Slots[i+Hor - _Map.Horizontal].block.nodeColor == _Map.Slots[i+Hor].block.nodeColor)
                            {

                                if (!currentMathces.Contains(_Map.Slots[i+Hor + _Map.Horizontal].block))
                                {
                                    currentMathces.Add(_Map.Slots[i+Hor + _Map.Horizontal].block);
                                }

                                if (!currentMathces.Contains(_Map.Slots[i+Hor - _Map.Horizontal].block))
                                {
                                    currentMathces.Add(_Map.Slots[i+Hor - _Map.Horizontal].block);
                                }

                                if (!currentMathces.Contains(_Map.Slots[i+Hor].block))
                                {
                                    currentMathces.Add(_Map.Slots[i+Hor].block);
                                }
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

    //특수 큐브를 만들 수 있는지 확인
    public void FindSpecialCube(MapManager _Map)
    {


        for (int i = 1; i < _Map.TopRight; i++)
        {
            for (int Num = _Map.TopLeft + _Map.Horizontal; Num < _Map.BottomLeft;)
            {
                if (_Map.Slots[Num + i].block.nodeColor != NodeColor.NC5_Blank)
                {
                    int Count = 1;
                    SpecialCubeList.Add(Num + i);

                    while (true)
                    {
                        if (_Map.Slots[Num + i + (_Map.Horizontal * Count)].block != null)
                        {
                            if (_Map.Slots[Num + i].block.nodeColor == 
                                _Map.Slots[Num + i + (_Map.Horizontal * Count)].block.nodeColor)
                            {
                                SpecialCubeList.Add(Num + i + (_Map.Horizontal * Count));
                                Count++;
                            }
                            else
                            {
                                if (Count < 4)
                                {
                                    Count = 1;
                                    break;
                                }
                                else if (Count < 5)
                                {


                                    if (SpecialCubeList.Contains(PuzzleManager.Instance.SelectNum))
                                    {
                                        SpecialList.Add(new SpecialData(PuzzleManager.Instance.SelectNum, SpecialCubeType.Vertical));
                                    }
                                    else if (SpecialCubeList.Contains(PuzzleManager.Instance.OtherNum))
                                    {
                                        SpecialList.Add(new SpecialData(PuzzleManager.Instance.OtherNum, SpecialCubeType.Vertical));
                                    }
                                    else
                                    {

                                        for (int x = 0; x < SpecialList.Count; x++)
                                        {
                                            if (SpecialCubeList.Contains(SpecialList[i].Num))
                                            {
                                                SpecialCubeList.Remove(SpecialList[i].Num);
                                            }
                                        }
                                        int rand = Random.Range(0, SpecialCubeList.Count);

                                        SpecialList.Add(new SpecialData(SpecialCubeList[rand], SpecialCubeType.Vertical));
                                    }

                                    //if()
                                    break;
                                }
                                else
                                {
                                    if (SpecialCubeList.Contains(PuzzleManager.Instance.SelectNum))
                                    {
                                        SpecialList.Add(new SpecialData(PuzzleManager.Instance.SelectNum, SpecialCubeType.Diagonal));
                                    }
                                    else if (SpecialCubeList.Contains(PuzzleManager.Instance.OtherNum))
                                    {
                                        SpecialList.Add(new SpecialData(PuzzleManager.Instance.OtherNum, SpecialCubeType.Diagonal));
                                    }
                                    else
                                    {

                                        for (int x = 0; x < SpecialList.Count; x++)
                                        {
                                            if (SpecialCubeList.Contains(SpecialList[i].Num))
                                            {
                                                SpecialCubeList.Remove(SpecialList[i].Num);
                                            }
                                        }
                                        int rand = Random.Range(0, SpecialCubeList.Count);

                                        SpecialList.Add(new SpecialData(SpecialCubeList[rand], SpecialCubeType.Diagonal));
                                    }
                                    //_Map.Slots[Num + i].cube.SpecialCube = true;
                                    //Debug.Log("특수블럭 대각선 생성");

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
                            else if (Count < 5)
                            {
                                if (SpecialCubeList.Contains(PuzzleManager.Instance.SelectNum))
                                {
                                    SpecialList.Add(new SpecialData(PuzzleManager.Instance.SelectNum, SpecialCubeType.Vertical));
                                }
                                else if (SpecialCubeList.Contains(PuzzleManager.Instance.OtherNum))
                                {
                                    SpecialList.Add(new SpecialData(PuzzleManager.Instance.OtherNum, SpecialCubeType.Vertical));
                                }
                                else
                                {

                                    for (int x = 0; x < SpecialList.Count; x++)
                                    {
                                        if (SpecialCubeList.Contains(SpecialList[i].Num))
                                        {
                                            SpecialCubeList.Remove(SpecialList[i].Num);
                                        }
                                    }
                                    int rand = Random.Range(0, SpecialCubeList.Count);

                                    SpecialList.Add(new SpecialData(SpecialCubeList[rand], SpecialCubeType.Vertical));
                                }



                                //Debug.Log("특수블럭 세로 생성");
                                //if()
                                break;
                            }
                            else
                            {
                                if (SpecialCubeList.Contains(PuzzleManager.Instance.SelectNum))
                                {
                                    SpecialList.Add(new SpecialData(PuzzleManager.Instance.SelectNum, SpecialCubeType.Diagonal));
                                }
                                else if (SpecialCubeList.Contains(PuzzleManager.Instance.OtherNum))
                                {
                                    SpecialList.Add(new SpecialData(PuzzleManager.Instance.OtherNum, SpecialCubeType.Diagonal));
                                }
                                else
                                {

                                    for (int x = 0; x < SpecialList.Count; x++)
                                    {
                                        if (SpecialCubeList.Contains(SpecialList[i].Num))
                                        {
                                            SpecialCubeList.Remove(SpecialList[i].Num);
                                        }
                                    }
                                    int rand = Random.Range(0, SpecialCubeList.Count);

                                    SpecialList.Add(new SpecialData(SpecialCubeList[rand], SpecialCubeType.Diagonal));
                                }
                                //_Map.Slots[Num + i].cube.SpecialCube = true;
                                //Debug.Log("특수블럭 대각선 생성");

                                break;
                            }
                        }
                       
                    }

                    Num += (Count * GameManager.Instance.MaxHorizon);
                    SpecialCubeList.Clear();
                }
                else
                {
                    Num += GameManager.Instance.MaxHorizon;
                }
            }

        }


        for (int i = _Map.TopLeft + _Map.Horizontal; i < _Map.BottomLeft; i += GameManager.Instance.MaxHorizon)
        {
            for (int Num = 0; Num < _Map.TopRight;)
            {
                if (_Map.Slots[Num + i].block.nodeColor != NodeColor.NC5_Blank)
                {
                    int Count = 1;
                    SpecialCubeList.Add(Num + i);

                    while (true)
                    {
                        if (_Map.Slots[Num + i + Count].block != null)
                        {

                            if (_Map.Slots[Num + i].block.nodeColor == _Map.Slots[Num + i + Count].block.nodeColor)
                            {
                                SpecialCubeList.Add(Num + i + Count);
                                Count++;

                            }
                            else
                            {
                                if (Count < 4)
                                {
                                    Count = 1;
                                    break;
                                }
                                else if (Count < 5)
                                {
                                    bool Cross = false;
                                    if (SpecialCubeList.Contains(PuzzleManager.Instance.SelectNum))
                                    {
                                        for (int z = 0; z < SpecialList.Count; z++)
                                        {
                                            if (SpecialList[z].Num == PuzzleManager.Instance.SelectNum)
                                            {
                                                Cross = true;
                                                break;
                                            }
                                        }
                                        if (Cross == false)
                                        {
                                            SpecialList.Add(new SpecialData(PuzzleManager.Instance.SelectNum, SpecialCubeType.Horizon));

                                            break;
                                        }
                                        else
                                        {
                                            SpecialCubeList.Remove(PuzzleManager.Instance.SelectNum);
                                        }

                                    }
                                    else if (SpecialCubeList.Contains(PuzzleManager.Instance.OtherNum))
                                    {

                                        for (int z = 0; z < SpecialList.Count; z++)
                                        {
                                            if (SpecialList[z].Num == PuzzleManager.Instance.OtherNum)
                                            {
                                                Cross = true;
                                                break;
                                            }
                                        }
                                        if (Cross == false)
                                        {
                                            SpecialList.Add(new SpecialData(PuzzleManager.Instance.OtherNum, SpecialCubeType.Horizon));

                                            break;
                                        }
                                        else
                                        {
                                            SpecialCubeList.Remove(PuzzleManager.Instance.OtherNum);
                                        }

                                    }
                                    

                                    int rand = Random.Range(0, SpecialCubeList.Count);
                                    SpecialList.Add(new SpecialData(SpecialCubeList[rand], SpecialCubeType.Horizon));
                                    break;
                                }
                                else
                                {
                                    bool Cross = false;
                                    if (SpecialCubeList.Contains(PuzzleManager.Instance.SelectNum))
                                    {
                                        for (int z = 0; z < SpecialList.Count; z++)
                                        {
                                            if (SpecialList[z].Num == PuzzleManager.Instance.SelectNum)
                                            {
                                                Cross = true;
                                                break;
                                            }
                                        }
                                        if (Cross == false)
                                        {
                                            SpecialList.Add(new SpecialData(PuzzleManager.Instance.SelectNum, SpecialCubeType.Diagonal));

                                            break;
                                        }
                                        else
                                        {
                                            SpecialCubeList.Remove(PuzzleManager.Instance.SelectNum);
                                        }
                                    }
                                    else if (SpecialCubeList.Contains(PuzzleManager.Instance.OtherNum))
                                    {
                                        for (int z = 0; z < SpecialList.Count; z++)
                                        {
                                            if (SpecialList[z].Num == PuzzleManager.Instance.OtherNum)
                                            {
                                                Cross = true;
                                                break;
                                            }
                                        }
                                        if (Cross == false)
                                        {
                                            SpecialList.Add(new SpecialData(PuzzleManager.Instance.OtherNum, SpecialCubeType.Diagonal));

                                            break;
                                        }
                                        else
                                        {
                                            SpecialCubeList.Remove(PuzzleManager.Instance.OtherNum);
                                        }
                                    }
                                    int rand = Random.Range(0, SpecialCubeList.Count);
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
                            else if (Count < 5)
                            {
                                bool Cross = false;
                                if (SpecialCubeList.Contains(PuzzleManager.Instance.SelectNum))
                                {
                                    for (int z = 0; z < SpecialList.Count; z++)
                                    {
                                        if (SpecialList[z].Num == PuzzleManager.Instance.SelectNum)
                                        {
                                            Cross = true;
                                            break;
                                        }
                                    }
                                    if (Cross == false)
                                    {
                                        SpecialList.Add(new SpecialData(PuzzleManager.Instance.SelectNum, SpecialCubeType.Horizon));

                                        break;
                                    }
                                    else
                                    {
                                        SpecialCubeList.Remove(PuzzleManager.Instance.SelectNum);
                                    }

                                }
                                else if (SpecialCubeList.Contains(PuzzleManager.Instance.OtherNum))
                                {

                                    for (int z = 0; z < SpecialList.Count; z++)
                                    {
                                        if (SpecialList[z].Num == PuzzleManager.Instance.OtherNum)
                                        {
                                            Cross = true;
                                            break;
                                        }
                                    }
                                    if (Cross == false)
                                    {
                                        SpecialList.Add(new SpecialData(PuzzleManager.Instance.OtherNum, SpecialCubeType.Horizon));

                                        break;
                                    }
                                    else
                                    {
                                        SpecialCubeList.Remove(PuzzleManager.Instance.OtherNum);
                                    }

                                }


                                int rand = Random.Range(0, SpecialCubeList.Count);
                                SpecialList.Add(new SpecialData(SpecialCubeList[rand], SpecialCubeType.Horizon));
                                break;
                            }
                            else
                            {
                                bool Cross = false;
                                if (SpecialCubeList.Contains(PuzzleManager.Instance.SelectNum))
                                {
                                    for (int z = 0; z < SpecialList.Count; z++)
                                    {
                                        if (SpecialList[z].Num == PuzzleManager.Instance.SelectNum)
                                        {
                                            Cross = true;
                                            break;
                                        }
                                    }
                                    if (Cross == false)
                                    {
                                        SpecialList.Add(new SpecialData(PuzzleManager.Instance.SelectNum, SpecialCubeType.Diagonal));

                                        break;
                                    }
                                    else
                                    {
                                        SpecialCubeList.Remove(PuzzleManager.Instance.SelectNum);
                                    }
                                }
                                else if (SpecialCubeList.Contains(PuzzleManager.Instance.OtherNum))
                                {
                                    for (int z = 0; z < SpecialList.Count; z++)
                                    {
                                        if (SpecialList[z].Num == PuzzleManager.Instance.OtherNum)
                                        {
                                            Cross = true;
                                            break;
                                        }
                                    }
                                    if (Cross == false)
                                    {
                                        SpecialList.Add(new SpecialData(PuzzleManager.Instance.OtherNum, SpecialCubeType.Diagonal));

                                        break;
                                    }
                                    else
                                    {
                                        SpecialCubeList.Remove(PuzzleManager.Instance.OtherNum);
                                    }
                                }
                                int rand = Random.Range(0, SpecialCubeList.Count);
                                break;
                            }
                        }


                    }
                    SpecialCubeList.Clear();
                    Num += Count;

                }
                else
                {
                    Num += 1;
                }
            }
        }


        SpecialCubeList.Clear();
    }

    //가로 특수큐브
    public void FindHorizonCube(MapManager _Map,int _SlotNum)
    {

        //int HorizonNum =0;


        //_Map.Slots[_SlotNum].block.GetComponent<SpecialCube>().specialCubeType = SpecialCubeType.Null;

        //for (int i = 0; i < _Map.Vertical; i++)
        //{
        //    if (_SlotNum < i * _Map.Horizontal)
        //    {
        //        HorizonNum = (i - 1) * _Map.Horizontal;
        //        break;
        //    }
        //}
        //for (int i = HorizonNum; i < HorizonNum + _Map.Horizontal; i++)
        //{
        //    if (_Map.Slots[i].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[i].nodeColor != NodeColor.NC5_Blank)
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

        //for (int i = HorizonNum; i < HorizonNum + _Map.Horizontal; i++)
        //{
        //    if (_Map.Slots[i].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[i].nodeColor != NodeColor.NC5_Blank)
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
        //int Vertical = _SlotNum % _Map.Horizontal;
        //_Map.Slots[_SlotNum].cube.specialCubeType = SpecialCubeType.Null;


        //for (int i = Vertical; i < _Map.BottomLeft; i += _Map.Horizontal)
        //{
        //    if (_Map.Slots[i].nodeColor != NodeColor.NC6_Player &&
        //         _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
        //         _Map.Slots[i].nodeColor != NodeColor.NC5_Blank)
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

        //for (int i = Vertical; i < _Map.BottomLeft; i += _Map.Horizontal)
        //{
        //    if (_Map.Slots[i].nodeColor != NodeColor.NC6_Player &&
        //         _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
        //         _Map.Slots[i].nodeColor != NodeColor.NC5_Blank)
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
        //    int Count = _SlotNum - ((_Map.Horizontal + 1) * CheckCount);

        //    if (Count < _Map.TopRight ||
        //        Count % _Map.Horizontal == 0)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC5_Blank)
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

        //    int Count = _SlotNum - ((_Map.Horizontal - 1) * CheckCount);

        //    if (Count <= _Map.TopRight ||
        //        Count % _Map.Horizontal == _Map.Horizontal - 1)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC5_Blank)
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
        //    int Count = _SlotNum + ((_Map.Horizontal - 1) * CheckCount);

        //    if (Count >= _Map.BottomLeft ||
        //        Count % _Map.Horizontal == 0)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC5_Blank)
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
        //    int Count = _SlotNum + ((_Map.Horizontal + 1) * CheckCount);

        //    if (Count >= _Map.BottomLeft ||
        //        Count % _Map.Horizontal == _Map.Horizontal - 1)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC5_Blank)
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
        //    int Count = _SlotNum - ((_Map.Horizontal + 1) * CheckCount);

        //    if (Count < _Map.TopRight ||
        //        Count % _Map.Horizontal == 0)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC5_Blank)
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
        //    int Count = _SlotNum - ((_Map.Horizontal - 1) * CheckCount);

        //    if (Count <= _Map.TopRight ||
        //        Count % _Map.Horizontal == _Map.Horizontal - 1)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC5_Blank)
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
        //    int Count = _SlotNum + ((_Map.Horizontal - 1) * CheckCount);

        //    if (Count >= _Map.BottomLeft ||
        //        Count % _Map.Horizontal == 0)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC5_Blank)
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
        //    int Count = _SlotNum + ((_Map.Horizontal + 1) * CheckCount);

        //    if (Count >= _Map.BottomLeft ||
        //        Count % _Map.Horizontal == _Map.Horizontal -1)
        //    {
        //        break;
        //    }

        //    if (_Map.Slots[Count].nodeColor != NodeColor.NC6_Player &&
        //        _Map.Slots[Count].nodeType != PuzzleSlot.NodeType.Null &&
        //        _Map.Slots[Count].nodeColor != NodeColor.NC5_Blank)
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
        PuzzleManager.Instance.CubeEvent = true;
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
        //if (_Map.Slots[_SlotNum - _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    //if (_Map.Slots[_SlotNum - _Map.Horizontal].nodeColor == NodeColor.NC7_Special)
        //    //{
        //    //    Special = false;
        //    //}

        //    _Map.Slots[_SlotNum - _Map.Horizontal].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}

        //if (_Map.Slots[_SlotNum + _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    //if (_Map.Slots[_SlotNum + _Map.Horizontal].nodeColor == NodeColor.NC7_Special)
        //    //{
        //    //    Special = false;
        //    //}

        //    _Map.Slots[_SlotNum + _Map.Horizontal].cube.DestroyCube(false, true, Damage, InvokeTime);
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
        //if (_Map.Slots[_SlotNum - _Map.Horizontal -1].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    _Map.Slots[_SlotNum - _Map.Horizontal -1].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}


        ////1시

        //if (_Map.Slots[_SlotNum - _Map.Horizontal + 1].nodeType != PuzzleSlot.NodeType.Null)
        //{

        //    _Map.Slots[_SlotNum - _Map.Horizontal + 1].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}

        //// 7시
        //if (_Map.Slots[_SlotNum + _Map.Horizontal - 1].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    _Map.Slots[_SlotNum + _Map.Horizontal - 1].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}


        //// 5시

        //if (_Map.Slots[_SlotNum + _Map.Horizontal + 1].nodeType != PuzzleSlot.NodeType.Null)
        //{
        //    _Map.Slots[_SlotNum + _Map.Horizontal + 1].cube.DestroyCube(false, true, Damage, InvokeTime);
        //}
        //_Map.Slots[_SlotNum].cube.DestroyCube(false, true, Damage, InvokeTime);



    }





}
