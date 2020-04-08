using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    public PuzzleManager puzzleManager;
    public PuzzleSlot puzzleSlot;
    public List<PuzzleSlot> currentMathces = new List<PuzzleSlot>();

    // Start is called before the first frame update
    void Start()
    {
        puzzleSlot = FindObjectOfType<PuzzleSlot>();
        puzzleManager = FindObjectOfType<PuzzleManager>();
    }

    public void FindAllMatches()
    {
        
        StartCoroutine(FindAllMatchesCo());
        
    }

    private IEnumerator FindAllMatchesCo()
    {
        
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < 144; i++)
        {
            // ??? NULL값으로 인한 강제 리턴현상 
            PuzzleSlot currentDot = puzzleManager.Slots[i];
            // ??? NULL값으로 인한 강제 리턴현상 
            if (currentDot.nodeColor != PuzzleSlot.NodeColor.Blank &&
                currentDot.nodeColor != PuzzleSlot.NodeColor.Player &&
                currentDot.nodeType != PuzzleSlot.NodeType.Null)
            {
                
                if (i > 12 && i < 143)
                {
                    PuzzleSlot leftCube = puzzleSlot.pSlots[i - 1];
                    PuzzleSlot rightCube = puzzleSlot.pSlots[i + 1];
                    if (leftCube.nodeType != PuzzleSlot.NodeType.Null && rightCube.nodeType != PuzzleSlot.NodeType.Null)
                    {
                        if (leftCube.tag == currentDot.tag && rightCube.tag == currentDot.tag)
                        {
                            if (!currentMathces.Contains(leftCube))
                            {
                                currentMathces.Add(leftCube);
                            }
                            leftCube.GetComponent<PuzzleManager>().isMatched = true;
                            leftCube.nodeColor = PuzzleSlot.NodeColor.Blank;
                            leftCube.cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .2f);

                            if (!currentMathces.Contains(rightCube))
                            {
                                currentMathces.Add(rightCube);
                            }
                            rightCube.GetComponent<PuzzleManager>().isMatched = true;
                            rightCube.nodeColor = PuzzleSlot.NodeColor.Blank;
                            rightCube.cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .2f);

                            if (!currentMathces.Contains(currentDot))
                            {
                                currentMathces.Add(currentDot);
                            }
                            currentDot.GetComponent<PuzzleManager>().isMatched = true;
                            currentDot.nodeColor = PuzzleSlot.NodeColor.Blank;
                            currentDot.cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .2f);
                        }
                    }

                }

                PuzzleSlot upCube = puzzleSlot.pSlots[i - 12];
                PuzzleSlot downCube = puzzleSlot.pSlots[i + 12];
                if (upCube.nodeType != PuzzleSlot.NodeType.Null && downCube.nodeType != PuzzleSlot.NodeType.Null)
                {
                    if (upCube.tag == currentDot.tag && downCube.tag == currentDot.tag)
                    {
                        if (!currentMathces.Contains(upCube))
                        {
                            currentMathces.Add(upCube);
                        }
                        upCube.GetComponent<PuzzleManager>().isMatched = true;
                        upCube.nodeColor = PuzzleSlot.NodeColor.Blank;
                        upCube.cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .2f);

                        if (!currentMathces.Contains(downCube))
                        {
                            currentMathces.Add(downCube);
                        }
                        downCube.GetComponent<PuzzleManager>().isMatched = true;
                        downCube.nodeColor = PuzzleSlot.NodeColor.Blank;
                        downCube.cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .2f);

                        if (!currentMathces.Contains(currentDot))
                        {
                            currentMathces.Add(currentDot);
                        }
                        currentDot.GetComponent<PuzzleManager>().isMatched = true;
                        currentDot.nodeColor = PuzzleSlot.NodeColor.Blank;
                        currentDot.cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .2f);
                    }
                }
            }
        }

    }
}
