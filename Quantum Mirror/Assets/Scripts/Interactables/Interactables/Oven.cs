using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : InteractableObject
{
    

    [Header( "References" )]
    public Animator animator;
    public Transform ovenPos;

    [Header( "Runtime" )]
    [HideInInspector] public bool isActivated;
    [HideInInspector] public bool isInOven = false;
    [HideInInspector] public PickUpObject objInOven;

    private float t = 0;

    private void Update()
	{
        if ( isInOven )
        {
            t += Time.deltaTime / 5.0f; // Divided by 5 to make it 5 seconds.
            Debug.Log( t );
            objInOven.GetComponent<Renderer>().material.color = Color.Lerp( Color.cyan, Color.red, t );
            objInOven.temperature = t;
        }
    }

	public override void Interact( Interactor interactor )
	{
        if ( objInOven ) {
            objInOven.rb.isKinematic = false;
            objInOven.rb.useGravity = true;
            for ( int i = 0; i < objInOven.colliders.Length; i++ ) {
                objInOven.colliders[ i ].enabled = true;
            }
            interactor.PickUp( objInOven );
            objInOven = null;
            isInOven = false;
		}
        else if ( interactor.objectInHand ) {
            objInOven = interactor.objectInHand;
            objInOven.rb.isKinematic = true;
            objInOven.rb.useGravity = false;
			for ( int i = 0; i < objInOven.colliders.Length; i++ ) {
                objInOven.colliders[ i ].enabled = false;
			}
            interactor.OnDrop();
            objInOven.transform.position = ovenPos.position;
            isInOven = true;
        }

        if ( !isActivated )
        {
            isActivated = true;
            animator.SetBool( "isActivated", isActivated );
        }
        else if ( isActivated )
        {
            isActivated = false;
            animator.SetBool( "isActivated", isActivated );
        }
    }

}
