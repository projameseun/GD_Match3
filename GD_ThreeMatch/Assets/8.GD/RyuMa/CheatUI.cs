using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatUI : MonoBehaviour
{
    public Button MapLoadButton;
    public Button GoEditorButton;

    FindMatches theMatch;
    PuzzleManager thePuzzle;

    // Start is called before the first frame update
    void Start()
    {
        theMatch = FindMatches.Instance;
        thePuzzle = PuzzleManager.Instance;
        this.gameObject.SetActive(GameManager.Instance.CheatMode ? true : false);

        if (MapLoadButton != null)
        {
            MapLoadButton.onClick.AddListener(() =>
            {
                LoadEditorMap();
            });
        }

        if (GoEditorButton != null)
        {
            GoEditorButton.onClick.AddListener(() =>
            {
                GoEditor();
            });
        }


    }

    // 나중에 맵을 세팅하는 종합세트
    public void LoadEditorMap()
    {
        SaveManager.Instance.SetMap(thePuzzle.GetMap());
    }


    public void GoEditor()
    {
        GameManager.Instance.SceneChange("SonMap",()=> {
            PuzzleMaker.Instance.m_MapName = GameManager.Instance.MapName;
            SaveManager.Instance.LoadMap(GameManager.Instance.MapName);
            SaveManager.Instance.EditorMapSet();
        });
    }


}
