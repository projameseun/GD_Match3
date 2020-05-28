using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerTouchManager : MonoBehaviour, IPointerClickHandler
{
    private PlayerUI playerUI;
    private PuzzleManager thePuzzle;
    private BattleManager theBattle;
    private GirlManager theGirl;
    private void Start()
    {
        playerUI = GetComponentInParent<PlayerUI>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theBattle = FindObjectOfType<BattleManager>();
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (thePuzzle.gameMode == PuzzleManager.GameMode.Battle &&
           theBattle.battleState != BattleState.BattleInit &&
           playerUI.GetSkillGauge() >= 1 &&
           playerUI.state != PlayerUIState.Die
           )
        {
            playerUI.CheckSkill();
        }
    }
}
