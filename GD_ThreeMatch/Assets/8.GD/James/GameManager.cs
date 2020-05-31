using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Net;


[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _Slot) => Slot = _Slot;

    public List<T> Slot;
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
    public MonsterSheet monsheet;
    public PortalSheet portalsheet;

    public SlotInfo(string type, MonsterSheet monsheet, PortalSheet portalsheet)
    {
        Type = type;
        this.monsheet = monsheet;
        this.portalsheet = portalsheet;
    }

    public SlotInfo() { }
}


public class GameManager : MonoBehaviour
{


    //public TextAsset MapBase;

    private List<SlotInfo> puzzleslotList = new List<SlotInfo>();
    private List<MapInfo> mapInfoList = new List<MapInfo>();



    private PuzzleManager thePuzzle;

    //string FilePath = Application.streamingAssetsPath + "/MapInfo.json";
    

    private PuzzleMaker theMaker;
    private MapManager theMoveMap;
    
    private void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theMaker = FindObjectOfType<PuzzleMaker>();
        theMoveMap = thePuzzle.theMoveMap;
        //InitSetting();
    }

    public List<MapInfo> MapInfoList
    {
        get
        {
            //MapManager _Map = thePuzzle.theMoveMap;
            mapInfoList = new List<MapInfo>();
            
            mapInfoList.Add(new MapInfo("MapName", theMaker.MapName));
          
            mapInfoList.Add(new MapInfo("TopRight", thePuzzle.theMoveMap.TopRight.ToString()));
            mapInfoList.Add(new MapInfo("BottomLeft", thePuzzle.theMoveMap.BottomLeft.ToString()));
            mapInfoList.Add(new MapInfo("BottomRight", thePuzzle.theMoveMap.BottomRight.ToString()));

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
            MapManager _Map = thePuzzle.theMoveMap;

            puzzleslotList = new List<SlotInfo>();
            for (int Hor = 0; Hor < _Map.BottomRight; Hor += _Map.Horizontal)
            {
                for (int i = 0; i <= _Map.TopRight; i++)
                {
                    puzzleslotList.Add(new SlotInfo(((int)_Map.Slots[i + Hor].nodeType).ToString(), _Map.Slots[i + Hor].monsterSheet, 
                                                _Map.Slots[i + Hor].portalSheet));;
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

   



    public void SaveBtn()
    {
        string FilePath = Application.streamingAssetsPath + "/" + theMaker.MapName + ".json";

        //print(FilePath);
        //리스트는 저장이 안되지만 크랠스는 저장이된다
        string jdata = JsonUtility.ToJson(new Serialization<MapInfo>(MapInfoList));
        //print(jdata);
        File.WriteAllText(FilePath, jdata);

        string FilePath2 = Application.streamingAssetsPath + "/" + theMaker.MapName + "Son.json";

        string jdata2 = JsonUtility.ToJson(new Serialization<SlotInfo>(PuzzleSlotList));
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        //print(jdata2);
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

    public void LoadBtn()
    {
        Debug.Log("로드를 눌렀습니다");
        string FilePath = Application.streamingAssetsPath + "/" + theMaker.MapName + ".json";
        //복호화
        string jdata = File.ReadAllText(FilePath);
        // byte[] bytes = System.Convert.FromBase64String(code);
        //string jdata = System.Text.Encoding.UTF8.GetString(bytes);
        //PlayerManager.instance.SetItemList();
        List<MapInfo> a_LoadMapList;
        a_LoadMapList = JsonUtility.FromJson<Serialization<MapInfo>>(jdata).Slot;
        mapInfoList = (a_LoadMapList);

        string FilePath2 = Application.streamingAssetsPath + "/" + theMaker.MapName + "Son.json";
        string jdata2 = File.ReadAllText(FilePath2);
        // byte[] bytes = System.Convert.FromBase64String(code);
        //string jdata = System.Text.Encoding.UTF8.GetString(bytes);
        //PlayerManager.instance.SetItemList();
        List<SlotInfo> a_LoadSlotList;
        a_LoadSlotList = JsonUtility.FromJson<Serialization<SlotInfo>>(jdata2).Slot;
        puzzleslotList = (a_LoadSlotList);


        theMoveMap.TopRight = int.Parse(mapInfoList[1].Value);
        theMoveMap.BottomLeft = int.Parse(mapInfoList[2].Value);
        theMoveMap.BottomRight = int.Parse(mapInfoList[3].Value);

        int ListCount = 0;
        for(int i=0; i < theMoveMap.Horizontal * theMoveMap.Vertical; i++)
        {
            theMoveMap.Slots[i].nodeType = PuzzleSlot.NodeType.Null;
            theMoveMap.Slots[i].nodeColor = NodeColor.NC8_Null;
            if(theMoveMap.Slots[i].cube != null)
            {
                theMoveMap.Slots[i].cube.Resetting();
                theMoveMap.Slots[i].cube = null;
            }
            
        }

        for (int Hor = 0; Hor < theMoveMap.BottomRight; Hor+=thePuzzle.theMoveMap.Horizontal)
        {
            for(int i=0; i<=theMoveMap.TopRight; i++)
            {
                theMoveMap.Slots[i + Hor].nodeType = (PuzzleSlot.NodeType)(int.Parse(puzzleslotList[ListCount].Type));
                theMoveMap.Slots[i + Hor].nodeColor = NodeColor.NC8_Null;
                if(theMoveMap.Slots[i + Hor].nodeType == PuzzleSlot.NodeType.Enemy)
                {
                    theMoveMap.Slots[i + Hor].monsterSheet = puzzleslotList[ListCount].monsheet;
                }
                else if(theMoveMap.Slots[i + Hor].nodeType == PuzzleSlot.NodeType.Portal)
                {
                    theMoveMap.Slots[i + Hor].portalSheet = puzzleslotList[ListCount].portalsheet;
                }

                ListCount++;
            }
        }

        theMaker.BT_TestStart();
    }


    public void InitSetting()
    {
        ////아이템의정보
        //string[] line = MapBase.text.Substring(0, MapBase.text.Length - 1).Split('\n');

        //Debug.Log(line.Length);
        //for (int i = 0; i < line.Length; i++)
        //{
        //    string[] row = line[i].Split('\t');
        //    //Debug.Log(row.Length);

        //    SetMapList.Add(new MapInfo(row[0], row[1],row[2] == "TRUE"));
        //    Debug.Log(SetMapList[i].Value);
      
        //}

     
    }
}