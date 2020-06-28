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
    // Start is called before the first frame update
    void Start()
    {
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
        });
        theSound.PlayBGM("TitleBGM");
    }


    void FadeOutInvoke()
    {
        theFade.FadeOutEvent();
    }
}
