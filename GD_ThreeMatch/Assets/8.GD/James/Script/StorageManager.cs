using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class StorageManager : MonoBehaviour
{

    public static StorageManager instance = null;

    string FilePath = Application.persistentDataPath + "/MyItem.txt";

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Save()
    {
        //SavePlyerInfo();  //플레이어정보
        SaveItem();         //아이템정보
    }
    public void Load()
    {
        //LoadPlaeyrInfo(); //플레이어정보 
        LoadItem();         //아이템정보
    }
    private void SaveItem()
    {
        print(FilePath);
        //리스트는 저장이 안되지만 크랠스는 저장이된다.
        string jdata = JsonUtility.ToJson(new ItemSerialization<ItemInfo>(PlayerManager.instance.GetItemList()));
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        File.WriteAllText(FilePath, jdata);
    }
    private void SavePlyerInfo()
    {

    }
 
    void LoadItem()
    {
        //복호화
        string jdata = File.ReadAllText(FilePath);
        // byte[] bytes = System.Convert.FromBase64String(code);
        //string jdata = System.Text.Encoding.UTF8.GetString(bytes);
        PlayerManager.instance.SetItemList();
      
    }

    private void LoadPlayerInfo()
    {

    }

}
