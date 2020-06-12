using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeManager : MonoBehaviour
{
    public Sprite[] LoadingSpirtes;

    public GameObject FadeBase;
    public Image FadeImage;
    public SpriteRenderer LoadingImage;
    public Image LoadingTextImage;
    public bool FadeEvent;
    public bool FadeEnd;
    Color color = new Color(1,1,1,0);
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
                    LoadingTextImage.color = color;
                    LoadingImage.color = color;
                }
                else
                {
                    FadeE[1] = true;
                    FadeEvent = true;
                    FadeTime = 3f;
                    color.a = FadeTime;
                    FadeImage.color = color;
                    LoadingTextImage.color = color;
                    LoadingImage.color = color;
                }
            }
            else
            {
                if (FadeTime > 0)
                {
                    FadeTime -= Time.deltaTime * 2;
                    color.a = FadeTime;
                    FadeImage.color = color;
                    LoadingTextImage.color = color;
                    LoadingImage.color = color;
                }
                else
                {
                    FadeE[1] = false;
                    FadeE[0] = false;
                    FadeEnd = true;
                    FadeTime = 0f;
                    color.a = FadeTime;
                    FadeImage.color = color;
                    LoadingTextImage.color = color;
                    LoadingImage.color = color;
                    FadeBase.SetActive(false);
                }
            }

        }
    }
    public void FadeIn()
    {
        FadeBase.SetActive(true);
        color = new Color(1, 1, 1, 0);
        FadeImage.color = color;
        FadeImage.color = color;
        LoadingTextImage.color = color;
        LoadingImage.color = color;
        FadeE[0] = true;
        FadeTime = 0f;
        LoadingImage.sprite = LoadingSpirtes[Random.Range(0, LoadingSpirtes.Length)];

    }


}
