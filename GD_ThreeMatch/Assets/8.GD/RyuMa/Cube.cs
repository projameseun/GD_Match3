using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    Vector2 TargetVec;
    float Speed;
    bool Move;
    bool OnlyOneEvent = false;
    bool DestroyEvent = false;
    float DestoryTime = 1.0f;
    public NodeColor nodeColor;
    public int Num;
    public float Size;
    Color color = new Color(1f,1f,1f,1f);
    SpriteRenderer SpriteRen;
    public SpriteRenderer MinimapSprite;


    private PuzzleManager thePuzzle;
    private ObjectManager theObject;
    private BattleManager theBattle;
    private void Start()
    {
        theBattle = FindObjectOfType<BattleManager>();
        theObject = FindObjectOfType<ObjectManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        SpriteRen = GetComponent<SpriteRenderer>();
    }



    private void FixedUpdate()
    {
        if (Move == true)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, TargetVec, Speed);

    
            if(Vector2.Distance(this.transform.position, TargetVec) <= Speed/2)
            {
                this.transform.position = TargetVec;
                Move = false;

                if(OnlyOneEvent == true)
                {
                    thePuzzle.CubeEvent = true;
                    OnlyOneEvent = false;
                }

            }
                
        }

        if (DestroyEvent == true)
        {
            DestoryTime -= Time.deltaTime*1.5f;

            if (DestoryTime < 0.5f)
            {
                //전투
                if(thePuzzle.gameMode == PuzzleManager.GameMode.Battle && theBattle.BattleStart == true)
                    theBattle.TakeDamage();
                DestroyCubeEvent();
                if (OnlyOneEvent == true)
                {
                    OnlyOneEvent = false;
                    thePuzzle.CubeEvent = true;
                }
            }
            color.a = DestoryTime;
            SpriteRen.color = color;
        }

    }


    public void MoveCube(Vector2 _vec, bool _Event = false, float _Speed = 0.5f)
    {
        Move = true;
        TargetVec = _vec;
        OnlyOneEvent = _Event;
        Speed = _Speed;
    }


    public void DestroyCube(bool _Event = false)
    {
        OnlyOneEvent = _Event;
        DestroyEvent = true;
        DestoryTime = 1f;
    }
    public void DestroyCubeEvent()
    {
        GameObject CubeEffect = theObject.FindObj("CubeE");

        Vector2 Target= new Vector2(0,0);
        if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int x = 0; x < 6; x++)
                {
                    if ((int)nodeColor == (int)thePuzzle.PlayerCubeUI[x].cubeColor)
                    {

                        Target = thePuzzle.PlayerCubeUI[x].transform.position;
                        Debug.Log(Target);
                    }
                }

            }
        }
        else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        { 
            //전투 이동 구하기
        }

        CubeEffect.GetComponent<CubeEffect>().SetCubeEffect(this.transform.position,
            Target,
            nodeColor, 0, 1, true
            );

        GameObject Paricle = theObject.FindObj("CubeP", false);
        Paricle.transform.position = this.transform.position;
        Paricle.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(
            0, this.GetComponent<SpriteRenderer>().sprite);
        Paricle.GetComponent<ParticleManager>().ParticleSetting(false, null, 5);
        Paricle.SetActive(true);
        DestoryTime = 0;
        DestroyEvent = false;
    }


    public void Resetting()
    {
        OnlyOneEvent = false;
        DestroyEvent = false;
        DestoryTime = 1f;
        SpriteRen.color = new Color(1, 1, 1, 1);
        this.gameObject.SetActive(false);
    }


}
