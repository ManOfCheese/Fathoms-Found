using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObserverType
{
	Player,
	Crab
}

public class Observer : MonoBehaviour
{

	[Header( "Settings" )]
	public float lookRange;
	public Transform raycastFrom;
	public LayerMask layerMask;
	public ObserverType observerType;
	public string observerName;

	[Header( "References" )]
	public FloatValue standardLookRange;
	public RunTimeSet<Observer> observers;

	[ReadOnly] public List<Observer> observedBy;
	[ReadOnly] public Observer observing;

	private void Awake()
	{
		observers.Add( this );
		if ( raycastFrom == null )
			raycastFrom = this.transform;
		if ( standardLookRange != null )
			lookRange = standardLookRange.Value;
	}

	private void Update()
	{
		RaycastHit hit;
		if ( Physics.Raycast( raycastFrom.transform.position, raycastFrom.transform.forward, out hit, lookRange, layerMask ) )
		{
			if ( hit.transform.GetComponent<Observer>() )
			{
				Observer other = hit.transform.GetComponent<Observer>();
				if ( other != observing )
				{
					if ( observing != null )
						observing.observedBy.Remove( this );
					observing = other;
					other.observedBy.Add( this );
				}
			}
			else if ( observing != null )
				observing.observedBy.Remove( this );
		}
	}

}
