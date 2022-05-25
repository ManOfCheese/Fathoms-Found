using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SampleTool : MonoBehaviour
{

	[Header( "References" )]
	public Animator toolAnimator;
	public BoolValue isInGestureMode;
	public GameObject tractorBeam;
	public GameObject sampleBallPrefab;
	public GameObject sampleTool;
	public RawImage reticle;
	public SampleSlot sampleSlot;
	public Transform shootFrom;

	[Header( "Sampler  Settings" )]
	public bool duplicateSample;
	public float sampleDuration;
	public float scaleModifier;
	public float ejaculationStrength;
	public Color reticleColor;

	[Header( "Sensor Settings" )]
	public Transform raycastFrom;
	public GameObject sensor;
	public LayerMask layerMask;
	public float sensorRange;

	[ReadOnly] public GameObject objectInRay;
	[ReadOnly] public Object sampleInGun;

	private float startSuckingTimeStamp = 0f;
	private bool sucking;
	private Color reticleStartColor;

	private void Awake()
	{
		reticleStartColor = reticle.color;
	}

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
		Debug.DrawRay( raycastFrom.position, raycastFrom.forward * sensorRange, Color.red );
		if ( Physics.Raycast( raycastFrom.position, raycastFrom.forward, out hit, sensorRange, layerMask ) )
		{
			sensor.transform.position = hit.point;
			//Debug.Log( hit.transform.gameObject.name );
			if ( hit.transform.gameObject.GetComponent<Object>() != null )
				objectInRay = hit.transform.parent.gameObject;
			else
				objectInRay = null;
		}
		else
		{
			sensor.transform.position = raycastFrom.transform.position + ( raycastFrom.forward * sensorRange );
			objectInRay = null;
		}

		if ( sucking && startSuckingTimeStamp != 0f && Time.time - startSuckingTimeStamp >= sampleDuration )
		{
			sampleSlot.sampleInSlot = Instantiate( sampleBallPrefab, sampleSlot.transform.position, sampleSlot.transform.rotation, sampleSlot.transform );
			Transform sample = Instantiate( objectInRay, sampleSlot.sampleInSlot.transform.position, sampleSlot.sampleInSlot.transform.rotation,
				sampleSlot.sampleInSlot.transform ).transform;
			sampleInGun = sample.GetComponentInChildren<Object>();
			sampleInGun.Seal();

			Destroy( sampleInGun.GetComponent<Rigidbody>() );
			Collider[] sampleColliders = sampleInGun.GetComponents<Collider>();
			for ( int i = 0; i < sampleColliders.Length; i++ )
				Destroy( sampleColliders[ i ] );

			sample.gameObject.layer = 3;
			for ( int i = 0; i < sample.transform.childCount; i++ )
				sample.transform.GetChild( i ).gameObject.layer = 3;

			MeshRenderer ballCollider = sampleSlot.sampleInSlot.GetComponent<MeshRenderer>();
			sampleInGun.meshRenderer.gameObject.transform.localScale *=
				Mathf.Max( ballCollider.bounds.size.x, ballCollider.bounds.size.y, ballCollider.bounds.size.z ) /
				Mathf.Max( sampleInGun.meshRenderer.bounds.size.x, sampleInGun.meshRenderer.bounds.size.y, sampleInGun.meshRenderer.bounds.size.z ) * scaleModifier;

			if ( !duplicateSample )
				Destroy( objectInRay.gameObject );

			sucking = false;
			startSuckingTimeStamp = 0f;
		}

		if ( objectInRay != null )
			reticle.color = reticleColor;
		else
			reticle.color = reticleStartColor;
	}

	public void Eject()
	{
		GameObject sample = sampleSlot.sampleInSlot;
		sample.transform.SetParent( null );
		Rigidbody sampleRB = sampleSlot.sampleInSlot.GetComponent<Rigidbody>();
		sampleRB.isKinematic = false;
		sampleRB.useGravity = true;
		sample.transform.position = shootFrom.position;
		sampleRB.AddForce( shootFrom.transform.forward * ejaculationStrength, ForceMode.Impulse );
		sample.GetComponent<Collider>().enabled = true;
		sampleInGun.Unseal();

		sampleSlot.OnItemRemoved( sampleSlot.sampleInSlot );
		sampleSlot.sampleInSlot = null;
		sampleInGun = null;
	}

	public void Sample( InputAction.CallbackContext value )
	{
		if ( value.performed && !isInGestureMode.Value && sampleSlot.sampleInSlot == null )
		{
			tractorBeam.SetActive( true );
			toolAnimator.SetBool( "Suck", true );
			if ( objectInRay != null )
			{
				startSuckingTimeStamp = Time.time;
				sucking = true;
			}
		}
		else if ( sampleSlot.sampleInSlot != null )
		{
			toolAnimator.SetTrigger( "Error" );
		}

		if ( value.canceled )
		{
			tractorBeam.SetActive( false );
			toolAnimator.SetBool( "Suck", false );
			startSuckingTimeStamp = 0f;
			sucking = false;
		}
	}

	public void StartEject( InputAction.CallbackContext value )
	{
		if ( value.performed && !isInGestureMode.Value && sampleSlot.sampleInSlot != null )
			toolAnimator.SetTrigger( "Eject" );
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
