using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ChangeMode
{
    Null = 0,
    Normal,
    Player,
    Enemy,
    Goal,
    Object
}



public class PuzzleMaker : MonoBehaviour
{

    public MapManager theMoveMap;
    public PlayerCube Player;
    public GameObject Goal;
    public int EnemyNum;
    [Space]
    public ChangeMode changeMode;
    public int SlotNum;
    public bool ButtonDown;
    [Space]
    //DB





    private PuzzleManager thePuzzle;
    private ObjectManager theObject;
    private void Start()
    {
        theObject = FindObjectOfType<ObjectManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }


    public void Update()
    {


        if (ButtonDown == true)
        {
            ButtonDown = false;
            BT_PuzzleMaker();
            SlotNum = 0;
        }
    }







    public void BT_PuzzleMaker()
    {
        if (changeMode == ChangeMode.Null)
        {
            theMoveMap.Slots[SlotNum].nodeType = PuzzleSlot.NodeType.Null;
            theMoveMap.Slots[SlotNum].nodeColor = NodeColor.Null;
            theMoveMap.Slots[SlotNum].TestText.color = new Color(1, 1, 1);
        }
        else if (changeMode == ChangeMode.Normal)
        {
            theMoveMap.Slots[SlotNum].nodeType = PuzzleSlot.NodeType.Normal;
            theMoveMap.Slots[SlotNum].nodeColor = NodeColor.Null;
            theMoveMap.Slots[SlotNum].TestText.color = new Color(0,0,0);
        }
        else if (changeMode == ChangeMode.Player)
        {
            theMoveMap.Slots[SlotNum].nodeType = PuzzleSlot.NodeType.Normal;
            theMoveMap.Slots[SlotNum].nodeColor = NodeColor.Player;
            theMoveMap.Slots[SlotNum].TestText.text = "P";
            theMoveMap.Slots[SlotNum].TestText.color = new Color(0, 0, 1);
        }
        else if (changeMode == ChangeMode.Enemy)
        {
            theMoveMap.Slots[SlotNum].nodeType = PuzzleSlot.NodeType.Enemy;
            theMoveMap.Slots[SlotNum].nodeColor = NodeColor.Null;
            theMoveMap.Slots[SlotNum].TestText.text = "E";
            theMoveMap.Slots[SlotNum].TestText.color = new Color(1, 0, 0);

        }
        else if (changeMode == ChangeMode.Goal)
        {
            theMoveMap.Slots[SlotNum].nodeType = PuzzleSlot.NodeType.Goal;
            theMoveMap.Slots[SlotNum].nodeColor = NodeColor.Null;
            theMoveMap.Slots[SlotNum].TestText.text = "G";
            theMoveMap.Slots[SlotNum].TestText.color = new Color(1, 0.8f, 0);
            Goal.transform.position = theMoveMap.Slots[SlotNum].transform.position;
            Transform Parent = theMoveMap.Slots[SlotNum].transform;
            Goal.transform.parent = Parent;
        }
        else if (changeMode == ChangeMode.Object)
        {

        }

        Debug.Log(changeMode + " " + SlotNum);






    }
    public void ShowSlotNum()
    {
        if (theMoveMap.Slots[0].TestText.text == "")
        {
            for (int i = 0; i < theMoveMap.Slots.Length; i++)
            {
                theMoveMap.Slots[i].TestText.text = i.ToString();
            }
        }
    }



    public void BT_SettingMap()
    {
        for (int i = 0; i < theMoveMap.Slots.Length; i++)
        {
            if (i <= theMoveMap.TopRight ||
                i >= theMoveMap.BottomLeft ||
                i % theMoveMap.Horizontal <= 0 ||
                i % theMoveMap.Horizontal >= theMoveMap.Horizontal - 1)
            {
                theMoveMap.Slots[i].nodeType = PuzzleSlot.NodeType.Null;
                theMoveMap.Slots[i].nodeColor = NodeColor.Null;

            }

            theMoveMap.Slots[i].SlotNum = i;
        }


        for (int i = 0; i < theMoveMap.Slots.Length; i++)
        {
            if (theMoveMap.Slots[i].nodeColor == NodeColor.Player)
            {
                Debug.Log(i + " 플레이어");

                GameObject Cube = theObject.FindObj("Cube");

                Cube.GetComponent<Cube>().nodeColor = NodeColor.Player;
                Cube.GetComponent<SpriteRenderer>().sprite = null;
                Cube.transform.position = theMoveMap.Slots[i].transform.position;
                theMoveMap.Slots[i].cube = Cube.GetComponent<Cube>();
                Transform Parent = theMoveMap.Slots[i].cube.transform;
                Player.transform.position = theMoveMap.Slots[i].transform.position;
                Player.transform.parent = Parent;
                Player.ChangeDirection(theMoveMap.direction);


                break;
            }
        }






        for (int i = 0; i < theMoveMap.Slots.Length; i++)
        {
            if (theMoveMap.Slots[i].nodeType != PuzzleSlot.NodeType.Null)
            {
                //변경하지 않을 큐브를 넣는다
                if (theMoveMap.Slots[i].nodeType != PuzzleSlot.NodeType.Enemy &&
                    theMoveMap.Slots[i].nodeType != PuzzleSlot.NodeType.Goal &&
                    theMoveMap.Slots[i].nodeColor != NodeColor.Player &&
                    theMoveMap.Slots[i].nodeType != PuzzleSlot.NodeType.Object)
                {
                    theMoveMap.Slots[i].nodeType = PuzzleSlot.NodeType.Normal;
                    theMoveMap.Slots[i].nodeColor = NodeColor.Null;
                    if (theMoveMap.Slots[i].cube != null)
                    {
                        theMoveMap.Slots[i].cube.Resetting();
                        theMoveMap.Slots[i].cube = null;
                    }
                }
                theMoveMap.Slots[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                theMoveMap.Slots[i].TestText.text = i.ToString();
            }
            else
            {
                theMoveMap.Slots[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                theMoveMap.Slots[i].TestText.text = i.ToString();
                theMoveMap.Slots[i].TestText.color = new Color(1, 1, 1);
            }
        }

        thePuzzle.NotMatchSetCube(theMoveMap);



    }


}
