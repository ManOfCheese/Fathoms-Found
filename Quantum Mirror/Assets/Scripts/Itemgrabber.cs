using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Itemgrabber : MonoBehaviour
{

    public BoolValue isUsingGestureCircle;
    public SampleSlot selectedSlot;
    public GameObject currentSample;

    void Update()
    {
        //Snap the arm to an item slot when colliding
    }

    public void OnTriggerStay( Collider collision )
    {
        if ( collision.GetComponent<SampleSlot>() )
            selectedSlot = collision.GetComponent<SampleSlot>();   
    }

    public void OnTriggerExit( Collider collision )
    {
        selectedSlot = null;
    }

    public void OnUseHand( InputAction.CallbackContext value )
    {
        //when I click, look for a colliding sample and grab it
        if ( value.performed )
        {
            if ( selectedSlot != null && selectedSlot.sampleInSlot != null )
            {
                Debug.Log("Grab!");

                currentSample = selectedSlot.sampleInSlot;
                currentSample.transform.SetParent( gameObject.transform );
                currentSample.transform.localPosition = Vector3.zero;
                selectedSlot.OnItemRemoved( currentSample );
            }
        }

        //When I let loose, look for an empty slot and slot it
        if ( value.canceled )
        {

            if ( selectedSlot == null && currentSample != null )
            {
                Debug.Log("Drop!");
                currentSample.transform.SetParent(null);
                currentSample.GetComponent<Rigidbody>().isKinematic = false;
                currentSample.GetComponent<Rigidbody>().useGravity = true;
                currentSample.GetComponent<Collider>().enabled = true;

                currentSample = null;
                selectedSlot = null;
            }

            if ( selectedSlot != null && selectedSlot.sampleInSlot == null && currentSample != null )
            {
                Debug.Log("Slot!");
                selectedSlot.OnItemSlotted( currentSample );
                currentSample.transform.SetParent( selectedSlot.transform );
                currentSample.transform.localPosition = Vector3.zero;
                currentSample.transform.localRotation = Quaternion.identity;
                currentSample.transform.localScale = Vector3.one;
                selectedSlot = null;
                currentSample = null;               
            }
        }

        //Else, drop it on the ground
    }
}
