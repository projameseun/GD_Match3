using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{


    public BoxCollider2D Bound;

    public GameObject SlotBase;
    public PuzzleSlot[] Slots;

    public Direction direction;




    public int Horizontal;
    public int Vertical;
    public int TopLeft;
    public int TopRight;
    public int BottomLeft;
    public int BottomRight;


    // Update is called once per frame
    void Update()
    {
        
    }
}
