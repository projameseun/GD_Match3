using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeUI : MonoBehaviour
{
    public enum CubeColor
    {
        Black = 0,
        Blue,
        Orange,
        Pink,
        Red,
        Yellow,
    }

    public Image CubeSprite;
    public Text CubeCountText;

    public CubeColor cubeColor;


    private PuzzleManager thePuzzle;
    private void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.)
    //}






    public void SetCubeUi(int _Num)
    {
        CubeSprite.sprite = thePuzzle.CubeSprites[_Num];
        cubeColor = (CubeColor)_Num;
    }


}
