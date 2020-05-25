using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class GameManager : MonoBehaviour
{


    public TextAsset MapBase;
    private List<MapInfo> SetMapList = new List<MapInfo>();
   
    private void Awake()
    {

        StorageManager.instance = new StorageManager();
        MapManager.instance = new MapManager();
     
    }
    private void Start()
    {
        //InitSetting();
    }



    public void SaveBtn()
    {
        
        
        //for (int i = 0; i < SetMapList.Count; i++)
        //{
        //    if (i == 0) SetMapList[i].Value = MapManager.instance.TopRight.ToString();
        //    if (i == 1) SetMapList[i].Value = MapManager.instance.BottomLeft.ToString();
        //    if (i == 2) SetMapList[i].Value = MapManager.instance.BottomRight.ToString();
        //}
        MapManager.instance.SaveMapInfo(SetMapList);
     

    }

    public void LoadBtn()
    {
        MapManager.instance.LoadMapInfo();

        MapManager.instance.SettinMapInfo();
    }


    public void InitSetting()
    {
        //아이템의정보
        string[] line = MapBase.text.Substring(0, MapBase.text.Length - 1).Split('\n');

        Debug.Log(line.Length);
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //Debug.Log(row.Length);

            SetMapList.Add(new MapInfo(row[0], row[1],row[2] == "TRUE"));
            Debug.Log(SetMapList[i].Value);
      
        }

     
    }
}