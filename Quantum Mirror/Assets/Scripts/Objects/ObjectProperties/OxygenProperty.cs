using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenProperty : ObjectProperty
{

	public OxygenDetector oxygenDetector;
	public List<ObjModifier> modifiers;
	public float oxydizationPoint;
	public float oxydizationDuration;
	public float deoxydizationDuration;

	private float t;

	private void Update()
	{
		if ( oxygenDetector.oxygenLevels > oxydizationPoint )
			t += Time.deltaTime / oxydizationDuration;
		else
			t -= Time.deltaTime / deoxydizationDuration;
		Mathf.Clamp01( t );

		for ( int i = 0; i < modifiers.Count; i++ )
		{
			modifiers[ i ].ModifyObject( t );
		}
	}
}
