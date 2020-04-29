using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerInfoSerialization<T>
{

    public PlayerInfoSerialization(List<T> _target) => target = _target;

    public List<T> target;
}

[System.Serializable]
public class PlayerInfo
{
    public string Type, Name, Explain, Value, Index;
    public bool isUsing;


    public PlayerInfo(string type, string name, string explain, string value, bool isUsing, string index)
    {
        Type = type;
        Name = name;
        Explain = explain;
        Value = value;
        this.isUsing = isUsing;
        Index = index;

    }
    public PlayerInfo() { }
}//public class ItemInfo

public class PlayerManager : MonoBehaviour
{
    static public PlayerManager instance;

    private List<PlayerInfo> MyPlayerInfoList = new List<PlayerInfo>();
    private List<ItemInfo> MyItemlist = new List<ItemInfo>();



    // private ItemInfo data ;
    // Start is called before the first frame update
    void Start()
    {
        


    }
    public List<ItemInfo> GetItemList()
    {
        //Debug.Log(MyItemlist.Count);
       
        return MyItemlist;
    }

    public List<PlayerInfo> GetPlayerList()
    {
        return MyPlayerInfoList;
    }
    public void AddItem(List<ItemInfo> a_ItemList)
    {
        ////끝나면
        //ItemInfo data = new ItemInfo();
        //data.Type = a_ItemName;

        MyItemlist = a_ItemList;
        // MyItemlist.Add(data);

        
        


        StorageManager.instance.Save();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
