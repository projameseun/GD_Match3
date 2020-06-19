using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{

    public Text SpeechText;
    public SpriteRenderer SpeechBox;
    public float LifeTime;
    Color Boxcolor = new Color(1,1,1,1);
    Color Speechcolor = new Color(0, 0, 0, 1);

    private ObjectManager theObject;

    private void Update()
    {
        if (LifeTime > 0)
        {
            LifeTime -= Time.deltaTime;

            if (LifeTime < 1)
            {
                Boxcolor.a = LifeTime;
                Speechcolor.a = LifeTime;
                SpeechBox.color = Boxcolor;
                SpeechText.color = Speechcolor;
            }


        }
        else
        {
            Resetting();
        }
    }



    public void SetSpeech(Vector2 _vec, string _Speech, float _LifeTime)
    {
        if (theObject == null)
            theObject = FindObjectOfType<ObjectManager>();

        this.transform.position = _vec;
        Boxcolor = SpeechBox.color;
        Speechcolor = SpeechText.color;
        SpeechText.text = _Speech;
        LifeTime = _LifeTime;
    }





    public void Resetting()
    {
        this.gameObject.SetActive(false);
        Boxcolor.a = 1;
        Speechcolor.a = 1;
        SpeechBox.color = Boxcolor;
        SpeechText.color = Speechcolor;

        theObject.SpeechBubbles.Enqueue(this.gameObject);

    }




}
