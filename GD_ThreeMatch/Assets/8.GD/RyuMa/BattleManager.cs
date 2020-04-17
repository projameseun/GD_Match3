using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class EnemyBase
{
    public string Name;
    public Sprite MonsterSprite;
    public int Hp;
    public int Damage;
    public int Count;

}



public class BattleManager : MonoBehaviour
{
    public EnemyBase[] Enemy;

    // UI,오브젝트
    public Image EnemyImage;
    public Text EnemyName;



    // 데이터베이스

    public int MaxHp;
    public int CurrentHp;

    public float GameTime;
    public int GameTurnCount;





    public void SetBattle(int _enemyNum)
    {
        EnemyImage.sprite = Enemy[_enemyNum].MonsterSprite;
        EnemyName.text = Enemy[_enemyNum].Name;
        MaxHp = Enemy[_enemyNum].Hp;
        CurrentHp = MaxHp;
        GameTime = 30;
    }







}
