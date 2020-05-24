using JetBrains.Annotations;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//MapInfo
[System.Serializable]
public class MapInfoSerialization<T>
{
    public MapInfoSerialization(List<T> _Info) =>Info = _Info;
   
    public List<T> Info;
}

public class MapInfo
{
    public string Name,Value;
    public bool isUsing;

    public MapInfo(string name, string value, bool isUsing)
    {
        Name = name;
        Value = value;
        this.isUsing = isUsing;
    }

}


public enum MapType
{ 
    M1_MoveMap = 0,
    M2_BattleMap,
    M3_Null
}



public class MapManager : MonoBehaviour
{
    static public MapManager instance;//싱글톤

    private List<MapInfo> mapinfoList = new List<MapInfo>();

    public MapType mapType;


    public GameObject SlotPrefab;
    public Vector2 CameraPos;

    public GameObject SlotBase;
    public PuzzleSlot[] Slots;

    public Direction direction;

    public int Horizontal;
    public int Vertical;
    public int TopLeft;
    public int TopRight;
    public int BottomLeft;
    public int BottomRight;

   

    public List<MapInfo> MapInfoList
    {
        get {

            return mapinfoList;
        }

        set
        {
            
            mapinfoList = value;
            for (int i = 0; i < mapinfoList.Count; i++)
            {

                if (i == 0) MapManager.instance.TopRight = int.Parse(mapinfoList[i].Value);
                if (i == 1) MapManager.instance.BottomLeft = int.Parse(mapinfoList[i].Value);
                if (i == 2) MapManager.instance.BottomRight = int.Parse(mapinfoList[i].Value);

            }
        }
    }// public List<MapInfo> MapInfoList

    public void SaveMapInfo(List<MapInfo> a_MapList)
    {
        MapInfoList = a_MapList;
       
        StorageManager.instance.SaveMapInfo();
    }

    public void LoadMapInfo()
    {
        
        StorageManager.instance.LoadMapInfo();
    }

    private void Start()
    {
        if (mapType == MapType.M1_MoveMap)
        {
            int Size = Horizontal * Vertical;
            Slots = new PuzzleSlot[Size];
            //GameObject SlotObj = Instantiate(SlotPrefab);
            for (int i = 0; i < Size; i++)
            {
                GameObject SlotObj = Instantiate(SlotPrefab);
                SlotObj.transform.parent = SlotBase.transform;
                SlotObj.gameObject.name = string.Format("Slot" + i);
                Slots[i] = SlotObj.GetComponent<PuzzleSlot>();
            }
        }
    }



}
