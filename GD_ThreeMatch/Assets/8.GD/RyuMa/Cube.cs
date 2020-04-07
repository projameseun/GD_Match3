using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    Vector2 TargetVec;
    float Speed;
    bool Move;
    bool OnlyOneEvent = false;


    public int Num;




    private PuzzleManager thePuzzle;

    private void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }



    private void FixedUpdate()
    {
        if (Move == true)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, TargetVec, Speed);

            if(Vector2.Distance(this.transform.position, TargetVec) <= 0)
            {
                Move = false;

                if(OnlyOneEvent == true)
                {
                    thePuzzle.CubeEvent = true;
                    OnlyOneEvent = false;
                }

            }
                
        }
    }


    public void MoveCube(Vector2 _vec, bool _Event = false, float _Speed = 0.05f)
    {
        Move = true;
        TargetVec = _vec;
        OnlyOneEvent = _Event;
        Speed = _Speed;
    }





}
