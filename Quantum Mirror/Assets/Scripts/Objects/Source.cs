using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour
{

	public Property sourceOf;
	public float valueAtCentre;
	public AnimationCurve fallOff;

	[HideInInspector] public SphereCollider sphereCollider;

	private void Awake()
	{
		sphereCollider = GetComponent<SphereCollider>();
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.GetComponentInChildren<Detector>() )
		{
			Detector[] detectors = other.gameObject.GetComponentsInChildren<Detector>();

			for ( int i = 0; i < detectors.Length; i++ )
			{
				if ( sourceOf == detectors[ i ].propertyToDetect )
					detectors[ i ].sources.Add( this );
			}
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.gameObject.GetComponentInChildren<Detector>() )
		{
			Detector[] detectors = other.gameObject.GetComponentsInChildren<Detector>();

			for ( int i = 0; i < detectors.Length; i++ )
			{
				if ( sourceOf == detectors[ i ].propertyToDetect )
					detectors[ i ].sources.Remove( this );
			}
		}
	}
}
