using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum State
    { 
        SmoothMove,
        Nothing,
        SonMap,
    }

    public State state;

    public Direction direction;

    public Camera MainCamera;

    public float CameraSpeed = 1f;
    public float Speed;
    public bool Down;
    public float HRadious;
    public float VRadious;

    public Vector2 MaxBound;
    public Vector2 MinBound;
    public Vector2 NowPos;
    public Vector3 MoveVec = new Vector3(0,0,-10f);
    float LerfTime = 0;
    float CurrentMoveSpeed = 0;

    //인게임 3Match 방향대로 플레이어이동 
    //버튼결정안나서 
    //전투씬 3Match 
    //

    private PuzzleManager thePuzzle;
    private PuzzleMaker theMaker;
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;


    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
    void Start()
    {
        theMaker = FindObjectOfType<PuzzleMaker>();
        MainCamera = GetComponent<Camera>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }

    // Update is called once per frame


    private void FixedUpdate()
    {
        if (state == State.SonMap)
        {
            if (Input.GetKey(KeyCode.W))
            {
                MoveVec += Vector3.up * Time.deltaTime * CameraSpeed;
                MoveVec.z = -10;
            }

            if (Input.GetKey(KeyCode.S))
            {
                MoveVec += Vector3.down * Time.deltaTime * CameraSpeed;
                MoveVec.z = -10;
            }

            if (Input.GetKey(KeyCode.A))
            {
                MoveVec += Vector3.left * Time.deltaTime * CameraSpeed;
                MoveVec.z = -10;
            }
            if (Input.GetKey(KeyCode.D))
            {
                MoveVec += Vector3.right * Time.deltaTime * CameraSpeed;
                MoveVec.z = -10;
            }



            this.transform.position = Vector3.Lerp(this.transform.position, MoveVec, Speed * Time.fixedDeltaTime);
        }
        if (state == State.SmoothMove)
        {
            if (Down == true)
            {
                if (LerfTime <= 0.5f)
                    LerfTime += Time.deltaTime;

                if (LerfTime > 0.25f)
                {
                    LerfTime = 1f;
                    CurrentMoveSpeed = Mathf.Lerp(CurrentMoveSpeed, CameraSpeed, 0.1f);
                }
               
            }
            else
            { 
                if(LerfTime >0)
                    LerfTime -= Time.deltaTime;

                if (LerfTime < 0.1f)
                {
                    LerfTime = 0;
                    CurrentMoveSpeed = 0;
                }
            }

            if (Down)
            {
                if (direction == Direction.Up)
                {
                    MoveVec += Vector3.up * Time.deltaTime * CurrentMoveSpeed;
                    MoveVec.z = -10;

                }
                else if (direction == Direction.Down)
                {
                    MoveVec += Vector3.down * Time.deltaTime * CurrentMoveSpeed;
                    MoveVec.z = -10;
                }
                else if (direction == Direction.Left)
                {
                    MoveVec += Vector3.left * Time.deltaTime * CurrentMoveSpeed;
                    MoveVec.z = -10;
                }
                else if (direction == Direction.Right)
                {
                    MoveVec += Vector3.right * Time.deltaTime * CurrentMoveSpeed;
                    MoveVec.z = -10;
                }
            }
            if (MoveVec.y + VRadious > MaxBound.y)
            {

                MoveVec = new Vector3(
                    this.transform.position.x,
                    MaxBound.y - VRadious, -10);
                Down = false;
            }
            if (MoveVec.y - VRadious < MinBound.y)
            {

                MoveVec = new Vector3(
                    this.transform.position.x,
                    MinBound.y + VRadious, -10);
                Down = false;
            }
            if (MoveVec.x - HRadious < MinBound.x)
            {

                MoveVec = new Vector3(
                    MinBound.x + HRadious,
                    this.transform.position.y, -10);
                Down = false;
            }
            if (MoveVec.x + HRadious > MaxBound.x)
            {

                MoveVec = new Vector3(
                    MaxBound.x - HRadious -0.01f,
                    this.transform.position.y, -10);
                Down = false;
            }
            this.transform.position = Vector3.Lerp(this.transform.position, MoveVec, Speed * Time.fixedDeltaTime);
        }

       
    }



    public void SetBound(MapManager _Map,Vector2 _TargetVec, bool _Move)
    {


        VRadious = 2 * Camera.main.orthographicSize; 
        HRadious = VRadious * Camera.main.aspect;
        VRadious /= 2;
        HRadious /= 2;
        MoveVec = new Vector3(_TargetVec.x, _TargetVec.y, -10);
        this.transform.position = MoveVec;
        if (_Move == true)
        {
            MaxBound.x = _Map.Slots[_Map.TopRight].transform.position.x+ 0.3f;
            MaxBound.y = _Map.Slots[_Map.TopRight].transform.position.y + 3f;
            MinBound.x = _Map.Slots[_Map.BottomLeft].transform.position.x - 0.3f;
            MinBound.y = _Map.Slots[_Map.BottomLeft].transform.position.y - 2.3f;


            if (Math.Abs(MaxBound.x - MinBound.x) < 5.8)
            {
                MoveVec.x = (MaxBound.x + MinBound.x) / 2;
                MinBound.x = MoveVec.x - HRadious;
                MaxBound.x = MoveVec.x + HRadious;

            }
            
            if (Math.Abs(MaxBound.y - MinBound.y) < 10)
            {
                MoveVec.y = (MaxBound.y + MinBound.y) / 2;

                MinBound.y = MoveVec.y - VRadious;
                MaxBound.y = MoveVec.y + VRadious;
            }


            if (MoveVec.y + VRadious > MaxBound.y)
            {

                MoveVec.y = MaxBound.y - VRadious;
            }
            if (MoveVec.y - VRadious < MinBound.y)
            {

                MoveVec.y = MinBound.y + VRadious;
            }
            if (MoveVec.x - HRadious < MinBound.x)
            {

                MoveVec.x = MinBound.x + HRadious;

            }
            if (MoveVec.x + HRadious > MaxBound.x)
            {

                MoveVec.x = MaxBound.x - HRadious;
            }


            state = State.SmoothMove;
            this.transform.position = MoveVec;

        }
        else
        {
            state = State.Nothing;
            this.transform.position = MoveVec;
        }






    }


    public void MoveDown(int _Direction)
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
