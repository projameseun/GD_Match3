using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPanel : Panel
{

    public override void Init()
    {
        base.Init();

        m_spriteRen[0].sprite = m_sprite[m_Count];

    }


}
