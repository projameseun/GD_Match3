using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public  class StorageManager : MonoBehaviour
{
   
    public static StorageManager instance = null;


    string FilePath = Application.persistentDataPath + "/MyItem.txt";

    void Start()
    {
        //경로
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        //리스트는 저장이 안되지만 크랠스는 저장이된다.
        string jdata = JsonUtility.ToJson(new Serialization<ItemInfo>(PlayerManager.instance.GetItemList()));
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        File.WriteAllText(FilePath, jdata);
    }

    
}
