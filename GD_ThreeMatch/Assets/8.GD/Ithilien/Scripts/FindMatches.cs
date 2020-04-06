using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMathces = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    //--------- Matched check2
    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i< board.width; i++)
        {
            for(int j = 0; j < board.height; j++)
            {
                GameObject currentDot = board.allDots[i, j];
                if(currentDot != null)
                {
                    if(i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if(leftDot != null && rightDot != null)
                        {
                            if(leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                if(!currentMathces.Contains(leftDot))
                                {
                                    currentMathces.Add(leftDot);
                                }
                                leftDot.GetComponent<Dot>().isMatched = true;

                                if (!currentMathces.Contains(rightDot))
                                {
                                    currentMathces.Add(rightDot);
                                }
                                rightDot.GetComponent<Dot>().isMatched = true;
                                if (!currentMathces.Contains(currentDot))
                                {
                                    currentMathces.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                }
                if (j > 0 && j < board.height - 1)
                {
                    GameObject upDot = board.allDots[i, j + 1];
                    GameObject downDot = board.allDots[i, j - 1];
                    if (upDot != null && downDot != null)
                    {
                        if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                        {
                            if (!currentMathces.Contains(upDot))
                            {
                                currentMathces.Add(upDot);
                            }
                            upDot.GetComponent<Dot>().isMatched = true;
                            if (!currentMathces.Contains(downDot))
                            {
                                currentMathces.Add(downDot);
                            }
                            downDot.GetComponent<Dot>().isMatched = true;
                            if (!currentMathces.Contains(currentDot))
                            {
                                currentMathces.Add(currentDot);
                            }
                            currentDot.GetComponent<Dot>().isMatched = true;
                        }
                    }
                }
            }
        }
    }
    //--------- Matched check2
}
