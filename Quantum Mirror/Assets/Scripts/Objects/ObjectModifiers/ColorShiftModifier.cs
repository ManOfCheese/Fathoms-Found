using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShiftModifier : ObjModifier
{

	[Header( "References" )]
	public Renderer objRenderer;

	[Header( "Settings" )]
	public Color lerpTo;

	private Material material;
	private Color startColor;

	public override void OnStart( Object obj )
	{
		base.OnStart( obj );
		canUseSpeed = false;
		material = objRenderer.material;
	}

	public override void WhileThresholdCrossed()
	{
		base.WhileThresholdCrossed();
		
		t += Time.deltaTime / changeDuration;
		Mathf.Clamp01( t );
		ModifyObjectPerc( t );
	}

	public override void WhileThresholdNotCrossed()
	{
		base.WhileThresholdNotCrossed();
		
		startColor = material.color;
		t = 0f;
	}

	public override void ModifyObjectPerc( float t )
	{
		base.ModifyObjectPerc( t );
		material.color = Color.Lerp( startColor, lerpTo, t );
	}
}
