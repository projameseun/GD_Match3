using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PuzzleMaker))]
public class PuzzleMakerEditor : Editor
{
    PuzzleMaker theMaker;

    public void OnEnable()
    {
        theMaker = target as PuzzleMaker;
    }

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //    serializedObject.Update();

    //    EditorGUILayout.BeginVertical(EditorStyles.textArea);
    //    GUILayout.Label("한개의 아틀라스(메테리얼)로 출력.");
    //    theMaker.Test = EditorGUILayout.Toggle("One_MatSystem", theMaker.Test);
    //    GUILayout.Label("★각 파티클의 Textures Sheet Animation의 X,Y값 세팅 필요★");
    //    if (theMaker.Test)
    //    {
    //        EditorGUILayout.BeginVertical(EditorStyles.textArea);
    //        GUILayout.Label("전체 갯수");
    //        EditorGUI.indentLevel++;
    //        theMaker.PlayerStartNum = EditorGUILayout.IntField("ImageTotalCnt", theMaker.PlayerStartNum);
    //        EditorGUI.indentLevel--;
    //        EditorGUILayout.EndVertical();
    //    }
    //}


}
