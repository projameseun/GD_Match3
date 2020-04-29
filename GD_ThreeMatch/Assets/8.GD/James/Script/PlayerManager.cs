using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private List<ItemInfo> MyItemlist = new List<ItemInfo>();
    static public PlayerManager instance;
   // private ItemInfo data ;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public List<ItemInfo> GetItemList()
    {
        return MyItemlist;
    }
    public void AddItem(string a_ItemName)
    {
        //끝나면
        ItemInfo data ;
        data = new ItemInfo();
        data.Type = a_ItemName;

        MyItemlist.Add(data);
        StorageManager.instance.Save();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
