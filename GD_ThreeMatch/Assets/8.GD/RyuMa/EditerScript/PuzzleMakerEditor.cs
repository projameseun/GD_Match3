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
            EditorUtil.DrawLabel("맵 이름", false, GUILayout.Width(50f));
            EditorUtil.DrawProperty("", serializedObject, "m_MapName", false, GUILayout.Width(150f));
            EditorGUILayout.EndHorizontal(); // 가로축 종료


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
            EditorGUILayout.BeginHorizontal(); // 가로축 시작



            EditorGUILayout.EndHorizontal(); // 가로축 종료
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorUtil.DrawLabel("---------------------", false, GUILayout.Width(200f));
        EditorGUILayout.EndHorizontal();

        if (EditorUtil.DrawButton_Click("인덱스 표시", GUILayout.Width(85f)))
        {
            theMaker.ShowIndex();




        }

        if (theMaker.MakerStart == true)
        {
            //블럭 체크
            BlockCheck();

        }
            
        







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


    public void BlockCheck()
    {

       

        //기본 블럭
        if (theMaker.m_CubeCh == true)
        {
            GUILayout.Space(10f);


            EditorUtil.DrawLabel("------큐브------", false, GUILayout.Width(200f));
            EditorUtil.DrawVariable_Field<PuzzleMaker>("큐브 색", theMaker, "m_NodeColor", true);
            if (theMaker.m_NodeColor == NodeColor.NC6_Null)
            {
                theMaker.m_NodeColor = NodeColor.NC5_Random;
            }
        }
    }

    public void PanelCheck()
    {
        // 배경 블럭
        if (theMaker.m_BackPanelCh == true)
        {
            GUILayout.Space(10f);


            EditorUtil.DrawLabel("------배경 판넬------", false, GUILayout.Width(200f));
            EditorUtil.DrawVariable_Field<PuzzleMaker>("큐브 색", theMaker, "m_NodeColor", true);
            if (theMaker.m_NodeColor == NodeColor.NC6_Null)
            {
                theMaker.m_NodeColor = NodeColor.NC5_Random;
            }
        }
    }


}
