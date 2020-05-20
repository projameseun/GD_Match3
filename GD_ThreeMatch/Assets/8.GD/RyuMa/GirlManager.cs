﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


[System.Serializable]
public class GirlHero
{
    public string Name;

    [Space]
    [Header("Spine")]
    public Material[] IllustMaterials;
    public SkeletonDataAsset[] IllustData;
    public Sprite SkillImage;
    public float[] IllustPosX;
    public float IllustPosY;
    public float IllustSize;

    [Space]
    [Header("DB")]

    public NodeColor nodeColor;
    public float Hp;
    public float SkillCount;
    public float SkillDamage;
    public int Level;
    public int Exp;




}



public class GirlManager : MonoBehaviour
{

    public GirlHero[] Girls;

    //UI,오브젝트
    public GameObject IllustUISlot;


    private PuzzleManager thePuzzle;
    private void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
    }




    public void SetIngameUI()
    { 
        
    
    }





}
