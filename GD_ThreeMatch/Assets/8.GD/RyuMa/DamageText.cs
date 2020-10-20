using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DamageText : MonoBehaviour
{
    public GameObject DamageObj;
    public GameObject EnemyHitObj;
    public Image[] NumImages;


    int NumCount;
    Stack<int> DamageList = new Stack<int>();
    bool FlotingEvent;
    float FlotingTime;
    float Speed = 0.3f;
    Vector2 Target;

    bool Punch;
    Vector3 SizeVec = new Vector3(1,1,1);
    float PunchTime;
    Color color = new Color(0,0,0,1);

    private ObjectManager theObject;

    // Update is called once per frame
    void Update()
    {
        if (FlotingEvent == true)
        {
            DamageObj.transform.position = Vector2.MoveTowards(
                DamageObj.transform.position,
                Target, Speed * Time.deltaTime);

            if (Punch == true)
            {
                PunchTime += Time.deltaTime * 12;
                if (PunchTime < 2)
                {
                    SizeVec.x = PunchTime;
                    SizeVec.y = PunchTime;
                }
                else
                {
                    PunchTime = 2;
                    Punch = false;
                }
                DamageObj.transform.localScale = SizeVec;
            }
            else if (Punch == false && PunchTime > 1)
            {
                PunchTime -= Time.deltaTime * 12;
                if (PunchTime > 1)
                {
                    SizeVec.x = PunchTime;
                    SizeVec.y = PunchTime;
                }
                else
                {
                    PunchTime = 1;
                }
                DamageObj.transform.localScale = SizeVec;
            }

            if (FlotingTime > 0)
            {
                FlotingTime -= Time.deltaTime;
                if (FlotingTime < 1)
                {
                    color.a = FlotingTime;
                    for (int i = 0; i < NumCount; i++)
                    {
                        NumImages[i].color = color;
                    }
                }
            }
            else
            {
                Resetting();
            }
        }
    }


    public void SetDamageText(Vector2 _StartVec, int _Value,bool EnemyHit , float _Time = 1.5f)
    {
        if (theObject == null)
            theObject = FindObjectOfType<ObjectManager>();

        EnemyHitObj.SetActive(EnemyHit);


        Vector2 StartPos = _StartVec;
        StartPos.x += Random.Range(-0.9f, 0.9f);
        StartPos.y += Random.Range(-0.3f, 0.3f);
        this.transform.position = StartPos;

        Punch = true;
        PunchTime = 1;
        int Damage = _Value;
        while (true)
        {
            DamageList.Push(Damage % 10);

            if (Damage >= 10)
            {
                Damage /= 10;

            }
            else
            {
                break;
            }
        }

        NumCount = DamageList.Count;

        for (int i = 0; i < NumCount; i++)
        {

            if (DamageList.Count > 0)
            {

                if (NumImages[i].gameObject.activeSelf == false)
                    NumImages[i].gameObject.SetActive(true);
                //NumImages[i].sprite = theObject.DamageSprites[DamageList.Pop()];

            }
            else
            {
                if (NumImages[i].gameObject.activeSelf == true)
                    NumImages[i].gameObject.SetActive(false);
            }

        }

        color.r = 1;
        color.g = 1;
        color.b = 1;
        color.a = 1;
        for (int i = 0; i < NumCount; i++)
        {
            NumImages[i].color = color;
        }
        FlotingTime = _Time;
        FlotingEvent = true;
        Target = this.transform.position;
        Target.y += 0.3f;
    }

    public void Resetting()
    {
        for (int i = 0; i < NumImages.Length; i++)
        {
            NumImages[i].gameObject.SetActive(false);
        }
        FlotingEvent = false;
        DamageObj.transform.position = this.transform.position;
        this.gameObject.SetActive(false);
     


    }




}
