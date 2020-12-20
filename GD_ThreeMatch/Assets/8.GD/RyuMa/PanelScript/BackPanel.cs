﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackPanel : Panel
{

    public override void Init(PuzzleSlot _slot, string[] Data)
    {
        base.Init(_slot, Data);

        m_spriteRen[0].sprite = m_sprite[int.Parse(Data[1])];

        panelType = PanelType.PT0_BackPanel;
    }


    public override void CreatBlock(BlockType type, string[] Data, bool Event = true)
    {
        base.CreatBlock(type, Data, Event);
    }



}
