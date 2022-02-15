using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

public class Handmovement : MonoBehaviour
{
    public float XSensitivity;
    public float YSensitivity;
    public GameObject center;
    public GameObject circle;
    public GameObject idle;
    public GameObject handmodel;

    public GameObject[] Digits;
    public GameObject[] ClosedDigits;

    public GameObject FPScontroller;

    public bool gestureMode;

    private float lerp = 0;
    public MouseLook lookX;

    private int i = 0;


    private void Start()
    {
        lookX = FPScontroller.GetComponent<FirstPersonController>().m_MouseLook;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            if (!gestureMode)
            {
                gestureMode = true;
            }
            else
            {
                gestureMode = false;
            }
        }


        if (gestureMode == true)
        {
            //Hand rotates outward from the gesture circle
            Vector3 relativepos = handmodel.transform.InverseTransformPoint(center.transform.position);
            relativepos.y = 0;

            Vector3 targetPostition = handmodel.transform.TransformPoint(relativepos);

            handmodel.transform.LookAt(targetPostition, handmodel.transform.up);

            circle.SetActive(true);
            lookX.XSensitivity = 0.1f;
            lookX.YSensitivity = 0.2f;

            //Moving the hand with the mouse as long as it's in the circle, otherwise move it slightly back to center
            float distance = Vector3.Distance(center.transform.position, transform.position);

            if (distance < 1)
            {
                float xMove = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
                float yMove = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;
                gameObject.transform.Translate(new Vector3(xMove, yMove, 0));
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards(transform.position, center.transform.position, 0.005f);
            }

        }
        else
        {
            gameObject.transform.localPosition = idle.transform.localPosition;
            circle.SetActive(false);

            lookX.XSensitivity = 2f;
            lookX.YSensitivity = 2f;

        }

        //Punch your hand forward when holding lmb, back when released
        Vector3 punchdestination = new Vector3(0, 0, 0.08f);

        if (Input.GetMouseButton(0))
        {
            if (gameObject.transform.localPosition.z < 0.4f)
            {
                gameObject.transform.Translate(punchdestination);
            }
        }
        else
        {
            if (gameObject.transform.localPosition.z >= 0f)
            {
                gameObject.transform.Translate(-punchdestination);
            }
        }

        //use scrollwheel to close hand (mode 1)


        /* if (Input.mouseScrollDelta.y < 0 && i <= 5)
         { 
             Debug.Log (i);
             Digits[i].SetActive(false);
             ClosedDigits[i].SetActive(true);
             i += 1;
         }

         if (Input.mouseScrollDelta.y > 0 && i > 0)
         {
             Debug.Log (i);
             Digits[i].SetActive(true);
             ClosedDigits[i].SetActive(false);
             i -= 1;
         } */

         if (Input.mouseScrollDelta.y < 0 && i <= 5)
        { 
            Debug.Log (i);
            foreach (GameObject digit in Digits)
            {
                digit.SetActive(true);
            }
            foreach (GameObject digit in ClosedDigits)
            {
                digit.SetActive(false);
            }

            Digits[i].SetActive(false);
            ClosedDigits[i].SetActive(true);
            i += 1;
        }

        if (Input.mouseScrollDelta.y > 0 && i > 0)
        {
            Debug.Log(i);
            foreach (GameObject digit in Digits)
            {
                digit.SetActive(true);
            }
            foreach (GameObject digit in ClosedDigits)
            {
                digit.SetActive(false);
            }

            Digits[i].SetActive(false);
            ClosedDigits[i].SetActive(true);
            i -= 1;
        }

        /*if (Input.mouseScrollDelta.y > 0 && i > 0)
        {
            Debug.Log(i);
            foreach (GameObject digit in Digits)
            {
                digit.SetActive(false);
            }
            foreach (GameObject digit in ClosedDigits)
            {
                digit.SetActive(true);
            }

            Digits[i].SetActive(true);
            ClosedDigits[i].SetActive(false);
            i += 1;
        }*/

    }
}
