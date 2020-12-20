using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;





public class Panel : MonoBehaviour
{

    public PanelType panelType;
    public NodeColor nodeColor;

    [HideInInspector] public PuzzleSlot m_Slot;


    public List<Sprite> m_sprite;
    public List<SpriteRenderer> m_spriteRen;


    public int m_Count;


    public bool m_Exist = true;         // 블럭이 들어갈 수 있는지

    public bool m_Switch = true;        // 판넬에 있는 블럭의 스위치가 가능한지

    public bool m_Gravity = true;       // 판넬 안에있는 블럭이 중력의 영향을 받는지

    public bool m_Match = true;         // 판넬 안에 있는 블럭이 메치가 가능한지

    public bool m_BlockBurst = true;    // 판넬 안 블럭이 터지는 이밴트 영향을 받는지

    public bool m_AroundBurst;          // 주변 버스트에 영향을 받는지

    public bool m_PanelBurst = true;    // 판넬이 터지는 이밴트에 영향을 받는지
    // 판넬이 파괴가 가능한지
    public bool m_Destroy;

    // 판넬에 있는 블럭의 스위치가 가능한지






    virtual public void Init(PuzzleSlot _slot,string[] Data)
    {
        this.transform.position = _slot.transform.position;
        m_Slot = _slot;



    }


    public void SetColor(NodeColor _color)
    {
        nodeColor = _color;
        if (_color == NodeColor.NC6_Null)
            return;

        m_spriteRen[0].sprite = m_sprite[(int)_color];
    }

    public void SetRandomColor()
    {

    }





    virtual public void CreatBlock(BlockType type = BlockType.Null, string[] Data= null, bool Event = true)
    {
        Block block = null;

        if (PuzzleManager.Instance.m_EditSeed.Count > 0)
        {
            int Count = PuzzleManager.Instance.m_EditSeed.Count - 1;


            block = BlockManager.Instance.CreatBlock(PuzzleManager.Instance.m_EditSeed[Count].type);
            m_Slot.m_Block = block;
            m_Slot.m_Block.Init(m_Slot, PuzzleManager.Instance.m_EditSeed[Count].Data);
            PuzzleManager.Instance.m_EditSeed.RemoveAt(Count);
            return;
        }

        if (type != BlockType.Null)
        {
            block = BlockManager.Instance.CreatBlock(type);
            m_Slot.m_Block = block;
            m_Slot.m_Block.Init(m_Slot, Data);
        }
        else
        {
            int RandomBlock = UnityEngine.Random.Range(0, PuzzleManager.Instance.m_BlockSeed.Count);

            block = BlockManager.Instance.CreatBlock(PuzzleManager.Instance.m_BlockSeed[RandomBlock].type);
            m_Slot.m_Block = block;
            m_Slot.m_Block.Init(m_Slot, PuzzleManager.Instance.m_BlockSeed[RandomBlock].Data);
        }
    }



    virtual public void BurstEvent(PuzzleSlot slot,Action action = null)
    {


    }


    virtual public void Resetting()
    {
        m_Slot.DestroyPanel(this);
        ObjectManager.Instance.ResetObj(this.gameObject);
    }




}
