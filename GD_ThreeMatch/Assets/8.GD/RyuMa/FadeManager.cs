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
    public bool FadeInEnd;
    public bool FadeOutEnd;
    Color color = new Color(1,1,1,0);
    float FadeTime = 0f;


    public void FadeOutEvent()
    {
        FadeBase.SetActive(true);
        color = new Color(1, 1, 1, 0);
        FadeImage.color = color;
        FadeImage.color = color;
        LoadingTextImage.color = color;
        LoadingImage.color = color;
        FadeTime = 0f;
        LoadingImage.sprite = LoadingSpirtes[Random.Range(0, LoadingSpirtes.Length)];
        StartCoroutine(FadeOut());
    }

    public void FadeInEvent()
    {

        FadeTime = 3f;
        color.a = FadeTime;
        FadeImage.color = color;
        LoadingTextImage.color = color;
        LoadingImage.color = color;
        StartCoroutine(FadeIn());
    }


    IEnumerator FadeOut()
    {
        while (true)
        {
            if (FadeTime < 1)
            {
                FadeTime += Time.deltaTime * 2;
                color.a = FadeTime;
                FadeImage.color = color;
                LoadingTextImage.color = color;
                LoadingImage.color = color;

                yield return new WaitForEndOfFrame();
            }
            else
            {
                FadeOutEnd = true;
                FadeTime = 1f;
                color.a = FadeTime;
                FadeImage.color = color;
                LoadingTextImage.color = color;
                LoadingImage.color = color;
                break;
            }
            
        }
    }

    IEnumerator FadeIn()
    {
        while (true)
        {
            if (FadeTime > 0)
            {
                FadeTime -= Time.deltaTime * 2;
                color.a = FadeTime;
                FadeImage.color = color;
                LoadingTextImage.color = color;
                LoadingImage.color = color;
                yield return new WaitForEndOfFrame();
            }
            else
            {
                FadeInEnd = true;
                FadeTime = 0f;
                color.a = FadeTime;
                FadeImage.color = color;
                LoadingTextImage.color = color;
                LoadingImage.color = color;
                break;
            }
        }
        FadeBase.SetActive(false);
    }



}
