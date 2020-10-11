using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
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

        string[] MapData = jdata.Split(new string[] { "[Next]" }, StringSplitOptions.None);

        List<MapInfo> a_LoadMapList;
        a_LoadMapList = JsonUtility.FromJson<Serialization<MapInfo>>(MapData[0]).Slot;

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
        a_LoadSlotList = JsonUtility.FromJson<Serialization<SlotInfo>>(MapData[1]).Slot;
        puzzleslotList = (a_LoadSlotList);




        int SlotListCount = 0;


        for (int y = 0; y < MatchBase.MaxHorizon * MatchBase.MaxVertical; y += MatchBase.MaxHorizon)
        {
            for (int x = 0; x < MatchBase.MaxHorizon; x++)
            {

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
                _Map.Slots[x + y].m_Image.enabled = (x <= PuzzleMaker.Instance.TopRight && y <= PuzzleMaker.Instance.BottomRight) ? true : false;
                _Map.Slots[x + y].m_Text.enabled = (x <= PuzzleMaker.Instance.TopRight && y <= PuzzleMaker.Instance.BottomRight) ? true : false;


            }
        }



        //for (int Hor = 0; Hor < PuzzleMaker.Instance.BottomRight; Hor += MatchBase.MaxHorizon)
        //{
        //    for (int i = 0; i < MatchBase.MaxHorizon; i++)
        //    {
        //        if (Hor < PuzzleMaker.Instance.BottomRight && i <= PuzzleMaker.Instance.TopRight)
        //        {
        //            _Map.Slots[Hor + i].SetSlot(puzzleslotList[SlotListCount]);
        //            SlotEditorBase.Instance.ChangeBlockImage((EditorSlot)_Map.Slots[Hor + i], (BlockType)puzzleslotList[SlotListCount].BlockType, (NodeColor)puzzleslotList[SlotListCount].m_Color);
        //            SlotEditorBase.Instance.ChangePanelImage((EditorSlot)_Map.Slots[Hor + i],
        //                (PanelType)puzzleslotList[SlotListCount].UpPanelType, puzzleslotList[SlotListCount].m_UpCount);
        //            SlotEditorBase.Instance.ChangePanelImage((EditorSlot)_Map.Slots[Hor + i],
        //                (PanelType)puzzleslotList[SlotListCount].MiddlePanelType, puzzleslotList[SlotListCount].m_MiddleCount);
        //            SlotEditorBase.Instance.ChangePanelImage((EditorSlot)_Map.Slots[Hor + i],
        //                (PanelType)puzzleslotList[SlotListCount].DownPanelType, puzzleslotList[SlotListCount].m_DownCount);
        //            SlotListCount++;
        //        }
        //        else
        //        {
        //            _Map.Slots[Hor + i].Resetting();
        //        }
        //        //puzzleslotList[Hor + i];

        //    }
        //}


        //string FilePath2 = Path.Combine(Application.streamingAssetsPath + "/" + PuzzleMaker.Instance.m_MapName + "Son.json");
        //reader = new WWW(FilePath2);

        //while (!reader.isDone)
        //{

        //}
        //// string jdata2 = File.ReadAllText(FilePath2);
        //byte[] bytes2 = reader.bytes;
        //string jdata2 = System.Text.Encoding.UTF8.GetString(bytes2);


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


}
