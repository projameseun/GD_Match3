using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move
}

public class Board : MonoBehaviour
{
    public GameState currentState = GameState.move;
    public int width;   // 가로
    public int height;  // 세로
    public int offSet;
    public GameObject tilePrefab;
    public GameObject[] dots;
    public GameObject destroyEffect;
    public GameObject[,] allDots;

    private BackgroundTile[,] allTiles;
    private FindMatches findMatches;

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }

    //-------- 보드 생성
    private void SetUp()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                //-------- 블록 생성시 슬라이드 방향설정
                Vector2 tempPosition = new Vector2(i, j + offSet);
                //-------- 블록 생성시 슬라이드 방향설정
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + ", " + j + " )";

                //-------- 블록 생성
                int dotToUse = Random.Range(0, dots.Length);

                //-------- 게임시작시 Match되지 않게 하기
                int maxIterations = 0;
                while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                    //Debug.Log("maxIterations : " + maxIterations);
                }
                maxIterations = 0;
                //-------- 게임시작시 Match되지 않게 하기

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Dot>().row = j;
                dot.GetComponent<Dot>().colume = i;
                dot.transform.parent = this.transform;
                dot.name = "(" + i + ", " + j + " )";
                //-------- 블록 생성

                allDots[i, j] = dot;
            }
        }
    }
    //-------- 보드 생성

    //-------- 게임시작시 Match되지 않게 하기
    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if(column > 1 && row > 1)
        {
            if(allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if(column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if(allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }
    //-------- 게임시작시 Match되지 않게 하기

    //-------- Match시 Dots 파괴
    // 파괴 Dots 체크
    private void DestroyMatchesAt(int column, int row)
    {
        if(allDots[column, row].GetComponent<Dot>().isMatched)
        {
            //--------- Matched check2
            findMatches.currentMathces.Remove(allDots[column, row]);
            //--------- Matched check2
            //--------- 파괴 파티클생성과 삭제
            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f);
            //--------- 파괴 파티클생성과 삭제
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    // Dots 파괴
    public void DestroyMatches()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allDots[i, j] != null)
                {
                    //DestroyMatchesAt(i, j);
                    
                }
            }
        }
        //-------- Dots 내려오게 하기
        StartCoroutine(DecreaseRowCo());
        //-------- Dots 내려오게 하기
    }
    //-------- Match시 Dots 파괴

    //-------- Dots 내려오게 하기
    //------------- TEST ----------------
    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++) 
        {
            for (int j = 0; j < height; j++)
            {
                if(allDots[i, j] == null)
                {
                    nullCount++;
                    //Debug.Log("nullCount : " + nullCount);
                }
                else if(nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        //-------- Dot채우기
        StartCoroutine(FillBoardCo());
        //-------- Dot채우기
    }
    //------------- TEST ----------------
    //-------- Dots 내려오게 하기

    //-------- Dot채우기
    private void RefillBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allDots[i, j] == null)
                {
                    //-------- 블록 생성시 슬라이드 방향설정
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    //-------- 블록 생성시 슬라이드 방향설정
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().colume = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for(int i = 0; i< width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allDots[i, j] != null)
                {
                    if(allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while(MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }

        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }
    //-------- Dot채우기
}
