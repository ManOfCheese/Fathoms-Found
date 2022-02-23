using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : InteractableObject
{
    
    public bool isActivated;
    public Animator animator;
    float t = 0;
    public PickUpObject objInOven;
    public Transform ovenPos;
    public bool isInOven = false;

    private void Update()
	{
        if ( isInOven )
        {
            t += Time.deltaTime / 5.0f; // Divided by 5 to make it 5 seconds.
            Debug.Log( t );
            objInOven.GetComponent<Renderer>().material.color = Color.Lerp( Color.cyan, Color.red, t );
        }
    }

	public override void Interact( Interactor player )
	{
        if ( objInOven ) {
            objInOven.rb.isKinematic = false;
            objInOven.rb.useGravity = true;
            for ( int i = 0; i < objInOven.colliders.Length; i++ ) {
                objInOven.colliders[ i ].enabled = true;
            }
            player.PickUp( objInOven );
            objInOven = null;
            isInOven = false;
		}
        else if ( player.objectInHand ) {
            objInOven = player.objectInHand;
            objInOven.rb.isKinematic = true;
            objInOven.rb.useGravity = false;
			for ( int i = 0; i < objInOven.colliders.Length; i++ ) {
                objInOven.colliders[ i ].enabled = false;
			}
            player.OnDrop();
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
