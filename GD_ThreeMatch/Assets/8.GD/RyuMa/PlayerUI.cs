using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

public enum PlayerUIState
{ 
    Null = 0,
    Idle,
    Die
}


public class PlayerUI : MonoBehaviour
{
    public MeshRenderer SpineMesh;
    public SkeletonAnimation SpinAnim;
    public PlayerUIState state;


    public GameObject Trigger;
    public Image HpStateImage;
    public Image HpSlider;
    public Text HpText;
    public Image SkillSlider;
    public Text SkillGaugeText;
    public Image GirlCubeImage;

    public int PlayerUINum;
    public NodeColor nodeColor;
    public float MaxHp;
    public float CurrentHp;
    public float MaxSkillGauge;
    public float CurrentSkillGauge;

    public bool SkillOn;



    private PuzzleManager thePuzzle;
    private GirlManager theGirl;
    private BattleManager theBattle;
    private void Start()
    {
        theBattle = FindObjectOfType<BattleManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theGirl = FindObjectOfType<GirlManager>();
    }


    // 캐릭터별 카드색, 0이면 왼쪽 1이면 오른쪽
    public void SetUi(int _nodeColor,int _PlayerNum)
    {
        if (_nodeColor == 0) //검은색
        {
            SkillSlider.color = new Color(0.42f, 0.42f, 0.42f);
        }
        else if (_nodeColor == 1) //파란색
        {
            SkillSlider.color = new Color(0.56f, 0.78f, 0.9f);
        }
        else if (_nodeColor == 2) // 주황색
        {
            SkillSlider.color = new Color(1f, 0.38f, 0.01f);
        }
        else if (_nodeColor == 3) // 핑크
        {
            SkillSlider.color = new Color(0.95f, 0.3f, 0.57f);
        }
        else if (_nodeColor == 4) // 빨간색
        {
            SkillSlider.color = new Color(0.94f, 0.11f, 0.01f);
        }
        else if (_nodeColor == 5) // 노란색
        {
            SkillSlider.color = new Color(1f, 0.89f, 0.51f);
        }
        nodeColor = (NodeColor)_nodeColor;
        PlayerUINum = _PlayerNum;
        GirlCubeImage.sprite = thePuzzle.GirlSprites[_nodeColor];
        MaxHp = theGirl.Girls[_nodeColor].Hp;
        CurrentHp = MaxHp;
        MaxSkillGauge = theGirl.Girls[_nodeColor].SkillCount;
        HpText.text = CurrentHp + "/" + MaxHp;
        CurrentSkillGauge = 0;
        SkillSlider.fillAmount = 0;
        SkillGaugeText.text = CurrentSkillGauge + "/" + MaxSkillGauge;


        SpineMesh.transform.localPosition = new Vector3(
                theGirl.Girls[_nodeColor].IllustPosX[_PlayerNum],
                theGirl.Girls[_nodeColor].IllustPosY,
                thePuzzle.IllustSlot.transform.position.z);
        SpineMesh.transform.localScale = new Vector3(
            theGirl.Girls[_nodeColor].IllustSize,
            theGirl.Girls[_nodeColor].IllustSize,
            1);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "AttackEffect")
        {

            TakeDamage(collision.GetComponent<AttackEffect>());


        }



    }


    public void CheckSkill()
    {
        theBattle.ReadySkill((SkillUI)PlayerUINum);
    }


    // 스킬 활성화 여부, true일 경우 스킬 활성화 이펙트
    public void SetSkill(bool On)
    {
        if (On == true)
        {
            SpinAnim.skeleton.SetColor(Color.black);
            SkillOn = true;
        }
        else
        {
            SpinAnim.skeleton.SetColor(new Color(1,1,1));
            SkillOn = false;
        }
    }







    public void TakeDamage(AttackEffect _Effect)
    {
        CurrentHp -= _Effect.DamageValue;
        if (CurrentHp < 0)
            CurrentHp = 0;
        HpSlider.fillAmount = CurrentHp / MaxHp;
        HpText.text = CurrentHp + "/" + MaxHp;
        if (CurrentHp <= 0)
        {
            PlayerDie();
        }
        else
        { 
            //데미지 애니메이션 추가하기
        }

        if (_Effect.AttackEvent == true)
        {
            theBattle.BattleEvent = true;
        }

        if (theBattle.EnemyAttackEffectList.Contains(_Effect.gameObject))
        {
            theBattle.EnemyAttackEffectList.Remove(_Effect.gameObject);
        }

        _Effect.Resetting();



    }


    public void AddSkillGauge(int _CubeCount)
    {

        if (_CubeCount < 0 || state == PlayerUIState.Die)
            return;

        CurrentSkillGauge += _CubeCount;
        if (CurrentSkillGauge > MaxSkillGauge)
            CurrentSkillGauge = MaxSkillGauge;

        SkillSlider.fillAmount = CurrentSkillGauge / MaxSkillGauge;

        if (CurrentSkillGauge == MaxSkillGauge)
            GirlCubeImage.color = new Color(1, 1, 1, 1);

        SkillGaugeText.text = CurrentSkillGauge + "/" + MaxSkillGauge;

    }

    public void ResetSkillGauge()
    {
        CurrentSkillGauge = 0;
        GirlCubeImage.color = new Color(1, 1, 1, 0);
        SkillSlider.fillAmount = CurrentSkillGauge / MaxSkillGauge;
        SkillGaugeText.text = CurrentSkillGauge + "/" + MaxSkillGauge;
    }


    public float GetSkillGauge()
    {

        return CurrentSkillGauge / MaxSkillGauge;
    }


    public void PlayerDie()
    {
        if ((int)theBattle.CurrentSkillUI == PlayerUINum)
        {
            theBattle.ReadySkill(SkillUI.UI2_Null);
        }

        state = PlayerUIState.Die;

        SpinAnim.AnimationState.SetAnimation(0, "Die", true);
        
    }


}
