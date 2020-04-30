﻿using System.Collections;
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
   
       
        return MyItemlist;
    }

    public List<ItemInfo> SetItemList(List<ItemInfo> a_ItemList)
    {

        MyItemlist = a_ItemList;
        for(int i=0; i<MyItemlist.Count; i++)
        {
           
            if (i == 0) ShopManager.instance.Feather = int.Parse(MyItemlist[i].Value);
            if (i == 1) ShopManager.instance.Coin = int.Parse(MyItemlist[i].Value);
            if (i == 2) ShopManager.instance.Plask = int.Parse(MyItemlist[i].Value);
        }
        return MyItemlist;
    }


    //==========Item
    public void SaveItem(List<ItemInfo> a_ItemList)
    {
        ////끝나면
        MyItemlist = a_ItemList;
        StorageManager.instance.SaveItem();
    }
    public void LoadItem()
    {
        ////끝나면
        
        StorageManager.instance.Load();
        
    }
    //==========Item
    public List<PlayerInfo> GetPlayerList()
    {

        return MyPlayerInfoList;
    }
    public void SavePlayerInfo(List<PlayerInfo> a_PlayerList)
    {
        MyPlayerInfoList = a_PlayerList;
        StorageManager.instance.SavePlyerInfo();
    }


    void Update()
    {
        
    }
}
