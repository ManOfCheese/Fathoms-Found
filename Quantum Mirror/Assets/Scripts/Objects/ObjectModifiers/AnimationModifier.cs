using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimParameterType
{
    Trigger,
    Bool,
    Float,
    Int
}

public class AnimationModifier : ObjModifier
{

    [Header( "References" )]
    public Animator animator;

    [Header( "Settings" )]
    [Tooltip( "What is the type of parameter you want to change in the animator." )]
    public AnimParameterType parameterType;
	[Tooltip( "The name of the parameter in the animator you want to change." )]
	public string parameterName;
	[Tooltip( "The value you want to give to the bool parameter." )]
	public bool boolParameter;
	[Tooltip( "If true the modifier will change the parameter over time using the changeSpeed or duration instead of setting it once when the threshold is crossed." )]
	public bool changeOverTime;
	[Tooltip( "If true it will set the value equal to the values detected by the detector." )]
	public bool linkToDetector;
	[Tooltip( "The value you want to give to the float parameter." )]
	public float floatParameter;
	[Tooltip( "The value you want to give to the int parameter." )]
	public int intParameter;
	[Tooltip( "The lowest value that the animator parameter is allowed to have." )]
	public float minValue;
	[Tooltip( "The highest value that the animator parameter is allowed to have." )]
	public float maxValue;
	[Tooltip( "The lowest value that the animator parameter is allowed to have." )]
	public int minValueInt;
	[Tooltip( "The highest value that the animator parameter is allowed to have." )]
	public int maxValueInt;

	[ReadOnly] public float floatValue;
	[ReadOnly] public int intValue;

	public override void OnStart( Object obj )
	{
		base.OnStart( obj );
		canUseSpeed = false;
	}

	public override void UpdateProperty()
	{
		base.UpdateProperty();

		if ( !linkToDetector ) { return; }

		switch ( parameterType )
		{
			case AnimParameterType.Trigger:
				break;
			case AnimParameterType.Bool:
				break;
			case AnimParameterType.Float:
				animator.SetFloat( parameterName, detector.propertyValue );
				break;
			case AnimParameterType.Int:
				animator.SetInteger( parameterName, Mathf.RoundToInt( detector.propertyValue ) );
				break;
			default:
				break;
		}
	}

	public override void OnThresholdCross()
	{
		base.OnThresholdCross();

		if ( linkToDetector ) { return; }
		switch ( parameterType )
		{
			case AnimParameterType.Trigger:
				animator.SetTrigger( parameterName );
				break;
			case AnimParameterType.Bool:
				animator.SetBool( parameterName, boolParameter );
				break;
			case AnimParameterType.Float:
				if ( !changeOverTime )
					animator.SetFloat( parameterName, floatParameter );
				break;
			case AnimParameterType.Int:
				if ( !changeOverTime )
					animator.SetInteger( parameterName, intParameter );
				break;
			default:
				break;
		}
	}

	public override void OnThresholdUncross()
	{
		base.OnThresholdUncross();

		floatValue = 0f;
	}

	public override void WhileThresholdCrossed()
	{
		base.WhileThresholdCrossed();

		if ( linkToDetector ) { return; }
		switch ( parameterType )
		{
			case AnimParameterType.Float:
				if ( changeOverTime )
				{
					floatValue = UpdateFloatParameter( floatValue );
					animator.SetFloat( parameterName, floatValue );
				}
				break;
			case AnimParameterType.Int:
				if ( changeOverTime )
				{
					intValue = Mathf.RoundToInt( UpdateFloatParameter( intValue ) );
					animator.SetInteger( parameterName, intValue );
				}
				break;
			default:
				break;
		}
	}

	public float UpdateFloatParameter( float parameter )
	{
		switch ( changeMode )
		{
			case ChangeMode.Speed:
				parameter += changeSpeed * Time.deltaTime;
				parameter = Mathf.Clamp( parameter, minValue, maxValue );
				break;
			case ChangeMode.Duration:
				parameter += Time.deltaTime / changeDuration;
				parameter = Mathf.Clamp01( parameter );
				break;
			default:
				break;
		}
		return parameter;
	}
}
