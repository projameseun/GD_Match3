using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraButtonManager : MonoBehaviour
{
    public Direction direction;
    public Sprite[] ButtonSprite;
    Image ButtonImage;
    

    // Start is called before the first frame update
    void Start()
    {
        ButtonImage = GetComponent<Image>();
    }


    public void ButtonChange(int _Num)
    {
        if ((int)direction == _Num)
        {
            ButtonImage.sprite = ButtonSprite[1];
        }else
            ButtonImage.sprite = ButtonSprite[0];
    }
}
