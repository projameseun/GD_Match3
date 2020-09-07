using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SlotBaseType
{ 
    Block,
    Panel
}


public class SlotEditorBase : MonoBehaviour
{

    public Image[] ImageList;


    [HideInInspector]
    public Block m_Block = null;
    [HideInInspector]
    public Panel m_Panel = null;

    public SlotBaseType BaseType;
    public GameObject m_BaseFrefab;


    private void Awake()
    {

        if (BaseType == SlotBaseType.Block)
        { 
        
        }


    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
