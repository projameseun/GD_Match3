using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;


[System.Serializable]
public class SkillSlot
{
    public int SkillNum;
    public int SkillCount;
    public float Percentage = 100;  // 스킬 확률

}



public enum AttackType
{ 
    Random1 =0,      // 랜덤으로 1명
    LowHpAttack,        // 가장 체력이 낮은 1명
    FullAttack      // 2명 모두
}





[System.Serializable]
public class EnemyBase
{
    public string EnemyName;
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
    public AttackType attackType;

}



public enum BattleState
{
    Null = 0,
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


    //쓰래기통
    List<int> ColorNumList = new List<int>();
    float DamageTime;
    Color DamageColor = new Color(1, 1, 1);

    private PuzzleManager thePuzzle;
    private FadeManager theFade;
    private void Start()
    {
       
        theFade = FindObjectOfType<FadeManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();

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
                DamageTime += Time.deltaTime*2;
                DamageColor.g = DamageTime;
                DamageColor.b = DamageTime;
                EnemyImage.color = DamageColor;
            }
            else
            {
                DamageEvent = false;
                DamageTime = 0f;
                EnemyImage.color = new Color(1,1,1);
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
                    EnemyAttackEvent();
                    battleState = BattleState.Null;
                    thePuzzle.state = PuzzleManager.State.Ready;
                    //몬스터 공격 이밴트
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
          
        TimeText.text = "Time : "+ ((int)GameTime).ToString();
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

    public float EnemyAttackEvent()
    {
        float rand = Random.Range(0.0f, 100.0f);
        int SkillNum = 0;

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
        float damage = EnemySkill[SkillNum].MultiplyValue * Enemy[SelectEnemyNum].DamageValue;

        if (EnemySkill[SkillNum].attackType == AttackType.Random1)
        {
            thePuzzle.playerUIs[0].TakeDamage((int)damage);
        }
        SetEnemyCount(Enemy[SelectEnemyNum].Count); 
        //TODO: 임시

        return damage;
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
