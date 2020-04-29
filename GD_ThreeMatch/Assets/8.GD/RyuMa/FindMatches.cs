using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{

    // 큐브를 터트릴 경우 조건을 통합적으로 표시해주는 구간
    //_Map.Slots[i].nodeColor != NodeColor.Player &&
    //_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
    //_Map.Slots[i].nodeColor != NodeColor.Blank &&





    public List<PuzzleSlot> currentMathces = new List<PuzzleSlot>();
    List<int> SpecialCubeList = new List<int>();



    private PuzzleManager thePuzzle;

    // Start is called before the first frame update
    void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }

    public void FindAllMatches(MapManager _Map,bool _ChangeBlank = true)
    {


        currentMathces = new List<PuzzleSlot>();
        for (int i = 0; i < _Map.Horizontal * _Map.Vertical; i++)
        {

            if (_Map.Slots[i].nodeColor != NodeColor.Player &&
                _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
                _Map.Slots[i].nodeColor != NodeColor.Blank &&
                _Map.Slots[i].cube.specialCubeType == SpecialCubeType.Null)
            {

                if (i > _Map.TopRight && i < _Map.BottomLeft)
                {

                    if (_Map.Slots[i-1].nodeType != PuzzleSlot.NodeType.Null &&
                        _Map.Slots[i+1].nodeType != PuzzleSlot.NodeType.Null &&
                        _Map.Slots[i +1].cube.specialCubeType == SpecialCubeType.Null &&
                        _Map.Slots[i -1].cube.specialCubeType == SpecialCubeType.Null)
                    {

                        if (_Map.Slots[i-1].nodeColor == _Map.Slots[i].nodeColor && _Map.Slots[i+1].nodeColor == _Map.Slots[i].nodeColor)
                        {
                            if (!currentMathces.Contains(_Map.Slots[i-1]))
                            {
                               
                                currentMathces.Add(_Map.Slots[i-1]);
                            }
                            thePuzzle.isMatched = true;
                            
                           

                            if (!currentMathces.Contains(_Map.Slots[i+1]))
                            {
                                currentMathces.Add(_Map.Slots[i+1]);
                            }


                            if (!currentMathces.Contains(_Map.Slots[i]))
                            {
                                currentMathces.Add(_Map.Slots[i]);
                            }

                            
                            
                        }
                    }

                    if (_Map.Slots[i + _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null &&
                        _Map.Slots[i - _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null &&
                        _Map.Slots[i + _Map.Horizontal].cube.specialCubeType == SpecialCubeType.Null &&
                        _Map.Slots[i - _Map.Horizontal].cube.specialCubeType == SpecialCubeType.Null)
                    {
                        if (_Map.Slots[i + _Map.Horizontal].nodeColor == _Map.Slots[i].nodeColor &&
                            _Map.Slots[i - _Map.Horizontal].nodeColor == _Map.Slots[i].nodeColor)
                        {

                            if (!currentMathces.Contains(_Map.Slots[i + _Map.Horizontal]))
                            {
                                currentMathces.Add(_Map.Slots[i + _Map.Horizontal]);
                            }
                            thePuzzle.isMatched = true;


                            if (!currentMathces.Contains(_Map.Slots[i - _Map.Horizontal]))
                            {
                                currentMathces.Add(_Map.Slots[i - _Map.Horizontal]);
                            }

                            if (!currentMathces.Contains(_Map.Slots[i]))
                            {
                                currentMathces.Add(_Map.Slots[i]);
                            }
                        }
                    }
                }
            }
        }
        if (_ChangeBlank == true)
        {
            for (int i = 0; i < currentMathces.Count; i++)
            {
                currentMathces[i].nodeColor = NodeColor.Blank;
            }
            currentMathces.Clear();
        }

    }

    public void FindSpecialCube(MapManager _Map)
    {


        for (int i = 0; i < _Map.TopRight - _Map.TopLeft; i++)
        {
            for (int Num = _Map.TopLeft + _Map.Horizontal; Num < _Map.BottomLeft;)
            {
                if (_Map.Slots[Num + i].nodeColor != NodeColor.Null)
                {
                    int Count = 1;
                    SpecialCubeList.Add(Num + i);

                    while (true)
                    {
                        if (_Map.Slots[Num + i + (_Map.Horizontal * Count)].cube != null)
                        {
                            if (_Map.Slots[Num + i].cube.nodeColor == _Map.Slots[Num + i + (_Map.Horizontal * Count)].cube.nodeColor)
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
                                    if (SpecialCubeList.Contains(thePuzzle.SelectNum))
                                    {
                                        _Map.Slots[thePuzzle.SelectNum].cube.SpecialCube = true;
                                        _Map.Slots[thePuzzle.SelectNum].cube.specialCubeType = SpecialCubeType.Vertical;
                                        _Map.Slots[thePuzzle.SelectNum].nodeColor = NodeColor.Special;
                                    }
                                    else if (SpecialCubeList.Contains(thePuzzle.OtherNum))
                                    {
                                        _Map.Slots[thePuzzle.OtherNum].cube.SpecialCube = true;
                                        _Map.Slots[thePuzzle.OtherNum].cube.specialCubeType = SpecialCubeType.Vertical;
                                        _Map.Slots[thePuzzle.OtherNum].nodeColor = NodeColor.Special;
                                    }
                                    else
                                    {
                                        int rand = Random.Range(0, SpecialCubeList.Count);
                                        _Map.Slots[SpecialCubeList[rand]].nodeColor = NodeColor.Special;
                                        _Map.Slots[SpecialCubeList[rand]].cube.SpecialCube = true;
                                        _Map.Slots[SpecialCubeList[rand]].cube.specialCubeType = SpecialCubeType.Vertical;
                                    }



                                    Debug.Log("특수블럭 세로 생성");
                                    //if()
                                    break;
                                }
                                else
                                {
                                    if (SpecialCubeList.Contains(thePuzzle.SelectNum))
                                    {
                                        _Map.Slots[thePuzzle.SelectNum].cube.SpecialCube = true;
                                        _Map.Slots[thePuzzle.SelectNum].nodeColor = NodeColor.Special;
                                        _Map.Slots[thePuzzle.SelectNum].cube.specialCubeType = SpecialCubeType.Hanoi;
                                    }
                                    else if (SpecialCubeList.Contains(thePuzzle.OtherNum))
                                    {
                                        _Map.Slots[thePuzzle.OtherNum].cube.SpecialCube = true;
                                        _Map.Slots[thePuzzle.OtherNum].nodeColor = NodeColor.Special;
                                        _Map.Slots[thePuzzle.OtherNum].cube.specialCubeType = SpecialCubeType.Hanoi;

                                    }
                                    else
                                    {
                                        int rand = Random.Range(0, SpecialCubeList.Count);

                                        _Map.Slots[SpecialCubeList[rand]].cube.SpecialCube = true;
                                        _Map.Slots[SpecialCubeList[rand]].nodeColor = NodeColor.Special;
                                        _Map.Slots[SpecialCubeList[rand]].cube.specialCubeType = SpecialCubeType.Hanoi;

                                    }
                                    _Map.Slots[Num + i].cube.SpecialCube = true;
                                    Debug.Log("특수블럭 대각선 생성");

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
                                if (SpecialCubeList.Contains(thePuzzle.SelectNum))
                                {
                                    _Map.Slots[thePuzzle.SelectNum].cube.SpecialCube = true;
                                    _Map.Slots[thePuzzle.SelectNum].cube.specialCubeType = SpecialCubeType.Vertical;
                                    _Map.Slots[thePuzzle.SelectNum].nodeColor = NodeColor.Special;
                                }
                                else if (SpecialCubeList.Contains(thePuzzle.OtherNum))
                                {
                                    _Map.Slots[thePuzzle.OtherNum].cube.SpecialCube = true;
                                    _Map.Slots[thePuzzle.OtherNum].cube.specialCubeType = SpecialCubeType.Vertical;
                                    _Map.Slots[thePuzzle.OtherNum].nodeColor = NodeColor.Special;
                                }
                                else
                                {
                                    int rand = Random.Range(0, SpecialCubeList.Count);
                                    _Map.Slots[SpecialCubeList[rand]].nodeColor = NodeColor.Special;
                                    _Map.Slots[SpecialCubeList[rand]].cube.SpecialCube = true;
                                    _Map.Slots[SpecialCubeList[rand]].cube.specialCubeType = SpecialCubeType.Vertical;
                                }



                                Debug.Log("특수블럭 세로 생성");
                                //if()
                                break;
                            }
                            else
                            {
                                if (SpecialCubeList.Contains(thePuzzle.SelectNum))
                                {
                                    _Map.Slots[thePuzzle.SelectNum].cube.SpecialCube = true;
                                    _Map.Slots[thePuzzle.SelectNum].nodeColor = NodeColor.Special;
                                    _Map.Slots[thePuzzle.SelectNum].cube.specialCubeType = SpecialCubeType.Hanoi;
                                }
                                else if (SpecialCubeList.Contains(thePuzzle.OtherNum))
                                {
                                    _Map.Slots[thePuzzle.OtherNum].cube.SpecialCube = true;
                                    _Map.Slots[thePuzzle.OtherNum].nodeColor = NodeColor.Special;
                                    _Map.Slots[thePuzzle.OtherNum].cube.specialCubeType = SpecialCubeType.Hanoi;

                                }
                                else
                                {
                                    int rand = Random.Range(0, SpecialCubeList.Count);

                                    _Map.Slots[SpecialCubeList[rand]].cube.SpecialCube = true;
                                    _Map.Slots[SpecialCubeList[rand]].nodeColor = NodeColor.Special;
                                    _Map.Slots[SpecialCubeList[rand]].cube.specialCubeType = SpecialCubeType.Hanoi;

                                }
                                _Map.Slots[Num + i].cube.SpecialCube = true;
                                Debug.Log("특수블럭 대각선 생성");

                                break;
                            }
                        }
                       
                    }

                    Num += (Count * _Map.Horizontal);
                    SpecialCubeList.Clear();
                }
                else
                {
                    Num += _Map.Horizontal;
                }
            }

        }


        for (int i = _Map.TopLeft + _Map.Horizontal; i < _Map.BottomLeft; i += _Map.Horizontal)
        {
            for (int Num = 0; Num < _Map.TopRight;)
            {
                if (_Map.Slots[Num + i].nodeColor != NodeColor.Null)
                {
                    int Count = 1;
                    SpecialCubeList.Add(Num + i);

                    while (true)
                    {
                        if (_Map.Slots[Num + i + Count].cube != null)
                        {

                            if (_Map.Slots[Num + i].cube.nodeColor == _Map.Slots[Num + i + Count].cube.nodeColor)
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
                                    if (SpecialCubeList.Contains(thePuzzle.SelectNum))
                                    {
                                        if (_Map.Slots[thePuzzle.SelectNum].cube.SpecialCube == false)
                                        {
                                            _Map.Slots[thePuzzle.SelectNum].cube.SpecialCube = true;
                                            _Map.Slots[thePuzzle.SelectNum].cube.specialCubeType = SpecialCubeType.Horizon;
                                            _Map.Slots[thePuzzle.SelectNum].nodeColor = NodeColor.Special;

                                            break;
                                        }
                                        else
                                        {
                                            SpecialCubeList.Remove(thePuzzle.SelectNum);
                                        }

                                    }
                                    else if (SpecialCubeList.Contains(thePuzzle.OtherNum))
                                    {
                                        if (_Map.Slots[thePuzzle.OtherNum].cube.SpecialCube == false)
                                        {
                                            _Map.Slots[thePuzzle.OtherNum].cube.SpecialCube = true;
                                            _Map.Slots[thePuzzle.OtherNum].nodeColor = NodeColor.Special;
                                            _Map.Slots[thePuzzle.OtherNum].cube.specialCubeType = SpecialCubeType.Horizon;

                                            break;
                                        }
                                        else
                                        {
                                            SpecialCubeList.Remove(thePuzzle.OtherNum);
                                        }
                                    }

                                    int rand = Random.Range(0, SpecialCubeList.Count);

                                    _Map.Slots[SpecialCubeList[rand]].cube.SpecialCube = true;
                                    _Map.Slots[SpecialCubeList[rand]].cube.specialCubeType = SpecialCubeType.Horizon;
                                    _Map.Slots[SpecialCubeList[rand]].nodeColor = NodeColor.Special;
                                    Debug.Log("특수블럭 가로 생성");

                                    break;
                                }
                                else
                                {
                                    if (SpecialCubeList.Contains(thePuzzle.SelectNum))
                                    {
                                        if (_Map.Slots[thePuzzle.SelectNum].cube.SpecialCube == false)
                                        {
                                            _Map.Slots[thePuzzle.SelectNum].cube.SpecialCube = true;
                                            _Map.Slots[thePuzzle.SelectNum].cube.specialCubeType = SpecialCubeType.Hanoi;
                                            _Map.Slots[thePuzzle.SelectNum].nodeColor = NodeColor.Special;
                                            break;
                                        }
                                        else
                                        {
                                            SpecialCubeList.Remove(thePuzzle.SelectNum);
                                        }

                                    }
                                    else if (SpecialCubeList.Contains(thePuzzle.OtherNum))
                                    {
                                        if (_Map.Slots[thePuzzle.OtherNum].cube.SpecialCube == false)
                                        {
                                            _Map.Slots[thePuzzle.OtherNum].cube.SpecialCube = true;
                                            _Map.Slots[thePuzzle.OtherNum].cube.specialCubeType = SpecialCubeType.Hanoi;
                                            _Map.Slots[thePuzzle.OtherNum].nodeColor = NodeColor.Special;
                                            break;
                                        }
                                        else
                                        {
                                            SpecialCubeList.Remove(thePuzzle.OtherNum);
                                        }
                                    }

                                    int rand = Random.Range(0, SpecialCubeList.Count);

                                    _Map.Slots[SpecialCubeList[rand]].cube.SpecialCube = true;
                                    _Map.Slots[SpecialCubeList[rand]].cube.specialCubeType = SpecialCubeType.Hanoi;
                                    _Map.Slots[SpecialCubeList[rand]].nodeColor = NodeColor.Special;
                                    Debug.Log("특수블럭 대각선 생성");

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
                                if (SpecialCubeList.Contains(thePuzzle.SelectNum))
                                {
                                    if (_Map.Slots[thePuzzle.SelectNum].cube.SpecialCube == false)
                                    {
                                        _Map.Slots[thePuzzle.SelectNum].cube.SpecialCube = true;
                                        _Map.Slots[thePuzzle.SelectNum].cube.specialCubeType = SpecialCubeType.Horizon;
                                        _Map.Slots[thePuzzle.SelectNum].nodeColor = NodeColor.Special;

                                        break;
                                    }
                                    else
                                    {
                                        SpecialCubeList.Remove(thePuzzle.SelectNum);
                                    }

                                }
                                else if (SpecialCubeList.Contains(thePuzzle.OtherNum))
                                {
                                    if (_Map.Slots[thePuzzle.OtherNum].cube.SpecialCube == false)
                                    {
                                        _Map.Slots[thePuzzle.OtherNum].cube.SpecialCube = true;
                                        _Map.Slots[thePuzzle.OtherNum].nodeColor = NodeColor.Special;
                                        _Map.Slots[thePuzzle.OtherNum].cube.specialCubeType = SpecialCubeType.Horizon;

                                        break;
                                    }
                                    else
                                    {
                                        SpecialCubeList.Remove(thePuzzle.OtherNum);
                                    }
                                }

                                int rand = Random.Range(0, SpecialCubeList.Count);

                                _Map.Slots[SpecialCubeList[rand]].cube.SpecialCube = true;
                                _Map.Slots[SpecialCubeList[rand]].cube.specialCubeType = SpecialCubeType.Horizon;
                                _Map.Slots[SpecialCubeList[rand]].nodeColor = NodeColor.Special;
                                Debug.Log("특수블럭 가로 생성");

                                break;
                            }
                            else
                            {
                                if (SpecialCubeList.Contains(thePuzzle.SelectNum))
                                {
                                    if (_Map.Slots[thePuzzle.SelectNum].cube.SpecialCube == false)
                                    {
                                        _Map.Slots[thePuzzle.SelectNum].cube.SpecialCube = true;
                                        _Map.Slots[thePuzzle.SelectNum].cube.specialCubeType = SpecialCubeType.Horizon;
                                        _Map.Slots[thePuzzle.SelectNum].nodeColor = NodeColor.Special;
                                        break;
                                    }
                                    else
                                    {
                                        SpecialCubeList.Remove(thePuzzle.SelectNum);
                                    }

                                }
                                else if (SpecialCubeList.Contains(thePuzzle.OtherNum))
                                {
                                    if (_Map.Slots[thePuzzle.OtherNum].cube.SpecialCube == false)
                                    {
                                        _Map.Slots[thePuzzle.OtherNum].cube.SpecialCube = true;
                                        _Map.Slots[thePuzzle.OtherNum].cube.specialCubeType = SpecialCubeType.Horizon;
                                        _Map.Slots[thePuzzle.OtherNum].nodeColor = NodeColor.Special;
                                        break;
                                    }
                                    else
                                    {
                                        SpecialCubeList.Remove(thePuzzle.OtherNum);
                                    }
                                }

                                int rand = Random.Range(0, SpecialCubeList.Count);

                                _Map.Slots[SpecialCubeList[rand]].cube.SpecialCube = true;
                                _Map.Slots[SpecialCubeList[rand]].cube.specialCubeType = SpecialCubeType.Horizon;
                                _Map.Slots[SpecialCubeList[rand]].nodeColor = NodeColor.Special;
                                Debug.Log("특수블럭 대각선 생성");

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


    public void FindHorizonCube(MapManager _Map,int _SlotNum)
    {
        int HorizonNum =0;
        for (int i = 0; i < _Map.Vertical; i++)
        {
            if (_SlotNum < i * _Map.Horizontal)
            {
                HorizonNum = (i - 1) * _Map.Horizontal;
                break;
            }
        }

        bool CubeEvent = true;

        for (int i = HorizonNum; i < HorizonNum + _Map.Horizontal; i++)
        {
            if (_Map.Slots[i].nodeColor != NodeColor.Player &&
                _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
                _Map.Slots[i].nodeColor != NodeColor.Blank)
            {
                _Map.Slots[i].cube.DestroyCube(CubeEvent);
                CubeEvent = false;
            }
        }


    }




}
