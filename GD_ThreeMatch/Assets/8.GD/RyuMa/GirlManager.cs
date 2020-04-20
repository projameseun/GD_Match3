using System.Collections;
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
    public float[] IllustPos;

    [Space]
    [Header("DB")]

    public NodeColor nodeColor;
    public float Hp;
    public float SkillCount;
    public int Level;
    public int Exp;




}



public class GirlManager : MonoBehaviour
{

    public GirlHero[] Girls;

}
