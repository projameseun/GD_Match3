using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StorageManager : MonoBehaviour
{
    public static StorageManager instance = null;


    //만약에 저방법을 사용할려면 에셋에다가 streamingAssets 이폴더를 꼮 생성해야된다 
    //그리고 안드로이든 pc든 이폴더가 만약에 존재하지앟는다면 불러와지지않는다 응근히 까다롭다
    // FilePath = Application.streamingAssetsPath + "MyItemText3.txt";

    string FilePath = Application.streamingAssetsPath + "/Son2.json";
    //string FilePath2 = Application.streamingAssetsPath + "/PlayerInfo.json";
    //string FilePath = Application.persistentDataPath + "/MyItem.json";
    //string FilePath2 = Application.persistentDataPath + "/PlayerInfo.json";


    //public void SaveMapInfo()
    //{
    //    MapManager _Map = FindObjectOfType<PuzzleManager>().theMoveMap;

    //    print(FilePath);
    //    //리스트는 저장이 안되지만 크랠스는 저장이된다.
    //    string jdata = JsonUtility.ToJson(new Serialization<PuzzleSlot>(MapManager.instance.PuzzleSlotList));
    //    //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
    //    print(jdata);
    //    File.WriteAllText(FilePath, jdata);
    //    //만약에 Json으로 변경할려면 경로를 변경해주면된다
    //    // string FilePath = Application.persistentDataPath + "/MyItem.json";
    //}


    //public void LoadMapInfo()
    //{
    //    Debug.Log("로드를 눌렀습니다");
    //    //복호화
    //    string jdata = File.ReadAllText(FilePath);
    //    // byte[] bytes = System.Convert.FromBase64String(code);
    //    //string jdata = System.Text.Encoding.UTF8.GetString(bytes);
    //    //PlayerManager.instance.SetItemList();
    //    List<MapInfo> a_LoadMapList;
    //    a_LoadMapList = JsonUtility.FromJson<MapInfoSerialization<MapInfo>>(jdata).Info;
    //    MapManager.instance.MapInfoList = (a_LoadMapList);
    //}


}
