using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Animator TitleAnim;



    bool TouchOnOff;

    private GameManager theGM;
    private FadeManager theFade;
    private SoundManager theSound;
    private PuzzleManager thePuzzle;
    // Start is called before the first frame update
    void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theSound = FindObjectOfType<SoundManager>();
        theGM = FindObjectOfType<GameManager>();
        theFade = FindObjectOfType<FadeManager>();
        TitleAnim.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (TouchOnOff == true)
                return;
            theSound.SEValue = 1;
            theSound.PlaySE("ButtonSE");
            TouchOnOff = true;
            TitleAnim.Play("Close");
            theSound.FadeOutBGM();
            Invoke("FadeOutInvoke", 0.5f);
            thePuzzle.SetPlayerUi();

        });
        theSound.PlayBGM("TitleBGM");
    }

    public void ReturnTitle()
    {
        TouchOnOff = false;
        TitleAnim.gameObject.SetActive(true);
        TitleAnim.Play("Return");
    }



    void FadeOutInvoke()
    {
        theFade.FadeOutEvent();
    }
}
