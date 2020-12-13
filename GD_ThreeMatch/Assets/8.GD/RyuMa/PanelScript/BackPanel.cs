using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPanel : Panel
{

    public override void Init(PuzzleSlot _slot, string[] Data)
    {
        base.Init(_slot, Data);

        m_spriteRen[0].sprite = m_sprite[int.Parse(Data[1])];
    }



    public override void CreatBlock(BlockType type, string[] Data)
    {

        Block block = BlockManager.Instance.CreatBlock(type);
        m_Slot.m_Block = block;
        m_Slot.m_Block.Init(m_Slot, Data);
    }


}
