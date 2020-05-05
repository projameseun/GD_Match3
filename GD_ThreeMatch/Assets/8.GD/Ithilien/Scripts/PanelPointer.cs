using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelPointer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject chatBox;
    public string[] chatString;
    public float LifeTime;
    public Text chatText;

    public void OnPointerDown (PointerEventData eventData)
    {
        
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        if (chatBox.activeSelf == false)
        {
            string randomStr = chatString[Random.Range(0, chatString.Length)];
            chatText.text = randomStr;
        }
        
        chatBox.SetActive(true);
    }
    
    void Update()
    {
        if (chatBox.activeSelf == true)
        {
            
            

            if (LifeTime > 0)
            {
                

                LifeTime -= Time.deltaTime;

                if (LifeTime <= 0)
                {
                    LifeTime = 3;
                    chatBox.SetActive(false);
                }
            }
        }

        
    }

    
}
