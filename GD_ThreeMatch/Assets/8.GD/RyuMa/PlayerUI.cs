using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public MeshRenderer SpineMesh;
    public SkeletonAnimation SpinAnim;

    public Image HpStateImage;
    public Image HpSlider;
    public Text HpText;
    public Image SkillSlider;
    public Text SkillGaugeText;
    public Image GirlCubeImage;

    public float MaxHp;
    public float CurrentHp;
    public float MaxSkillGauge;
    public float CurrentSkillGauge;



    private PuzzleManager thePuzzle;
    private GirlManager theGirl;
    private void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theGirl = FindObjectOfType<GirlManager>();
    }


    // 캐릭터별 카드색, 0이면 왼쪽 1이면 오른쪽
    public void SetUi(int _nodeColor,int _Pos)
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

        GirlCubeImage.sprite = thePuzzle.GirlSprites[_nodeColor];
        MaxHp = theGirl.Girls[_nodeColor].Hp;
        CurrentHp = MaxHp;
        MaxSkillGauge = theGirl.Girls[_nodeColor].SkillCount;
        HpText.text = CurrentHp + "/" + MaxHp;
        CurrentSkillGauge = 0;
        SkillSlider.fillAmount = 0;
        SkillGaugeText.text = CurrentSkillGauge + "/" + MaxSkillGauge;

        Debug.Log(thePuzzle.IllustSlot.transform.position.y);
        SpineMesh.transform.localPosition = new Vector3(
                theGirl.Girls[_nodeColor].IllustPosX[_Pos],
                theGirl.Girls[_nodeColor].IllustPosY,
                thePuzzle.IllustSlot.transform.position.z);
        SpineMesh.transform.localScale = new Vector3(
            theGirl.Girls[_nodeColor].IllustSize,
            theGirl.Girls[_nodeColor].IllustSize,
            1);
    }

    
    public void TakeDamage(int _Damage)
    {
        CurrentHp -= _Damage;
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

    }


    public void AddSkillGauge(int _CubeCount)
    {

        if (_CubeCount < 0)
            return;

        CurrentSkillGauge += _CubeCount;
        if (CurrentSkillGauge > MaxSkillGauge)
            CurrentSkillGauge = MaxSkillGauge;

        SkillSlider.fillAmount = CurrentSkillGauge / MaxSkillGauge;

        if (CurrentSkillGauge == MaxSkillGauge)
            GirlCubeImage.color = new Color(1, 1, 1, 1);

        SkillGaugeText.text = CurrentSkillGauge + "/" + MaxSkillGauge;

    }


    public void PlayerDie()
    {
        SpinAnim.AnimationState.SetAnimation(0, "Die", true);
        
    }


}
