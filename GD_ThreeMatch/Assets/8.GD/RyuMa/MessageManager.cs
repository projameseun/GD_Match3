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


    [Header("화이트 박스")]
    public Vector2 WhitePos;
    public Vector2 WhiteSize;
    [Header("대화창")]
    public string Name;
    [TextArea]
    public string Dec;
    public FaceType faceType;
    public float TextBoxPosy;
    [Header("효과음")]
    public string SeName;
    [Header("설명 이미지")]
    public int DecImageNum = -1;
    public Vector2 DecImagePos;

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
    public SpriteRenderer DecImage;

    public Sprite[] FaceSprite;
    public Sprite[] DecSprites;

    Queue<string> DecQ = new Queue<string>();
    public bool TutoTouch;
    public bool MessageEnd;

    public int CurrentProgress = 0;


    WaitForSeconds wait = new WaitForSeconds(0.01f);
    Vector4 TextSize = new Vector4(0, 0, 0, 0);
    int Num = 0;
    int Count = 0;
    string Dec = "";
    string SoundName = "";
    int DecImageNum = 0;






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
        for (int i = 0; i < Messages[CurrentProgress].Decs.Length; i++)
        {
            DecQ.Enqueue(Messages[CurrentProgress].Decs[i].Dec);
        }
        StartCoroutine(ShowMessageCor(CheckEnd));

    }


    IEnumerator ShowMessageCor(bool CheckEnd)
    {

        Num = 0;
        Dec = "";
        SoundName = "";
        DecImageNum = -1;
        while (DecQ.Count > 0)
        {
            //화이트박스
            WhiteBox.transform.localPosition = Messages[CurrentProgress].Decs[Num].WhitePos;
            WhiteBox.transform.localScale = Messages[CurrentProgress].Decs[Num].WhiteSize;

            //대화창
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
                NameText.text = Messages[CurrentProgress].Decs[Num].Name;
                FaceSpriteRen.sprite = FaceSprite[(int)Messages[CurrentProgress].Decs[Num].faceType];
            }
            Dec = DecQ.Dequeue();

            //효과음
            SoundName = Messages[CurrentProgress].Decs[Num].SeName;
            if (SoundName != "")
            {
                theSound.PlaySE(SoundName);
            }



            

            // 설명 이미지
            DecImageNum = Messages[CurrentProgress].Decs[Num].DecImageNum;
            if (DecImageNum != -1)
            {
                DecImage.enabled = false;
            }
            else
            {
                DecImage.enabled = true;
                DecImage.sprite = DecSprites[DecImageNum];
                DecImage.transform.localPosition = Messages[CurrentProgress].Decs[Num].DecImagePos;
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
