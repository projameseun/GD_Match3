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

    public void FindAllMatches(PuzzleSlot[] _Slot,bool _ChangeBlank = true)
    {
        currentMathces = new List<PuzzleSlot>();
        for (int i = 0; i < puzzleManager.Horizontal * puzzleManager.Vertical; i++)
        {

            if (_Slot[i].nodeColor != PuzzleSlot.NodeColor.Player &&
                _Slot[i].nodeType != PuzzleSlot.NodeType.Null &&
                _Slot[i].nodeColor != PuzzleSlot.NodeColor.Blank)
            {

                if (i > puzzleManager.TopRight && i < puzzleManager.BottomLeft)
                {

                    if (_Slot[i-1].nodeType != PuzzleSlot.NodeType.Null &&
                        _Slot[i+1].nodeType != PuzzleSlot.NodeType.Null)
                    {

                        if (_Slot[i-1].nodeColor == _Slot[i].nodeColor && _Slot[i+1].nodeColor == _Slot[i].nodeColor)
                        {
                            if (!currentMathces.Contains(_Slot[i-1]))
                            {
                               
                                currentMathces.Add(_Slot[i-1]);
                            }
                            puzzleManager.isMatched = true;
                            
                           

                            if (!currentMathces.Contains(_Slot[i+1]))
                            {
                                currentMathces.Add(_Slot[i+1]);
                            }


                            if (!currentMathces.Contains(_Slot[i]))
                            {
                                currentMathces.Add(_Slot[i]);
                            }

                            
                            
                        }
                    }

                    if (_Slot[i + puzzleManager.Horizontal].nodeType != PuzzleSlot.NodeType.Null &&
                   _Slot[i - puzzleManager.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                    {
                        if (_Slot[i + puzzleManager.Horizontal].nodeColor == _Slot[i].nodeColor &&
                            _Slot[i - puzzleManager.Horizontal].nodeColor == _Slot[i].nodeColor)
                        {

                            if (!currentMathces.Contains(_Slot[i + puzzleManager.Horizontal]))
                            {
                                currentMathces.Add(_Slot[i + puzzleManager.Horizontal]);
                            }
                            puzzleManager.isMatched = true;


                            if (!currentMathces.Contains(_Slot[i - puzzleManager.Horizontal]))
                            {
                                currentMathces.Add(_Slot[i - puzzleManager.Horizontal]);
                            }

                            if (!currentMathces.Contains(_Slot[i]))
                            {
                                currentMathces.Add(_Slot[i]);
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
                currentMathces[i].nodeColor = PuzzleSlot.NodeColor.Blank;
            }
            currentMathces.Clear();
        }

    }

    
}
