using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//깃털,도박사의동전,플라스크
//양피지, 다이아모드, 머니, 레벨

[System.Serializable]
public class Item
{
    public string Type, Name, Explain, Number, Index;
    public bool isUsing;

    public Item(string type, string name, string explain, string number, string index, bool isUsing)
    {
        Type = type;
        Name = name;
        Explain = explain;
        Number = number;
        Index = index;
        this.isUsing = isUsing;
    }
}

public class ShopManager : MonoBehaviour
{
    public List<Item> ItemList, MyItemList, CurItemList;
    public GameObject[] Slot;

    public void Start()
    {
        for(int i=0; i<Slot.Length; i++)
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
       for(int i=0; i<Slot.Length; i++)
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

}
