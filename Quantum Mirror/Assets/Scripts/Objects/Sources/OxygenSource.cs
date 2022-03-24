using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenSource : MonoBehaviour
{

    public float oxygenAtCentre;
    public AnimationCurve oxygenFallOff;

	[HideInInspector] public SphereCollider sphereCollider;

	private void Awake() {
		sphereCollider = GetComponent<SphereCollider>();
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.GetComponentInChildren<OxygenDetector>() ) 
		{
			other.gameObject.GetComponentInChildren<OxygenDetector>().oxygenSources.Add( this );
		}
	}

	private void OnTriggerExit( Collider other ) 
	{
		if ( other.gameObject.GetComponentInChildren<OxygenDetector>() ) {
			other.gameObject.GetComponentInChildren<OxygenDetector>().oxygenSources.Remove( this );
		}
	}

}
