using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class StorageManager : MonoBehaviour
{

    public static StorageManager instance = null;
 
    string FilePath = Application.persistentDataPath + "/MyItem.json";
    string FilePath2 = Application.persistentDataPath + "/PlayerInfo.json";

    void Start()
    {


    }
    // Update is called once per frame
    void Update()
    {

    }

    //public void Save()
    //{
    //    SavePlyerInfo();  //플레이어정보
    //    SaveItem();         //아이템정보
    //}
    public void Load()
    {
        //LoadPlaeyrInfo(); //플레이어정보 
        LoadItem();         //아이템정보
    }
    public void SaveItem()
    {
        print(FilePath);
        //리스트는 저장이 안되지만 크랠스는 저장이된다.
        string jdata = JsonUtility.ToJson(new ItemSerialization<ItemInfo>(PlayerManager.instance.MyItemList));
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        File.WriteAllText(FilePath, jdata);
        //만약에 Json으로 변경할려면 경로를 변경해주면된다
        // string FilePath = Application.persistentDataPath + "/MyItem.json";
    }
    public void SavePlyerInfo()
    {
        print(FilePath2);
        //리스트는 저장이 안되지만 크랠스는 저장이된다.
        string jdata = JsonUtility.ToJson(new PlayerInfoSerialization<PlayerInfo>(PlayerManager.instance.GetPlayerList()));
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        File.WriteAllText(FilePath2, jdata);
    }
 
    public void LoadItem()
    {
        Debug.Log("로드를 눌렀습니다");
        //복호화
        string jdata = File.ReadAllText(FilePath);
        // byte[] bytes = System.Convert.FromBase64String(code);
        //string jdata = System.Text.Encoding.UTF8.GetString(bytes);
        //PlayerManager.instance.SetItemList();
        List<ItemInfo> a_LoadItemList;
       a_LoadItemList = JsonUtility.FromJson<ItemSerialization<ItemInfo>>(jdata).target;
       PlayerManager.instance.MyItemList = (a_LoadItemList);
    }

    public void LoadPlayerInfo()
    {
        //복호화
        string jdata = File.ReadAllText(FilePath2);
        // byte[] bytes = System.Convert.FromBase64String(code);
        //string jdata = System.Text.Encoding.UTF8.GetString(bytes);
        //PlayerManager.instance.SetItemList();
        List<PlayerInfo> a_PlayerInfoList;
        a_PlayerInfoList = JsonUtility.FromJson<PlayerInfoSerialization<PlayerInfo>>(jdata).target;
        PlayerManager.instance.SetPlayerInfoList(a_PlayerInfoList);
    }

}
