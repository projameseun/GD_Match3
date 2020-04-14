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

    public void FindAllMatches(bool _ChangeBlank = true)
    {
        currentMathces = new List<PuzzleSlot>();
        for (int i = 0; i < puzzleManager.Horizontal * puzzleManager.Vertical; i++)
        {

            if (puzzleManager.MoveSlots[i].nodeColor != PuzzleSlot.NodeColor.Player &&
                puzzleManager.MoveSlots[i].nodeType != PuzzleSlot.NodeType.Null &&
                puzzleManager.MoveSlots[i].nodeColor != PuzzleSlot.NodeColor.Blank)
            {

                if (i > puzzleManager.TopRight && i < puzzleManager.BottomLeft)
                {

                    if (puzzleManager.MoveSlots[i-1].nodeType != PuzzleSlot.NodeType.Null &&
                        puzzleManager.MoveSlots[i+1].nodeType != PuzzleSlot.NodeType.Null)
                    {

                        if (puzzleManager.MoveSlots[i-1].nodeColor == puzzleManager.MoveSlots[i].nodeColor && puzzleManager.MoveSlots[i+1].nodeColor == puzzleManager.MoveSlots[i].nodeColor)
                        {
                            if (!currentMathces.Contains(puzzleManager.MoveSlots[i-1]))
                            {
                               
                                currentMathces.Add(puzzleManager.MoveSlots[i-1]);
                            }
                            puzzleManager.isMatched = true;
                            
                           

                            if (!currentMathces.Contains(puzzleManager.MoveSlots[i+1]))
                            {
                                currentMathces.Add(puzzleManager.MoveSlots[i+1]);
                            }


                            if (!currentMathces.Contains(puzzleManager.MoveSlots[i]))
                            {
                                currentMathces.Add(puzzleManager.MoveSlots[i]);
                            }

                            
                            
                        }
                    }

                    if (puzzleManager.MoveSlots[i + puzzleManager.Horizontal].nodeType != PuzzleSlot.NodeType.Null &&
                   puzzleManager.MoveSlots[i - puzzleManager.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                    {
                        if (puzzleManager.MoveSlots[i + puzzleManager.Horizontal].nodeColor == puzzleManager.MoveSlots[i].nodeColor &&
                            puzzleManager.MoveSlots[i - puzzleManager.Horizontal].nodeColor == puzzleManager.MoveSlots[i].nodeColor)
                        {

                            if (!currentMathces.Contains(puzzleManager.MoveSlots[i + puzzleManager.Horizontal]))
                            {
                                currentMathces.Add(puzzleManager.MoveSlots[i + puzzleManager.Horizontal]);
                            }
                            puzzleManager.isMatched = true;


                            if (!currentMathces.Contains(puzzleManager.MoveSlots[i - puzzleManager.Horizontal]))
                            {
                                currentMathces.Add(puzzleManager.MoveSlots[i - puzzleManager.Horizontal]);
                            }

                            if (!currentMathces.Contains(puzzleManager.MoveSlots[i]))
                            {
                                currentMathces.Add(puzzleManager.MoveSlots[i]);
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
