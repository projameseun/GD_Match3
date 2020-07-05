using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndManager : MonoBehaviour
{
    public GameObject GameEndPanel;

    public Button YesBtn;
    public Button NoBtn;


    public bool GameEndOn = true;

    private GameManager theGM;
    private PuzzleManager thePuzzle;




    private void Start()
    {
        GameEndOn = true;
        theGM = FindObjectOfType<GameManager>();
        thePuzzle = FindObjectOfType<PuzzleManager>();
        if (YesBtn != null)
        {
            YesBtn.onClick.AddListener(() =>
            {
                BT_Yes();
            });
        }
        if (NoBtn != null)
        {
            NoBtn.onClick.AddListener(() =>
            {
                BT_No();
            });
        }
    }

    private void Update()
    {

        if (GameEndOn == true)
        {
            if (theGM.state == GMState.GM00_Title)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CheckEsc();
                }
            }
            else if (theGM.state == GMState.GM00_Tutorial || theGM.state == GMState.GM02_InGame)
            {
                if (thePuzzle.state == PuzzleManager.State.Ready)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        CheckEsc();
                    }
                }
            }
            else if (theGM.state == GMState.GM01_Lobby)
            {

            }
        }


      

        
    }

    public void CheckEsc()
    {
        if (GameEndPanel.activeSelf == false)
        {
            GameEndPanel.SetActive(true);

        }
        else
            GameEndPanel.SetActive(false);
    }



    public void BT_Yes()
    {
        Application.Quit();
    }

    public void BT_No()
    {
        GameEndPanel.SetActive(false);
    }



}
