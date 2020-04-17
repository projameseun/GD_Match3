using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnemyBase
{
    public string Name;
    public Sprite MonsterSprite;
    public float Hp;
    public int Damage;
    public int Count;

}



public class BattleManager : MonoBehaviour
{
    public EnemyBase[] Enemy;

    // UI,오브젝트
    public Image EnemyImage;
    public Text EnemyName;
    public Image EnemyHpImage;
    public Text TimeText;


    // 데이터베이스
    public bool BattleStart;    // 배틀 시작시 true
    public float MaxHp;         // 몬스터 최대체력
    public float CurrentHp;     // 몬스터 현제체력

    public float GameTime;      // 게임 남은시간
    public int GameTurnCount;   // 적 공격카운트
    public bool DamageEvent;    // 몬스터 데미지 받으면 실행하는 이밴트
    public float DamageTime;
    Color DamageColor = new Color(1, 1, 1);

    private PuzzleManager thePuzzle;
    private FadeManager theFade;
    private void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
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
        EnemyImage.sprite = Enemy[_enemyNum].MonsterSprite;
        EnemyName.text = Enemy[_enemyNum].Name;
        MaxHp = Enemy[_enemyNum].Hp;
        CurrentHp = MaxHp;
        GameTime = 30;
        BattleStart = true;
    }


    public void UILoad()
    {
        GameTime -= Time.deltaTime;
        EnemyHpImage.fillAmount = CurrentHp / MaxHp;
        if (GameTime < 0)
        {
            BattleStart = false;
            theFade.FadeIn();
        }
        else if (CurrentHp <= 0)
        {
            BattleStart = false;
            theFade.FadeIn();
        }
          
        TimeText.text = "Time : "+ ((int)GameTime).ToString();
    }

    public void TakeDamage()
    {
        DamageEvent = true;
        DamageTime = 0f;
        DamageColor.r = 1f;
        DamageColor.g = 0f;
        DamageColor.b = 0f;
        EnemyImage.color = DamageColor;
        CurrentHp -= 1;

    }






}
