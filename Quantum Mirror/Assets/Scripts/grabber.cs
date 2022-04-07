using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class grabber : MonoBehaviour
{

    public bool isHolding = false;
    public bool drop = false;
    public bool inSlot;
    public GameObject snapPlane;

    public void Update ()
    {

    }

    public void OnUseHand( InputAction.CallbackContext value )
    {
        if ( value.performed && isHolding == false )
            drop = true;

        if (value.performed && isHolding == true )         
            drop = false;
    }

    private void OnTriggerEnter(Collider collision)
    {       

            if ( collision.CompareTag( "Sample" ) && drop == true && inSlot == false )
            {
                snapPlane = collision.gameObject.transform.GetChild(0).gameObject;

                Debug.Log("drop!");

                snapPlane.SetActive(true);
                collision.gameObject.transform.SetParent(null);
                collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                foreach ( Transform trans in collision.gameObject.GetComponentsInChildren<Transform>( true ) )
                    trans.gameObject.layer = 0;
                snapPlane.layer = 9;
                isHolding = true;
            }

            if ( collision.CompareTag( "Sample" ) && drop == false && inSlot == false )
            {
                snapPlane = collision.gameObject.transform.GetChild(0).gameObject;

                Debug.Log("grab!");
                snapPlane.SetActive(false);
                collision.gameObject.transform.SetParent(gameObject.transform);
                collision.gameObject.transform.localPosition = Vector3.zero;
                collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                foreach ( Transform trans in collision.gameObject.GetComponentsInChildren<Transform>( true ) )
                    trans.gameObject.layer = 3;

                snapPlane.layer = 9;

                drop = false;
                isHolding = false;
            }
        //}

    }
}
