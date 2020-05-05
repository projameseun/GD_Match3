using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button gameStartBtn;
    

    [Header("Top UI")]
    public Text text_Timer;
    public Text text_Count;

    public int maxStamina;
    public float filledTime;

    
    private int currentStamina = 3; // 테스트 추후 변경
    private float copyTime;

    // Start is called before the first frame update
    void Start()
    {
        //currentStamina = maxStamina;
        copyTime = filledTime;

        text_Count.text = currentStamina + " / " + maxStamina;

        if (gameStartBtn != null)
        {
            if (currentStamina > 0)
            {
                gameStartBtn.onClick.AddListener(() =>
                {
                    currentStamina--;
                    SceneManager.LoadScene("InGame");
                });
            }
        }
    }

    void Update()
    {
        if(currentStamina < maxStamina)
        {
            filledTime -= Time.deltaTime;
            text_Timer.text = GetMinute(filledTime) + ":" + GetSecond(filledTime);
        }

        if(filledTime <= 0)
        {
            if (currentStamina < maxStamina)
            {
                currentStamina++;
                filledTime = copyTime;
                text_Count.text = currentStamina + " / " + maxStamina;
            }
        }
    }

    public static int GetMinute(float _time)
    {
        return (int)((_time / 60) % 60);
    }

    public static int GetSecond(float _time)
    {
        return (int)(_time % 60);
    }
}
