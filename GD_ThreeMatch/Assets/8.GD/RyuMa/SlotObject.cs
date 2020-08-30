using System.Collections;
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


public class SlotObject : Panel
{
    public MapType mapType;

    public GameObject EnemySkull;
    public SpriteRenderer SpriteRen;
    public SpriteRenderer MiniMap;
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



    public void Resetting()
    {
        this.gameObject.SetActive(false);
        SpriteRen.sortingOrder = 1;
        theObject.SlotPanels.Enqueue(this.gameObject);
    }



}
