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



[System.Serializable]
public class EnemyIndex
{
    public int EnemyNum;
    public int ChanceMeet;
}

public class PuzzleMaker : A_Singleton<PuzzleMaker>
{
    [HideInInspector]
    public bool MakerStart = false;
    public int m_MapType;


    public BlockType m_blockType;
    public PanelType m_PanelType;
    public NodeColor m_NodeColor;
    public int m_Count;
    public string m_Data1;
    public string m_Data2;
    public string m_Data3;
    public string m_Data4;
    public string[] SlotData;

    public int m_Value;

    // 블럭
    public bool m_CubeCh;
    public bool m_RockCh;

    // 판넬
    public bool m_BackPanelCh;
    public bool m_PortalCh;
    public bool m_WallCh;
    public bool m_CageCh;

    public bool[] m_CubeSpawn;






    public MapMainType mapMainType;
    [HideInInspector]
    public MapManager EditorMap
    {
        get 
        {
          return FindObjectOfType<MapManager>();
        }
        set
        {
            EditorMap = value;
        }
    }


    [HideInInspector]
    public Block SelectBlock;


    [HideInInspector]
    public Panel SelectPanel;


    public string m_MapName;
    public int StartIndex;

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


    protected override void Init()
    {
        base.Init();
        MakerStart = true;
        m_CubeSpawn = new bool[MatchBase.ColorKinds];
        //EditorMap = FindObjectOfType<MapManager>();

        //인스펙터를 처음에 해준다
        Selection.activeGameObject = this.gameObject;
    }

    private void Start()
    {
        Selection.activeGameObject = this.gameObject;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShowIndex();
        }
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




    //모든 블럭, 판넬 에디텉 비활성화
    public void AllCheckOff()
    {

        //블럭
        m_CubeCh = false;
        m_RockCh = false;


        //판넬
        m_BackPanelCh = false;
        m_PortalCh = false;
        m_WallCh = false;
        m_CageCh = false;
    }





    // 맵의 처음값을 넣고 세팅해준다
    public void SettingMap()
    {


        TopLeft = 0;
        TopRight = m_Horizon - 1;
        BottomLeft = MatchBase.MaxHorizon * (m_Vertical-1);
        BottomRight = BottomLeft + TopRight;

        for (int i = 0; i < m_CubeSpawn.Length; i++)
        {
            m_CubeSpawn[i] = true;
        }

        EditorMap.Horizon = m_Horizon;
        EditorMap.Vertical = m_Vertical;
        EditorMap.TopRight = TopRight;
        EditorMap.BottomLeft = BottomLeft;
        EditorMap.BottomRight = BottomRight;
        string[] RandomCub = { "0", "5" };
        string[] BasicBack = { "0", "0" };
        for (int y = 0; y < MatchBase.MaxHorizon * MatchBase.MaxVertical; y+= MatchBase.MaxHorizon)
        {
            for (int x = 0; x < MatchBase.MaxHorizon; x++)
            {
                EditorMap.Slots[x + y].Resetting();
                EditorMap.Slots[x + y].m_Image.enabled = (x <= TopRight && y <= BottomRight) ? true : false;
                EditorMap.Slots[x + y].m_Text.enabled = (x <= TopRight && y <= BottomRight) ? true : false;

                if (x <= TopRight && y <= BottomRight)
                {
                    EditorMap.Slots[x + y].gameObject.SetActive(true);
                    if (y == 0 || y == EditorMap.BottomLeft || x == 0 || x == EditorMap.TopRight)
                    {
                        SlotEditorBase.Instance.ChangePanelImage((EditorSlot)EditorMap.Slots[x + y], BasicBack);
                    }
                    else
                    {
                        SlotEditorBase.Instance.ChangeBlockImage((EditorSlot)EditorMap.Slots[x + y], RandomCub);
                    }
                }
                else 
                {
                    EditorMap.Slots[x + y].gameObject.SetActive(false);
                }

            }
        }

    }


    bool IndexOnOff = true;
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
