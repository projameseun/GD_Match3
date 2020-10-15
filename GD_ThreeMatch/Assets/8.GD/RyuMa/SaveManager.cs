using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


[System.Serializable]
public class MapInfo
{
    public string Name, Value;


    public MapInfo(string name, string value)
    {
        Name = name;
        Value = value;

    }
    public MapInfo(string name, int value)
    {
        Name = name;
        Value = value.ToString();

    }
}



public class SaveManager : G_Singleton<SaveManager>
{
    private List<SlotInfo> puzzleslotList = new List<SlotInfo>();
    private List<MapInfo> mapInfoList = new List<MapInfo>();
    private PlayerSaveData playerSaveData = new PlayerSaveData();

    public List<SlotInfo> PuzzleSlotList
    {
        get
        {
            MapManager _Map = PuzzleMaker.Instance.EditorMap;

            puzzleslotList.Clear();


            for (int Hor = 0; Hor < _Map.BottomRight; Hor += MatchBase.MaxHorizon)
            {
                for (int i = 0; i <= _Map.TopRight; i++)
                {
                    puzzleslotList.Add(new SlotInfo(_Map.Slots[i + Hor].GetComponent<EditorSlot>()));

                }
            }
            return puzzleslotList;
        }

        set
        {
            puzzleslotList = value;
        }
    }

    public List<MapInfo> MapInfoList
    {

        get
        {
            //MapManager _Map = thePuzzle.theMoveMap;
            mapInfoList.Clear();

            mapInfoList.Add(new MapInfo("MapName", PuzzleMaker.Instance.m_MapName));
            mapInfoList.Add(new MapInfo("Horizon", PuzzleMaker.Instance.m_Horizon));
            mapInfoList.Add(new MapInfo("Horizon", PuzzleMaker.Instance.m_Vertical));

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


    public void SaveMap()
    {
        //string FilePath = Application.streamingAssetsPath + "/" + theMaker.MapName + ".json";
        string FilePath = Path.Combine(Application.streamingAssetsPath, PuzzleMaker.Instance.m_MapName + ".json");

        //byte[] bytes = reader.bytes;
        //string FileName = System.Text.Encoding.UTF8.GetString(bytes);
        //print(FilePath);
        //리스트는 저장이 안되지만 크랠스는 저장이된다
        string jdata = JsonUtility.ToJson(new Serialization<MapInfo>(MapInfoList), true);


        jdata += "[Next]";
        //print(jdata);


        //string FilePath2 = Application.streamingAssetsPath +"/" + theMaker.MapName + "Son.json";
        jdata += JsonUtility.ToJson(new Serialization<SlotInfo>(PuzzleSlotList), true);
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        //print(jdata2);
        Debug.Log("FilePath = " + FilePath);
        Debug.Log("jdata = " + jdata);
        File.WriteAllText(FilePath, jdata);
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

        GameManager.Instance.MapData = jdata.Split(new string[] { "[Next]" }, StringSplitOptions.None);

        //PuzzleMaker.Instance.BT_TestStart();
    }
    public void EditorMapSet()
    {

        List<MapInfo> a_LoadMapList;
        a_LoadMapList = JsonUtility.FromJson<Serialization<MapInfo>>(GameManager.Instance.MapData[0]).Slot;

        PuzzleMaker.Instance.m_Horizon = int.Parse(a_LoadMapList[1].Value);
        PuzzleMaker.Instance.m_Vertical = int.Parse(a_LoadMapList[2].Value);

        PuzzleMaker.Instance.TopRight = PuzzleMaker.Instance.m_Horizon - 1;
        PuzzleMaker.Instance.BottomLeft = MatchBase.MaxHorizon * (PuzzleMaker.Instance.m_Vertical - 1);
        PuzzleMaker.Instance.BottomRight = PuzzleMaker.Instance.BottomLeft + PuzzleMaker.Instance.TopRight;


        MapManager _Map = PuzzleMaker.Instance.EditorMap;
        _Map.Horizon = PuzzleMaker.Instance.m_Horizon;
        _Map.Vertical = PuzzleMaker.Instance.m_Vertical;
        _Map.TopRight = PuzzleMaker.Instance.TopRight;
        _Map.BottomLeft = PuzzleMaker.Instance.BottomLeft;
        _Map.BottomRight = PuzzleMaker.Instance.BottomRight;



        List<SlotInfo> a_LoadSlotList;
        a_LoadSlotList = JsonUtility.FromJson<Serialization<SlotInfo>>(GameManager.Instance.MapData[1]).Slot;
        puzzleslotList = (a_LoadSlotList);




        int SlotListCount = 0;


        for (int y = 0; y < MatchBase.MaxHorizon * MatchBase.MaxVertical; y += MatchBase.MaxHorizon)
        {
            for (int x = 0; x < MatchBase.MaxHorizon; x++)
            {
                _Map.Slots[x + y].m_Image.enabled = (x <= PuzzleMaker.Instance.TopRight && y <= PuzzleMaker.Instance.BottomRight) ? true : false;
                _Map.Slots[x + y].m_Text.enabled = (x <= PuzzleMaker.Instance.TopRight && y <= PuzzleMaker.Instance.BottomRight) ? true : false;

                if (x <= PuzzleMaker.Instance.TopRight && y <= PuzzleMaker.Instance.BottomRight)
                {
                    _Map.Slots[x + y].SetSlot(puzzleslotList[SlotListCount]);
                    SlotEditorBase.Instance.ChangeBlockImage((EditorSlot)_Map.Slots[x + y], (BlockType)puzzleslotList[SlotListCount].BlockType, (NodeColor)puzzleslotList[SlotListCount].m_Color);

                    SlotEditorBase.Instance.ChangePanelImage((EditorSlot)_Map.Slots[x + y],
                        (PanelType)puzzleslotList[SlotListCount].UpPanelType, puzzleslotList[SlotListCount].m_UpCount);

                    SlotEditorBase.Instance.ChangePanelImage((EditorSlot)_Map.Slots[x + y],
                        (PanelType)puzzleslotList[SlotListCount].MiddlePanelType, puzzleslotList[SlotListCount].m_MiddleCount);

                    SlotEditorBase.Instance.ChangePanelImage((EditorSlot)_Map.Slots[x + y],
                        (PanelType)puzzleslotList[SlotListCount].DownPanelType, puzzleslotList[SlotListCount].m_DownCount);
                    SlotListCount++;

                }
                else
                {
                    _Map.Slots[x + y].Resetting();
                }
                


            }
        }
    }

}
