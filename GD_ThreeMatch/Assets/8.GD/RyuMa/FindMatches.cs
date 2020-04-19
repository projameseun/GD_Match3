using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    public PuzzleManager puzzleManager;
    public List<PuzzleSlot> currentMathces = new List<PuzzleSlot>();

    // Start is called before the first frame update
    void Start()
    {
        puzzleManager = FindObjectOfType<PuzzleManager>();
    }

    public void FindAllMatches(MapManager _Map,bool _ChangeBlank = true)
    {


        int _Horizontal = _Map.Horizontal;
        int _Vertical = _Map.Vertical;
        int _TopRight = _Map.TopRight;
        int _BottomLeft = _Map.BottomLeft;




        currentMathces = new List<PuzzleSlot>();
        for (int i = 0; i < _Horizontal * _Vertical; i++)
        {

            if (_Map.Slots[i].nodeColor != NodeColor.Player &&
                _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
                _Map.Slots[i].nodeColor != NodeColor.Blank)
            {

                if (i > _TopRight && i < _BottomLeft)
                {

                    if (_Map.Slots[i-1].nodeType != PuzzleSlot.NodeType.Null &&
                        _Map.Slots[i+1].nodeType != PuzzleSlot.NodeType.Null)
                    {

                        if (_Map.Slots[i-1].nodeColor == _Map.Slots[i].nodeColor && _Map.Slots[i+1].nodeColor == _Map.Slots[i].nodeColor)
                        {
                            if (!currentMathces.Contains(_Map.Slots[i-1]))
                            {
                               
                                currentMathces.Add(_Map.Slots[i-1]);
                            }
                            puzzleManager.isMatched = true;
                            
                           

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

                    if (_Map.Slots[i + _Horizontal].nodeType != PuzzleSlot.NodeType.Null &&
                   _Map.Slots[i - _Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                    {
                        if (_Map.Slots[i + _Horizontal].nodeColor == _Map.Slots[i].nodeColor &&
                            _Map.Slots[i - _Horizontal].nodeColor == _Map.Slots[i].nodeColor)
                        {

                            if (!currentMathces.Contains(_Map.Slots[i + _Horizontal]))
                            {
                                currentMathces.Add(_Map.Slots[i + _Horizontal]);
                            }
                            puzzleManager.isMatched = true;


                            if (!currentMathces.Contains(_Map.Slots[i - _Horizontal]))
                            {
                                currentMathces.Add(_Map.Slots[i - _Horizontal]);
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

    
}
