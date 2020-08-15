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





    public override void DestroyCube()
    {
        base.DestroyCube();
        if (specialCubeType == SpecialCubeType.Null)
            return;

        MapManager _Map = null;
        PuzzleManager.Instance.state = PuzzleManager.State.SpecialCubeEvent;

        if (PuzzleManager.Instance.gameMode == PuzzleManager.GameMode.MoveMap)
        {
            _Map = PuzzleManager.Instance.theMoveMap;
        }
        else if (PuzzleManager.Instance.gameMode == PuzzleManager.GameMode.Battle)
        {
            _Map = PuzzleManager.Instance.theBattleMap;
        }

        FindMatches.Instance.SpecialCubeEvent(_Map, Num, specialCubeType);


    }


}
