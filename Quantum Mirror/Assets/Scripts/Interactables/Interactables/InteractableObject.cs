using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public virtual void Interact( Interactor interactor )
    {

    }

	private void OnTriggerEnter( Collider other )
	{
		if ( other.GetComponent<Interactor>() )
		{
			Interact( other.GetComponent<Interactor>() );
		}
	}

}
