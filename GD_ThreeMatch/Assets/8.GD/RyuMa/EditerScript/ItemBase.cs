using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class ItemBase : MonoBehaviour, IPointerDownHandler
{

    public Image m_ItemImage;
    public Image SelectImage;

    public int Num;
    public SlotBaseType BaseType;



    public void OnPointerDown(PointerEventData eventData)
    {
        if (BaseType == SlotBaseType.Null)
            return;
        SlotEditorBase.Instance.SelectItem(Num, BaseType);
    }

    // 처음 초기화
    public void SetItem(Sprite _sprite, int _Num , SlotBaseType _Type)
    {
        SelectImage.enabled = _Type == SlotBaseType.Null ? false : true;
        m_ItemImage.enabled = _Type == SlotBaseType.Null ? false : true;
        m_ItemImage.sprite = _sprite;
        SelectImage.color = new Color(1, 1, 1);
        Num = _Num;
        BaseType = _Type;
    }

    public void ItemOnOff(bool _OnOff)
    {
        SelectImage.color = _OnOff ? new Color(0, 0, 0) : new Color(1, 1, 1);

    }

    

}
