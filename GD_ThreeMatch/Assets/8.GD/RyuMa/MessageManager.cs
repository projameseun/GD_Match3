using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum FaceType
{ 
    FT0_NULL = -1,
    FT1_ALICE,
    FT2_Beryl,
    FT3_Slime,
    FT4_Slime_Angry,


}



[System.Serializable]
public class MessageDec
{
    public Vector2 WhitePos;
    public Vector2 WhiteSize;
    public string Name;
    [TextArea]
    public string Dec;
    public FaceType faceType;
    public float TextBoxPosy;
    public string SeName;
}


[System.Serializable]
public class Message
{
    public MessageDec[] Decs;
}


public class MessageManager : MonoBehaviour
{

    public Message[] Messages;


    public GameObject MessageBase;
    public GameObject DecBase;
    public GameObject WhiteBox;
    public GameObject FaceBox;
    public GameObject NameBox;
    public Button TouchPanel;
    public TextMeshPro NameText;
    public TextMeshPro MessageText;
    
    public SpriteRenderer FaceSpriteRen;

    public Sprite[] FaceSprite;


    Queue<string> NameQ = new Queue<string>();
    Queue<string> DecQ = new Queue<string>();
    Queue<string> SoundQ = new Queue<string>();
    public bool TutoTouch;
    public bool MessageEnd;

    public int CurrentProgress = 0;


    WaitForSeconds wait = new WaitForSeconds(0.01f);
    Vector4 TextSize = new Vector4(0, 0, 0, 0);


    private SoundManager theSound;
    private void Start()
    {
        theSound = FindObjectOfType<SoundManager>();
        if (TouchPanel != null)
        {
            TouchPanel.onClick.AddListener(() =>
            {
                TutoTouch = true;
            });
        }
    }



    public void ShowMessageText(int _Progress, bool CheckEnd = false)
    {
        CurrentProgress = _Progress;
        MessageBase.SetActive(true);
        DecQ.Clear();
        NameQ.Clear();
        SoundQ.Clear();
        for (int i = 0; i < Messages[CurrentProgress].Decs.Length; i++)
        {
            NameQ.Enqueue(Messages[CurrentProgress].Decs[i].Name);
            DecQ.Enqueue(Messages[CurrentProgress].Decs[i].Dec);
            SoundQ.Enqueue(Messages[CurrentProgress].Decs[i].SeName);
        }
        StartCoroutine(ShowMessageCor(CheckEnd));

    }


    IEnumerator ShowMessageCor(bool CheckEnd)
    {
        int Num = 0;
        int Count = 0;
        string Dec = "";
        string SoundName = "";
        while (DecQ.Count > 0)
        {
            WhiteBox.transform.localPosition = Messages[CurrentProgress].Decs[Num].WhitePos;
            WhiteBox.transform.localScale = Messages[CurrentProgress].Decs[Num].WhiteSize;
            DecBase.transform.localPosition = new Vector3(0, Messages[CurrentProgress].Decs[Num].TextBoxPosy, 0);
            if (Messages[CurrentProgress].Decs[Num].faceType == FaceType.FT0_NULL)
            {
                if (FaceBox.activeSelf == true)
                {
                    NameBox.SetActive(false);
                    FaceBox.SetActive(false);
                    TextSize.x = -20;
                    MessageText.margin = TextSize;
                }
              
            }
            else
            {
                if (FaceBox.activeSelf == false)
                {
                    NameBox.SetActive(true);
                    FaceBox.SetActive(true);
                    TextSize.x = 0;
                    MessageText.margin = TextSize;
                }
                
                FaceSpriteRen.sprite = FaceSprite[(int)Messages[CurrentProgress].Decs[Num].faceType];
            }
            NameText.text = NameQ.Dequeue();
            Dec = DecQ.Dequeue();
            SoundName = SoundQ.Dequeue();
            if (SoundName != "")
            {
                theSound.PlaySE(SoundName);
            }

            Count = 0;
            Num++;
            MessageText.text = "";
            while (TutoTouch == false)
            {
                if (Count < Dec.Length)
                {
                    MessageText.text += Dec[Count];
                    Count++;
                }
                yield return wait;
            }
            while (Count < Dec.Length)
            {
                MessageText.text += Dec[Count];
                Count++;
                TutoTouch = false;
            }
            while (TutoTouch == false)
            {
                yield return null;
            }
            TutoTouch = false;
            yield return null;
            Dec = "";
            MessageText.text = "";
        }
        if (CheckEnd == true)
        {
            MessageEnd = true;
        }

        MessageBase.SetActive(false);


    }
}
