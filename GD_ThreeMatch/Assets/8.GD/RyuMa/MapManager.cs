using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MapType
{
    M1_MoveMap = 0,
    M2_BattleMap,
    M3_Editer
}



public class MapManager : MonoBehaviour
{
    public MapType mapType;


    public GameObject SlotPrefab;
    public Vector2 CameraPos;

    public GameObject SlotBase;
    public PuzzleSlot[] Slots;

    public Direction direction;
    public bool FirstBattle = false;

    public Vector2 CellSize = new Vector2(0.6f,0.6f);



    public int Horizon;
    public int Vertical;


    public int TopLeft;
    public int TopRight;
    public int BottomLeft;
    public int BottomRight;


    private void Start()
    {

        int Size = MatchBase.MaxHorizon * MatchBase.MaxVertical;
        Slots = new PuzzleSlot[Size];

        for (int i = 0; i < Size; i++)
        {
            GameObject SlotObj = Instantiate(SlotPrefab);
            SlotObj.transform.position = 
                new Vector2(this.transform.position.x + 0.3f + CellSize.x * (i % MatchBase.MaxHorizon),
               this.transform.position.y - 0.3f + -CellSize.y * (int)(i / MatchBase.MaxHorizon));
            SlotObj.transform.SetParent(SlotBase.transform);
            //SlotObj.transform.parent = SlotBase.transform;
            SlotObj.gameObject.name = string.Format("Slot" + i);
           //SlotObj.gameObject.SetActive(false);
            Slots[i] = SlotObj.GetComponent<PuzzleSlot>();
            Slots[i].SetSlot(i);
        }
    }


    


}
