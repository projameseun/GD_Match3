using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPanel : Panel
{

    public string PortalMapName;



    public override void Init(PuzzleSlot _slot, string[] Data)
    {
        base.Init(_slot, Data);

        PortalMapName = Data[1];
        m_Count = int.Parse(Data[2]);
    }




}
