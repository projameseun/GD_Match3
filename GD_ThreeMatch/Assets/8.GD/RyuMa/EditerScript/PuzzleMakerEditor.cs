using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PuzzleMaker))]
public class PuzzleMakerEditor : Editor
{
    PuzzleMaker theMaker;


    string[] Test = { "Test" };


    bool MapSetting = false;
    public void OnEnable()
    {
        theMaker = target as PuzzleMaker;
    }
    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        GUILayout.Space(10f);
        serializedObject.Update();
       
        MapSetting = EditorGUILayout.Toggle("맵 설정", MapSetting);
        if (MapSetting == false)
        {
            EditorGUILayout.BeginHorizontal(); // 가로축 시작
            EditorUtil.DrawLabel("가로 길이", false, GUILayout.Width(60f));
            EditorUtil.DrawProperty("", serializedObject, "m_Horizon", false, GUILayout.Width(50f));
            EditorGUILayout.EndHorizontal(); // 가로축 종료


            EditorGUILayout.BeginHorizontal();
            EditorUtil.DrawLabel("세로 길이", false, GUILayout.Width(60f));
            EditorUtil.DrawProperty("", serializedObject, "m_Vertical", false, GUILayout.Width(50f));
            EditorGUILayout.EndHorizontal();
            if (EditorUtil.DrawButton_Click("맵 세팅 시작", GUILayout.Width(85f)))
            {
                MapSetting = true;
                theMaker.SettingMap();
            }
        }

        GUILayout.Space(10f); // 빈공간 추가
        EditorGUILayout.BeginHorizontal();
        EditorUtil.DrawLabel("맵 이름", false, GUILayout.Width(50f));
        EditorUtil.DrawProperty("", serializedObject, "m_MapName", false, GUILayout.Width(150f));

        if (EditorUtil.DrawButton_Click("맵 불러오기", GUILayout.Width(85f)))
        {

        }
        EditorGUILayout.EndHorizontal();










        serializedObject.ApplyModifiedProperties();
        //
        ////EditorGUILayout.BeginHorizontal();
        //EditorUtil.DrawLabel("STAGE", false, GUILayout.Width(45f));
        //if (EditorUtil.DrawButton_Click("<<", GUILayout.Width(50f)))
        //{
        //    theMaker.SettingMap();
        //}
        //GUILayout.Space(20f);

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
