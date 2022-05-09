using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleModifier : Modifier
{

    public Transform objTransform;
	public bool linkToDetector;
	public float linkedScaleModifier = 1f;
	public Vector3 minScale;
    public Vector3 maxScale;

	[HideInInspector] public Vector3 startScale;

	public override void OnStart( Object obj )
	{
		base.OnStart( obj );
		
		objTransform.localScale = startScale;
	}

	public override void WhileThresholdCrossed()
	{
		base.WhileThresholdCrossed();

		if ( linkToDetector )
		{
			Vector3 newScale = startScale + ( Vector3.one * detector.propertyValue * linkedScaleModifier );
			objTransform.localScale = new Vector3( Mathf.Clamp( newScale.x, minScale.x, maxScale.x ), Mathf.Clamp( newScale.y, minScale.y, maxScale.y ), 
				Mathf.Clamp( newScale.z, minScale.z, maxScale.z ) );
		}
		else
		{
			if ( changeMode == ChangeMode.Duration )
			{
				t += Time.deltaTime / changeDuration;
				Mathf.Clamp01( t );
				ModifyObjectPerc( t );
			}
			else
			{
				t += Time.deltaTime * changeSpeed;
				Mathf.Clamp01( t );
				ModifyObjectStep( t );
			}
		}
	}

	public override void ModifyObjectPerc( float t )
	{
		base.ModifyObjectPerc( t );
		objTransform.localScale = Vector3.Lerp( startScale, maxScale, t );
	}

	public override void ModifyObjectStep( float t )
	{
		base.ModifyObjectPerc( t );
		float newMagnitude = objTransform.localScale.magnitude * t;
		newMagnitude = Mathf.Clamp( newMagnitude, startScale.magnitude, maxScale.magnitude );
		objTransform.localScale = objTransform.localScale.normalized * newMagnitude;
	}
}
