using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using JetBrains.Annotations;

//깃털,도박사의동전,플라스크
//양피지, 다이아모드, 머니, 레벨

[System.Serializable]
 public class ItemSerialization<T>
{
    
    public ItemSerialization(List<T> _target) => target = _target;

    public List<T> target;
}

[System.Serializable]
public class ItemInfo
{
    public string Type, Name, Explain, Value, Index;
    public bool isUsing;


    public ItemInfo(string type, string name, string explain, string value, bool isUsing, string index)
    {
        Type = type;
        Name = name;
        Explain = explain;
        Value = value;
        this.isUsing = isUsing;
        Index = index;


    }
    public ItemInfo() { }
}//public class ItemInfo

public class ShopManager : MonoBehaviour
{
     public GameObject[] Slot;
    
    public GameObject LevelTxt, ParchmentTxt, DiamondTxt, MoneyTxt;
                       

    public static ShopManager instance = null;


    public void Start()
     {
      
       //값테스트
       ValueSetting();
   
        //5개의 메뉴는 모두 끄고 시작한다
         for (int i = 0; i < Slot.Length; i++)
         {
             // Debug.Log(Slot.Length);
             Slot[i].SetActive(false);
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


