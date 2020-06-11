using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum MapMainType
{ 
    M0_Forest = 0,

}

public enum ChangeMode
{
    Ch0_Null = 0,
    Ch1_Normal,
    Ch2_Enemy,
    Ch3_Portal,
    Ch4_Object
}

[System.Serializable]
public class EnemyIndex
{
    public int EnemyNum;
    public int ChanceMeet;
}

public class PuzzleMaker : MonoBehaviour
{

    public MapManager theMoveMap;
    public PlayerCube Player;
    public GameObject TestStartBt;
    public GameObject IngameUi;
    public GameObject SonMapStartBt;
    public GameObject SaveButton;
    public GameObject LoadButton;
    public GameObject SonMapBase;
    public bool PuzzleMakerStart;

    public MapMainType mapMainType;
    public ChangeMode changeMode;


    [Header("MapSetting")]
    public string MapName;
    public int TopLeft;
    public int TopRight;
    public int BottomLeft;
    public int BottomRight;




    [Header("EnemySetting")]
    public Color EnemyColor;
    public int MonsterImageIndex;
    public int addEnemyMeet;            //몬스터랑 조우할 확률의 증가량
    [Space]
    public EnemyIndex[] enemyIndex;     //몬스터 정보
    [Space]
    public bool OnlyOneEnemy;           //한번만 생성하는 몬스터일 경우 true
    public int DataSheet;               //한번만 생성이라면 사용할 데이터시트의 번호

    [Header("PortalSetting")]

    public string MoveMapName;      //이동할 맵의 이름
    public int PlayerStartPos;      //플레이어가 시작할 슬롯의 넘버

    [Header("ObjectSetting")]
    public SlotObjectSheet objectType;
    public int SkinObjectNum;
    public string SkinName;

    [Header("TestPlaySetting")]
    public int PlayerStartNum;

    private PuzzleManager thePuzzle;
    private ObjectManager theObject;
    private CameraManager theCam;
    private GameManager theGM;
    private void Start()
    {
        theGM = FindObjectOfType<GameManager>();
        theCam = FindObjectOfType<CameraManager>();
        theObject = FindObjectOfType<ObjectManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();

        if (SaveButton != null)
        {
            SaveButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                theGM.SaveBtn();
            });
        }
        if (LoadButton != null)
        {
            LoadButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                theGM.LoadMap();
                SonMapBase.SetActive(false);
                SonMapStartBt.SetActive(false);
            });
        }

        
    }




    public void BT_PuzzleMaker(PuzzleSlot _Slot, int _SlotNum)
    {
        if (_Slot.portalSheet != null)
            _Slot.portalSheet = null;

        if (_Slot.monsterSheet != null)
            _Slot.monsterSheet = null;


        if (changeMode == ChangeMode.Ch0_Null)
        {
            _Slot.nodeType = PuzzleSlot.NodeType.Null;
            _Slot.nodeColor = NodeColor.NC8_Null;
            _Slot.TestText.text = ""; //string.Format("N(" + _SlotNum + ")");
            _Slot.TestText.color = new Color(1, 1, 1);
            _Slot.SlotSheet.SlotSheet = SlotObjectSheet.NULL;
            _Slot.SlotSheet.SkinName = "";
        }
        else if (changeMode == ChangeMode.Ch1_Normal)
        {
            _Slot.nodeType = PuzzleSlot.NodeType.Normal;
            _Slot.nodeColor = NodeColor.NC8_Null;
            _Slot.TestText.text = "C"; //string.Format(_SlotNum.ToString());
            _Slot.TestText.color = new Color(0, 0, 0);
            _Slot.SlotSheet.SlotSheet = SlotObjectSheet.ST_0_SlotPanel;
        }
        //else if (changeMode == ChangeMode.Player)
        //{
        //    _Slot.nodeType = PuzzleSlot.NodeType.Normal;
        //    _Slot.nodeColor = NodeColor.Player;
        //    _Slot.TestText.text = "P";
        //    _Slot.TestText.color = new Color(0, 0, 1);
        //}
        else if (changeMode == ChangeMode.Ch2_Enemy)
        {
            _Slot.nodeType = PuzzleSlot.NodeType.Enemy;
            _Slot.nodeColor = NodeColor.NC8_Null;
            _Slot.TestText.text = "E";
            _Slot.TestText.color = EnemyColor;
            _Slot.SlotSheet.SlotSheet = SlotObjectSheet.ST_1_Enemy;
            _Slot.monsterSheet = new MonsterSheet();

            _Slot.monsterSheet.OnlyOneEnemy = OnlyOneEnemy;
            if (OnlyOneEnemy == true)
                _Slot.monsterSheet.OnlyOneNum = DataSheet;

            _Slot.monsterSheet.SlotImageIndex = MonsterImageIndex;
            _Slot.monsterSheet.addEnemyMeet = addEnemyMeet;
            _Slot.monsterSheet.EnemyIndex = new int[enemyIndex.Length];
            _Slot.monsterSheet.EnemyChance = new int[enemyIndex.Length];
            for (int i = 0; i < enemyIndex.Length; i++)
            {
                _Slot.monsterSheet.EnemyIndex[i] = enemyIndex[i].EnemyNum;
                _Slot.monsterSheet.EnemyChance[i] = enemyIndex[i].ChanceMeet;
            }

        }
        else if (changeMode == ChangeMode.Ch3_Portal)
        {
            _Slot.nodeType = PuzzleSlot.NodeType.Portal;
            _Slot.nodeColor = NodeColor.NC8_Null;
            _Slot.TestText.text = "P";//string.Format("P(" + _SlotNum + ")");
            _Slot.TestText.color = new Color(0, 0, 1f);

            _Slot.portalSheet = new PortalSheet();
            _Slot.portalSheet.MapName = MoveMapName;
            _Slot.portalSheet.NextPosNum = PlayerStartPos;
            _Slot.SlotSheet.SlotSheet = SlotObjectSheet.ST_2_Portal;
        }
        else if (changeMode == ChangeMode.Ch4_Object)
        {
            _Slot.nodeType = PuzzleSlot.NodeType.Null;
            _Slot.nodeColor = NodeColor.NC8_Null;
            _Slot.SlotSheet.SlotSheet = objectType;
            _Slot.SlotSheet.ObjectNum = SkinObjectNum;
            _Slot.SlotSheet.SkinName = SkinName;
            _Slot.TestText.text = ((int)objectType).ToString();

        }

    }

    public void BT_SonMapStart()
    {
        LoadButton.SetActive(false);
        SonMapStartBt.SetActive(false);
        TestStartBt.SetActive(true);
        theCam.MoveVec = theCam.gameObject.transform.position;
        theCam.MoveVec.z = -10;
        theCam.state = CameraManager.State.SonMap;
        PuzzleMakerStart = true;
        IngameUi.SetActive(false);
        ShowSlotNum();
    }

    public void ShowSlotNum()
    {

        theMoveMap.TopLeft = TopLeft;
        theMoveMap.TopRight = TopRight;
        theMoveMap.BottomLeft = BottomLeft;
        theMoveMap.BottomRight = BottomRight;


        for (int i = 0; i < theMoveMap.Slots.Length; i++)
        {
            if (i <= theMoveMap.TopRight ||
                i >= theMoveMap.BottomLeft ||
                i % theMoveMap.Horizontal <= 0 ||
                i % theMoveMap.Horizontal >= theMoveMap.TopRight)
            {
                theMoveMap.Slots[i].nodeType = PuzzleSlot.NodeType.Null;
                theMoveMap.Slots[i].nodeColor = NodeColor.NC8_Null;
                theMoveMap.Slots[i].SlotSheet.SlotSheet = SlotObjectSheet.NULL;
                theMoveMap.Slots[i].TestText.enabled = false; //string.Format("N(" + _SlotNum + ")");

            }
            else
            {
                theMoveMap.Slots[i].TestText.enabled = true;
                theMoveMap.Slots[i].TestText.text = i.ToString();
                theMoveMap.Slots[i].nodeType = PuzzleSlot.NodeType.Normal;
                theMoveMap.Slots[i].SlotSheet.SlotSheet = SlotObjectSheet.ST_0_SlotPanel;
            }

            if ((i <= theMoveMap.TopRight && i >=0) ||
               (i >= theMoveMap.BottomLeft && i <= theMoveMap.BottomRight) ||
               (i % theMoveMap.Horizontal == 0 
               && i <theMoveMap.BottomRight) ||
               (i % theMoveMap.Horizontal == theMoveMap.TopRight &&
               i <= theMoveMap.BottomRight))
            {
                theMoveMap.Slots[i].SlotSheet.SlotSheet = objectType;
                theMoveMap.Slots[i].SlotSheet.ObjectNum = SkinObjectNum;
                theMoveMap.Slots[i].SlotSheet.SkinName = SkinName;
                theMoveMap.Slots[i].TestText.enabled = true;
                theMoveMap.Slots[i].TestText.text = ((int)objectType).ToString();
            }
            theMoveMap.Slots[i].nodeColor = NodeColor.NC8_Null;
            theMoveMap.Slots[i].SlotNum = i;
            
        }
    }


    public void BT_TestStart()
    {
        TestStartBt.SetActive(false);
        SaveButton.SetActive(true);
        IngameUi.SetActive(true);
        thePuzzle.SetPlayerUi();
        thePuzzle.LoadMap(theMoveMap,false);
        PuzzleMakerStart = false;
        theMoveMap.Slots[PlayerStartNum].nodeColor = NodeColor.NC6_Player;
        theMoveMap.Slots[PlayerStartNum].cube.nodeColor = NodeColor.NC6_Player;
        theMoveMap.Slots[PlayerStartNum].cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        Player.transform.position = theMoveMap.Slots[PlayerStartNum].cube.transform.position;
        Player.transform.SetParent(theMoveMap.Slots[PlayerStartNum].cube.transform);

        theCam.state = CameraManager.State.SmoothMove;
        thePuzzle.gameMode = PuzzleManager.GameMode.MoveMap;
        thePuzzle.state = PuzzleManager.State.Ready;
        theCam.SetBound(theMoveMap, Player.transform.position, true);

    }




}
