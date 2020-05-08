using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    
    public AudioSource audioSource = null;
    public string SoundName;

    float SoundValue = 1f;
    float EventTime = 0;
    bool FadeIn = false;
    bool FadeOut = false;

    private SoundManager theSound;
    private void Start()
    {
        theSound = FindObjectOfType<SoundManager>();
    }


    private void Update()
    {
        if (FadeIn == true)
        {
            if (EventTime < 1)
            {
                EventTime += Time.deltaTime;
                audioSource.volume = EventTime * SoundValue;
            }
            else
            {
                EventTime = 1;
                FadeIn = false;
                audioSource.volume = EventTime * SoundValue;

            }
        }

        if (FadeOut == true)
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
        }
    }



    public void PlaySound(AudioClip _clip, string _Name,float _Value, bool _Loop = false)
    {

        audioSource.clip = _clip;
        SoundName = _Name;
        audioSource.loop = _Loop;
        SoundValue = 1f;
        audioSource.volume = SoundValue * _Value;
        audioSource.Play();

    }

    public void FadeInSound(AudioClip _clip, string _Name, float _Value, bool _Loop = false)
    {
        audioSource.clip = _clip;
        SoundName = _Name;
        audioSource.loop = _Loop;
        SoundValue = _Value;
        EventTime = 0;
        audioSource.volume = 0;
        audioSource.Play();
    }
    public void FadeOutSound(AudioClip _clip, string _Name, float _Value, bool _Loop = false)
    {
        audioSource.clip = _clip;
        SoundName = _Name;
        audioSource.loop = _Loop;
        SoundValue = _Value;
        EventTime = 1;
        audioSource.volume = SoundValue;
        audioSource.Play();
    }



}
