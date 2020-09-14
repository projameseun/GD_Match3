using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PanelType
{ 
    BackPanel,
    Portal
}



public class Panel : MonoBehaviour
{

    public PanelType panelType;

    public List<Sprite> m_sprite;
    public List<SpriteRenderer> m_spriteRen;
    public int m_Count;

    public bool m_Burst;
    public bool m_Destroy;


    virtual public void Init()
    { 
    
    }






}
