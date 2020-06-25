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
    FT3_Slime

}



[System.Serializable]
public class MessageDec
{
    public Vector2 WhitePos;
    public Vector2 WhiteSize;
    [TextArea]
    public string Dec;
    public FaceType faceType;
    public float TextBoxPosy;

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
    public Button TouchPanel;
    public TextMeshPro MessageText;
    public SpriteRenderer FaceSpriteRen;

    public Sprite[] FaceSprite;
    


    Queue<string> DecQ = new Queue<string>();
    public bool TutoTouch;

    public int CurrentProgress = 0;


    WaitForSeconds wait = new WaitForSeconds(0.01f);
    Vector4 TextSize = new Vector4(0, 0, 0, 0);



    private GameManager theGM;
    private void Start()
    {
        theGM = FindObjectOfType<GameManager>();
        if (TouchPanel != null)
        {
            TouchPanel.onClick.AddListener(() =>
            {
                TutoTouch = true;
            });
        }
    }



    public void ShowMessageText(int _Progress)
    {
        CurrentProgress = _Progress;
        MessageBase.SetActive(true);
        DecQ.Clear();

        for (int i = 0; i < Messages[CurrentProgress].Decs.Length; i++)
        {
            DecQ.Enqueue(Messages[CurrentProgress].Decs[i].Dec);
        }
        StartCoroutine(ShowMessageCor());

    }


    IEnumerator ShowMessageCor()
    {
        int Num = 0;
        int Count = 0;
        string Dec = "";
        while (DecQ.Count > 0)
        {
            WhiteBox.transform.localPosition = Messages[CurrentProgress].Decs[Num].WhitePos;
            WhiteBox.transform.localScale = Messages[CurrentProgress].Decs[Num].WhiteSize;
            DecBase.transform.localPosition = new Vector3(0, Messages[CurrentProgress].Decs[Num].TextBoxPosy, 0);
            if (Messages[CurrentProgress].Decs[Num].faceType == FaceType.FT0_NULL)
            {
                if (FaceBox.activeSelf == true)
                {
                    FaceBox.SetActive(false);
                    TextSize.x = -23;
                    MessageText.margin = TextSize;
                }
              
            }
            else
            {
                if (FaceBox.activeSelf == false)
                {
                    FaceBox.SetActive(true);
                    TextSize.x = 0;
                    MessageText.margin = TextSize;
                }
                
                FaceSpriteRen.sprite = FaceSprite[(int)Messages[CurrentProgress].Decs[Num].faceType];
            }

            Dec = DecQ.Dequeue();
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

        MessageBase.SetActive(false);


    }
}
