using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DamageText : MonoBehaviour
{
    public TextMeshPro damageText;
    public GameObject EnemyHitObj;


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
            damageText.transform.position = Vector2.MoveTowards(
                damageText.transform.position,
                Target, Speed * Time.deltaTime);

            if (Punch == true)
            {
                PunchTime += Time.deltaTime * 12;
                if (PunchTime < 2)
                {
                    SizeVec.x = PunchTime/10;
                    SizeVec.y = PunchTime/10;
                }
                else
                {
                    PunchTime = 2;
                    Punch = false;
                }
                damageText.transform.localScale = SizeVec;
            }
            else if (Punch == false && PunchTime > 1)
            {
                PunchTime -= Time.deltaTime * 12;
                if (PunchTime > 1)
                {
                    SizeVec.x = PunchTime/10;
                    SizeVec.y = PunchTime/10;
                }
                else
                {
                    PunchTime = 1;
                }
                damageText.transform.localScale = SizeVec;
            }

            if (FlotingTime > 0)
            {
                FlotingTime -= Time.deltaTime;
                if (FlotingTime < 1)
                {
                    color.a = FlotingTime;
                    damageText.color = color;
                }
            }
            else
            {
                Resetting();
            }
        }
    }


    public void SetDamageText(Vector2 _StartVec, string _Value,bool EnemyHit , float _Time = 1.5f)
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



        damageText.text = _Value;
        color = damageText.color;
        color.a = 1;
        damageText.color = color;
        FlotingTime = _Time;
        FlotingEvent = true;
        Target = this.transform.position;
        Target.y += 0.3f;
    }

    public void Resetting()
    {

        FlotingEvent = false;
        damageText.transform.position = this.transform.position;
        this.gameObject.SetActive(false);
        theObject.DamageTexts.Enqueue(this.gameObject);

    }




}
