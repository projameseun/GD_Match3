using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PanelType
{ 
    NULL,
    Portal
}



public class Panel : MonoBehaviour
{

    public PanelType panelType;

    public List<SpriteRenderer> m_spriteRen;
    int Num;


    public bool Defense;




    virtual public void Init()
    { 
    
    }






}
