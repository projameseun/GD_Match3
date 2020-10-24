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
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.EndHorizontal();
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
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorUtil.DrawLabel("맵 이름", false, GUILayout.Width(50f));
            EditorUtil.DrawProperty("", serializedObject, "m_MapName", false, GUILayout.Width(150f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (EditorUtil.DrawButton_Click("맵 저장", GUILayout.Width(85f)))
            {
                SaveManager.Instance.SaveMap(PuzzleMaker.Instance.m_MapName);
                GameManager.Instance.MapName = PuzzleMaker.Instance.m_MapName;
            }
            if (EditorUtil.DrawButton_Click("맵 로드", GUILayout.Width(85f)))
            {
                SaveManager.Instance.LoadMap(PuzzleMaker.Instance.m_MapName);
                SaveManager.Instance.EditorMapSet();
            }
            if (EditorUtil.DrawButton_Click("맵 실행", GUILayout.Width(85f)))
            {
                SaveManager.Instance.SaveMap(PuzzleMaker.Instance.m_MapName);
                GameManager.Instance.MapName = PuzzleMaker.Instance.m_MapName;
                GameManager.Instance.CheatMode = true;
                Debug.Log("map name = " + GameManager.Instance.MapName);
                GameManager.Instance.SceneChange("Ingame",null);
                SaveManager.Instance.LoadMap(GameManager.Instance.MapName);

            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10f); // 빈공간 추가
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
            PanelCheck();

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
        GUILayout.Space(10f);
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
            theMaker.SlotData = new string[] { "0", ((int)theMaker.m_NodeColor).ToString() };



        }
    }

    public void PanelCheck()
    {
        GUILayout.Space(10f);
        // 배경 블럭
        if (theMaker.m_BackPanelCh == true)
        {


            EditorUtil.DrawLabel("------배경 판넬------", false, GUILayout.Width(200f));
            EditorUtil.DrawVariable_Field<PuzzleMaker>("이미지 번호", theMaker, "m_Count", true);

            theMaker.SlotData = new string[] { "0", theMaker.m_Count.ToString() };

        }
        else if (theMaker.m_PortalCh == true)
        {
            EditorUtil.DrawLabel("------포탈 판넬------", false, GUILayout.Width(200f));
            EditorGUILayout.BeginHorizontal();
      
            EditorUtil.DrawLabel("이동 맵 이름", false, GUILayout.Width(120f));
            EditorUtil.DrawProperty("", serializedObject, "m_Data1", true);
            EditorGUILayout.EndHorizontal();
            EditorUtil.DrawVariable_Field<PuzzleMaker>("인덱스 번호", theMaker, "m_Count", true);

            theMaker.SlotData = new string[] { "1", theMaker.m_Data1, theMaker.m_Count.ToString() };

        }
    }


}
