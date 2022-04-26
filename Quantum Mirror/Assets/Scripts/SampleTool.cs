using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTool : MonoBehaviour
{

	public Transform raycastFrom;
	public GameObject sensor;
	public LayerMask layerMask;
	public float sensorRange;

	private void Update()
	{
		RaycastHit hit;
		if ( Physics.Raycast( raycastFrom.position, raycastFrom.forward, out hit, sensorRange, layerMask ) )
			sensor.transform.position = hit.point;
		else
			sensor.transform.position = raycastFrom.transform.position + ( raycastFrom.forward * sensorRange );
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere( sensor.transform.position, 1f );
	}

}
