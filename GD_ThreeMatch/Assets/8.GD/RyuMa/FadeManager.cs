using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeManager : MonoBehaviour
{
    public GameObject FadeBase;
    public Image FadeImage;
    public bool FadeEvent;

    Color color = new Color();
    bool[] FadeE = new bool[2];
    float FadeTime = 0f;


    // Update is called once per frame
    void Update()
    {
        Fade();
      
    }
    void Fade()
    {
        if (FadeE[0] == true)
        {
            if (FadeE[1] == false)
            {
                if (FadeTime < 1)
                {
                    FadeTime += Time.deltaTime * 2;
                    color.a = FadeTime;
                    FadeImage.color = color;
                }
                else
                {
                    FadeE[1] = true;
                    FadeEvent = true;
                    FadeTime = 1f;
                    color.a = FadeTime;
                    FadeImage.color = color;
                }
            }
            else
            {
                if (FadeTime > 0)
                {
                    FadeTime -= Time.deltaTime * 2;
                    color.a = FadeTime;
                    FadeImage.color = color;
                }
                else
                {
                    FadeE[1] = false;
                    FadeE[0] = false;

                    FadeTime = 0f;
                    color.a = FadeTime;
                    FadeImage.color = color;
                    FadeBase.SetActive(false);
                }
            }

        }
    }
    public void FadeIn()
    {
        FadeBase.SetActive(true);
        color = new Color(0, 0, 0, 0);
        FadeImage.color = color;
        FadeE[0] = true;
        FadeTime = 0f;


    }


}
