using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static HappyRyuMa.GameMaker;
public class CameraButtonManager : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IDragHandler
{
    public Direction direction;
    public Image[] ButtonImages;
    public Sprite[] ButtonSprite;

    Direction CurrentDir;
    bool Down;
    public Vector2 CurrentVec;
    public float AngleZ;

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
        for (int i = 0; i < ButtonImages.Length; i++)
        {
            if (i == _Dir)
            {
                ButtonImages[i].sprite = ButtonSprite[0];
            }
            else
                ButtonImages[i].sprite = ButtonSprite[1];
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
            CheckDir();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (Down == true)
        {
            theCamera.Down = false;
            Down = false;
        }
    }





    public void CheckDir()
    {

        AngleZ = GetAngleZ(CurrentVec, this.transform.position);

        if (AngleZ <= 45 || AngleZ > 315)
        {
            theCamera.direction = Direction.Up;
        }
        else if (AngleZ > 45 && AngleZ <= 135)
        {
            theCamera.direction = Direction.Left;
        }
        else if (AngleZ > 135 && AngleZ <= 225)
        {
            theCamera.direction = Direction.Down;
        }
        else
        {
            theCamera.direction = Direction.Right;
        }

        if (CurrentDir != theCamera.direction)
        {
            CurrentDir = theCamera.direction;
            thePuzzle.BT_ChangeDirection((int)CurrentDir);
        }
    }


}
