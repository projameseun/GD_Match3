using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpecialCubeType
{
    Null = -1,
    Vertical = 0,
    Horizon,
    Diagonal,
}
public class SpecialCube : Block
{
    public SpecialCubeType specialCubeType;



    public override void BurstEvent(MapManager _map, int _num, float DelayTime = 0)
    {
        //base.BurstEvent(_map, _num);

        switch (specialCubeType)
        {
            case SpecialCubeType.Horizon:
                HorizonEvent(_map, _num);
                break;
            case SpecialCubeType.Vertical:
                HorizonEvent(_map, _num);
                break;
            case SpecialCubeType.Diagonal:
                HorizonEvent(_map, _num);
                break;
        }

    }

    public void HorizonEvent(MapManager _Map, int _SlotNum)
    {

        int HorizonNum = 0;


        _Map.Slots[_SlotNum].block.GetComponent<SpecialCube>().specialCubeType = SpecialCubeType.Null;

        for (int i = 0; i < _Map.Vertical; i++)
        {
            if (_SlotNum < i * _Map.Horizontal)
            {
                HorizonNum = (i - 1) * GameManager.Instance.MaxHorizon;
                break;
            }
        }

        for (int i = HorizonNum; i < HorizonNum + _Map.Horizontal; i++)
        {
            if (_Map.Slots[i].block.Burst == false)
            {
                if (i == _SlotNum)
                    continue;

                if (_Map.Slots[i].block.blockType == BlockType.SpecialCube)
                {
                    if(_Map.Slots[i].block.GetComponent<SpecialCube>().specialCubeType != SpecialCubeType.Horizon)
                    {
                        CheckBoom = true;
                    }
                }



            }
        }

        for (int i = HorizonNum; i < HorizonNum + _Map.Horizontal; i++)
        {
            if (_Map.Slots[i].nodeColor != NodeColor.NC6_Player &&
                _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
                _Map.Slots[i].nodeColor != NodeColor.NC5_Blank)
            {
                if (_Map.Slots[i].cube.specialCubeType == SpecialCubeType.Horizon)
                    _Map.Slots[i].cube.specialCubeType = SpecialCubeType.Null;
                _Map.Slots[i].cube.DestroyCube(true, true);

            }
        }


    }






    //public override void DestroyCube()
    //{
    //    base.DestroyCube();
    //    if (specialCubeType == SpecialCubeType.Null)
    //        return;

    //    MapManager _Map = null;
    //    PuzzleManager.Instance.state = PuzzleManager.State.SpecialCubeEvent;

    //    if (PuzzleManager.Instance.gameMode == PuzzleManager.GameMode.MoveMap)
    //    {
    //        _Map = PuzzleManager.Instance.theMoveMap;
    //    }
    //    else if (PuzzleManager.Instance.gameMode == PuzzleManager.GameMode.Battle)
    //    {
    //        _Map = PuzzleManager.Instance.theBattleMap;
    //    }

    //    FindMatches.Instance.SpecialCubeEvent(_Map, Num, specialCubeType);


    //}


}
