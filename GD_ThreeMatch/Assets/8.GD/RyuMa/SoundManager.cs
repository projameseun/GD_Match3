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


    [HideInInspector] public SoundSource BGMSound = null;
    [HideInInspector] public List<SoundSource> SESound = new List<SoundSource>();
    [HideInInspector] public List<SoundSource> UISeSound = new List<SoundSource>();


    Dictionary<string, AudioClip> BGMList =new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> SEList = new Dictionary<string, AudioClip>();



    public float BGMValue = 1f;
    public float SEValue = 1f;


    public GameObject SoundFrefab;

    string CurrentSeName;
    int SeCount = 0;
    bool AutoPlay = false;
    bool CheckSe;
    float CheckSeTime;


    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        for (int i = 0; i < BGMSoundslots.Length; i++)
        {
            BGMList.Add(BGMSoundslots[i].SoundName, BGMSoundslots[i].clip);
        }
        for (int i = 0; i < SESoundslots.Length; i++)
        {
            SEList.Add(SESoundslots[i].SoundName, SESoundslots[i].clip);
        }

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
            UISeSound.Add(SoundSEObj.GetComponent<SoundSource>());
            SoundSEObj.transform.SetParent(this.gameObject.transform);
            SoundSEObj.name = "UISE";

        }
    }



    public void PlayBGM(string _Name)
    {
        if (BGMList.ContainsKey(_Name) == false)
        {
            Debug.Log("해당 이름이 없습니다");
            return;
        }
        BGMSound.PlaySound(BGMList[_Name], BGMValue, true);
        
    }
    public void FadeOutBGM()
    {
        BGMSound.FadeOutEvent();
    }


    public void PlaySE(string _Name)
    {
        if (SEList.ContainsKey(_Name) == false)
        {
            Debug.Log("해당 이름이 없습니다");
            return;
        }

        if (CurrentSeName == _Name && CheckSe == true)
        {
            return;
        }
        CurrentSeName = _Name;
        CheckSeTime = 0.08f;
        if (CheckSe == false)
        {
            CheckSe = true;
            StartCoroutine(CheckSeCor());
        }

        SESound[SeCount].PlaySound(SEList[_Name], SEValue);
        SeCount++;
        if (SeCount >= SESound.Count)
        {
            SeCount = 0;
        }

    }

    public void StopAllSE(string _Name)
    {
        for (int i = 0; i < SESound.Count; i++)
        {
            SESound[i].audioSource.Stop();
        }
    }


    public void PlayUISE(string _Name)
    {
        AutoPlay = !AutoPlay;
        if (AutoPlay == false)
        {
            UISeSound[0].PlaySound(SEList[_Name], SEValue);
        }
        else
        {
            UISeSound[1].PlaySound(SEList[_Name], SEValue);
        }
    }


    IEnumerator CheckSeCor()
    {
        while (CheckSe == true)
        {
            if (CheckSeTime > 0)
            {
                CheckSeTime -= Time.deltaTime;
            }
            else
            {
                CheckSeTime = 0;
                CheckSe = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
