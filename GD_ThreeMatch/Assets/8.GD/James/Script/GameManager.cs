using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class GameManager : MonoBehaviour
{
    public GameObject FeatherTxt,CoinTxt,PlaskTxt;
    public TextAsset ItemBase, PlayerInfo;
    private List<ItemInfo> SetIemlist = new List<ItemInfo>();
    private void Awake()
    {
        StorageManager.instance = new StorageManager();
        ShopManager.instance    = new ShopManager();
        PlayerManager.instance  =  new PlayerManager();
    }
    private void Start()
    {

        string[] line = ItemBase.text.Substring(0, ItemBase.text.Length - 1).Split('\n');

        // Debug.Log(line.Length);
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //Debug.Log(row.Length);

            SetIemlist.Add(new ItemInfo(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));

        }

        PlayerManager.instance.AddItem(SetIemlist);
        // PlayerManager.instance.AddItem("BBB");

       // StorageManager.instance.Save();
    }

    public void SaveBtn()
    {

       // PlayerManager.instance.AddItem();
    }

    public void LoadBtn()
    {

    }
}
