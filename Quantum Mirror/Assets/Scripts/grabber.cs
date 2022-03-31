using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class grabber : MonoBehaviour
{

    public bool isHolding = false;
    public GameObject snapPlane;

    public void Update ()
    {

    }

    public void OnUseHand(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            Debug.Log("Drop?");
            isHolding = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Sample"))
        {
            if (isHolding == true)
            {
                Debug.Log("drop!");

                snapPlane.SetActive(true);
                collision.gameObject.transform.SetParent(null);
                foreach (Transform trans in collision.gameObject.GetComponentsInChildren<Transform>(true))
                {
                    trans.gameObject.layer = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Sample"))
        {
            snapPlane = collision.gameObject.transform.GetChild(0).gameObject;

                Debug.Log("grab!");
                snapPlane.SetActive(false);
                collision.gameObject.transform.position = Vector3.zero;
                collision.gameObject.transform.SetParent(gameObject.transform, false);               
                foreach (Transform trans in collision.gameObject.GetComponentsInChildren<Transform>(true))
                {
                    trans.gameObject.layer = 3;
                }     
        }

    }

}
