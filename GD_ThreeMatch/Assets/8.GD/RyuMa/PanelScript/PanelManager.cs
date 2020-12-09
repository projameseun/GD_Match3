using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PanelType
{
    Null = -1,
    PT0_BackPanel,
    PT1_Portal,
    PT2_Wall,

    PT3_Cage,


}




[System.Serializable]
public class PanelList
{
    public PanelType type;
    public GameObject Panel;
}

public class PanelManager : A_Singleton<PanelManager>
{
    public List<PanelList> panelLists = new List<PanelList>();

    public Panel CreatePanel(PanelType _type)
    {
        PanelList _Panel = null;
        _Panel = panelLists.Find(find => find.type == _type);

        return _Panel != null ? ObjectManager.Instance.FindObj<Panel>(_Panel.Panel) : null;
    }

}
