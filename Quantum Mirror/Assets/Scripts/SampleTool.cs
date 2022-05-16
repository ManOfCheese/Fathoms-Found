using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleTool : MonoBehaviour
{

	[Header( "References" )]
	public BoolValue isInGestureMode;
	public GameObject sampleBallPrefab;
	public GameObject sampleTool;
	public SampleSlot sampleSlot;
	public Transform shootFrom;

	[Header( "Sampler  Settings" )]
	public bool duplicateSample;
	public float sampleDuration;
	public float scaleModifier;
	public float ejectionStrength;

	[Header( "Sensor Settings" )]
	public Transform raycastFrom;
	public GameObject sensor;
	public LayerMask layerMask;
	public float sensorRange;

	[ReadOnly] private Object objectInRay;
	[ReadOnly] private Object sampleInGun;

	private void OnEnable()
	{
		sampleSlot.itemRemoved += OnItemRemoved;
		sampleSlot.itemSlotted += OnItemSlotted;
	}

	private void OnDisable()
	{
		sampleSlot.itemRemoved -= OnItemRemoved;
		sampleSlot.itemSlotted -= OnItemSlotted;
	}


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
		if ( value.performed && objectInRay != null && !isInGestureMode.Value && sampleSlot.sampleInSlot == null )
		{
			sampleSlot.sampleInSlot = Instantiate( sampleBallPrefab, sampleSlot.transform.position, sampleSlot.transform.rotation, sampleSlot.transform );
			Transform sample = Instantiate( objectInRay.transform.parent, sampleSlot.sampleInSlot.transform.position, sampleSlot.sampleInSlot.transform.rotation, 
				sampleSlot.sampleInSlot.transform ).transform;
			sampleInGun = sample.GetComponentInChildren<Object>();
			sampleInGun.Seal();

			sampleInGun.gameObject.layer = 3;
			sample.gameObject.layer = 3;
			for ( int i = 0; i < sample.transform.childCount; i++ )
				sample.transform.GetChild( i ).gameObject.layer = 3;

			if ( sample.GetComponentInChildren<MeshRenderer>() && sampleInGun.GetComponent<MeshRenderer>() )
			{
				MeshRenderer sampleCollider = sample.GetComponentInChildren<MeshRenderer>();
				MeshRenderer ballCollider = sampleInGun.GetComponent<MeshRenderer>();
				sample.transform.localScale *=
					Mathf.Max( ballCollider.bounds.size.x, ballCollider.bounds.size.y, ballCollider.bounds.size.z ) /
					Mathf.Max( sampleCollider.bounds.size.x, sampleCollider.bounds.size.y, sampleCollider.bounds.size.z ) * scaleModifier;
			}

			if ( !duplicateSample )
				Destroy( objectInRay.transform.parent.gameObject );
		}
	}

	public void Eject( InputAction.CallbackContext value )
	{
		if ( value.performed && !isInGestureMode.Value && sampleSlot.sampleInSlot != null )
		{
			GameObject sample = sampleSlot.sampleInSlot;
			sample.transform.SetParent( null );
			Rigidbody sampleRB = sampleSlot.sampleInSlot.GetComponent<Rigidbody>();
			sampleRB.isKinematic = false;
			sampleRB.useGravity = true;
			sample.transform.position = shootFrom.position;
			sampleRB.AddForce( shootFrom.transform.forward * ejectionStrength, ForceMode.Impulse );
			sample.GetComponent<Collider>().enabled = true;
			sampleInGun.Unseal();

			sampleSlot.OnItemRemoved( sampleSlot.sampleInSlot );
			sampleSlot.sampleInSlot = null;
		}
	}

	public void OnItemSlotted( GameObject _object )
	{
		sampleInGun = _object.GetComponentInChildren<Object>();
	}

	public void OnItemRemoved( GameObject _object )
	{
		sampleInGun = null;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere( sensor.transform.position, 1f );
	}

}
