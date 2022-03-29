using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleModifier : ObjModifier
{

    public Transform objTransform;
    public Vector3 startScale;
    public Vector3 minScale;
    public Vector3 maxScale;

	public override void OnStart( Object obj )
	{
		objTransform.localScale = startScale;
	}

	public override void ModifyObjectPerc( float t )
	{
		base.ModifyObjectPerc( t );
		objTransform.localScale = Vector3.Lerp( minScale, maxScale, t );
	}

	public override void ModifyObjectStep( float t )
	{
		base.ModifyObjectPerc( t );
		float newMagnitude = objTransform.localScale.magnitude * t;
		newMagnitude = Mathf.Clamp( newMagnitude, minScale.magnitude, maxScale.magnitude );
		objTransform.localScale = objTransform.localScale.normalized * newMagnitude;
	}
}
