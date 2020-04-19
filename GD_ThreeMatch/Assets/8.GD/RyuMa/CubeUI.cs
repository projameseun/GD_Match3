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
            if (uIType == UIType.PlayerUI)
            {
                if ((int)collision.GetComponent<CubeEffect>().nodeColor ==
                    (int)cubeColor)
                {
                    collision.GetComponent<CubeEffect>().UiSet(this);
                }
            }
          
        }
    }






    public void SetCubeUi(int _Num)
    {
        CubeSprite.sprite = thePuzzle.CubeSprites[_Num];
        cubeColor = (NodeColor)_Num;

    }



}
