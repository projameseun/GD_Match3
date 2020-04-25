using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpecialCubeType
{ 
    Vertical = 0,
    Horizon,
    Hanoi,
    Null
}

public class Cube : MonoBehaviour
{
    public SpecialCubeType specialCubeType = SpecialCubeType.Null;

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
    public bool SpecialCube = false;
   


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
                if (OnlyOneEvent == true)
                {
                    OnlyOneEvent = false;
                    thePuzzle.CubeEvent = true;
                }

                if (SpecialCube == true)
                {
                    color.a = 1;
                    DestroyEvent = false;
                    SpriteRen.color = color;
                    SpriteRen.sprite = thePuzzle.SpecialSprites[(int)specialCubeType];

                    return;

                }
                else
                {
                    DestroyCubeEvent();
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
        if (SpecialCube == true)
        {
            if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
            {
                thePuzzle.theMoveMap.Slots[Num].nodeColor = nodeColor;
            }
            else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
            {
                thePuzzle.theBattleMap.Slots[Num].nodeColor = nodeColor;
            }
        }


    }
    public void DestroyCubeEvent()
    {

        int CubeNum = 1;   // 플레이어면 1, 적이면 -1
        int CubeTarget = 0;  // 플레이어면 0, 적이면 1
        GameObject Target = null;
        if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int x = 0; x < 6; x++)
                {
                    if ((int)nodeColor == (int)thePuzzle.PlayerCubeUI[x].cubeColor)
                    {

                        Target = thePuzzle.PlayerCubeUI[x].gameObject;
                    }
                }

            }
        }
        else if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        {
            bool CheckEnemyColor = false;
            //전투 이동 구하기
            for (int i = 0; i < theBattle.EnemyCubeUi.Length; i++)
            {
                if (theBattle.EnemyCubeUi[i].gameObject.activeSelf == true)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        if ((int)nodeColor == (int)theBattle.EnemyCubeUi[x].cubeColor)
                        {
                            if (theBattle.EnemyCubeUi[x].CubeCount > 0)
                            {
                                Target = theBattle.EnemyCubeUi[x].gameObject;
                                CubeNum = -1;
                                CubeTarget = 1;
                                i = 6;
                                x = 6;
                                CheckEnemyColor = true;
                            }
                        }
                    }
                }
            }
            if (CheckEnemyColor == false)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        if ((int)nodeColor == (int)thePuzzle.PlayerCubeUI[x].cubeColor)
                        {
                            CubeNum = 1;
                            Target = thePuzzle.PlayerCubeUI[x].gameObject;
                        }
                    }

                }
            }

        }

        theObject.CubeEffectEvent(this.transform.position, Target, nodeColor,
            (CubeEffectType)CubeTarget, CubeNum, true);

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
        specialCubeType = SpecialCubeType.Null;
        SpecialCube = false;
        OnlyOneEvent = false;
        DestroyEvent = false;
        DestoryTime = 1f;
        SpriteRen.color = new Color(1, 1, 1, 1);
        this.gameObject.SetActive(false);
    }


}
