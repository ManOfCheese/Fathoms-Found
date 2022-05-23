using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour {

    [Header( "Settings" )]
    public int interactRange;
    public LayerMask interactMask;

    [Header( "References" )]
    public Animator anim;
    public Transform handPosition;
    public GameObject cam;
    public Move_Action approachPlayer;
    public Move_Action runAway;

    [Header( "Read Only" )]
    [ReadOnly] public InteractableObject interactableObject;
    [ReadOnly] public PickUpObject pickUpObject;
    [ReadOnly] public PickUpObject objectInHand;

    public delegate void EmptyVoidAction();
    public static event EmptyVoidAction LookAtNPC;
    public static event EmptyVoidAction LookAway;

    public delegate void ObjectAction( ObjectType obj );
    public static event ObjectAction PickUpObject;
    public static event ObjectAction DropObject;
    public static event ObjectAction UseObject;

    private AlienManager lookingAt;

    // Update is called once per frame
    void Update() 
    {
        RaycastHit hit;
        if ( Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity ) ) {
			if ( hit.transform.GetComponentInChildren<AlienManager>() ) {
				if ( lookingAt == null ) {
                    AlienManager lookingAt = hit.transform.GetComponentInChildren<AlienManager>();
                    lookingAt.OnLookAt();
                }
			}
            else if ( lookingAt != null ) {
                lookingAt.OnLookAway();
                lookingAt = null;
            }
        }

		pickUpObject = null;
        interactableObject = null;

        //Check for object to interact with or pick up.
        if ( Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, interactRange, interactMask ) )
		{
            if ( objectInHand == null && hit.collider.GetComponent<PickUpObject>() )
			{
                pickUpObject = hit.collider.GetComponent<PickUpObject>();
            }
            else if ( hit.collider.GetComponent<InteractableObject>() )
			{
                interactableObject = hit.collider.GetComponent<InteractableObject>();
            }
		}
    }

    public void PickUp( PickUpObject obj ) 
    {
        obj.transform.parent = handPosition.transform;
        obj.transform.localPosition = obj.pickUpPoint.localPosition;
        obj.transform.localRotation = Quaternion.Euler( obj.inHandOrientation );
        obj.GetComponent<Rigidbody>().isKinematic = true;
        objectInHand = obj;
        obj = null;
        PickUpObject?.Invoke( objectInHand.objectType );
    }

    public void OnInteract( InputAction.CallbackContext value )
	{
        if ( value.performed ) {
            if ( interactableObject ) {
                DropObject?.Invoke( objectInHand.objectType );
                interactableObject.Interact( this );
            }
            else if ( pickUpObject ) {
                PickUp( pickUpObject );
            }
            else if( objectInHand ) {
                anim.SetTrigger( "UseObject" );
                UseObject?.Invoke( objectInHand.objectType );
            }
        }
	}

    public void OnDrop()
	{
        if ( objectInHand )
		{
            objectInHand.transform.parent = null;
            objectInHand.GetComponent<Rigidbody>().isKinematic = false;
            DropObject?.Invoke( objectInHand.objectType );
            objectInHand = null;
        }
	}
}
