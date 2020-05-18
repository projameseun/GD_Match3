﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ChangeMode
{
    Null = 0,
    Normal,
    Player,
    Enemy,
    Portal,
    Object
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
    public GameObject IngameUi;

    [HideInInspector] public bool PuzzleMakerStart;
    public ChangeMode changeMode;



    [Header("MapSetting")]
    public string MapName;
    public int TopLeft;
    public int TopRight;
    public int BottomLeft;
    public int BottomRight;




    [Header("EnemySetting")]
    public Color EnemyColor;
    public int addEnemyMeet;            //몬스터랑 조우할 확률의 증가량
    [Space]
    public EnemyIndex[] enemyIndex;     //몬스터 정보
    [Space]
    public bool OnlyOneEnemy;           //한번만 생성하는 몬스터일 경우 true
    public int DataSheet;               //한번만 생성이라면 사용할 데이터시트의 번호

    [Header("PortalSetting")]

    public string MoveMapName;      //이동할 맵의 이름
    public int PlayerStartPos;      //플레이어가 시작할 슬롯의 넘버


    private PuzzleManager thePuzzle;
    private ObjectManager theObject;
    private CameraManager theCam;
    private void Start()
    {
        theCam = FindObjectOfType<CameraManager>();
        theObject = FindObjectOfType<ObjectManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }




    public void BT_PuzzleMaker(PuzzleSlot _Slot, int _SlotNum)
    {
        if (_Slot.portalSheet != null)
            _Slot.portalSheet = null;

        if (_Slot.monsterSheet != null)
            _Slot.monsterSheet = null;


        if (changeMode == ChangeMode.Null)
        {
            _Slot.nodeType = PuzzleSlot.NodeType.Null;
            _Slot.nodeColor = NodeColor.Null;
            _Slot.TestText.text = "N"; //string.Format("N(" + _SlotNum + ")");
            _Slot.TestText.color = new Color(1, 1, 1);
        }
        else if (changeMode == ChangeMode.Normal)
        {
            _Slot.nodeType = PuzzleSlot.NodeType.Normal;
            _Slot.nodeColor = NodeColor.Null;
            _Slot.TestText.text = "C"; //string.Format(_SlotNum.ToString());
            _Slot.TestText.color = new Color(0,0,0);
        }
        //else if (changeMode == ChangeMode.Player)
        //{
        //    _Slot.nodeType = PuzzleSlot.NodeType.Normal;
        //    _Slot.nodeColor = NodeColor.Player;
        //    _Slot.TestText.text = "P";
        //    _Slot.TestText.color = new Color(0, 0, 1);
        //}
        else if (changeMode == ChangeMode.Enemy)
        {
            _Slot.nodeType = PuzzleSlot.NodeType.Enemy;
            _Slot.nodeColor = NodeColor.Null;
            _Slot.TestText.text = "E";
            _Slot.TestText.color = EnemyColor;
            _Slot.monsterSheet = new MonsterSheet();

            _Slot.monsterSheet.OnlyOneEnemy = OnlyOneEnemy;
            if (OnlyOneEnemy == true)
                _Slot.monsterSheet.OnlyOneNum = DataSheet;

            _Slot.monsterSheet.addEnemyMeet = addEnemyMeet;
            _Slot.monsterSheet.EnemyIndex = new int[enemyIndex.Length];
            _Slot.monsterSheet.EnemyChance = new int[enemyIndex.Length];
            for (int i = 0; i < enemyIndex.Length; i++)
            {
                _Slot.monsterSheet.EnemyIndex[i] = enemyIndex[i].EnemyNum;
                _Slot.monsterSheet.EnemyChance[i] = enemyIndex[i].ChanceMeet;
            }

        }
        else if (changeMode == ChangeMode.Portal)
        {
            _Slot.nodeType = PuzzleSlot.NodeType.Portal;
            _Slot.nodeColor = NodeColor.Null;
            _Slot.TestText.text = "P";//string.Format("P(" + _SlotNum + ")");
            _Slot.TestText.color = new Color(0, 0, 1f);

            _Slot.portalSheet = new PortalSheet();
            _Slot.portalSheet.MapName = MoveMapName;
            _Slot.portalSheet.NextPosNum = PlayerStartPos;
        }

    }

    public void BT_SonMapStart()
    {
        theCam.MoveVec = theCam.gameObject.transform.position;
        theCam.MoveVec.z = -10;
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
                theMoveMap.Slots[i].nodeColor = NodeColor.Null;
                theMoveMap.Slots[i].TestText.text = "N"; //string.Format("N(" + _SlotNum + ")");
                theMoveMap.Slots[i].TestText.color = new Color(1, 1, 1);
            }
            else
            {
                theMoveMap.Slots[i].TestText.text = i.ToString();
                theMoveMap.Slots[i].nodeType = PuzzleSlot.NodeType.Normal;
            }
            theMoveMap.Slots[i].nodeColor = NodeColor.Null;
            theMoveMap.Slots[i].SlotNum = i;
            theMoveMap.Slots[i].TestText.enabled = true;
        }
    }



}
