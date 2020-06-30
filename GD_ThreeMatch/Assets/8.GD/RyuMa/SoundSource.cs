using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    
    public AudioSource audioSource = null;

    float SoundValue = 1f;
    float EventTime = 0;
    bool FadeOut = false;

    private SoundManager theSound;
    private void Start()
    {
        theSound = FindObjectOfType<SoundManager>();
    }





    public void PlaySound(AudioClip _clip,float _Value, bool _Loop = false)
    {

        audioSource.clip = _clip;
        audioSource.loop = _Loop;
        SoundValue = _Value;
        audioSource.volume = SoundValue;
        audioSource.Play();

    }



    public void FadeOutEvent(float _Value)
    {
        SoundValue = _Value;
        if (FadeOut == false)
        {
            FadeOut = true;
            EventTime = 1f;
            StartCoroutine(FadeOutCor());
        }
    }


    IEnumerator FadeOutCor()
    {
        while (FadeOut == true)
        {
            if (EventTime > 0)
            {
                EventTime -= Time.deltaTime;
                audioSource.volume = EventTime * SoundValue;
            }
            else
            {
                EventTime = 0;
                FadeOut = false;
                audioSource.Stop();
            }
            yield return new WaitForEndOfFrame();
        }
    }




}
