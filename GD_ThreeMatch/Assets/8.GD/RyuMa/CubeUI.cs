using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeUI : MonoBehaviour
{
    public enum UIType
    { 
        PlayerUI,
        EnemyUI,
    }




    public Image CubeSprite;
    public Text CubeCountText;
    public int UINum;

    public UIType uIType;
    public NodeColor cubeColor;
    public int CubeCount;


    private PuzzleManager thePuzzle;
    private void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CubeEffect")
        {
            if (uIType == UIType.PlayerUI&&
                collision.GetComponent<CubeEffect>().cubeEffectType == CubeEffectType.GoPlayer)
            {
                if ((int)collision.GetComponent<CubeEffect>().nodeColor ==
                    (int)cubeColor)
                {
                    collision.GetComponent<CubeEffect>().UiSet(this,UINum);
                }
            }
            else if (uIType == UIType.EnemyUI &&
              collision.GetComponent<CubeEffect>().cubeEffectType == CubeEffectType.GoEnemy)
            {
                if ((int)collision.GetComponent<CubeEffect>().nodeColor ==
                    (int)cubeColor)
                {
                    collision.GetComponent<CubeEffect>().UiSet(this, UINum);
                }
            }

        }
    }






    public void SetCubeUi(int _ColorNum, int _UiNum,Sprite _sprite,int _CubeCount = 0)
    {
        UINum = _UiNum;
        CubeSprite.sprite = _sprite;
        cubeColor = (NodeColor)_ColorNum;
        CubeCount = _CubeCount;
        CubeCountText.text = _CubeCount.ToString();
    }



}
