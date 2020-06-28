using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FadeManager : MonoBehaviour
{
    enum MapNameState
    {
        MS0_End = 0,
        MS1_FadeIn,
        MS2_FadeOut,
        
    }
    //UI

    public GameObject BlackChatBase;
    public Text TitleText;
    public Text DesText;
    public Sprite[] LoadingSpirtes;

    public GameObject FadeBase;
    public Image FadeImage;
    public SpriteRenderer LoadingImage;
    public Image LoadingTextImage;

    public GameObject MapNameBase;
    public TextMeshPro TitleNameText;

    public GameObject BattleBase;
    public Animator BattleAnim;
    public Image EnemyImage;
    public Image PlayerImage;




    //DB

    public bool FadeInEnd;
    public bool FadeOutEnd;
    public bool BattleAnimEnd;
    Color color = new Color(1,1,1,0);
    float FadeTime = 0f;

    //MapNameDB
    float MapNameEventTime;
    MapNameState mapNameState;
    Color TextColor = new Color(1, 1, 1, 1);

    private SoundManager theSound;

    private void Start()
    {
        theSound = FindObjectOfType<SoundManager>();
        if (BlackChatBase != null)
        {
            BlackChatBase.GetComponent<Button>().onClick.AddListener(() =>
            {
                theSound.FadeOutBGM();
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


    public void ShowMapNameEvent(string _string)
    {
        TitleNameText.text = _string;
        MapNameBase.SetActive(true);
        MapNameEventTime = 0f;
        if (mapNameState == MapNameState.MS0_End)
        {
            mapNameState = MapNameState.MS1_FadeIn;
            StartCoroutine(ShowMapName());
        }
        else
        {
            mapNameState = MapNameState.MS1_FadeIn;
        }
       
    }


    public void ShowBattleInit(Sprite _Player, Sprite _Enemy)
    {
        PlayerImage.sprite = _Player;
        EnemyImage.sprite = _Enemy;
        BattleBase.SetActive(true);
        BattleAnim.Play("Idle");
        
    }
    public void ShowBattleAnim()
    {
        BattleAnim.Play("Show");
        theSound.PlaySE("VS");
        StartCoroutine(CheckBattleAnim());
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
                mapNameState = MapNameState.MS0_End;
                break;
            }
        }
        FadeBase.SetActive(false);
    }


    IEnumerator ShowMapName()
    {
        while (mapNameState != MapNameState.MS0_End)
        {
            if (mapNameState == MapNameState.MS1_FadeIn)
            {
                MapNameEventTime += Time.deltaTime;
                if (MapNameEventTime > 2)
                {
                    MapNameEventTime = 2f;
                    mapNameState = MapNameState.MS2_FadeOut;
                }
            }
            else if (mapNameState == MapNameState.MS2_FadeOut)
            {
                MapNameEventTime -= Time.deltaTime;
                if (MapNameEventTime <0)
                {
                    mapNameState = MapNameState.MS0_End;
                }
            }
            TextColor.a = MapNameEventTime;
            TitleNameText.color = TextColor;
            yield return new WaitForEndOfFrame();
        }
        MapNameBase.SetActive(false);
    }

    IEnumerator CheckBattleAnim()
    {
        yield return new WaitForSeconds(3.0f);
        BattleAnimEnd = true;
        BattleBase.SetActive(false);
    }

}
