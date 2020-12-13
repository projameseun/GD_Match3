using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PuzzleMaker))]
public class PuzzleMakerEditor : Editor
{
    PuzzleMaker theMaker;
    string[] setname = { "World", "Battle" };


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

        //시작지점 지우면 안됨
        serializedObject.Update();
       
        MapSetting = EditorGUILayout.Toggle("맵 설정", MapSetting);

        theMaker.m_MapType = GUILayout.Toolbar(theMaker.m_MapType, setname, new GUILayoutOption[] { GUILayout.Width(150) });
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
            EditorUtil.DrawVariable_Field<PuzzleMaker>("도착 인덱스 번호", theMaker, "StartIndex", true);
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
                GameManager.Instance.StartIndex = PuzzleMaker.Instance.StartIndex;
                GameManager.Instance.CheatMode = true;
                GameManager.Instance.SceneChange("Ingame",()=> {
                    SaveManager.Instance.LoadMap(GameManager.Instance.MapName);
                });

            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10f); // 빈공간 추가


        if (EditorUtil.DrawButton_Click("인덱스 표시", GUILayout.Width(85f)))
        {
            theMaker.ShowIndex();
        }

        if (theMaker.MakerStart == true)
        {
            EditorGUILayout.BeginVertical(EditorStyles.textArea);
            //블럭 체크
            BlockCheck();
            PanelCheck();
            EditorGUILayout.EndVertical();
        }






        if (theMaker.MakerStart == true)
        {
            EditorUtil.DrawLabel("Appear Color", false, GUILayout.Width(120f));

            if (theMaker.m_CubeSpawn != null && theMaker.m_CubeSpawn.Length != 0)
            {
                for (int i = 0; i < MatchBase.ColorKinds; i++)
                {
                    if(i % 2 == 0)
                        EditorGUILayout.BeginHorizontal();
                    theMaker.m_CubeSpawn[i] = EditorGUILayout.Toggle(((NodeColor)i).ToString(), theMaker.m_CubeSpawn[i]);

                    if (i % 2 == 1 || i == MatchBase.ColorKinds - 1)
                        EditorGUILayout.EndHorizontal();



                }
            }

        }


        //마지막 끝나는 지점 지우면 안됨
        serializedObject.ApplyModifiedProperties();


    }


    public void BlockCheck()
    {
        //기본 블럭
        if (theMaker.m_CubeCh == true)
        {
            EditorUtil.DrawLabel("------큐브------", false, GUILayout.Width(200f));
            EditorUtil.DrawVariable_Field<PuzzleMaker>("큐브 색", theMaker, "m_NodeColor", true);
            if (theMaker.m_NodeColor == NodeColor.NC6_Null)
            {
                theMaker.m_NodeColor = NodeColor.NC5_Random;
            }
            theMaker.SlotData = new string[] { "0", ((int)theMaker.m_NodeColor).ToString() };



        }
        else if (theMaker.m_RockCh == true)
        {
            EditorUtil.DrawLabel("------바위------", false, GUILayout.Width(200f));
            EditorUtil.DrawVariable_Field<PuzzleMaker>("바위 카운트", theMaker, "m_Count", true);

            theMaker.SlotData = new string[] { "3", theMaker.m_Count.ToString() };
        }
    }

    public void PanelCheck()
    {
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
            EditorUtil.DrawVariable_Field<PuzzleMaker>("도착 인덱스 번호", theMaker, "m_Count", true);

            theMaker.SlotData = new string[] { "1", theMaker.m_Data1, theMaker.m_Count.ToString() };
        }
        else if (theMaker.m_WallCh)
        {
            EditorUtil.DrawLabel("------ 벽 판넬 ------", false, GUILayout.Width(200f));
            EditorUtil.DrawVariable_Field<PuzzleMaker>("바위 카운트", theMaker, "m_Count", true);

            theMaker.SlotData = new string[] { "2", theMaker.m_Count.ToString() };
        }
        else if (theMaker.m_CageCh)
        {
            EditorUtil.DrawLabel("------ 감옥 판넬 ------", false, GUILayout.Width(200f));
            EditorUtil.DrawVariable_Field<PuzzleMaker>("감옥 카운트", theMaker, "m_Count", true);

            theMaker.SlotData = new string[] { "3", theMaker.m_Count.ToString() };
        }

    }


}
