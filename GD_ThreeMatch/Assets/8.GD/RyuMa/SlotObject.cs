﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SlotMapType
{ 
    NULL = 0,
    S_0_Forest,
}

public enum SlotObjectSheet
{ 
    NULL = -1,
    S_0_Object,
    S_1_Object,
    S_2_Object,
    S_3_Object,
    S_4_Object,
    S_5_Object,
    S_6_Object,
    ST_0_SlotPanel = 1000,
    ST_1_Enemy,
    ST_2_Portal,

}


public class SlotObject : MonoBehaviour
{
    public MapType mapType;

    public GameObject EnemySkull;
    public SpriteRenderer SpriteRen;

    int SlotNum;


    private ObjectManager theObject;
    private PuzzleMaker theMaker;
    private PuzzleManager thePuzzle;
    private GameManager theGM;
    private void Start()
    {
        if (theObject == null)
            theObject = FindObjectOfType<ObjectManager>();
        if (theMaker == null)
            theMaker = FindObjectOfType<PuzzleMaker>();
        if (thePuzzle == null)
            thePuzzle = FindObjectOfType<PuzzleManager>();
        if (theGM == null)
            theGM = FindObjectOfType<GameManager>();
        
    }


    public void SetSlotObject(SlotObjectSheet _Sheet, MapType _mapType, int _SlotNum)
    {

        mapType = _mapType;
        SlotNum = _SlotNum;
        if (theObject == null)
            theObject = FindObjectOfType<ObjectManager>();
        if (theMaker == null)
            theMaker = FindObjectOfType<PuzzleMaker>();
        if (thePuzzle == null)
            thePuzzle = FindObjectOfType<PuzzleManager>();
        if (theGM == null)
            theGM = FindObjectOfType<GameManager>();
        if (_Sheet == SlotObjectSheet.NULL)
        {
            SpriteRen.sprite = null;
        }
        else if (_Sheet == SlotObjectSheet.ST_1_Enemy)
        {
            if (thePuzzle.theMoveMap.Slots[SlotNum].monsterSheet.OnlyOneEnemy == true)
            {
                if (theGM.EnemyDataSheet[thePuzzle.theMoveMap.Slots[SlotNum].monsterSheet.OnlyOneNum] == true)
                {
                    thePuzzle.theMoveMap.Slots[SlotNum].nodeType = PuzzleSlot.NodeType.Normal;
                    SpriteRen.sprite = theObject.SlotPanelSprite[_SlotNum % 2];
                    return;
                }
            }
            EnemySkull = theObject.SpawnEnemySkull(new Vector2(this.transform.position.x + 0.17f,this.transform.position.y +0.07f));
            SpriteRen.sprite = theObject.EnemySlotSprite;
            thePuzzle.theMoveMap.Slots[_SlotNum].slotObject = this;

        }
        else if (_Sheet == SlotObjectSheet.ST_2_Portal)
        {
            theObject.SpawnPortal(this.transform.position);
        }
        else if (_Sheet == SlotObjectSheet.ST_0_SlotPanel)
        {

            SpriteRen.sprite = theObject.SlotPanelSprite[_SlotNum % 2];
        }
        else
        {

           
            switch (theMaker.mapMainType)
            {
                case MapMainType.M0_Forest:
                    if((int)_Sheet < theObject.ForestSprites.Length)
                        SpriteRen.sprite = theObject.ForestSprites[(int)_Sheet];
                    break;
            }
        }
        if ((int)_Sheet < 1000)
        {
            SpriteRen.sortingOrder = 11;
        }
        else
        {
            SpriteRen.sortingOrder = 1;
        }
    }

    public void ResetEnemy()
    {
        EnemySkull.SetActive(false);
        EnemySkull = null;
        SpriteRen.sprite = theObject.SlotPanelSprite[SlotNum % 2];
    }



    public void Resetting()
    {
        this.gameObject.SetActive(false);
        theObject.SlotPanels.Enqueue(this.gameObject);
    }



}
