using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleModifier : ObjModifier
{

    public Transform objTransform;
    public Vector3 minScale;
    public Vector3 maxScale;

	public override void ModifyObject( float t )
	{
		base.ModifyObject( t );
		objTransform.localScale = Vector3.Lerp( minScale, maxScale, t );
	}
}