using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UseItemType
{ 
    Null,


}


[System.Serializable]
public class UseItem
{
    public string ItemName;
    public Sprite ItemImage;
    public UseItemType itemType;
   

}





public class ItemManager : MonoBehaviour
{
    public UseItem[] useItems;







}
