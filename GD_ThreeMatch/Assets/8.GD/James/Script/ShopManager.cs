using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//깃털,도박사의동전,플라스크
//양피지, 다이아모드, 머니, 레벨

[System.Serializable]
public class ItemInfo
{
    public string Type, Name, Explain, Number, Index;
    public bool isUsing;


    public ItemInfo(string type, string name, string explain, string number, bool isUsing, string index)
    {
        Type = type;
        Name = name;
        Explain = explain;
        Number = number;
        this.isUsing = isUsing;
        Index = index;


    }
}

public class ShopManager : MonoBehaviour
{
       public List<ItemInfo> ItemList, MyItemList, CurItemList;
       public GameObject[] Slot;
       public TextAsset UserInfo;
       public GameObject LevelTxt, ParchmentTxt, DiamondTxt, MoneyTxt;
   
       public void Start()
       {
   
       //값테스트
       ValueSetting();
   
           //print(UserInfo);
   
           for (int i = 0; i < Slot.Length; i++)
           {
               // Debug.Log(Slot.Length);
               Slot[i].SetActive(false);
           }
   
           string[] line = UserInfo.text.Substring(0, UserInfo.text.Length - 1).Split('\n');
           for (int i = 0; i < line.Length; i++)
           {
               string[] row = line[i].Split('\t');
   
               ItemList.Add(new ItemInfo(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));
   
           }
   
   
       }
   
       public void FreeClick()
       {
   
       }
       public void FeatherClick() //깃털버튼
       {
   
       }
   
       public void CoinClick() //도박사의동전버튼
       {
   
       }
   
       public void PlaskClick() //용암프라스크버튼
       {
   
       }
   
       public void ItemBuyBtn(int a_idx)
       {
           for (int i = 0; i < Slot.Length; i++)
           {
               if (i == a_idx)
                   Slot[i].SetActive(true);
               else
                   Slot[i].SetActive(false);
           }
       }
   
       public void InfoBtn(int a_idx)   //소녀상세
       {
           for (int i = 0; i < Slot.Length; i++)
           {
               if (i == a_idx)
                   Slot[i].SetActive(true);
               else
                   Slot[i].SetActive(false);
   
           }
       }
   
       public void MainBtn(int a_idx)
       {
           for (int i = 0; i < Slot.Length; i++)
           {
               if (i == a_idx)
                   Slot[i].SetActive(true);
               else
                   Slot[i].SetActive(false);
           }
       }
   
       public void AchivBtn(int a_idx)  //업적
       {
           for (int i = 0; i < Slot.Length; i++)
           {
               if (i == a_idx)
                   Slot[i].SetActive(true);
               else
                   Slot[i].SetActive(false);
           }
       }
   
       public void SetBtn(int a_idx)    //환경설정
       {
           for (int i = 0; i < Slot.Length; i++)
           {
               if (i == a_idx)
                   Slot[i].SetActive(true);
               else
                   Slot[i].SetActive(false);
           }
       }
   
    public void ValueSetting()
    {
       LevelTxt.GetComponentInChildren<Text>().text = "Lv01";
       ParchmentTxt.GetComponentInChildren<Text>().text = "0/0";
       DiamondTxt.GetComponentInChildren<Text>().text = "10,000";
       MoneyTxt.GetComponentInChildren<Text>().text = "100,0000";
    }

}//  public class ShopManager : MonoBehaviour


