using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{

	[Header( "References" )]
	public Property propertyToDetect;
	[Tooltip( "If not empty this detector will save it's propertyValue into this variable object." )]
	public FloatValue variableObjectToUpdate;

	[Header( "Runtime" )]
    [ReadOnly] public float propertyValue;

	public List<Source> sources;

	private void Update()
	{
		propertyValue = 0f;

		for ( int i = 0; i < sources.Count; i++ )
		{
			if ( sources[i] == null )
			{
				sources.RemoveAt( i );
				continue;
			}
			float dist = Vector3.Distance( sources[ i ].transform.position, transform.position );
			float perc = dist / sources[ i ].sphereCollider.bounds.extents.y;
			float oxygenLevel = sources[ i ].valueAtCentre * sources[ i ].fallOff.Evaluate( perc );
			propertyValue += oxygenLevel;
		}
		if ( variableObjectToUpdate != null )
			variableObjectToUpdate.Value = propertyValue;
	}

}
