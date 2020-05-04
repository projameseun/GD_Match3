using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;


[System.Serializable]
public class SkillSlot
{
    public int SkillNum;            // 스킬 인덱스
    public int SkillCount;          // 스킬 횟수
    public float Percentage = 100;  // 스킬 확률
    public float SkillCoolDown;     // 스킬 쿨다운

}


// 스킬 범위
public enum AttackType
{ 
    Random1 =0,      // 랜덤으로 1명
    LowHpAttack,        // 가장 체력이 낮은 1명
    FullAttack      // 2명 모두
}
public enum EnemyTribe
{ 
    Null = 0,
    Slime
}

public enum EnemyRating
{ 
    Normal = 0,
    MiddleBoss,
    Boss
}




[System.Serializable]
public class EnemyBase
{
    public string EnemyName;
    public EnemyTribe enemyTribe;
    public EnemyRating enemyRating;
    public Sprite MonsterSprite;
    public float DamageValue;
    public int Count;
    public int[] CubeCount;
    public SkillSlot[] skillSlots;
}


[System.Serializable]
public class EnemySkill
{
    public string SkillName;
    public float MultiplyValue = 1;
    public int SkillEffectNum = 0;
    public AttackType attackType;
  

}



public enum BattleState
{
    Null = 0,
    BattleInit,
    EnemyAttack,



}





public class BattleManager : MonoBehaviour
{


    public EnemyBase[] Enemy;
    public EnemySkill[] EnemySkill;
    public BattleState battleState;
    [Space]
    // UI,오브젝트
    public Image EnemyImage;
    public Text EnemyName;
    public Image EnemyHpImage;
    public Text TimeText;
    public Text EnemyCountText;
    public CubeUI[] EnemyCubeUi;
    [Space]

    // 데이터베이스
    public bool BattleStart;    // 배틀 시작시 true
    public int SelectEnemyNum;  // 선택된 몬스터의 숫자
    public float MaxHp;         // 몬스터 최대체력
    public float CurrentHp;     // 몬스터 현제체력
    public float GameTime;      // 게임 남은시간
    public int CurrentEnemyCount;   // 적 공격카운트
    public bool DamageEvent;    // 몬스터 데미지 받으면 실행하는 이밴트
    public bool BattleEvent; //
    public int CurrentAttackCount;
    public float MaxSkillCoolDown;
    public float CurrentSkillCoolDown;
    public bool AttackEndEvent;

    //쓰래기통
    List<int> ColorNumList = new List<int>();
    float DamageTime;
    Color DamageColor = new Color(1, 1, 1);
    bool AttackInit;
    int SkillNum = 0;
    float damage = 0;
    Vector2 StartVec = new Vector2();
    GameObject TargetVec = null;









    private ObjectManager theObject;
    private PuzzleManager thePuzzle;
    private FadeManager theFade;
    private void Start()
    {

        theFade = FindObjectOfType<FadeManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theObject = FindObjectOfType<ObjectManager>();


        ColorNumList.Add(0);
        ColorNumList.Add(1);
        ColorNumList.Add(2);
        ColorNumList.Add(3);
        ColorNumList.Add(4);
        ColorNumList.Add(5);
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
                EnemyImage.color = DamageColor;
            }
            else
            {
                DamageEvent = false;
                DamageTime = 0f;
                EnemyImage.color = new Color(1, 1, 1);
            }
        }


        if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        {
            if (BattleStart)
            {
                UILoad();


            }

            if (thePuzzle.state == PuzzleManager.State.BattleEvent)
            {
                if (battleState == BattleState.EnemyAttack)
                {
                    if (BattleEvent == true)
                    {
                        EnemyAttackEnd();
                        return;
                    }
                    
                    EnemyAttackEvent();
                    //몬스터 공격 이밴트
                }
                else if (battleState == BattleState.BattleInit)
                {
                    thePuzzle.CheckEnemyCubeCount();
                }


            }




        }


    }


    public void SetBattle(int _enemyNum)
    {

        SelectEnemyNum = _enemyNum;
        EnemyImage.sprite = Enemy[_enemyNum].MonsterSprite;
        SetEnemyCount(Enemy[_enemyNum].Count);
        EnemyCountText.text = Enemy[_enemyNum].Count.ToString();
        MaxHp = 0;
        GameTime = 30;
        List<int> ColorNum = new List<int>(ColorNumList);


        for (int i = 0; i < EnemyCubeUi.Length; i++)
        {
            if (i < Enemy[_enemyNum].CubeCount.Length)
            {
                EnemyCubeUi[i].transform.parent.gameObject.SetActive(true);
                int RandColor = Random.Range(0, ColorNum.Count);
                EnemyCubeUi[i].SetCubeUi(ColorNum[RandColor], i,
                    thePuzzle.CubeSprites[ColorNum[RandColor]], Enemy[_enemyNum].CubeCount[i]);

                MaxHp += Enemy[_enemyNum].CubeCount[i];
                ColorNum.Remove(RandColor);
            }
            else
            {
                EnemyCubeUi[i].cubeColor = NodeColor.Null;
                EnemyCubeUi[i].transform.parent.gameObject.SetActive(false);
            }


        }
        TimeText.text = "Time : " + ((int)GameTime).ToString();
        CurrentHp = MaxHp;
    }


    public void UILoad()
    {
        GameTime -= Time.deltaTime;
        EnemyHpImage.fillAmount = CurrentHp / MaxHp;
        if (GameTime < 0)
        {

            Debug.Log("타임 오버");
            BattleStart = false;
            theFade.FadeIn();
        }
        else if (CurrentHp <= 0)
        {
            Debug.Log("배틀 승리");
            BattleStart = false;
            theFade.FadeIn();
        }

        TimeText.text = "Time : " + ((int)GameTime).ToString();
    }

    public void TakeDamage(int DamageCount)
    {
        DamageEvent = true;
        DamageTime = 0f;
        DamageColor.r = 1f;
        DamageColor.g = 0f;
        DamageColor.b = 0f;
        EnemyImage.color = DamageColor;
        CurrentHp += DamageCount;

    }



    // 공격 이밴트
    // 공격 시작시 해당 유닛의 스킬을 확률값에 맞게 정한다
    // 시작시 초기화 단계후 쿨다운에 맞춰서 스킬을 발사한다
    // 마지막 스킬에는 스킬 앤드 이밴트를 받아 스킬이 끝났다는걸 알려준다
    public void EnemyAttackEvent()
    {
        if (AttackInit == false)
        {
            float rand = Random.Range(0.0f, 100.0f);
            SkillNum = 0;

            if (Enemy[SelectEnemyNum].skillSlots.Length > 1)
            {
                for (int i = 0; i < Enemy[SelectEnemyNum].skillSlots.Length; i++)
                {
                    if (Enemy[SelectEnemyNum].skillSlots[i].Percentage >= rand)
                    {
                        SkillNum = Enemy[SelectEnemyNum].skillSlots[i].SkillNum;
                        break;
                    }
                }
            }
            damage = EnemySkill[SkillNum].MultiplyValue * Enemy[SelectEnemyNum].DamageValue;
            AttackInit = true;
            CurrentAttackCount = Enemy[SelectEnemyNum].skillSlots[SkillNum].SkillCount;
            MaxSkillCoolDown = Enemy[SelectEnemyNum].skillSlots[SkillNum].SkillCoolDown;
            CurrentSkillCoolDown = 0;
            AttackEndEvent = false;
        }
        else
        {

            if (CurrentAttackCount == 0)
                return;

            if (CurrentSkillCoolDown > 0)
            {
                CurrentSkillCoolDown -= Time.deltaTime;
            }
            else
            {
                CurrentAttackCount--;
                if (CurrentAttackCount == 0)
                    AttackEndEvent = true;
                // 쿨타임 적용
                CurrentSkillCoolDown = MaxSkillCoolDown;
                // 파티클 시작지점 적용 나중에 추가할것
                StartVec = EnemyImage.transform.position;


                // 스킬 타입분류
                if (EnemySkill[SkillNum].attackType == AttackType.Random1)
                {

                    if (thePuzzle.playerUIs[0].CurrentHp <= 0)
                    {
                        TargetVec = thePuzzle.playerUIs[1].Trigger.gameObject;
                    }
                    else if (thePuzzle.playerUIs[1].CurrentHp <= 0)
                    {
                        TargetVec = thePuzzle.playerUIs[0].Trigger.gameObject;
                    }
                    else
                    {
                        int randUI = Random.Range(0, 2);
                        TargetVec = thePuzzle.playerUIs[randUI].Trigger.gameObject;
                    }


                }
                else if (EnemySkill[SkillNum].attackType == AttackType.LowHpAttack)
                {
                    if (thePuzzle.playerUIs[0].CurrentHp <= 0)
                    {
                        TargetVec = thePuzzle.playerUIs[1].Trigger.gameObject;
                    }
                    else if (thePuzzle.playerUIs[1].CurrentHp <= 0)
                    {
                        TargetVec = thePuzzle.playerUIs[0].Trigger.gameObject;
                    }
                    else
                    {
                        if (thePuzzle.playerUIs[0].CurrentHp == thePuzzle.playerUIs[1].CurrentHp)
                        {
                            int randUI = Random.Range(0, 2);
                            TargetVec = thePuzzle.playerUIs[randUI].Trigger.gameObject;
                        }
                        else
                        {
                            if (thePuzzle.playerUIs[0].CurrentHp <= thePuzzle.playerUIs[1].CurrentHp)
                            {
                                TargetVec = thePuzzle.playerUIs[0].Trigger.gameObject;
                            }
                            else
                            {
                                TargetVec = thePuzzle.playerUIs[1].Trigger.gameObject;
                            }
                        }

                    }
                }
                else if (EnemySkill[SkillNum].attackType == AttackType.FullAttack)
                {
                    TargetVec = thePuzzle.playerUIs[0].Trigger.gameObject;
                    theObject.AttackEffectEvent(EnemyImage.transform.position,
                  TargetVec, (int)damage, EnemySkill[SkillNum].SkillEffectNum, false, true);

                    TargetVec = thePuzzle.playerUIs[1].Trigger.gameObject;
                    theObject.AttackEffectEvent(EnemyImage.transform.position,
                  TargetVec, (int)damage, EnemySkill[SkillNum].SkillEffectNum, AttackEndEvent, true);

                    return;
                }


                theObject.AttackEffectEvent(EnemyImage.transform.position,
                    TargetVec, (int)damage, EnemySkill[SkillNum].SkillEffectNum, AttackEndEvent, true);
            }

          

        }



    }
    public void EnemyAttackEnd()
    {
        battleState = BattleState.Null;
        thePuzzle.state = PuzzleManager.State.Ready;
        BattleEvent = false;
        SetEnemyCount(Enemy[SelectEnemyNum].Count);
        AttackInit = false;
    }




    public void SetEnemyCount(int _Count)
    {
        CurrentEnemyCount += _Count;
        if (CurrentEnemyCount < 0)
            CurrentEnemyCount = 0;
        EnemyCountText.text = CurrentEnemyCount.ToString();
    }


    public void Resetting()
    {
        SelectEnemyNum = 0;
        MaxHp = 0;
        CurrentHp = 0;
        CurrentEnemyCount = 0;
        GameTime = 0;
        CurrentEnemyCount = 0;
        BattleStart = false;
        DamageEvent = false;
    }





}
