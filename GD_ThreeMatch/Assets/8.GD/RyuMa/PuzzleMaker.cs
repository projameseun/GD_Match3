using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

public class PuzzleMaker : G_Singleton<PuzzleMaker>
{

    public bool MakerStart = false;

    [HideInInspector]
    public List<bool> m_BlockCh;
    [HideInInspector]
    public List<bool> m_PanelCh;
    public BlockType m_blockType;
    public PanelType m_PanelType;

    public NodeColor m_NodeColor;


    // 블럭
    public bool m_CubeCh;

    // 판넬
    public int m_Count;

    public bool m_BackPanelCh;


    public MapMainType mapMainType;
    public ChangeMode changeMode;
    [HideInInspector]
    public MapManager EditorMap;


    [HideInInspector]
    public Block SelectBlock;


    [HideInInspector]
    public Panel SelectPanel;


    public string m_MapName;

    public int m_Horizon;
    public int m_Vertical;
    [HideInInspector]
    public int TopLeft;
    [HideInInspector]
    public int TopRight;
    [HideInInspector]
    public int BottomLeft;
    [HideInInspector]
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

    [Header("TestPlaySetting")]
    public int PlayerStartNum;

    
    public void Awake()
    {
        MakerStart = true;

        m_BlockCh.Add(m_CubeCh);
    }

    private void Start()
    {

        EditorMap = FindObjectOfType<MapManager>();
       

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SettingMap();
        }
    }



    public void BT_PuzzleMaker(PuzzleSlot _Slot, int _SlotNum)
    {
        //if (changeMode == ChangeMode.Ch0_Null)
        //{
        //    _Slot.nodeType = PuzzleSlot.NodeType.Null;
        //    _Slot.nodeColor = NodeColor.NC8_Null;
        //    _Slot.TestText.text = ""; //string.Format("N(" + _SlotNum + ")");
        //    _Slot.TestText.color = new Color(1, 1, 1);
        //    _Slot.SlotSheet.SlotSheet = SlotObjectSheet.NULL;
        //}
        //else if (changeMode == ChangeMode.Ch1_Normal)
        //{
        //    _Slot.nodeType = PuzzleSlot.NodeType.Normal;
        //    _Slot.nodeColor = NodeColor.NC8_Null;
        //    _Slot.TestText.text = "C"; //string.Format(_SlotNum.ToString());
        //    _Slot.TestText.color = new Color(0, 0, 0);
        //    _Slot.SlotSheet.SlotSheet = SlotObjectSheet.ST_0_SlotPanel;
        //}
        ////else if (changeMode == ChangeMode.Player)
        ////{
        ////    _Slot.nodeType = PuzzleSlot.NodeType.Normal;
        ////    _Slot.nodeColor = NodeColor.Player;
        ////    _Slot.TestText.text = "P";
        ////    _Slot.TestText.color = new Color(0, 0, 1);
        ////}
        //else if (changeMode == ChangeMode.Ch2_Enemy)
        //{
        //    _Slot.nodeType = PuzzleSlot.NodeType.Enemy;
        //    _Slot.nodeColor = NodeColor.NC8_Null;
        //    _Slot.TestText.text = "E";
        //    _Slot.TestText.color = EnemyColor;
        //    _Slot.SlotSheet.SlotSheet = SlotObjectSheet.ST_1_Enemy;
        //    _Slot.monsterSheet = new MonsterSheet();

        //    _Slot.monsterSheet.OnlyOneEnemy = OnlyOneEnemy;
        //    if (OnlyOneEnemy == true)
        //        _Slot.monsterSheet.OnlyOneNum = DataSheet;

        //    _Slot.monsterSheet.SlotImageIndex = MonsterImageIndex;
        //    _Slot.monsterSheet.addEnemyMeet = addEnemyMeet;
        //    _Slot.monsterSheet.EnemyIndex = new int[enemyIndex.Length];
        //    _Slot.monsterSheet.EnemyChance = new int[enemyIndex.Length];
        //    for (int i = 0; i < enemyIndex.Length; i++)
        //    {
        //        _Slot.monsterSheet.EnemyIndex[i] = enemyIndex[i].EnemyNum;
        //        _Slot.monsterSheet.EnemyChance[i] = enemyIndex[i].ChanceMeet;
        //    }

        //}
        //else if (changeMode == ChangeMode.Ch3_Portal)
        //{
        //    _Slot.nodeType = PuzzleSlot.NodeType.Portal;
        //    _Slot.nodeColor = NodeColor.NC8_Null;
        //    _Slot.TestText.text = "P";//string.Format("P(" + _SlotNum + ")");
        //    _Slot.TestText.color = new Color(0, 0, 1f);

        //    _Slot.portalSheet = new PortalSheet();
        //    _Slot.portalSheet.MapName = MoveMapName;
        //    _Slot.portalSheet.NextPosNum = PlayerStartPos;
        //    _Slot.SlotSheet.SlotSheet = SlotObjectSheet.ST_2_Portal;
        //}
        //else if (changeMode == ChangeMode.Ch4_Object)
        //{
        //    _Slot.nodeType = PuzzleSlot.NodeType.Null;
        //    _Slot.nodeColor = NodeColor.NC8_Null;
        //    _Slot.SlotSheet.SlotSheet = objectType;
        //    _Slot.TestText.text = ((int)objectType).ToString();

        //}

    }

    public void BT_SonMapStart()
    {
        //LoadButton.SetActive(false);
        //SonMapStartBt.SetActive(false);
        //TestStartBt.SetActive(true);
        //theTitle.TitleAnim.gameObject.SetActive(false);
        //theGM.state = GMState.GM02_InGame;
        //theCam.MoveVec = theCam.gameObject.transform.position;
        //theCam.MoveVec.z = -10;
        //theCam.state = CameraManager.State.SonMap;
        //PuzzleMakerStart = true;
        //IngameUi.SetActive(false);
        ShowSlotNum();
    }

    public void ShowSlotNum()
    {

        //theMoveMap.TopLeft = TopLeft;
        //theMoveMap.TopRight = TopRight;
        //theMoveMap.BottomLeft = BottomLeft;
        //theMoveMap.BottomRight = BottomRight;


        //for (int i = 0; i < theMoveMap.Slots.Length; i++)
        //{
        //    if (i <= theMoveMap.TopRight ||
        //        i >= theMoveMap.BottomLeft ||
        //        i % theMoveMap.Horizontal <= 0 ||
        //        i % theMoveMap.Horizontal >= theMoveMap.TopRight)
        //    {
        //        theMoveMap.Slots[i].nodeType = PuzzleSlot.NodeType.Null;
        //        theMoveMap.Slots[i].nodeColor = NodeColor.NC8_Null;
        //        theMoveMap.Slots[i].SlotSheet.SlotSheet = SlotObjectSheet.NULL;
        //        theMoveMap.Slots[i].TestText.enabled = false; //string.Format("N(" + _SlotNum + ")");

        //    }
        //    else
        //    {
        //        theMoveMap.Slots[i].TestText.enabled = true;
        //        theMoveMap.Slots[i].TestText.text = i.ToString();
        //        theMoveMap.Slots[i].nodeType = PuzzleSlot.NodeType.Normal;
        //        theMoveMap.Slots[i].SlotSheet.SlotSheet = SlotObjectSheet.ST_0_SlotPanel;
        //    }

        //    if ((i <= theMoveMap.TopRight && i >=0) ||
        //       (i >= theMoveMap.BottomLeft && i <= theMoveMap.BottomRight) ||
        //       (i % theMoveMap.Horizontal == 0 
        //       && i <theMoveMap.BottomRight) ||
        //       (i % theMoveMap.Horizontal == theMoveMap.TopRight &&
        //       i <= theMoveMap.BottomRight))
        //    {
        //        theMoveMap.Slots[i].SlotSheet.SlotSheet = objectType;
        //        theMoveMap.Slots[i].TestText.enabled = true;
        //        theMoveMap.Slots[i].TestText.text = ((int)objectType).ToString();
        //    }
        //    theMoveMap.Slots[i].nodeColor = NodeColor.NC8_Null;
        //    theMoveMap.Slots[i].SlotNum = i;
            
        //}
    }


   
    public void BT_TestStart(bool Fade = true)
    {
        //TestStartBt.SetActive(false);
        //SaveButton.SetActive(true);
        //// 위에껀 나중에 지워준다

        //IngameUi.SetActive(true);
        //thePuzzle.LoadMap(theMoveMap,false);
        //PuzzleMakerStart = false;
        //theMoveMap.Slots[PlayerStartNum].nodeColor = NodeColor.NC6_Player;
        //theMoveMap.Slots[PlayerStartNum].cube.nodeColor = NodeColor.NC6_Player;
        //theMoveMap.Slots[PlayerStartNum].cube.SpriteRen.color = new Color(0, 0, 0, 0);
        //Player.transform.position = theMoveMap.Slots[PlayerStartNum].cube.transform.position;
        //Player.transform.SetParent(theMoveMap.Slots[PlayerStartNum].cube.transform);

        //theCam.state = CameraManager.State.SmoothMove;
        //thePuzzle.gameMode = PuzzleManager.GameMode.MoveMap;
        //thePuzzle.SetMoveCount();
        //theCam.SetBound(theMoveMap, Player.transform.position, true);
        //if(Fade == true)
        //    theFade.FadeInEvent();
    }








    // 맵의 처음값을 넣고 세팅해준다
    public void SettingMap()
    {


        TopLeft = 0;
        TopRight = m_Horizon - 1;
        BottomLeft = MatchBase.MaxHorizon * (m_Vertical-1);
        BottomRight = BottomLeft + TopRight;

        EditorMap.Horizon = m_Horizon;
        EditorMap.Vertical = m_Vertical;
        EditorMap.TopRight = TopRight;
        EditorMap.BottomLeft = BottomLeft;
        EditorMap.BottomRight = BottomRight;

        for (int y = 0; y < MatchBase.MaxHorizon * MatchBase.MaxVertical; y+= MatchBase.MaxHorizon)
        {
            for (int x = 0; x < MatchBase.MaxHorizon; x++)
            {
                EditorMap.Slots[x + y].block = EditorMap.Slots[x + y].gameObject.AddComponent<Block>();
                EditorMap.Slots[x + y].m_Image.enabled = (x <= TopRight && y <= BottomRight) ? true : false;
                EditorMap.Slots[x + y].m_Text.enabled = (x <= TopRight && y <= BottomRight) ? true : false;
            }
        }

    }


    bool IndexOnOff = false;
    public void ShowIndex()
    {
        IndexOnOff = !IndexOnOff;

        for (int y = 0; y < MatchBase.MaxHorizon * MatchBase.MaxVertical; y += MatchBase.MaxHorizon)
        {
            for (int x = 0; x < MatchBase.MaxHorizon; x++)
            {

                if (x > TopRight || x+y > BottomRight)
                    continue;

               
                EditorMap.Slots[x + y].m_Text.enabled = IndexOnOff;
            }
        }
    }




}
