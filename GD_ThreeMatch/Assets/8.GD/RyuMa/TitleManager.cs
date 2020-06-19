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
    // Start is called before the first frame update
    void Start()
    {
        theGM = FindObjectOfType<GameManager>();
        theFade = FindObjectOfType<FadeManager>();
        TitleAnim.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (TouchOnOff == true)
                return;

            TouchOnOff = true;
            TitleAnim.Play("Close");
            Invoke("FadeOutInvoke", 0.5f);
        });
    }


    void FadeOutInvoke()
    {
        theFade.FadeOutEvent();
    }
}
