using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerTouchManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private PlayerUI playerUI;



    bool TouchDown;



    private PuzzleManager thePuzzle;
    private BattleManager theBattle;
    private GirlManager theGirl;
    private void Start()
    {
        playerUI = GetComponentInParent<PlayerUI>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theBattle = FindObjectOfType<BattleManager>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle &&
            theBattle.battleState != BattleState.BattleInit &&
            TouchDown == false)
        {
            TouchDown = true;
            Debug.Log("클릭함");
        }
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (TouchDown == true)
        {
            TouchDown = false;
        }
    }


    public void GirlSkill(SelectGirl _Girl)
    {
        if (_Girl == SelectGirl.G1a000)
        {

        }
        else if (_Girl == SelectGirl.G2_Alice)
        { 
        
        }
        else if (_Girl == SelectGirl.G3a222)
        {

        }
        else if (_Girl == SelectGirl.G4_Beryl)
        {

        }
        else if (_Girl == SelectGirl.G5a444)
        {

        }
        else if (_Girl == SelectGirl.G6a555)
        {

        }
    }




}
