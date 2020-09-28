﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Net;


[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _Slot) => Slot = _Slot;

    public Serialization(PlayerSaveData _Data) => Data = _Data;


    public List<T> Slot;

    public PlayerSaveData Data;


}


[System.Serializable]
public class MapInfo
{
    public string Name, Value;


    public MapInfo(string name, string value)
    {
        Name = name;
        Value = value;

    }
}

[System.Serializable]
public class SlotInfo
{
    public string Type;

    public int BlockType;
    public int UpPanelType;
    public int MiddlePanelType;
    public int DownPanelType;

    public SlotInfo(BlockType _Block, PanelType _UpPanel, PanelType _MiddlePanel, PanelType _DownPanel)
    {
        BlockType = (int)_Block;
        UpPanelType = (int)_UpPanel;
        MiddlePanelType = (int)_MiddlePanel;
        DownPanelType = (int)_DownPanel;
    }
    //public MonsterSheet monsheet;
    //public PortalSheet portalsheet;
    //public SlotObjectSheets slotObject;

    //public SlotInfo(string type, MonsterSheet monsheet, PortalSheet portalsheet, SlotObjectSheets _slotObject)
    //{
    //    Type = type;
    //    this.monsheet = monsheet;
    //    this.portalsheet = portalsheet;
    //    this.slotObject = _slotObject;
    //}
    public SlotInfo()
    { 
        
    }

}


[System.Serializable]
public class PlayerSaveData
{
    public List<bool> MonsterDataSheet;
    public List<bool> ProgressDataSheet;
    public int CurrentProgressNum;

    public PlayerSaveData()
    {
        MonsterDataSheet = new List<bool>();
        ProgressDataSheet = new List<bool>();
        CurrentProgressNum = 0;
    }

}
public enum GameMode
{ 
    Gaming = 0,
    Editor
}


public enum GMState
{ 
    GM00_Title = 0,
    GM00_Tutorial,
    GM01_Lobby,
    GM02_InGame,
    
}


public class GameManager : G_Singleton<GameManager>
{
    public bool CheatMode;
    public GMState state;
    public GameMode gameMode;


    public List<bool> EnemyDataSheet = new List<bool>(200);
    public List<bool> ProgressDataSheet = new List<bool>(200);
    public int CurrentProgressNum = 0;

    float deltaTime = 0.0f;
    Color GUIColor = new Color(1, 1, 1, 1);
    GUIStyle style = new GUIStyle();
    int w = 0, h = 0;
    Rect rect;

    protected override void Init()
    {
        Application.targetFrameRate = 60;
        if (CheatMode == true)
        {
            w = Screen.width;
            h = Screen.height;

            style = new GUIStyle();

            rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = GUIColor;
        }
    }
    void Update()
    {
        if (CheatMode)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        }
       
    }
    void OnGUI()
    {
        if (CheatMode == true) 
        {
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", deltaTime * 1000.0f, 1.0f / deltaTime);
            GUI.Label(rect, text, style);
        }
            
    }


    //public TextAsset MapBase;

    private List<SlotInfo> puzzleslotList = new List<SlotInfo>();
    private List<MapInfo> mapInfoList = new List<MapInfo>();
    private PlayerSaveData playerSaveData = new PlayerSaveData();

    private void Start()
    {

        //InitSetting();
    }

    // 게임 저장할 데이터
    public PlayerSaveData SaveData
    {
        get
        {
            playerSaveData = new PlayerSaveData();
            playerSaveData.MonsterDataSheet = new List<bool>(EnemyDataSheet);
            playerSaveData.ProgressDataSheet = new List<bool>(ProgressDataSheet);
            playerSaveData.CurrentProgressNum = CurrentProgressNum;
            return playerSaveData;
        }
        set
        {
            playerSaveData = value;
        }
    }

    public List<MapInfo> MapInfoList
    {
        get
        {
            //MapManager _Map = thePuzzle.theMoveMap;
            mapInfoList = new List<MapInfo>();

            mapInfoList.Add(new MapInfo("MapMainType", ((int)PuzzleMaker.Instance.mapMainType).ToString()));
            mapInfoList.Add(new MapInfo("MapName", PuzzleMaker.Instance.m_MapName));
          
            //mapInfoList.Add(new MapInfo("TopRight", thePuzzle.theMoveMap.TopRight.ToString()));
            //mapInfoList.Add(new MapInfo("BottomLeft", thePuzzle.theMoveMap.BottomLeft.ToString()));
            //mapInfoList.Add(new MapInfo("BottomRight", thePuzzle.theMoveMap.BottomRight.ToString()));

            return mapInfoList;

        }
        set
        {
            mapInfoList = value;
            
        }
    }

    public List<SlotInfo> PuzzleSlotList
    {
        get
        {
            MapManager _Map = PuzzleMaker.Instance.EditorMap;

            puzzleslotList = new List<SlotInfo>();
            for (int Hor = 0; Hor < _Map.BottomRight; Hor += MatchBase.MaxHorizon)
            {
                for (int i = 0; i <= _Map.TopRight; i++)
                {
                    //puzzleslotList.Add(new SlotInfo(((int)_Map.Slots[i + Hor].block.blockType).ToString(), _Map.Slots[i + Hor].monsterSheet, 
                    //                            _Map.Slots[i + Hor].portalSheet, _Map.Slots[i + Hor].SlotSheet));;
                    //Debug.Log(puzzleslotList[i + Hor].nodeType);

                }
            }
            return puzzleslotList;
        }

        set
        {
            puzzleslotList = value;
        }
    }// public List<MapInfo> MapInfoList



    public void SaveGameData()
    {
        Debug.Log("저장");
        string FilePath = Path.Combine(Application.persistentDataPath,"PlayerData.json");
        string jdata = JsonUtility.ToJson(new Serialization<PlayerSaveData>(SaveData), true);
        Debug.Log(FilePath);
        File.WriteAllText(FilePath, jdata);
    }

    public void LoadGameData()
    {
        Debug.Log("불러오기");
        string FilePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        string jdata = File.ReadAllText(FilePath);
        PlayerSaveData NewData;
        NewData = JsonUtility.FromJson<Serialization<PlayerSaveData>>(jdata).Data;
        playerSaveData = (NewData);
    }



    public void SaveBtn()
    {
        //string FilePath = Application.streamingAssetsPath + "/" + theMaker.MapName + ".json";
        string FilePath = Path.Combine(Application.streamingAssetsPath, PuzzleMaker.Instance.m_MapName + ".json");

        //byte[] bytes = reader.bytes;
        //string FileName = System.Text.Encoding.UTF8.GetString(bytes);
        //print(FilePath);
        //리스트는 저장이 안되지만 크랠스는 저장이된다
        string jdata = JsonUtility.ToJson(new Serialization<MapInfo>(MapInfoList), true);
        //print(jdata);
        Debug.Log("FilePath = " + FilePath);
        Debug.Log("jdata = " + jdata);


        File.WriteAllText(FilePath, jdata);



        //string FilePath2 = Application.streamingAssetsPath +"/" + theMaker.MapName + "Son.json";
        string FilePath2 = Path.Combine(Application.streamingAssetsPath, PuzzleMaker.Instance.m_MapName + "Son.json");
        string jdata2 = JsonUtility.ToJson(new Serialization<SlotInfo>(PuzzleSlotList) , true);
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        //print(jdata2);
        Debug.Log("FilePath2 = " + FilePath2);
        Debug.Log("jdata2 = " + jdata2);



        File.WriteAllText(FilePath2, jdata2);
        //만약에 Json으로 변경할려면 경로를 변경해주면된다
        // string FilePath = Application.persistentDataPath + "/MyItem.json";
        //for (int i = 0; i < SetMapList.Count; i++)
        //{
        //    if (i == 0) SetMapList[i].Value = MapManager.instance.TopRight.ToString();
        //    if (i == 1) SetMapList[i].Value = MapManager.instance.BottomLeft.ToString();
        //    if (i == 2) SetMapList[i].Value = MapManager.instance.BottomRight.ToString();
        //}


        Debug.Log("저장하였습니다");

    }

    public void LoadMap()
    {
        //Debug.Log("로드를 눌렀습니다");

        //string FilePath = Application.streamingAssetsPath +"/"+ theMaker.MapName + ".json";
        string FilePath = Path.Combine(Application.streamingAssetsPath, PuzzleMaker.Instance.m_MapName + ".json");

        //복호화
        //Debug.Log("FilePath = " + FilePath);

        WWW reader = new WWW(FilePath);

        while (!reader.isDone)
        {

        }

        //string jdata = File.ReadAllText(FilePath);

        byte[] bytes = reader.bytes;
        string jdata = System.Text.Encoding.UTF8.GetString(bytes);
        List<MapInfo> a_LoadMapList;
        a_LoadMapList = JsonUtility.FromJson<Serialization<MapInfo>>(jdata).Slot;
        mapInfoList = (a_LoadMapList);

        string FilePath2 = Path.Combine( Application.streamingAssetsPath + "/" + PuzzleMaker.Instance.m_MapName + "Son.json");
        reader = new WWW(FilePath2);

        while (!reader.isDone)
        {

        }
        // string jdata2 = File.ReadAllText(FilePath2);
        byte[] bytes2 = reader.bytes;
        string jdata2 = System.Text.Encoding.UTF8.GetString(bytes2);
        List<SlotInfo> a_LoadSlotList;
        a_LoadSlotList = JsonUtility.FromJson<Serialization<SlotInfo>>(jdata2).Slot;
        puzzleslotList = (a_LoadSlotList);

        PuzzleMaker.Instance.mapMainType = (MapMainType)int.Parse(mapInfoList[0].Value);
        PuzzleMaker.Instance.m_MapName = mapInfoList[1].Value;

        //theMoveMap.TopRight = int.Parse(mapInfoList[2].Value);
        //theMoveMap.BottomLeft = int.Parse(mapInfoList[3].Value);
        //theMoveMap.BottomRight = int.Parse(mapInfoList[4].Value);

        int ListCount = 0;

        //TODO
        //for(int i=0; i < theMoveMap.Horizontal * theMoveMap.Vertical; i++)
        //{
        //    theMoveMap.Slots[i].block. = PuzzleSlot.NodeType.Null;
        //    theMoveMap.Slots[i].nodeColor = NodeColor.NC8_Null;
        //    theMoveMap.Slots[i].SlotSheet.SlotSheet = SlotObjectSheet.NULL; 
        //    if (theMoveMap.Slots[i].cube != null)
        //    {
        //        theMoveMap.Slots[i].cube.Resetting();
        //        theMoveMap.Slots[i].cube = null;
        //    }

        //}

        //for (int Hor = 0; Hor < theMoveMap.BottomRight; Hor+=thePuzzle.theMoveMap.Horizontal)
        //{
        //    for(int i=0; i<=theMoveMap.TopRight; i++)
        //    {
        //        theMoveMap.Slots[i + Hor].nodeType = (PuzzleSlot.NodeType)(int.Parse(puzzleslotList[ListCount].Type));
        //        theMoveMap.Slots[i + Hor].nodeColor = NodeColor.NC8_Null;
        //        theMoveMap.Slots[i + Hor].SlotSheet = puzzleslotList[ListCount].slotObject;
        //        if (theMoveMap.Slots[i + Hor].nodeType == PuzzleSlot.NodeType.Enemy)
        //        {
        //            theMoveMap.Slots[i + Hor].monsterSheet = puzzleslotList[ListCount].monsheet;
        //        }
        //        else if(theMoveMap.Slots[i + Hor].nodeType == PuzzleSlot.NodeType.Portal)
        //        {
        //            theMoveMap.Slots[i + Hor].portalSheet = puzzleslotList[ListCount].portalsheet;
        //        }

        //        ListCount++;
        //    }
        //}

        PuzzleMaker.Instance.BT_TestStart();
    }

    public void GameOver()
    {
        //theTitle.TitleAnim.gameObject.SetActive(true);
        //state = GMState.GM00_Title;
        //thePuzzle.state = PuzzleManager.State.Ready;
        //thePuzzle.gameMode = PuzzleManager.GameMode.Null;
        //theObject.ResettingAllObj();

    }
}