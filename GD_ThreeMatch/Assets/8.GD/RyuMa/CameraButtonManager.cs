using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static HappyRyuMa.GameMaker;
public class CameraButtonManager : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IDragHandler
{
    public Image[] ButtonImages;
    public Sprite[] ButtonSprite;
    public GameObject JoyStickObj;


    public Direction CurrentDir;
    bool Down;
    Vector2 CurrentVec;
    float AngleZ;
    public float Radio;
    Vector2 JoyStickVec;
    Vector2 StartVec;
    private CameraManager theCamera;
    private PuzzleManager thePuzzle;
    // Start is called before the first frame update
    void Start()
    {
        thePuzzle = FindObjectOfType<PuzzleManager>();
        theCamera = FindObjectOfType<CameraManager>();
    }


    public void ButtonChange(int _Dir)
    {
        CurrentDir = (Direction)_Dir;
        for (int i = 0; i < ButtonImages.Length; i++)
        {
            if (i == _Dir)
            {
                ButtonImages[i].sprite = ButtonSprite[0];
            }
            else
            {
                ButtonImages[i].sprite = ButtonSprite[1];

            }
        }
        
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Down == false)
        {
            theCamera.Down = true;
            Down = true;
            CurrentVec = eventData.position;
            CurrentVec = Camera.main.ScreenToWorldPoint(CurrentVec);
            CheckDir();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Down == true)
        {
            CurrentVec = eventData.position;
            CurrentVec = Camera.main.ScreenToWorldPoint(CurrentVec);
            JoyStickVec = CurrentVec;
            StartVec = this.transform.position;
            JoyStickVec = JoyStickVec - StartVec;
            JoyStickVec = Vector2.ClampMagnitude(JoyStickVec, Radio);
            JoyStickObj.transform.position = JoyStickVec + StartVec;
            CheckDir();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        TouchUp();
    }





    public void CheckDir()
    {

        AngleZ = GetAngleZ(CurrentVec, this.transform.position);

        if (AngleZ <= 45 || AngleZ > 315)
        {
            CurrentDir = Direction.Up;
            theCamera.direction = Direction.Up;
        }
        else if (AngleZ > 45 && AngleZ <= 135)
        {
            CurrentDir = Direction.Left;
            theCamera.direction = Direction.Left;
        }
        else if (AngleZ > 135 && AngleZ <= 225)
        {
            CurrentDir = Direction.Down;
            theCamera.direction = Direction.Down;
        }
        else
        {
            CurrentDir = Direction.Right;
            theCamera.direction = Direction.Right;
        }

        if (CurrentDir != thePuzzle.theMoveMap.direction)
        {
            CurrentDir = theCamera.direction;
            thePuzzle.BT_ChangeDirection((int)CurrentDir);
        }
    }


    public void TouchUp()
    {
        if (Down == true)
        {
            JoyStickObj.transform.position = this.transform.position;
            theCamera.Down = false;
            Down = false;
        }
    }


}
