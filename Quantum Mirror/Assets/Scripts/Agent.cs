using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentType
{
	Player,
	Crab
}

public class Agent : MonoBehaviour
{

	[Header( "Settings" )]
	public float lookRange;
	public Transform raycastFrom;
	public LayerMask layerMask;
	public AgentType agentType;
	public string agentName;

	[Header( "References" )]
	public RunTimeSet<Agent> agents;
	[ReadOnly] public List<Agent> lookedAtBy;
	[ReadOnly] public Agent lookingAt;
	private void Awake()
	{
		agents.Add( this );
		if ( raycastFrom == null )
			raycastFrom = this.transform;
	}

	private void Update()
	{
		RaycastHit hit;
		if ( Physics.Raycast( raycastFrom.transform.position, raycastFrom.transform.forward, out hit, lookRange, layerMask ) )
		{
			if ( hit.transform.GetComponent<Agent>() )
			{
				Agent other = hit.transform.GetComponent<Agent>();
				if ( other != lookingAt )
				{
					if ( lookingAt != null )
						lookingAt.lookedAtBy.Remove( this );
					lookingAt = other;
					other.lookedAtBy.Add( this );
				}
			}
			else
				lookingAt.lookedAtBy.Remove ( this );
		}
	}

}
