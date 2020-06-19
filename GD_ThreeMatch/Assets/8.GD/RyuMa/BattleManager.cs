﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Spine;
using TMPro;


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
    public SkeletonDataAsset IllustData;
    public int MinDamageValue;
    public int MaxDamageValue;
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
    public AttackEffectType attackEffectType;
  

}



public enum BattleState
{
    Null = 0,
    BattleInit,
    EnemyAttack,
    PlayerDie,
    EnemyDie,
    PlayerWind,
    PlayerLose



}



public enum SkillUI
{ 
    UI0_FirstGirl = 0,
    UI1_SecondGril,
    UI2_Null
}

public class BattleManager : MonoBehaviour
{


    public EnemyBase[] Enemy;
    public EnemySkill[] EnemySkill;
    public Sprite[] EnemyUiSprites;
    public Sprite[] ComboSprites;
    public BattleState battleState;
    [Space]
    // UI,오브젝트
    public SkeletonAnimation EnemyAnim;
    public TextMeshPro EnemyCountText;
    public CubeUI[] EnemyCubeUi;
    public Image[] ComboNumImages;
    public Image ComboBase;
    public Image ComboGuageBG;
    public Image ComboCoolDownImage;
    public GameObject ComboGrid;

    [Space]

    // 데이터베이스
    public bool BattleStart;    // 배틀 시작시 true
    public int SelectEnemyNum;  // 선택된 몬스터의 숫자
    public int MaxHp;         // 몬스터 최대체력
    public int CurrentHp;     // 몬스터 현제체력
    public float GameTime;      // 게임 남은시간
    public int CurrentEnemyCount;   // 적 공격카운트
    public bool BattleEvent; // 몬스터 공격이 끝나면 true
    public List<GameObject> PlayerAttackEffectList; //플레이어가 마지막에 공격하고 날라가는 큐브 이펙트
    public List<GameObject> EnemyAttackEffectList; //몬스터가 공격한 이팩트 리스트
    public int ComboValue = 1;
    public float ComboStack = 10f;
    float CurrentComboCoolDown = 1;
    public float MaxComboCoolDown = 1.5f;
    [HideInInspector] public SkillUI CurrentSkillUI;  //현재 사용중인 혹은 사용 하려는 스킬의 UI대상
    [HideInInspector] public float AttackEffectEventTime;
    [HideInInspector] public bool SkillEventOnOff = false; //스킬을 하고있는지 아닌지 체크

    //쓰래기통
    List<int> ColorNumList = new List<int>();
    float DamageTime;
    Color DamageColor = new Color(1, 1, 1);
    bool AttackInit;
    int SkillNum = 0;   //선택된 enemyskill 의 인덱스 넘버
    int damage = 0;     //선택된 몬스터의 대미지 계산 결과값
    Vector2 StartVec;    // 파티클 시작지점 적용 나중에 추가할것
    GameObject TargetVec = null;
    bool DamageEvent;    // 몬스터 데미지 받으면 실행하는 이밴트
    int CurrentAttackCount; //현재 남은 공격횟수
    float MaxSkillCoolDown; // 최대 공격횟수
    float CurrentSkillCoolDown; // 스킬 쿨다운
    bool AttackEndEvent; // 마지막 공격때 적용
    float Player1CacHp; // 소녀체력계산
    float Player2CacHp; // 소녀체력계산
    List<int> ComboNumList = new List<int>(); //콤보 숫자 리스트

    bool[] ComboEvent = new bool[3];        //콤보 이밴트
    
    float ComboEventTime;   //콤보 이밴트 시간
    float CurrentNumSize;   //현재 콤보 이미지 사이즈
    float MaxNumSize;       //마지막 이미지 사이즈
    float OverNumSize;      //연출용 가장 큰 이미지 사이즈
    Color ComboColor = new Color(1, 1, 1, 1);

    private ObjectManager theObject;
    private PuzzleManager thePuzzle;
    private FadeManager theFade;
    private CameraManager theCamera;
    private void Start()
    {
        CurrentSkillUI = SkillUI.UI2_Null;
        theCamera = FindObjectOfType<CameraManager>();
        theFade = FindObjectOfType<FadeManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theObject = FindObjectOfType<ObjectManager>();


        ColorNumList.Add(0);
        ColorNumList.Add(1);
        ColorNumList.Add(2);
        ColorNumList.Add(3);
        ColorNumList.Add(4);
    }

    private void Update()
    {
        if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle)
        {

            //콤보 이밴트
            if (ComboEvent[0] == true)
            {
                if (ComboEvent[1] == false)
                {
                    if (CurrentNumSize < OverNumSize)
                    {
                        CurrentNumSize += Time.deltaTime * 5;
                    }
                    else
                    {
                        ComboEvent[1] = true;
                    }
                }
                else if (ComboEvent[2] == false)
                {
                    if (CurrentNumSize > MaxNumSize)
                    {
                        CurrentNumSize -= Time.deltaTime * 3;
                    }
                    else
                    {
                        CurrentNumSize = MaxNumSize;
                        ComboEvent[0] = false;
                        ComboEvent[1] = false;
                    }
                }
                ComboGrid.transform.localScale = new Vector3(CurrentNumSize, CurrentNumSize, 1);
            }
            if (ComboEvent[2] == true)
            {
                if (ComboEventTime > 0)
                {
                    ComboEventTime -= Time.deltaTime;
                    
                }
                else
                {
                    ComboEventTime = 0;
                    ComboEvent[0] = false;
                    ComboEvent[1] = false;
                    ComboEvent[2] = false;
                    ComboBase.gameObject.SetActive(false);
                    ComboNumImages[0].gameObject.SetActive(false);
                    ComboNumImages[1].gameObject.SetActive(false);
                    ComboNumImages[2].gameObject.SetActive(false);
                }
                ComboColor.a = ComboEventTime;
                ComboGuageBG.color = ComboColor;
                ComboBase.color = ComboColor;
                for (int i = 0; i < 3; i++)
                {
                    ComboNumImages[i].color = ComboColor;
                }
            }


            //몬스터가 데미지를 입으면 색을 빨간색으로 해주는 이밴트
            if (DamageEvent == true)
            {
                if (DamageTime <= 1)
                {
                    DamageTime += Time.deltaTime * 2;
                    DamageColor.g = DamageTime;
                    DamageColor.b = DamageTime;
                    EnemyAnim.skeleton.SetColor(DamageColor);
                }
                else
                {
                    DamageEvent = false;
                    DamageTime = 0f;
                    EnemyAnim.skeleton.SetColor(new Color(1, 1, 1));
                }
            }

            if (thePuzzle.state == PuzzleManager.State.BattleEvent)
            {
                //처음 배틀 시작할 때 세팅을 해준다
                if (battleState == BattleState.BattleInit)
                {
                    if (theFade.FadeInEnd == true)
                    {
                        thePuzzle.CheckEnemyCubeCount();
                    }
                    
                        
                }
                // 몬스터가 공격할 때 실행한다
                else if (battleState == BattleState.EnemyAttack)
                {
                    if (PlayerAttackEffectList.Count > 0 || DamageEvent == true)
                        return;

                    if (BattleEvent == true && EnemyAttackEffectList.Count ==0)
                    {
                        EnemyAttackEnd();
                        return;
                    }

                    EnemyAttackEvent();
                    if (AttackEffectEventTime > 0)
                    {
                        AttackEffectEventTime -= Time.deltaTime;
                    }


                    //몬스터 공격 이밴트
                }
                // 몬스터가 죽었을 때 실행한다
                else if (battleState == BattleState.EnemyDie)
                {
                    if (theFade.FadeOutEnd == true)
                    {
                        Debug.Log(thePuzzle.CubeEvent);
                        theFade.FadeOutEnd = false;
                        theFade.CloseBlackChat();
                        thePuzzle.ChangeGameMode();
                        thePuzzle.CheckEnemyData();
                        thePuzzle.CubeEvent = false;
                        theFade.FadeInEvent(false);
                    }
                }


            }
        }


    }


    public void SetBattle(int _enemyNum)
    {
        //ComboCoolDownImage.fillAmount = 0;
        ComboBase.gameObject.SetActive(false);
        ComboValue = 1;
        SelectEnemyNum = _enemyNum;
        EnemyAnim.skeletonDataAsset = Enemy[_enemyNum].IllustData;
        EnemyAnim.Initialize(true);
        EnemyAnim.state.Event += HandleEvent;
        CurrentEnemyCount = 0;
        SetEnemyCount(Enemy[_enemyNum].Count);
        EnemyCountText.text = Enemy[_enemyNum].Count.ToString();
        MaxHp = 0;
        GameTime = 30;
        List<int> ColorNum = new List<int>(ColorNumList);


        for (int i = 0; i < EnemyCubeUi.Length; i++)
        {
            if (i < Enemy[_enemyNum].CubeCount.Length)
            {
                EnemyCubeUi[i].CubeSprite.gameObject.SetActive(true);
  
                int RandColor = Random.Range(0, ColorNum.Count);
                EnemyCubeUi[i].SetCubeUi(ColorNum[RandColor], i,
                    EnemyUiSprites[ColorNum[RandColor]], Enemy[_enemyNum].CubeCount[i]);

                MaxHp += Enemy[_enemyNum].CubeCount[i];
                ColorNum.Remove(RandColor);
            }
            else
            {
                EnemyCubeUi[i].cubeColor = NodeColor.NC8_Null;
                EnemyCubeUi[i].CubeSprite.gameObject.SetActive(false);
            }


        }

        CurrentHp = MaxHp;
    }

    public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "Hit_End")
        {

            EnemyAnim.AnimationState.SetAnimation(0, "Idle", true);
        }

        if (e.Data.Name == "Attack_End")
        {

            EnemyAnim.AnimationState.SetAnimation(0, "Idle", true);
        }

    }



    public void EndBattle()
    {
        battleState = BattleState.EnemyDie;
        thePuzzle.state = PuzzleManager.State.BattleEvent;
        Debug.Log("배틀 승리");
        BattleStart = false;
        ResetCombo();
        //ComboCoolDownImage.fillAmount = 0;
        EnemyAnim.AnimationState.SetAnimation(0, "Die", true);
        theFade.ShowBlackChat("전투 승리","");
        //theFade.FadeIn();
    }



    public void TakeDamage(int DamageCount)
    {
        
        if (DamageCount > 0)
        {
            Debug.Log("데미지가 플러스로 나옴");
            return;
        }

        if (DamageTime > 0)
        {
            
            EnemyAnim.AnimationState.SetAnimation(0, "Hit", false);
        }
        if (CurrentHp <= 0)
        {
            EnemyAnim.AnimationState.SetAnimation(0, "Die", true);
        }



        DamageEvent = true;
        DamageTime = 0f;
        DamageColor.r = 1f;
        DamageColor.g = 0f;
        DamageColor.b = 0f;
        EnemyAnim.skeleton.SetColor(DamageColor);
        
        CurrentHp += DamageCount;

        if (CurrentHp <= 0)
        {
            EndBattle();
        }

    }



    // 공격 이밴트
    // 공격 시작시 해당 유닛의 스킬을 확률값에 맞게 정한다
    // 시작시 초기화 단계후 쿨다운에 맞춰서 스킬을 발사한다
    // 마지막 스킬에는 스킬 앤드 이밴트를 받아 스킬이 끝났다는걸 알려준다
    public void EnemyAttackEvent()
    {
        if (AttackInit == false)
        {
            ResetCombo();
            EnemyAnim.AnimationState.SetAnimation(0, "Attack", false);

            float rand = Random.Range(0.0f, 100.0f);
            SkillNum = 0;
            Player1CacHp = thePuzzle.playerUIs[0].CurrentHp;
            Player2CacHp = thePuzzle.playerUIs[1].CurrentHp;

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

            AttackInit = true;
            CurrentAttackCount = Enemy[SelectEnemyNum].skillSlots[SkillNum].SkillCount;
            if (CurrentAttackCount == 0)
                CurrentAttackCount = 1;
            MaxSkillCoolDown = Enemy[SelectEnemyNum].skillSlots[SkillNum].SkillCoolDown;
            CurrentSkillCoolDown = 0;
            AttackEndEvent = false;

            if (EnemySkill[SkillNum].attackEffectType == AttackEffectType.ET3_)
            {
                AttackEffectEventTime = 1.5f;
            }


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
                //대미지 계산
                damage = (int)(Random.Range(Enemy[SelectEnemyNum].MinDamageValue,
                Enemy[SelectEnemyNum].MaxDamageValue + 1) *
                EnemySkill[SkillNum].MultiplyValue);

                CurrentAttackCount--;
                if (CurrentAttackCount == 0)
                    AttackEndEvent = true;
                // 쿨타임 적용
                CurrentSkillCoolDown = MaxSkillCoolDown;
                // 파티클 시작지점 적용 나중에 추가할것
                StartVec = EnemyAnim.gameObject.transform.position;


                // 스킬 타입분류
                // 랜덤으로 1명
                if (EnemySkill[SkillNum].attackType == AttackType.Random1)
                {

                    if (Player1CacHp <= 0)
                    {
                        TargetVec = thePuzzle.playerUIs[1].Trigger.gameObject;
                    }
                    else if (Player2CacHp <= 0)
                    {
                        TargetVec = thePuzzle.playerUIs[0].Trigger.gameObject;
                    }
                    else
                    {
                        int randUI = Random.Range(0, 2);
                        TargetVec = thePuzzle.playerUIs[randUI].Trigger.gameObject;
                    }


                }
                // 채력이 가장 낮은 플레이어
                else if (EnemySkill[SkillNum].attackType == AttackType.LowHpAttack)
                {
                    if (Player1CacHp <= 0)
                    {
                        TargetVec = thePuzzle.playerUIs[1].Trigger.gameObject;
                    }
                    else if (Player2CacHp <= 0)
                    {
                        TargetVec = thePuzzle.playerUIs[0].Trigger.gameObject;
                    }
                    else
                    {
                        if (Player1CacHp == Player2CacHp)
                        {
                            int randUI = Random.Range(0, 2);
                            TargetVec = thePuzzle.playerUIs[randUI].Trigger.gameObject;
                        }
                        else
                        {
                            if (Player1CacHp <= Player2CacHp)
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
                // 전체공격
                else if (EnemySkill[SkillNum].attackType == AttackType.FullAttack)
                {
                    TargetVec = thePuzzle.playerUIs[0].Trigger.gameObject;
                    EnemyAttackEffectList.Add(theObject.AttackEffectEvent(StartVec,
                  TargetVec, damage, EnemySkill[SkillNum].SkillEffectNum, false, EnemySkill[SkillNum].attackEffectType)
                        );
                    Player1CacHp -= damage;


                    TargetVec = thePuzzle.playerUIs[1].Trigger.gameObject;
                    EnemyAttackEffectList.Add(
                    theObject.AttackEffectEvent(StartVec,
                  TargetVec, damage, EnemySkill[SkillNum].SkillEffectNum, AttackEndEvent, EnemySkill[SkillNum].attackEffectType)
                    );
                    Player2CacHp -= damage;
                    return;
                }


                if (TargetVec == thePuzzle.playerUIs[0].Trigger.gameObject)
                {
                    Player1CacHp -= damage;
                    if (Player1CacHp < 0)
                        Player1CacHp = 0;
                }

                else if (TargetVec == thePuzzle.playerUIs[1].Trigger.gameObject)
                {
                    Player2CacHp -= damage;
                    if (Player2CacHp < 0)
                        Player2CacHp = 0;
                }

                EnemyAttackEffectList.Add(
                theObject.AttackEffectEvent(StartVec,
                    TargetVec, damage, EnemySkill[SkillNum].SkillEffectNum, AttackEndEvent, EnemySkill[SkillNum].attackEffectType)
                );
            }

          

        }



    }

    // 몬스터의 공격이 끝났을 때
    public void EnemyAttackEnd()
    {
        // 게임 게속 진행
        if (CheckPlayerRetire() == false)
        {
            battleState = BattleState.Null;
            thePuzzle.state = PuzzleManager.State.Ready;
            BattleEvent = false;
            SetEnemyCount(Enemy[SelectEnemyNum].Count);
            AttackInit = false;
        }
        else // 플레이어 2명 모두 죽음
        {
            battleState = BattleState.PlayerDie;
            Debug.Log("전멸");
            BattleStart = false;
            theFade.FadeOutEvent();

        }


    }




    public void EnemyPEvent(Vector2 vec)
    {
        switch (EnemySkill[SkillNum].SkillEffectNum)
        {
            case 0:
               theObject.SlimePEvent(vec);
               break;
        }
    }


    // 플레이어가 모두 죽었는지 체크한다. true면 전멸, false면 계속 진행
    public bool CheckPlayerRetire()
    {
        bool Retire = false;
        if (thePuzzle.playerUIs[0].CurrentHp <= 0 &&
            thePuzzle.playerUIs[1].CurrentHp <= 0)
        {
            Retire = true;
        }
        return Retire;
    }



    public void EnemyDieEvent()
    { 
    
    }



    public void AddComboValue()
    {
        CurrentComboCoolDown = MaxComboCoolDown;
        ComboCoolDownImage.fillAmount = 1;
        ComboValue++;
        if (ComboValue > 1)
        {
            ComboEvent[0] = true;
            ComboEvent[1] = false;
            ComboEvent[2] = false;
            ComboStack += ComboValue;
            ComboNumList.Clear();
            int ComboNum = ComboValue - 1;
            if (ComboNum == 1)
            {
                ComboBase.gameObject.SetActive(true);
                ComboGuageBG.color = new Color(1, 1, 1, 1);
                ComboBase.color = new Color(1, 1, 1, 1);
                for (int i = 0; i < 3; i++)
                {
                    ComboNumImages[i].color = new Color(1,1,1,1);
                }
            }
            
            MaxNumSize = 0.7f + (ComboNum * 0.08f);
            if (MaxNumSize > 1.5f)
            {
                MaxNumSize = 1.5f;
            }
            CurrentNumSize = MaxNumSize - 0.2f;
            OverNumSize = MaxNumSize * 1.5f;

            if (ComboNum > 999)
                ComboNum = 999;
            while (true)
            {


                ComboNumList.Add(ComboNum % 10);

                if (ComboNum >= 10)
                {
                    ComboNum /= 10;

                }
                else
                {
                    break;
                }
                
            }

            for (int i = 0; i <3; i++)
            {

                if (ComboNumList.Count > 0)
                {

                    if (ComboNumImages[i].gameObject.activeSelf == false)
                        ComboNumImages[i].gameObject.SetActive(true);
                    ComboNumImages[i].sprite = ComboSprites[ComboNumList[ComboNumList.Count-1]];
                    ComboNumList.RemoveAt(ComboNumList.Count - 1);
                }
                else
                {
                    if (ComboNumImages[i].gameObject.activeSelf == true)
                        ComboNumImages[i].gameObject.SetActive(false);
                }
                
            }
        }

    }
    public void ResetCombo()
    {
        ComboValue = 1;
        ComboStack = 10;
        CurrentComboCoolDown = 0;
        ComboEventTime = 1f;
        ComboEvent[0] = false;
        ComboEvent[1] = false;
        ComboEvent[2] = true;
    }
    public void CheckComboCoolDonw()
    {
        if (ComboValue > 1)
        {
            if (CurrentComboCoolDown > 0)
            {
                CurrentComboCoolDown -= Time.deltaTime;
                ComboCoolDownImage.fillAmount = CurrentComboCoolDown / MaxComboCoolDown;
            }
            else
            {
                ResetCombo();

            }
        }
       
    }



    public void ReadySkill(SkillUI _Num)
    {
        if (CurrentSkillUI != _Num)
        {
            CurrentSkillUI = _Num;
            CheckSkillUI((int)CurrentSkillUI);
        }
        //이미 선택한 플레이어를 눌러서 취소함
        else if (_Num == SkillUI.UI2_Null)
        {
            CurrentSkillUI = SkillUI.UI2_Null;
            CheckSkillUI(2);
        }
        else
        {
            CurrentSkillUI = SkillUI.UI2_Null;
            CheckSkillUI(2);
        }


    }

    public void CheckSkillUI(int _Num)
    {
        for (int i = 0; i < 2; i++)
        {
            if (i == _Num)
            {
                thePuzzle.playerUIs[i].SetSkill(true);
            } 
            else
            {
                thePuzzle.playerUIs[i].SetSkill(false);
            }
        }
    }
    






    // 몬스터의 카운터를 세팅한다
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
        ComboValue = 1;
        ComboStack = 10;
        MaxHp = 0;
        CurrentHp = 0;
        CurrentEnemyCount = 0;
        GameTime = 0;
        CurrentEnemyCount = 0;
        thePuzzle.Player.CurrentEnemyMeetChance = 0;
        BattleStart = false;
        DamageEvent = false;
    }
}
