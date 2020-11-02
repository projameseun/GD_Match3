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
        base.CreatBlock(type, Data);

        BlockType blocktype = BlockType.BT0_Cube;

        Block block = BlockManager.Instance.CreatBlock(blocktype);
        m_Slot.m_Block = block;
        m_Slot.m_Block.Init(m_Slot, null);
    }


}
