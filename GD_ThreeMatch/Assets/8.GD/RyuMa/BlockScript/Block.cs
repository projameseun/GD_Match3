using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

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

    // 폭발 주변에서 영향이 있는지
    public bool AroundBurst;

    // 반칸을 채우는지
    public bool Gravity;

    // 빈칸을 넘어서 채울 수 있는가
    public bool GravityJump;

    // 이동이 가능한지
    public bool Switch;

    // 흔들리는 기능
    public bool Shake;




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
    virtual public void BurstEvent(PuzzleSlot _Slot, Action action = null)
    {

    }

    // 기본적으로 블럭을 움직이게 하는 함수
    virtual public void MoveEvent(PuzzleSlot Target,float speed)
    {
        m_Slot = Target;
        TargetVec = Target.transform.position;
        Speed = speed;
        DOTween.Kill(this.transform);
        StopAllCoroutines();
        if (this.gameObject.activeSelf == true)
            StartCoroutine(MoveCor());
    }
    IEnumerator MoveCor()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DOMove(TargetVec, Speed).SetEase(Ease.Linear));
        while (Vector2.Distance(transform.position, TargetVec) >= 0.05f)
        {

            yield return null;
        }
        transform.position = TargetVec;
        seq.Kill();

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
        int Random = UnityEngine.Random.Range(0, thePuzzle.m_BlockSeed.Count);

        nodeColor = (NodeColor)int.Parse(thePuzzle.m_BlockSeed[Random].Data[1]);

        if (Init == true)
           m_spriteRen[0].sprite = m_sprite[(int)nodeColor];
    }






    bool shakking;

    //블럭이 이동후 멈췄을 때 애니메이션
    virtual public void DropEndAnim(Direction _DIR, GameObject _DownPivot = null, GameObject _UpPivot = null)
    {
        if(Shake == false)
           return ;
        if (_DownPivot == null)
            return;
        if (_UpPivot == null)
            return;
        Sequence seq = DOTween.Sequence();
        switch (_DIR)
        {
            case Direction.Down:
                // 위에서 아래
                //seq.Append(Pivot2.transform.DOLocalMoveY(Pivot1Distance - 0.05f, 0.1f));
                seq.Insert(0, _DownPivot.transform.DOScaleY(0.75f, 0.1f));
                seq.Insert(0, _DownPivot.transform.DOScaleX(1.1f, 0.1f));
                seq.Insert(0.1f, _DownPivot.transform.DOScaleY(1, 0.1f));
                seq.Insert(0.1f, _DownPivot.transform.DOScaleX(1f, 0.1f));
                //seq.Append(Pivot2.transform.DOLocalMoveY(PivotDistance, 0.1f));
                break;

            case Direction.Up:
                seq.Insert(0, _UpPivot.transform.DOScaleY(0.75f, 0.1f));
                seq.Insert(0, _UpPivot.transform.DOScaleX(1.1f, 0.1f));
                seq.Insert(0.1f, _UpPivot.transform.DOScaleX(1f, 0.1f));
                seq.Insert(0.1f, _UpPivot.transform.DOScaleY(1, 0.1f));
                //seq.Append(Pivot2.transform.DOLocalMoveY(-Pivot1Distance - 0.05f, 0.1f));
                //seq.Append(Pivot2.transform.DOLocalMoveY(-Pivot1Distance, 0.1f));
                break;

            case Direction.Left:
                seq.Append(_DownPivot.transform.DOLocalMoveX(0.05f, 0.1f));
                seq.Append(_DownPivot.transform.DOLocalMoveX(0, 0.1f));
                break;
            case Direction.Right:
                seq.Append(_DownPivot.transform.DOLocalMoveX(-0.05f, 0.1f));
                seq.Append(_DownPivot.transform.DOLocalMoveX(0, 0.1f));
                break;
        }

        seq.AppendCallback(() =>
        {
            shakking = false;
        });
    }

    //Target 방향 기준으로 흔들리는 애니메이션
    virtual public void ShakeEvent(Vector2 Target, GameObject _Pivot = null)
    {
        if (Shake == false)
            return;

        Vector2 vec = Target - (Vector2)this.transform.position;

        float AngleZ = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
        //이미지의 0도 중심이 아래일 경우 +90
        //이미지의 0도 중심이 위일 경우 -90
        AngleZ -= 90f;
        if (AngleZ < 0)
            AngleZ += 360;
        vec.Normalize();
        shakking = true;
        Sequence seq = DOTween.Sequence();
        seq.Insert(0, _Pivot.transform.DOLocalMove(-vec * 0.15f, 0.13f).SetRelative());
        seq.Insert(0.13f, _Pivot.transform.DOLocalMove(vec * 0.05f, 0.13f).SetRelative());
        seq.Insert(0.26f, _Pivot.transform.DOLocalMove(new Vector3(0, -0.3f, 0), 0.14f));
        //왼쪽
        if (AngleZ < 170 && AngleZ > 10)
        {
            seq.Insert(0, _Pivot.transform.DORotate(new Vector3(0, 0, -30), 0.13f));
            seq.Insert(0.13f, _Pivot.transform.DORotate(new Vector3(0, 0, 15), 0.13f));
            seq.Insert(0.26f, _Pivot.transform.DORotate(new Vector3(0, 0, 0), 0.14f));
        }
        //오른쪽
        else if (AngleZ > 190 && AngleZ < 350)
        {
            seq.Insert(0, _Pivot.transform.DORotate(new Vector3(0, 0, 30), 0.13f));
            seq.Insert(0.13f, _Pivot.transform.DORotate(new Vector3(0, 0, -15), 0.13f));
            seq.Insert(0.26f, _Pivot.transform.DORotate(new Vector3(0, 0, 0), 0.14f));
        }




        seq.Insert(0, _Pivot.transform.DOScaleY(0.8f, 0.2f));
        seq.Insert(0, _Pivot.transform.DOScaleX(1.1f, 0.2f));
        seq.Insert(0.2f, _Pivot.transform.DOScaleY(1f, 0.2f));
        seq.Insert(0.2f, _Pivot.transform.DOScaleX(1f, 0.2f));

        seq.InsertCallback(0.2f, () =>
        {
            shakking = false;
        });


    }





}
