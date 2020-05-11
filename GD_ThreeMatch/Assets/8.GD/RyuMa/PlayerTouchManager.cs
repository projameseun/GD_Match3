using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerTouchManager : MonoBehaviour, IPointerDownHandler
{

    private PlayerUI playerUI;



    bool TouchDown;



    private PuzzleManager thePuzzle;
    private BattleManager theBattle;
    private void Start()
    {
        playerUI = GetComponentInParent<PlayerUI>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theBattle = FindObjectOfType<BattleManager>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle &&
            theBattle.battleState != BattleState.BattleInit)
        {
            TouchDown = !TouchDown;
            Debug.Log("클릭함");
        }
       
    }
}
