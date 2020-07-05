using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HappyRyuMa.GameMaker;
public class PortalArrowManager : MonoBehaviour
{
    public Animator Anim;

    Vector2 PortalVec;
    Vector2 CameraVec;
    Vector2 CurrentVec;

    Vector3 Euler = new Vector3(0,0,0);
    GameObject Camera;
    float AngleZ;

    public bool CheckShow;

    

    private PuzzleManager thePuzzle;
    private ObjectManager theObject;


    private void Update()
    {
        if (thePuzzle.gameMode == PuzzleManager.GameMode.MoveMap)
        {
            ArrowPos();
        }
    }




    public void SetPortalArrow(Vector2 _PortalVec)
    {

        if (thePuzzle == null)
            thePuzzle = FindObjectOfType<PuzzleManager>();
        if (Camera == null)
            Camera = FindObjectOfType<CameraManager>().gameObject;
        if (theObject == null)
            theObject = FindObjectOfType<ObjectManager>();
        CheckShow = true;
        Anim.Play("Show");

        PortalVec = _PortalVec;
    }


    public void ArrowPos()
    {
        CameraVec = Camera.transform.position;
        if (Mathf.Abs(CameraVec.x - PortalVec.x) < 3 &&
            Mathf.Abs(CameraVec.y - PortalVec.y) < 2.6)
        {
            if (CheckShow == true)
            {
                CheckShow = false;
                Anim.Play("Close");
            }
            return;
        }

        if (CheckShow == false)
        {
            CheckShow = true;
            Anim.Play("Show");
        }

        if (Mathf.Abs(CameraVec.x - PortalVec.x) < 2.4)
        {
            CurrentVec.x =  PortalVec.x;
        }
        else
        {
            if (CameraVec.x < PortalVec.x)
            {
                CurrentVec.x = CameraVec.x + 2.4f;
            }else
                CurrentVec.x = CameraVec .x - 2.4f;
        }

        if (Mathf.Abs(CameraVec.y - PortalVec.y) < 1.9)
        {
            CurrentVec.y = PortalVec.y;
        }
        else
        {
            if (CameraVec.y < PortalVec.y)
            {
                CurrentVec.y = CameraVec.y +1.9f;
            }
            else
                CurrentVec.y = CameraVec .y - 1.9f;
        }
        AngleZ = GetAngleZ(PortalVec, CurrentVec);

        if (AngleZ < 45 || AngleZ >= 315)
        {
            Euler.z = 90;
        }
        else if (AngleZ >= 45 && AngleZ < 135)
        {
            Euler.z = 180;
        }
        else if (AngleZ >= 135 && AngleZ < 225)
        {
            Euler.z = 270;
        }
        else if (AngleZ >= 225 && AngleZ < 315)
        {
            Euler.z = 0;
        }
        this.transform.eulerAngles = Euler;
        this.transform.position = CurrentVec;
    }




    public void Resetting()
    {

        this.gameObject.SetActive(false);
        //theObject.PortalArrows.Enqueue(this.gameObject);
    }

}
