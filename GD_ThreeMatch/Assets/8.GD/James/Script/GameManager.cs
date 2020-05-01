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
    private List<PlayerInfo> SetPlayerlist = new List<PlayerInfo>();
    private void Awake()
    {
        
        StorageManager.instance = new StorageManager();
        ShopManager.instance    = new ShopManager();
        PlayerManager.instance  =  new PlayerManager();
    }
    private void Start()
    {
        InitSetting();
    }

    void Update()
    {
        TestItemValueText();
    }

    public void SaveBtn()
    {

        //Item Save
        for(int i=0; i<SetIemlist.Count; i++)
        {
            if (i == 0) SetIemlist[i].Value = ShopManager.instance.Feather.ToString();
            if (i == 1) SetIemlist[i].Value = ShopManager.instance.Coin.ToString();
            if (i == 2) SetIemlist[i].Value = ShopManager.instance.Plask.ToString();
        }

        //PlayerInfo Save
        for (int i = 0; i < SetPlayerlist.Count; i++)
        {
           
            // Debug.Log("플레이어 리스트저ㅓ장");
            if (i == 0) SetPlayerlist[i].Value = ShopManager.instance.Level.ToString();
            if (i == 1) SetPlayerlist[i].Value = ShopManager.instance.Parchment.ToString();
            if (i == 2) SetPlayerlist[i].Value = ShopManager.instance.Luby.ToString();
            if (i == 3) SetPlayerlist[i].Value = ShopManager.instance.Money.ToString();
        }

        PlayerManager.instance.SavePlayerInfo(SetPlayerlist);
        PlayerManager.instance.SaveItem(SetIemlist);

        //PlayerSave

       
    }

    public void LoadBtn()
    {
        PlayerManager.instance.LoadItem();
        PlayerManager.instance.LoadPlayerInfo();
    }


    public void TestItemValueText()
    {
       
        FeatherTxt.GetComponent<Text>().text = "깃털:" + ShopManager.instance.Feather;
        CoinTxt.GetComponent<Text>().text = "코인:" + ShopManager.instance.Coin;
        PlaskTxt.GetComponent<Text>().text = "플라스크:" + ShopManager.instance.Plask;
    }

    public void InitSetting()
    {
        //아이템의정보
        string[] line = ItemBase.text.Substring(0, ItemBase.text.Length - 1).Split('\n');

        // Debug.Log(line.Length);
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //Debug.Log(row.Length);

            SetIemlist.Add(new ItemInfo(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));

        }

        //플레이어의 정보
        string[] line2 = PlayerInfo.text.Substring(0, PlayerInfo.text.Length - 1).Split('\n');

        // Debug.Log(line.Length);
        for (int i = 0; i < line2.Length; i++)
        {
            string[] row = line2[i].Split('\t');
            //Debug.Log(row.Length);

            SetPlayerlist.Add(new PlayerInfo(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));

        }
        //Debug.Log(SetPlayerlist.Count);
        //플레이어의 정보
    }
}
