using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{

	[Header( "References" )]
	public Property propertyToDetect;

	[Header( "Runtime" )]
    [ReadOnly] public float propertyValue;

	public List<Source> sources;

	private void Update()
	{
		propertyValue = 0f;

		for ( int i = 0; i < sources.Count; i++ )
		{
			float dist = Vector3.Distance( sources[ i ].transform.position, transform.position );
			float perc = dist / sources[ i ].sphereCollider.bounds.extents.y;
			Debug.Log( dist + " | " + sources[ i ].sphereCollider.bounds.extents.y + " | " + perc );
			float oxygenLevel = sources[ i ].valueAtCentre * sources[ i ].fallOff.Evaluate( perc );
			propertyValue += oxygenLevel;
		}
	}

}
