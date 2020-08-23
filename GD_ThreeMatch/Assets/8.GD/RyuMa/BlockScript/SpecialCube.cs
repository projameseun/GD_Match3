﻿using System.Collections;
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
    public float DelayTime;

    public override void ChangeState(int _Num)
    {
        specialCubeType = (SpecialCubeType)_Num;
    }


    public override void BurstEvent(MapManager _map, int _num)
    {
        //base.BurstEvent(_map, _num);

        if (specialCubeType != SpecialCubeType.Null)
        {
            PuzzleManager.Instance.EventUpdate();
        }


        switch (specialCubeType)
        {
            case SpecialCubeType.Horizon:
                HorizonEvent(_map, _num);
                break;
            case SpecialCubeType.Vertical:
                VerticalEvent(_map, _num);
                break;
            case SpecialCubeType.Diagonal:
                HorizonEvent(_map, _num);
                break;
        }

    }

    public void HorizonEvent(MapManager _Map, int _SlotNum)
    {

        int HorizonNum = 0;


        specialCubeType = SpecialCubeType.Null;

        for (int i = 0; i < GameManager.Instance.MaxVertical; i++)
        {
            if (_SlotNum < i * GameManager.Instance.MaxHorizon)
            {
                HorizonNum = (i - 1) * GameManager.Instance.MaxHorizon;
                break;
            }
        }

        for (int i = HorizonNum; i < HorizonNum + GameManager.Instance.MaxHorizon; i++)
        {
            if (i == _SlotNum)
                continue;
            if (_Map.Slots[i].block.blockType == BlockType.SpecialCube)
            {
                if (_Map.Slots[i].block.GetComponent<SpecialCube>().specialCubeType == SpecialCubeType.Horizon)
                {
                    _Map.Slots[i].block.ChangeState(-1);
                }
            }

            _Map.Slots[i].BlockBurst(DelayTime);

        }


    }


    public void VerticalEvent(MapManager _Map, int _SlotNum)
    {
        int Vertical = _SlotNum % GameManager.Instance.MaxHorizon;
        specialCubeType = SpecialCubeType.Null;


        for (int i = Vertical; i < _Map.BottomLeft; i += GameManager.Instance.MaxHorizon)
        {
            if (i == _SlotNum)
                continue;
            if (_Map.Slots[i].block.blockType == BlockType.SpecialCube)
            {
                if (_Map.Slots[i].block.GetComponent<SpecialCube>().specialCubeType == SpecialCubeType.Horizon)
                {
                    _Map.Slots[i].block.ChangeState(-1);
                }
            }

            _Map.Slots[i].BlockBurst(DelayTime);

        }
    }




    public void DiagonalEvent(MapManager _Map, int _SlotNum)
    {

        int CheckCount = 1;


        specialCubeType = SpecialCubeType.Null;



        // 11시 방향 확인
        while (true)
        {
            int Count = _SlotNum - ((GameManager.Instance.MaxHorizon + 1) * CheckCount);

            if (Count < _Map.TopRight ||
                Count % GameManager.Instance.MaxHorizon == 0)
            {
                break;
            }
            if (_Map.Slots[Count].block.blockType == BlockType.SpecialCube)
            {
                if (_Map.Slots[Count].block.GetComponent<SpecialCube>().specialCubeType == SpecialCubeType.Horizon)
                {
                    _Map.Slots[Count].block.ChangeState(-1);
                }
            }
            _Map.Slots[Count].BlockBurst(DelayTime);
            CheckCount++;

        }


        CheckCount = 1;
        // 1시 방향 확인
        while (true)
        {

            int Count = _SlotNum - ((GameManager.Instance.MaxHorizon - 1) * CheckCount);

            if (Count <= _Map.TopRight ||
                Count % GameManager.Instance.MaxHorizon == GameManager.Instance.MaxHorizon - 1)
            {
                break;
            }

            if (_Map.Slots[Count].block.blockType == BlockType.SpecialCube)
            {
                if (_Map.Slots[Count].block.GetComponent<SpecialCube>().specialCubeType == SpecialCubeType.Horizon)
                {
                    _Map.Slots[Count].block.ChangeState(-1);
                }
            }
            _Map.Slots[Count].BlockBurst(DelayTime);
            CheckCount++;
        }

        CheckCount = 1;

        // 7시 방향 확인
        while (true)
        {
            int Count = _SlotNum + ((GameManager.Instance.MaxHorizon - 1) * CheckCount);

            if (Count >= _Map.BottomLeft ||
                Count % GameManager.Instance.MaxHorizon == 0)
            {
                break;
            }

            if (_Map.Slots[Count].block.blockType == BlockType.SpecialCube)
            {
                if (_Map.Slots[Count].block.GetComponent<SpecialCube>().specialCubeType == SpecialCubeType.Horizon)
                {
                    _Map.Slots[Count].block.ChangeState(-1);
                }
            }
            _Map.Slots[Count].BlockBurst(DelayTime);
            CheckCount++;
        }


        CheckCount = 1;

        // 5시 방향
        while (true)
        {
            int Count = _SlotNum + ((GameManager.Instance.MaxHorizon + 1) * CheckCount);

            if (Count >= _Map.BottomLeft ||
                Count % GameManager.Instance.MaxHorizon == GameManager.Instance.MaxHorizon - 1)
            {
                break;
            }

            if (_Map.Slots[Count].block.blockType == BlockType.SpecialCube)
            {
                if (_Map.Slots[Count].block.GetComponent<SpecialCube>().specialCubeType == SpecialCubeType.Horizon)
                {
                    _Map.Slots[Count].block.ChangeState(-1);
                }
            }
            _Map.Slots[Count].BlockBurst(DelayTime);
            CheckCount++;
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
