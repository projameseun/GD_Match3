using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{



    //게임오브젝트 리스트
    public List<GameObject> Cubes; //큐브 리스트
    public List<GameObject> CubeParticles;
    public List<GameObject> CubeEfs;
    public List<GameObject> SpeechBubbles;


    //게임오브젝트 프리팹
    public GameObject Cube; //큐브 프리팹
    public GameObject CubeParticle;
    public GameObject CubeEf;
    public GameObject SpeechObj;




    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < 400; i++)
        {
            GameObject x = Instantiate(Cube);
            x.SetActive(false);
            Cubes.Add(x);
        }
        for (int i = 0; i < 100; i++)
        {
            GameObject x = Instantiate(CubeParticle);
            x.SetActive(false);
            CubeParticles.Add(x);
        }
        for (int i = 0; i < 100; i++)
        {
            GameObject x = Instantiate(CubeEf);
            x.SetActive(false);
            CubeEfs.Add(x);
        }
        for (int i = 0; i < 5; i++)
        {
            GameObject x = Instantiate(SpeechObj);
            x.SetActive(false);
            SpeechBubbles.Add(x);
        }
    }



    //사용하고싶은 오브젝트를 찾는 함수(오브젝트 이름, 오브젝트 활성화)
    public GameObject FindObj(string _Name, bool _Active = true)
    {
        List<GameObject> List = null;
        GameObject Frefab = null;
        switch (_Name)
        {
            case "Cube":
                List = Cubes;
                Frefab = Cube;
                break;
            case "CubeP":
                List = CubeParticles;
                Frefab = CubeParticle;
                break;
            case "CubeE":
                List = CubeEfs;
                Frefab = CubeEf;
                break;
            case "Speech":
                List = SpeechBubbles;
                Frefab = SpeechObj;
                break;
        
        }


        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].activeSelf == false)
            {
                List[i].SetActive(_Active);
                return List[i];
            }
        }

        //만약 모든 리스트에 오브젝트들이 활성화되어있다면 오브젝트를 추가하고 넣는다
        GameObject X = Instantiate(Frefab);
        X.SetActive(_Active);
        List.Add(X);

        return X;
    }


    // 큐브 이펙트를 사용하는 함수
    public GameObject CubeEffectEvent(Vector2 _StartVec, GameObject _Target,NodeColor _NodeColor,
        CubeEffectType _CubeTarget, int _CubeCount, bool _RandStart)
    {

        GameObject CubeEffect = FindObj("CubeE");
        CubeEffect.GetComponent<CubeEffect>().SetCubeEffect(_StartVec,
                   _Target,
                   _NodeColor, _CubeTarget, _CubeCount, _RandStart
                   );
        return CubeEffect;

    }

    // 말풍선 이밴트를 사용하는 함수
    public GameObject SpeechEvent(Vector2 _StartVec, string _Speech, float _LifeTime)
    {
        GameObject Speech = FindObj("Speech");
        Speech.GetComponent<SpeechBubble>().SetSpeech(
            _StartVec, _Speech, _LifeTime);
        return Speech;

    }

    public GameObject CubeParticleEvent(Vector2 TargetVec,Sprite _sprite)
    {
        GameObject Paricle = FindObj("CubeP", false);
        Paricle.transform.position = TargetVec;
        Paricle.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(
            0, _sprite);
        Paricle.GetComponent<ParticleManager>().ParticleSetting(false, null, 5);
        Paricle.SetActive(true);
        return Paricle;
    }


    public void ResettingObj()
    {
        for(int i = 0; i < Cubes.Count; i++)
        {
            if(Cubes[i].activeSelf)
            {
                Cubes[i].GetComponent<Cube>().Resetting();
            }
        }

        for(int i = 0; i < CubeParticles.Count; i++)
        {
            if(CubeParticles[i].activeSelf)
            {
                CubeParticles[i].GetComponent<ParticleManager>().Resetting();
            }
        }

        for (int i = 0; i < CubeEfs.Count; i++)
        {
            if (CubeEfs[i].activeSelf)
            {
                CubeEfs[i].GetComponent<CubeEffect>().Resetting();
            }
        }

        for (int i = 0; i < SpeechBubbles.Count; i++)
        {
            if (SpeechBubbles[i].activeSelf)
            {
                SpeechBubbles[i].GetComponent<SpeechBubble>().Resetting();
            }
        }



    }


}
