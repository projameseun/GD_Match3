using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeManager : MonoBehaviour
{
    public GameObject BlackChatBase;
    public Text TitleText;
    public Text DesText;


    public Sprite[] LoadingSpirtes;

    public GameObject FadeBase;
    public Image FadeImage;
    public SpriteRenderer LoadingImage;
    public Image LoadingTextImage;
    public bool FadeInEnd;
    public bool FadeOutEnd;
    Color color = new Color(1,1,1,0);
    float FadeTime = 0f;



    

    //BlackChat
    public bool Touch;


    private void Start()
    {
        if (BlackChatBase != null)
        {
            BlackChatBase.GetComponent<Button>().onClick.AddListener(() =>
            {
                FadeOutEvent();
            });
        }
    }



    public void FadeOutEvent(bool Show = true)
    {
        FadeBase.SetActive(true);
        color = new Color(1, 1, 1, 0);
        FadeImage.color = color;
        FadeImage.color = color;
        LoadingTextImage.color = color;
        LoadingImage.color = color;
        FadeTime = 0f;
        LoadingImage.sprite = LoadingSpirtes[Random.Range(0, LoadingSpirtes.Length)];
        StartCoroutine(FadeOut(Show));
    }

    public void FadeInEvent(bool Show = true)
    {
        FadeTime = 3f;
        color.a = FadeTime;
        FadeImage.color = color;
        LoadingTextImage.color = color;
        LoadingImage.color = color;
        StartCoroutine(FadeIn(Show));
    }


    public void ShowBlackChat(string _Title, string _Des)
    {
        BlackChatBase.SetActive(true);
        TitleText.text = _Title;
        DesText.text = _Des;

    }

    public void CloseBlackChat()
    {
        BlackChatBase.SetActive(false);
    }





    IEnumerator FadeOut(bool Show)
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
                if(Show == true)
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


    IEnumerator FadeIn(bool Show)
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
                if(Show == true)
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
