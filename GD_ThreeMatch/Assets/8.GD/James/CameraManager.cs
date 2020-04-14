using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum State
    { 
        SmoothMove,
        Nothing
    }

    public State state;

    public Direction direction;

    public Camera MainCamera;

    public BoxCollider2D Bound;

    public float CameraSpeed = 1f;
    public float Speed;
    bool Down;
    public float HRadious;
    public float VRadious;

    public Vector2 MaxBound;
    public Vector2 MinBound;
    public Vector2 NowPos;
    public Vector3 MoveVec = new Vector3(0,0,-10f);

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
        if (state == State.SmoothMove)
        {
            if (Down)
            {
                if (direction == Direction.Up)
                {
                    MoveVec += Vector3.up * Time.deltaTime * CameraSpeed;
                    MoveVec.z = -10;

                    if (MoveVec.y + VRadious > MaxBound.y)
                    {

                        MoveVec = new Vector3(
                            this.transform.position.x,
                            MaxBound.y - VRadious, -10);
                        Down = false;
                        return;
                    }

                }
                else if (direction == Direction.Down)
                {
                    MoveVec += Vector3.down * Time.deltaTime * CameraSpeed;
                    MoveVec.z = -10;
                    if (MoveVec.y - VRadious < MinBound.y)
                    {

                        MoveVec = new Vector3(
                            this.transform.position.x,
                            MinBound.y + VRadious, -10);
                        Down = false;
                        return;
                    }
                }
                else if (direction == Direction.Left)
                {
                    MoveVec += Vector3.left * Time.deltaTime * CameraSpeed;
                    MoveVec.z = -10;
                    if (MoveVec.x - HRadious < MinBound.x)
                    {

                        MoveVec = new Vector3(
                            MinBound.x + HRadious,
                            this.transform.position.y, -10);
                        Down = false;
                        return;
                    }
                }
                else if (direction == Direction.Right)
                {
                    MoveVec += Vector3.right * Time.deltaTime * CameraSpeed;
                    MoveVec.z = -10;
                    if (MoveVec.x + HRadious > MaxBound.x)
                    {

                        MoveVec = new Vector3(
                            MaxBound.x - HRadious,
                            this.transform.position.y, -10);
                        Down = false;
                        return;
                    }
                }
            }
            this.transform.position = Vector3.Lerp(this.transform.position, MoveVec, Speed * Time.fixedDeltaTime);
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

        }
        else if (_Direction == 1) //아래
        {
            direction = Direction.Down;

        }
        else if (_Direction == 2) // 왼쪽
        {
            direction = Direction.Left;
      
        }
        else if (_Direction == 3) //오른쪽
        {
            direction = Direction.Right;
     
        }

    }

    public void MoveUp()
    {
        Down = false;
    }













}
