using JetBrains.Annotations;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public enum MapType
{ 
    M1_MoveMap = 0,
    M2_BattleMap,
    M3_Null
}



public class MapManager : MonoBehaviour
{
    //static public MapManager instance;//싱글톤

   

    public MapType mapType;


    public GameObject SlotPrefab;
    public Vector2 CameraPos;

    public GameObject SlotBase;
    public PuzzleSlot[] Slots;

    public Direction direction;

    public int Horizontal;
    public int Vertical;
    public int TopLeft;
    public int TopRight;
    public int BottomLeft;
    public int BottomRight;

 

    private void Start()
    {
        if (mapType == MapType.M1_MoveMap)
        {
            int Size = Horizontal * Vertical;
            Slots = new PuzzleSlot[Size];
            //GameObject SlotObj = Instantiate(SlotPrefab);
            for (int i = 0; i < Size; i++)
            {
                GameObject SlotObj = Instantiate(SlotPrefab);
                SlotObj.transform.parent = SlotBase.transform;
                SlotObj.gameObject.name = string.Format("Slot" + i);
                Slots[i] = SlotObj.GetComponent<PuzzleSlot>();
                Slots[i].SlotNum = i;
            }
        }
    }

   



}
