using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShiftModifier : ObjModifier
{

	public Renderer renderer;
	public bool lerpFromCurrentColor;
	public Color startColor;
	public Color endColor;

	public override void ModifyObjectPerc( float t )
	{
		base.ModifyObjectPerc( t );
		Material mat = renderer.material;
		mat.color = Color.Lerp( startColor, endColor, t );
	}

}
