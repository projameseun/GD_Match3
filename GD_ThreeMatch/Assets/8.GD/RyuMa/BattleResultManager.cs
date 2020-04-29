using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultManager : MonoBehaviour
{
    //UI, 오브젝트
    public GameObject ResultBase;
    public Text CubeCountText;
    public Text UseItemText;
    public Text UseSkillText;
    public Text CurrentPointText;
    public Text AllPointText;





    //DB
    public int DestroyCubeCount;
    public int UseItemCount;
    public int UseSkillCount;
    



    private PuzzleManager thePuzzle;
    private FadeManager theFade;
    // Start is called before the first frame update
    void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }


    public void SetTextUI()
    {
        CubeCountText.text = string.Format("파괴한 블록 개수(" + DestroyCubeCount + ") x 1.0 = " + DestroyCubeCount);
        UseItemText.text = string.Format("사용한 아이템(" + UseItemCount + ") x 30.0 = " + UseItemCount * 30);
        UseSkillText.text = string.Format("사용한 스킬(" + UseSkillCount + ") x 50.0 = " + UseSkillCount * 50);
        CurrentPointText.text = string.Format("기존 보유 점수 = " + thePuzzle.CurrentPoint);
        AllPointText.text = string.Format(
            (DestroyCubeCount + (UseItemCount * 30) + (UseSkillCount * 50) + thePuzzle.CurrentPoint).ToString());
    }


    public void BT_Next()
    {
        ResultBase.SetActive(false);
        Resetting();
    }



    public void Resetting()
    {
        DestroyCubeCount = 0;
        UseItemCount = 0;
        UseSkillCount = 0;



    }


}
