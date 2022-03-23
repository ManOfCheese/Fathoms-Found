using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    
	public bool interactOnEnter;
	public bool interactOnStay;
	public bool interactOnExit;

	public virtual void Interact( Interactor interactor )
    {

    }

	private void OnTriggerEnter( Collider other )
	{
		if ( interactOnEnter && other.GetComponentInChildren<Interactor>() )
		{
			Interact( other.GetComponentInChildren<Interactor>() );
		}
	}

	private void OnTriggerStay( Collider other )
	{
		if ( interactOnStay && other.GetComponentInChildren<Interactor>() )
		{
			Interact( other.GetComponentInChildren<Interactor>() );
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( interactOnExit && other.GetComponentInChildren<Interactor>() )
		{
			Interact( other.GetComponentInChildren<Interactor>() );
		}
	}

}
