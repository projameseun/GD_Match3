using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PanelType
{ 
    Null = 0,
    BackPanel,
    Portal
}



public class Panel : MonoBehaviour
{

    public PanelType panelType;

    public NodeColor nodeColor;


    public List<Sprite> m_sprite;
    public List<SpriteRenderer> m_spriteRen;
    public int m_Count;

    public bool m_Burst;
    public bool m_Destroy;


    [HideInInspector]
    public int m_Value;


    virtual public void Init()
    { 
    
    }





    virtual public void Resetting()
    {
        ObjectManager.Instance.ResetObj(this.gameObject);
    }




}
