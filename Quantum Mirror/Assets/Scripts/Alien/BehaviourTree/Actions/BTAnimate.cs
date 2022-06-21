using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTAnimate : BTActionNode
{

	public bool waitForAnimationToFinish;
	public string stateName;

	[Space( 10 )]

    public AnimParameterType animParameterType;
    public string paramName;
    public bool boolValue;
    public float floatValue;
    public int intValue;

	private bool hasEnteredState;

    protected override void OnStart() {
		switch ( animParameterType )
		{
			case AnimParameterType.Trigger:
				context.animator.SetTrigger( paramName );
				break;
			case AnimParameterType.Bool:
				context.animator.SetBool( paramName, boolValue );
				break;
			case AnimParameterType.Float:
				context.animator.SetFloat( paramName, floatValue );
				break;
			case AnimParameterType.Int:
				context.animator.SetInteger( paramName, intValue );
				break;
			default:
				break;
		}
	}

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
		if ( waitForAnimationToFinish )
		{
			if ( context.animator.GetCurrentAnimatorStateInfo( 0 ).IsName( stateName ) )
				hasEnteredState = true;

			if ( hasEnteredState )
			{
				if ( context.animator.GetCurrentAnimatorStateInfo( 0 ).normalizedTime > 1 && !context.animator.IsInTransition( 0 ) )
					return State.Success;
				else
					return State.Running;
			}
			else
				return State.Running;
		}
		else
			return State.Success;
    }
}
