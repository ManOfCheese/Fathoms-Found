using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleTool : MonoBehaviour
{

	public BoolValue isInGestureMode;

	[Header( "Sampler  Settings" )]
	public GameObject sampleTool;
	public Transform ballTransform;
	public GameObject sampleBallPrefab;
	public float sampleDuration;
	public float scaleModifier;

	[Header( "Sensor Settings" )]
	public Transform raycastFrom;
	public GameObject sensor;
	public LayerMask layerMask;
	public float sensorRange;

	private Object objectInRay;
	private GameObject sampleBall;

	private void Update()
	{
		RaycastHit hit;
		if ( Physics.Raycast( raycastFrom.position, raycastFrom.forward, out hit, sensorRange, layerMask ) )
		{
			sensor.transform.position = hit.point;
			if ( hit.transform.gameObject.GetComponent<Object>() != null )
				objectInRay = hit.transform.gameObject.GetComponent<Object>();
		}
		else
		{
			sensor.transform.position = raycastFrom.transform.position + ( raycastFrom.forward * sensorRange );
			objectInRay = null;
		}
	}

	public void Sample( InputAction.CallbackContext value )
	{
		if ( value.performed && objectInRay != null && !isInGestureMode.Value )
		{
			sampleBall = Instantiate( sampleBallPrefab, ballTransform.position, ballTransform.rotation, ballTransform.transform );
			Transform sample = Instantiate( objectInRay.objectVisuals, sampleBall.transform.position, sampleBall.transform.rotation, sampleBall.transform ).transform;
			sampleBall.gameObject.layer = 3;
			sample.gameObject.layer = 3;
			for ( int i = 0; i < sample.transform.childCount; i++ )
			{
				sample.transform.GetChild( i ).gameObject.layer = 3;
			}

			if ( sample.GetComponentInChildren<MeshRenderer>() && sampleBall.GetComponent<MeshRenderer>() )
			{
				MeshRenderer sampleCollider = sample.GetComponentInChildren<MeshRenderer>();
				MeshRenderer ballCollider = sampleBall.GetComponent<MeshRenderer>();
				sample.transform.localScale *=
					Mathf.Max( ballCollider.bounds.size.x, ballCollider.bounds.size.y, ballCollider.bounds.size.z ) /
					Mathf.Max( sampleCollider.bounds.size.x, sampleCollider.bounds.size.y, sampleCollider.bounds.size.z ) * scaleModifier;
				sampleBall = null;
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere( sensor.transform.position, 1f );
	}

}
