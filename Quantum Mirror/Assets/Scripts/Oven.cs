using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : InteractableObject
{
    
    public bool isActivated;
    public Animator animator;
    float t = 0;
    public PickUpObject objInOven;
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

	public override void Interact( ItemGrabber player )
	{
        if ( player.objectInHand != null )
		{
            objInOven = player.objectInHand;
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
