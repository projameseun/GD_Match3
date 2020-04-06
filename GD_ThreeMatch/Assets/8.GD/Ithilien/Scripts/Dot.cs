using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int colume;  // 행
    public int row;     // 열
    public int previousColumn;  // 이전 행
    public int previousRow;     // 이전 열
    public float swipeAngle = 0;
    public float swipeResist = 1f;  // 스와이프시 최소 저항값
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    private FindMatches findMatches;
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    private GameObject otherDot;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //colume = targetX;
        //previousRow = row;
        //previousColumn = colume;
    }

    // Update is called once per frame
    void Update()
    {
        //--------- Matched check
        //FindMatches();
        if(isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }
        //--------- Matched check

        //--------- Dot 이동
        targetX = colume;
        targetY = row;
        // 좌우 이동
        if(Mathf.Abs(targetX - transform.position.x) > 0.1)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if(board.allDots[colume, row] != this.gameObject)
            {
                board.allDots[colume, row] = this.gameObject;
            }
            //--------- Matched check2
            findMatches.FindAllMatches();
            //--------- Matched check2
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }
        // 상하 이동
        if (Mathf.Abs(targetY - transform.position.y) > 0.1)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if (board.allDots[colume, row] != this.gameObject)
            {
                board.allDots[colume, row] = this.gameObject;
            }
            //--------- Matched check2
            findMatches.FindAllMatches();
            //--------- Matched check2
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
        //--------- Dot 이동
    }

    //--------- Dot 이동체크
    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        
        if (otherDot != null)
        {
            //--------- Match실패시 자리 되돌리기
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().colume = colume;
                row = previousRow;
                colume = previousColumn;

                yield return new WaitForSeconds(.5f);
                board.currentState = GameState.move;
            }
            //--------- Match실패시 자리 되돌리기
            //--------- Match성공시 Dots 파괴
            else
            {
                board.DestroyMatches();
            }
            //--------- Match성공시 Dots 파괴
            otherDot = null;
        }
    }
    //--------- Dot 이동체크

    //--------- Swipe 각도 계산
    private void OnMouseDown()
    {
        if (board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // MyVec
            //Debug.Log(firstTouchPosition);
        }
    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Vec
            CalulateAngle();
        }
    }

    private void CalulateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * Mathf.Rad2Deg; //180 / Mathf.PI;
            Debug.Log(swipeAngle);
            MovePieces();
            board.currentState = GameState.wait;
        }
        else
        {
            board.currentState = GameState.move;
        }
    }
    //--------- Swipe 각도 계산

    //--------- Dot 이동
    private void MovePieces()
    {
        // Right Swipe
        if(swipeAngle > -45 && swipeAngle <= 45 && colume < board.width - 1)
        {
            otherDot = board.allDots[colume + 1, row];
            previousRow = row;
            previousColumn = colume;
            otherDot.GetComponent<Dot>().colume -= 1;
            colume += 1;
        }
        // Up Swipe
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            otherDot = board.allDots[colume, row + 1];
            previousRow = row;
            previousColumn = colume;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        // Left Swipe
        else if ((swipeAngle > 135 || swipeAngle <= -135) && colume > 0)
        {
            otherDot = board.allDots[colume - 1, row];
            previousRow = row;
            previousColumn = colume;
            otherDot.GetComponent<Dot>().colume += 1;
            colume -= 1;
        }
        // Down Swipe
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            otherDot = board.allDots[colume, row - 1];
            previousRow = row;
            previousColumn = colume;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        //--------- Match실패시 자리 되돌리기
        StartCoroutine(CheckMoveCo());
        //--------- Match실패시 자리 되돌리기
    }
    //--------- Dot 이동

    //--------- Matched check
    private void FindMatches()
    {
        if(colume > 0 && colume < board.width - 1)
        {
            GameObject leftDot1 = board.allDots[colume - 1, row];
            GameObject rightDot1 = board.allDots[colume + 1, row];
            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot1 = board.allDots[colume, row + 1];
            GameObject downDot1 = board.allDots[colume, row - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
    //--------- Matched check
}
