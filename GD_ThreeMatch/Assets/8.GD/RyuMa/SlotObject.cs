using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotObjectSheet
{ 
    NULL = -1,
    S_0_SlotPanel,
    S_1_Forest,
    S_2_Rock,
}


public class SlotObject : MonoBehaviour
{

    private ObjectManager theObject;
    private void Start()
    {
        theObject = FindObjectOfType<ObjectManager>();
    }


    public void SetSlotObject(Vector2 StartPos, SlotObjectSheet _Sheet)
    {
        if(theObject == null)
            theObject = FindObjectOfType<ObjectManager>();
        this.transform.position = StartPos;
        if (_Sheet == SlotObjectSheet.NULL)
        {
            this.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = theObject.SlotObjectSprites[(int)_Sheet];
        }
        
    }


    public void Resetting()
    {
        this.gameObject.SetActive(false);
    }



}
