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
	public float sampleInBallScaleModifier;
	public float sampleInRayScaleModifier;
	public float ejaculationStrength;
	public Color reticleColor;

	[Header( "Sensor Settings" )]
	public Transform raycastFrom;
	public GameObject sensor;
	public LayerMask layerMask;
	public float sensorRange;

	[ReadOnly] public GameObject objectInRay;
	[ReadOnly] public Object sampleInGun;

	private GameObject samplingObject;
	private Vector3 lerpFrom;
	private Vector3 lerpTo;
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
		}
		else
		{
			sensor.transform.position = raycastFrom.transform.position + ( raycastFrom.forward * sensorRange );
			objectInRay = null;
		}


		//Move the sample along the path of the tractor beam.
		if ( samplingObject != null && startSuckingTimeStamp != 0f && Time.time - startSuckingTimeStamp < sampleDuration )
		{
			Debug.Log( ( Time.time - startSuckingTimeStamp ) / sampleDuration );
			float totalDistance = Vector3.Distance( objectInRay.transform.position, this.transform.position );
			float distanceLeft = Vector3.Distance( samplingObject.transform.position, this.transform.position );
			float timeLeft = sampleDuration - ( Time.time - startSuckingTimeStamp );

			samplingObject.transform.position += ( this.transform.position - samplingObject.transform.position ).normalized * ( distanceLeft / timeLeft * Time.deltaTime ); 
		}

		if ( sucking && startSuckingTimeStamp != 0f && Time.time - startSuckingTimeStamp >= sampleDuration )
		{
			Destroy( samplingObject );
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
				Mathf.Max( sampleInGun.meshRenderer.bounds.size.x, sampleInGun.meshRenderer.bounds.size.y, sampleInGun.meshRenderer.bounds.size.z ) * sampleInBallScaleModifier;

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
		if ( sampleInGun != null )
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
				samplingObject = Instantiate( objectInRay, objectInRay.transform.position, objectInRay.transform.rotation );
				MeshRenderer samplingRenderer = samplingObject.GetComponentInChildren<MeshRenderer>();
				float biggestDimension = Mathf.Max( samplingRenderer.bounds.size.x, samplingRenderer.bounds.size.y, samplingRenderer.bounds.size.z );
				samplingObject.transform.localScale = Vector3.one / biggestDimension * sampleInRayScaleModifier;
				samplingObject.layer = 3;
				lerpFrom = objectInRay.transform.position;
				lerpTo = this.transform.position;
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
			Destroy( samplingObject );
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
