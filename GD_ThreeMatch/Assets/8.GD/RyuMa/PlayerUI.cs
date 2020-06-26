using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using Spine;
using TMPro;

public enum PlayerUIState
{ 
    Null = 0,
    Idle,
    Die
}


public class PlayerUI : MonoBehaviour
{
    public SkeletonAnimation SpinAnim;
    public PlayerUIState state;


    public GameObject Trigger;
    public Image HpStateImage;
    public Image HpSlider;
    public Image HpRedSlider;
    public TextMeshPro HpText;
    public Image SkillBgImage;
    public Image SkillSlider;
    public TextMeshPro SkillGaugeText;
    public Image GirlCubeImage;

    public int PlayerUINum;
    public SelectGirl selectGirl;
    public float MaxHp;
    public float CurrentHp;
    public float MaxSkillGauge;
    public float CurrentSkillGauge;

    public bool SkillOn;
    

    bool HpRedEventOnOff = false;
    float HpRedEventTime;

    //trunk
    string AnimName;
    GameObject ClickP;

    //데미지 이벤트
    bool DamageEvent;
    float DamageTime;
    Color DamageColor = new Color(1,1,1,1);

    //스킬 on off 이밴트
    bool SkillOnEvent;
    bool AddScale;
    Vector3 ScaleVec = new Vector3(1,1,1);
    float ImageScale;

    private PuzzleManager thePuzzle;
    private GirlManager theGirl;
    private BattleManager theBattle;
    private ObjectManager theObject;

    private void Start()
    {
        theObject = FindObjectOfType<ObjectManager>();
        theBattle = FindObjectOfType<BattleManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theGirl = FindObjectOfType<GirlManager>();
        
    }

    private void Update()
    {
        if (DamageEvent == true)
        {
            if (DamageTime <= 1)
            {
                DamageTime += Time.deltaTime * 2;
                DamageColor.g = DamageTime;
                DamageColor.b = DamageTime;
                SpinAnim.skeleton.SetColor(DamageColor);
            }
            else
            {
                DamageEvent = false;
                DamageTime = 0f;
                SpinAnim.skeleton.SetColor(new Color(1, 1, 1));
            }
        }

        if (SkillOnEvent == true)
        {
            if (AddScale == true)
            {
                ImageScale += Time.deltaTime*2.5f;
                if (ImageScale >= 1.3f)
                {
                    AddScale = false;
                }
            }
            else
            {
                ImageScale -= Time.deltaTime*2.5f;
                if (ImageScale <= 0.8f)
                {
                    AddScale = true;
                }
            }
            if (ImageScale > 0.9f && ImageScale < 1.1f)
            {
                ScaleVec.x = ImageScale;
                ScaleVec.y = ImageScale;
            }
          
            GirlCubeImage.transform.localScale = ScaleVec;
        }
    }
    public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "Hit_End")
        {
            ChangeAnim("Idle",true);
        }
        if (e.Data.Name == "Attack_End")
        {
            ChangeAnim("Idle", true);
        }
    }

    // 캐릭터별 카드색, 0이면 왼쪽 1이면 오른쪽
    public void SetUi(int _nodeColor,int _PlayerNum)
    {
        SpinAnim.skeletonDataAsset = theGirl.Girls[_nodeColor].IllustData;
        SpinAnim.Initialize(true);
        SpinAnim.state.Event += HandleEvent;


        SkillSlider.sprite = thePuzzle.PlayerSkillSprites[_nodeColor];
        SkillBgImage.sprite = thePuzzle.PlayerSkillBGSprites[_nodeColor];
        
        if (_nodeColor == 0) //파란색
        {
            GirlCubeImage.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
        else if (_nodeColor == 1) // 초록색
        {
            GirlCubeImage.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_nodeColor == 2) // 핑크
        {
            GirlCubeImage.transform.localScale = new Vector3(1, 1,1);
        }
        else if (_nodeColor == 3) // 빨간색
        {
            GirlCubeImage.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        else if (_nodeColor == 4) // 노란색
        {
            GirlCubeImage.transform.localScale = new Vector3(1, 1, 1);
        }
        GirlCubeImage.sprite = thePuzzle.CubeSprites[_nodeColor];
        selectGirl = (SelectGirl)_nodeColor;
        PlayerUINum = _PlayerNum;
       
        MaxHp = theGirl.Girls[_nodeColor].Hp;
        CurrentHp = MaxHp;
        HpRedSlider.fillAmount = 1;
        MaxSkillGauge = theGirl.Girls[_nodeColor].SkillCount;

        HpText.text = string.Format("{0:#,###}/{1:#,###}", CurrentHp, MaxHp);
        CurrentSkillGauge = 0;
        SkillSlider.fillAmount = 0;
        SkillGaugeText.text = CurrentSkillGauge + "/" + MaxSkillGauge;


        SpinAnim.transform.localPosition = new Vector3(
                theGirl.Girls[_nodeColor].IllustPosX[_PlayerNum],
                theGirl.Girls[_nodeColor].IllustPosY,
                thePuzzle.IllustSlot.transform.position.z);
        SpinAnim.transform.localScale = new Vector3(
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
    public void ChangeSelectGirl()
    {
        thePuzzle.ChangePlayer((SelectGirl)((int)selectGirl));
    }


    // 스킬 활성화 여부, true일 경우 스킬 활성화 이펙트
    public void SetSkill(bool On)
    {
        if (On == true)
        {
            thePuzzle.Player.SetSpine((int)selectGirl);

   
            ClickP = theObject.SpawnClickP(Trigger.transform.position);
            SkillOnEvent = true;
            ImageScale = 1;
            SkillOn = true;
        }
        else
        {
            if (ClickP != null)
            {
                if (ClickP.activeSelf == true)
                {
                    ClickP.SetActive(false);
                    ClickP = null;
                }
            }
            SkillOnEvent = false;
            ImageScale = 1;
            GirlCubeImage.transform.localScale = new Vector3(1, 1, 1);
            SkillOn = false;
        }
    }

    public void ChangeAnim(string _state, bool _Loop = false)
    {
        if (_state == AnimName)
            return;
        SpinAnim.AnimationState.SetAnimation(0, _state, _Loop);
        AnimName = _state;
    }

     





    public void TakeDamage(AttackEffect _Effect)
    {
        theObject.DamageTextEvent(Trigger.transform.position, _Effect.DamageValue.ToString());
        TakeDamageEvent(_Effect.DamageValue);


        if (_Effect.AttackEvent == true)
        {
            theBattle.BattleEvent = true;
        }

        if (theBattle.EnemyAttackEffectList.Contains(_Effect.gameObject))
        {
            theBattle.EnemyAttackEffectList.Remove(_Effect.gameObject);
        }

        //몬스터 파티클
        theBattle.EnemyPEvent(this.transform.position);


        _Effect.Resetting();



    }

    public void TakeDamageEvent(float _Value)
    {
        CurrentHp -= (int)_Value;
        if (CurrentHp < 0)
            CurrentHp = 0;
        HpSlider.fillAmount = CurrentHp / MaxHp;
        HpText.text = string.Format("{0:#,###}/{1:#,###}", CurrentHp, MaxHp);

        HpRedEventTime = 1f;
        if (HpRedEventOnOff == false)
        {
            
            HpRedEventOnOff = true;
            StartCoroutine(HpRedEvent());
        }


        if (CurrentHp <= 0)
        {
            PlayerDie();
        }
        else
        {
            ChangeAnim("Hit");
            DamageEvent = true;
            DamageTime = 0f;

            //데미지 애니메이션 추가하기
        }
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
        SkillOnEvent = false;
        ImageScale = 1;
        GirlCubeImage.transform.localScale = new Vector3(1, 1, 1);
        SkillOn = false;
        ChangeAnim("Attack");
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
        ChangeAnim("Die");
        DamageEvent = false;
        DamageTime = 0f;
        SpinAnim.skeleton.SetColor(new Color(1, 1, 1));
        if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        {
            if (thePuzzle.selectGirl == selectGirl)
            {
                if (PlayerUINum == 0)
                {
                    thePuzzle.Player.SetSpine((int)thePuzzle.playerUIs[1].selectGirl);
                }
                else
                {
                    thePuzzle.Player.SetSpine((int)thePuzzle.playerUIs[0].selectGirl);
                }
            }
        }
      
        if ((int)theBattle.CurrentSkillUI == PlayerUINum)
        {
            theBattle.ReadySkill(SkillUI.UI2_Null);
        }

        state = PlayerUIState.Die;

        //if ((int)thePuzzle.selectGirl == (int)selectGirl)
        //{
        //    if (PlayerUINum == 0)
        //    {
        //        if(thePuzzle.playerUIs[1].state != PlayerUIState.Die)
        //        thePuzzle.selectGirl = (SelectGirl)thePuzzle.playerUIs[1].selectGirl;
        //    }
        //    else if (PlayerUINum == 1)
        //    {
        //        if (thePuzzle.playerUIs[0].state != PlayerUIState.Die)
        //            thePuzzle.selectGirl = (SelectGirl)thePuzzle.playerUIs[0].selectGirl;
        //    }
        //}


    }


    IEnumerator HpRedEvent()
    {
        while (HpRedSlider.fillAmount >= (CurrentHp / MaxHp))
        {
            if (HpRedEventTime > 0)
            {
                HpRedEventTime -= Time.deltaTime;
            }
            else
            {
                HpRedSlider.fillAmount -= Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
        HpRedEventTime = 0;
        HpRedSlider.fillAmount = (CurrentHp / MaxHp);
        HpRedEventOnOff = false;
    }



}
