using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Direction direction;

    public Camera MainCamera;

    public BoxCollider2D Bound;

    public float Speed;
    bool Down;
    float Lerp = 0;
    public float HRadious;
    public float VRadious;

    public Vector2 MaxBound;
    public Vector2 MinBound;
    public Vector2 NowPos;
    public Vector2 MoveVec;

    //인게임 3Match 방향대로 플레이어이동 
    //버튼결정안나서 
    //전투씬 3Match 
    //

    private PuzzleManager thePuzzle;
    void Start()
    {
        MainCamera = GetComponent<Camera>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        CheckSizeInit();
    }

    // Update is called once per frame


    private void FixedUpdate()
    {
        if (Down)
        {
            if (direction == Direction.Up)
            {
                if (this.transform.position.y + VRadious > MaxBound.y)
                {

                    this.transform.position = new Vector3(
                        this.transform.position.x,
                        MaxBound.y - VRadious,-10);
                    Down = false;
                    Lerp = 0;
                    return;
                }

            }
            else if (direction == Direction.Down)
            {
                if (this.transform.position.y - VRadious < MinBound.y)
                {

                    this.transform.position = new Vector3(
                        this.transform.position.x,
                        MinBound.y + VRadious,-10);
                    Down = false;
                    Lerp = 0;
                    return;
                }
            }
            else if (direction == Direction.Left)
            {
                if (this.transform.position.x - HRadious < MinBound.x)
                {

                    this.transform.position = new Vector3(
                        MinBound.x + HRadious,
                        this.transform.position.y,-10);
                    Down = false;
                    Lerp = 0;
                    return;
                }
            }
            else if (direction == Direction.Right)
            {
                if (this.transform.position.x + HRadious > MaxBound.x)
                {

                    this.transform.position = new Vector3(
                        MaxBound.x - HRadious,
                        this.transform.position.y,-10);
                    Down = false;
                    Lerp = 0;
                    return;
                }
            }

            Lerp = Mathf.Lerp(Lerp, 20, 0.01f);
            this.transform.Translate(MoveVec * Lerp  *Time.fixedDeltaTime* Speed);
        }
    }



    public void CheckSizeInit()
    {
        MinBound = Bound.bounds.min;
        MaxBound = Bound.bounds.max;
     
        VRadious = 2 * Camera.main.orthographicSize; 
        HRadious = VRadious * Camera.main.aspect;
        VRadious /= 2;
        HRadious /= 2;

    }


    public void MoveDonw(int _Direction)
    {
        Down = true;




        if (_Direction == 0)  // 위
        {
            direction = Direction.Up;
            MoveVec = Vector2.up;
        }
        else if (_Direction == 1) //아래
        {
            direction = Direction.Down;
            MoveVec = Vector2.down;
        }
        else if (_Direction == 2) // 왼쪽
        {
            direction = Direction.Left;
            MoveVec = Vector2.left;
        }
        else if (_Direction == 3) //오른쪽
        {
            direction = Direction.Right;
            MoveVec = Vector2.right;
        }

    }

    public void MoveUp()
    {
        Down = false;
        Lerp = 0;
    }













}
