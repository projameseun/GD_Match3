using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum NodeColor
{
    NC0_Blue = 0,
    NC1_Green,
    NC2_Pink,
    NC3_Red,
    NC4_Yellow,
    NC5_Random,
    NC6_Null
    
}




public class Block : MonoBehaviour
{





    //블럭이 무슨 블럭인지
    public BlockType blockType;

    //블럭의 색
    public NodeColor nodeColor;

    public List<Sprite> m_sprite;
    public List<SpriteRenderer> m_spriteRen;



    [HideInInspector] public PuzzleSlot m_Slot;




    // 매치가 가능한지
    public bool Match;

    // 연계시 폭발하는지
    public bool Burst;

    // 반칸을 채우는지
    public bool Gravity;

    // 빈칸을 넘어서 채울 수 있는가
    public bool GravityJump;

    // 이동이 가능한지
    public bool Switch;


    // burst중인지 아닌지 체크
    [HideInInspector]
    public bool Bursting;

    // 다용도 값
    [HideInInspector]
    public int m_Value;


    //Trunk
    Vector2 TargetVec;
    float Speed;


    [HideInInspector]
    PuzzleManager thePuzzle;




    virtual protected void Awake()
    {
        
    }


    //처음 생성시 연출 및 시작 부분
    virtual public void Init(PuzzleSlot _slot, string[] Data)
    {
        if (thePuzzle == null)
            thePuzzle = PuzzleManager.Instance;

        this.transform.position = _slot.transform.position;

        m_Slot = _slot;

        this.transform.SetParent(null);
        this.transform.localScale = Vector3.one;


    }


    virtual public void ChangeState(int _Num)
    { 
        
    }


    //매치및 이펙트로 터지는 이밴트
    virtual public void BurstEvent(PuzzleSlot _Slot)
    {

    }

    // 기본적으로 블럭을 움직이게 하는 함수
    virtual public void MoveEvent(PuzzleSlot Target, float _Speed)
    {
        m_Slot = Target;
        TargetVec = Target.transform.position;
        Speed = _Speed;
        if (this.gameObject.activeSelf == true)
            StartCoroutine(MoveCor());
    }
    IEnumerator MoveCor()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DOMove(TargetVec, Speed)).SetEase(Ease.Linear);

        while (Vector2.Distance(transform.position, TargetVec) >= 0.05f)
        {
            yield return null;
        }
        transform.position = TargetVec;

    }


    virtual public void Resetting()
    {
        ObjectManager.Instance.ResetObj(this.gameObject);
    }



    public void SetColor(NodeColor _color)
    {
        nodeColor = _color;
        if (_color == NodeColor.NC6_Null)
            return;
        if (_color == NodeColor.NC5_Random)
        {
            Debug.Log("렌덤은 들어오면 안됨!!");
            return;
        }
      
        m_spriteRen[0].sprite = m_sprite[(int)_color];
    }

    public void SetRandomColor(bool Init = true)
    {
        nodeColor = (NodeColor)Random.Range(0, 5);
        if(Init == true)
           m_spriteRen[0].sprite = m_sprite[(int)nodeColor];
    }

}
