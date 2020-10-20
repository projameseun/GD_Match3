using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatUI : MonoBehaviour
{
    public Button MapLoadButton;


    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(GameManager.Instance.CheatMode ? true : false);

        if (MapLoadButton != null)
        {
            MapLoadButton.onClick.AddListener(() =>
            {
                LoadEditorMap();
            });
        }

    }


    public void LoadEditorMap()
    {
        SaveManager.Instance.SetMap(PuzzleManager.Instance.theMoveMap);



    }


}
