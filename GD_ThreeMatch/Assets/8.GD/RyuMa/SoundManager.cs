using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundSlot
{
    public string SoundName;
    public AudioClip clip;
}


public class SoundManager : MonoBehaviour
{
    public SoundSlot[] BGMSoundslots;
    public SoundSlot[] SESoundslots;


    private SoundSource BGMSound = null;
    private List<SoundSource> SESound = new List<SoundSource>();
    private List<SoundSource> AutoSeSound = new List<SoundSource>();

    public float BGMValue = 1f;
    public float SEValue = 1f;


    public GameObject SoundFrefab;

    bool AutoPlay = false;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        GameObject SoundObj = Instantiate(SoundFrefab);
        BGMSound = SoundObj.GetComponent<SoundSource>();
        SoundObj.transform.SetParent(this.gameObject.transform);
        SoundObj.name = "BGM";

        for (int i = 0; i < 5; i++)
        {
            GameObject SoundSEObj = Instantiate(SoundFrefab);
            SESound.Add(SoundSEObj.GetComponent<SoundSource>());
            SoundSEObj.transform.SetParent(this.gameObject.transform);
            SoundSEObj.name = "SE";

        }

        for (int i = 0; i < 2; i++)
        {
            GameObject SoundSEObj = Instantiate(SoundFrefab);
            AutoSeSound.Add(SoundSEObj.GetComponent<SoundSource>());
            SoundSEObj.transform.SetParent(this.gameObject.transform);
            SoundSEObj.name = "AutoSE";

        }
    }



    public void PlayBGM(string _Name)
    {
        for (int i = 0; i < BGMSoundslots.Length; i++)
        {
            if (BGMSoundslots[i].SoundName == _Name)
            {
                BGMSound.PlaySound(BGMSoundslots[i].clip, _Name, BGMValue, true);
                return;
            }
        }
        
    }


    public void PlaySE(string _Name)
    {
        for (int i = 0; i < SESoundslots.Length; i++)
        {
            if (SESoundslots[i].SoundName == _Name)
            {
                for (int x = 0; x < SESound.Count; x++)
                {
                    if (SESound[x].audioSource.isPlaying == false)
                    {
                        SESound[x].PlaySound(SESoundslots[i].clip, _Name, SEValue);
                        return;
                    }
                    if (x == SESound.Count - 1)
                    {
                        GameObject SoundSEObj = Instantiate(SoundFrefab);
                        SESound.Add(SoundSEObj.GetComponent<SoundSource>());
                        SoundSEObj.transform.SetParent(this.gameObject.transform);
                        SoundSEObj.name = "SE";
                        SoundSEObj.GetComponent<SoundSource>().PlaySound(SESoundslots[i].clip, _Name, SEValue);
                        return;
                    }
                }
            }
        }
    }

    public void StopAllSE(string _Name)
    {
        for (int i = 0; i < SESound.Count; i++)
        {
            SESound[i].audioSource.Stop();
            SESound[i].SoundName = "";
        }
    }


    public void AutoSE(string _Name)
    {
        for (int i = 0; i < SESoundslots.Length; i++)
        {
            if (SESoundslots[i].SoundName == _Name)
            {
                AutoPlay = !AutoPlay;
                if (AutoPlay == false)
                {
                    AutoSeSound[0].PlaySound(SESoundslots[i].clip, _Name, SEValue);
                }
                else
                {
                    AutoSeSound[1].PlaySound(SESoundslots[i].clip, _Name, SEValue);
                }
            }
        }
    }

}
