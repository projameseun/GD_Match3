using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Blank = -2,
    Null = -1,
    BT0_Cube,
    BT1_SpecialCube,
    BT2_Player,


}


[System.Serializable]
public class BlockList
{
    public BlockType type;
    public GameObject Block;
}


public class BlockManager : A_Singleton<BlockManager>
{
    public List<BlockList> blockLists = new List<BlockList>();




    public Block CreatBlock(BlockType _type)
    {
        BlockList _Block = null;
        _Block = blockLists.Find(find => find.type == _type);

        
        return _Block != null? ObjectManager.Instance.FindObj<Block>(_Block.Block) : null;
        
    }



}
