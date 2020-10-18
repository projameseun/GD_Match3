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
                MapManager map = PuzzleManager.Instance.theMoveMap;
            });
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
