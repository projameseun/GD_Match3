using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using JetBrains.Annotations;
using UnityEngine.Serialization;
using System;



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
    
    public int Level, Parchment, Luby, Money;
    public int Feather, Coin, Plask;

    public GameObject[] Slot;
    public GameObject LevelTxt, ParchmentTxt, DiamondTxt, MoneyTxt;
    public static ShopManager instance = null;
   
    //30:60
    //1:60
    //Time에대한 변수들
    private float m_Time;
    public float m_WaitTime; //1800 30분
    public int ParchmentThirty;
    public int Second;
    public bool is_Parchment;
    //Time에대한 변수들

    public void Start()
     {
        //초기값설정
        Level = 10;
        Parchment = 0;
        Luby = 100000;
        Money = 100000;

        is_Parchment = true;
        //ParchmentThirty = 30;

        //값테스트
        ValueSetting();
   
        //5개의 메뉴는 모두 끄고 시작한다
         for (int i = 0; i < Slot.Length; i++)
         {
             // Debug.Log(Slot.Length);
             Slot[i].SetActive(false);
         }
      

        m_Time = m_WaitTime;

     }
    void Update()
    {
        //30:60
        //1:60
        if (is_Parchment)
        {
            m_Time -= Time.deltaTime;
            if (m_Time < 0f)
            {
                if (Parchment == 5)
                    is_Parchment = !is_Parchment;

                if (Second <= 0)
                {
                    Second = 10;
                    ParchmentThirty -= 1;
                    Parchment++;

                }// if (Second <= 0)

                //Debug.Log(m_Time.ToString(ParchmentThirty + ":" + Second));
                Second -= 1;
                m_Time = m_WaitTime;
            }// if (m_Time < 0f)
        }// if (is_Parchment)
        ValueSetting();
    }

    public void FreeClick()
     {
        Debug.Log("Free Click");
     }
     public void FeatherClick() //깃털버튼
     {
        if (ShopManager.instance.Luby <= 0)
        {
            Debug.Log("루비가 부족합니다");
            return;
        }
        ShopManager.instance.Luby -= 100;
        instance.Feather++;
     }
   
     public void CoinClick() //도박사의동전버튼
     {
        if (ShopManager.instance.Luby <= 0)
        {
            Debug.Log("루비가 부족합니다");
            return;
        }
        ShopManager.instance.Luby -= 100;
        instance.Coin++;
     }
   
     public void PlaskClick() //용암프라스크버튼
     {
        if (ShopManager.instance.Luby <= 0)
        {
            Debug.Log("루비가 부족합니다");
            return;
        }
        ShopManager.instance.Luby -= 100;
        instance.Plask++;
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
       LevelTxt.GetComponentInChildren<Text>().text = "Lv" + ShopManager.instance.Level.ToString();
        if (is_Parchment) ParchmentTxt.GetComponentInChildren<Text>().text = Parchment.ToString() + "/5\n" + ParchmentThirty + ":" + Second;
        else
        {
            ParchmentTxt.GetComponentInChildren<Text>().fontSize = 60;
            ParchmentTxt.GetComponentInChildren<Text>().text = ShopManager.instance.Parchment.ToString() + "/5\n";
        }
       DiamondTxt.GetComponentInChildren<Text>().text = string.Format("{0}",ShopManager.instance.Luby.ToString("n0"));
       MoneyTxt.GetComponentInChildren<Text>().text = string.Format("{0}",ShopManager.instance.Money.ToString("n0"));
    }


}//  public class ShopManager : MonoBehaviour


