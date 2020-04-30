using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;


public enum AttackType
{ 
    R1 =0,      // 랜덤으로 1명
    LR1,        // 가장 체력이 낮은 1명
    R2          // 2명 모두
}





[System.Serializable]
public class EnemyBase
{
    public string EnemyName;
    public Sprite MonsterSprite;
    public float Damage;
    public int Count;
    public int[] CubeCount;

}


[System.Serializable]
public class EnemySkill
{
    public string SkillName;
    public float SkillDamage = 1;
    public AttackType attackType;

}





public class BattleManager : MonoBehaviour
{




    public EnemyBase[] Enemy;
    public EnemySkill[] EnemySkill;
    [Space]
    // UI,오브젝트
    public Image EnemyImage;
    public Text EnemyName;
    public Image EnemyHpImage;
    public Text TimeText;
    public CubeUI[] EnemyCubeUi;
    [Space]

    // 데이터베이스
    public bool BattleStart;    // 배틀 시작시 true
    public float MaxHp;         // 몬스터 최대체력
    public float CurrentHp;     // 몬스터 현제체력
    public int SelectEnemyNum;  // 선택된 몬스터의 숫자
    public float GameTime;      // 게임 남은시간
    public int GameTurnCount;   // 적 공격카운트
    public bool DamageEvent;    // 몬스터 데미지 받으면 실행하는 이밴트
    public float DamageTime;
    Color DamageColor = new Color(1, 1, 1);

    //쓰래기통
    List<int> ColorNumList = new List<int>();



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
        }


    }


    public void SetBattle(int _enemyNum)
    {
        SelectEnemyNum = _enemyNum;
        EnemyImage.sprite = Enemy[_enemyNum].MonsterSprite;
        EnemyName.text = Enemy[_enemyNum].EnemyName;
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






    public void MonAttack(int _Value)
    { 
        
    }





}
