using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TremorSource : MonoBehaviour
{
	
	[Header( "References" )]
	public SphereCollider sphereCollider;
	public FloatValue tremorFallOff;

	[Header( "Settings" )]
	public float tremorIntensity;
	public bool tremorOnCollision;

    [HideInInspector] public List<AlienManager> alienListeners;

	private void Awake()
	{
		alienListeners = new List<AlienManager>();
	}

	private void OnValidate()
	{
		sphereCollider.radius = tremorIntensity;
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.GetComponent<AlienManager>() )
			alienListeners.Add( other.GetComponent<AlienManager>() );
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.GetComponent<AlienManager>() )
			alienListeners.Remove( other.GetComponent<AlienManager>() );
	}

	private void OnCollisionEnter( Collision collision )
	{
		if ( tremorOnCollision )
		{
			if ( collision.collider.GetComponent<Object>() )
			{
				Object other = collision.collider.GetComponent<Object>();
				sphereCollider.radius = tremorIntensity * other.hardness;
			}
			else
				sphereCollider.radius = tremorIntensity;

			Tremor();
		}
	}

	public void SetTremorIntesity( float _intensity )
	{
		tremorIntensity = _intensity;
		sphereCollider.radius = tremorIntensity;
	}

	public void Tremor()
	{
		for ( int i = 0; i < alienListeners.Count; i++ )
		{
			alienListeners[ i ].OnTremor( this.transform.position, sphereCollider.radius /
				( Vector3.Distance( this.transform.position, alienListeners[ i ].transform.position ) * tremorFallOff.Value ) );
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.gray;
		Gizmos.DrawWireSphere( transform.position, tremorIntensity );
	}

}
