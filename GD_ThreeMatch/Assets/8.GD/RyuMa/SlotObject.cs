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
    S_1_Object,
    S_2_Object,
    ST_0_SlotPanel = 1000,
    ST_1_Enemy,
    ST_2_Portal,

}


public class SlotObject : MonoBehaviour
{
    public MapType mapType;


    private ObjectManager theObject;
    private PuzzleMaker theMaker;
    private void Start()
    {
        if (theObject == null)
            theObject = FindObjectOfType<ObjectManager>();
        if (theMaker == null)
            theMaker = FindObjectOfType<PuzzleMaker>();
    }


    public void SetSlotObject(Vector2 StartPos, SlotObjectSheet _Sheet, MapType _mapType, int _SlotNum)
    {

        mapType = _mapType;

        if (theObject == null)
            theObject = FindObjectOfType<ObjectManager>();
        if (theMaker == null)
            theMaker = FindObjectOfType<PuzzleMaker>();

        this.transform.position = StartPos;
        if (_Sheet == SlotObjectSheet.NULL)
        {
            this.GetComponent<SpriteRenderer>().sprite = null;
        }
        else if (_Sheet == SlotObjectSheet.ST_1_Enemy)
        {
            this.GetComponent<SpriteRenderer>().sprite = theObject.EnemySlotSprite;

        }
        else if (_Sheet == SlotObjectSheet.ST_2_Portal)
        {
            //this.GetComponent<SpriteRenderer>().sprite = theObject.PortalSlotSprite;
            theObject.SpawnPortal(this.transform.position);
        }
        else if (_Sheet == SlotObjectSheet.ST_0_SlotPanel)
        {
            
            this.GetComponent<SpriteRenderer>().sprite = theObject.SlotPanelSprite[_SlotNum % 2];
        }
        else
        {
            switch (theMaker.mapMainType)
            {
                case MapMainType.M0_Forest:
                    this.GetComponent<SpriteRenderer>().sprite = theObject.ForestSprites[(int)_Sheet];
                    break;
            }
            
        }
        
    }


    public void Resetting()
    {
        this.gameObject.SetActive(false);
    }



}
