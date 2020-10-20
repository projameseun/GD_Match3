using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockList
{
    public BlockType type;
    public GameObject Block;
}


public class BlockManager : A_Singleton<BlockManager>
{
    public List<BlockList> blockLists = new List<BlockList>();




    public GameObject CreatBlock(BlockType _type)
    {
        BlockList _Block = null;
        _Block = blockLists.Find(find => find.type == _type);

        
        return _Block != null? ObjectManager.Instance.FindObj(_Block.Block) : null;
        
    }



}
