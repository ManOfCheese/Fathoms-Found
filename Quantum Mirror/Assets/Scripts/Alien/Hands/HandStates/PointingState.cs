using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class PointingState : State<HandController>
{
	#region singleton
	//Create a single instance of this state for all state machines.
	private static PointingState _instance;

	private PointingState()
	{
		if ( _instance != null )
		{
			return;
		}

		_instance = this;
	}

	public static PointingState Instance
	{
		get
		{
			if ( _instance == null )
			{
				new PointingState();
			}

			return _instance;
		}
	}
	#endregion

	public override void EnterState( HandController _o )
	{
		_o.ikManager.handsAvailable--;
		_o.newLookAtTarget = _o.oldLookAtTarget;
		for ( int j = 0; j < _o.fingerAnimators.Length; j++ )
			_o.fingerAnimators[ j ].SetBool( "FingerOpen", false );
	}

	public override void UpdateState( HandController _o )
	{
		_o.handTransform.LookAt( new Vector3( _o.oldLookAtTarget.x, _o.oldLookAtTarget.y, -_o.oldLookAtTarget.z ) );

		float speed = _o.pointSpeed * Time.deltaTime;

		if ( _o.pointState == PointState.Holding )
		{
			if ( Time.time - _o.holdTimeStamp > _o.holdPointFor )
			{
				_o.pointState = _o.pointState = PointState.Ending;
				_o.handTarget = _o.idleHandTarget.position + ( new Vector3( 0f, 1f, 0f ) * _o.heightOffset );
				for ( int j = 0; j < _o.fingerAnimators.Length; j++ )
					_o.fingerAnimators[ j ].SetBool( "FingerOpen", true );
			}
		}
		else if ( _o.pointState == PointState.Starting )
		{
			//Move arm
			if ( speed > Vector3.Distance( _o.transform.position, _o.handTarget ) )
			{
				_o.transform.position = _o.handTarget;
				_o.holdTimeStamp = Time.time;
				_o.pointState = PointState.Holding;
			}
			else
				_o.transform.position = Vector3.MoveTowards( _o.transform.position, _o.handTarget, speed );

			_o.newLookAtTarget = _o.handTarget;
		}
		else if ( _o.pointState == PointState.Ending )
		{
			//Move arm
			if ( speed > Vector3.Distance( _o.transform.position, _o.handTarget ) )
			{
				_o.transform.position = _o.handTarget;
				_o.stateMachine.ChangeState( _o.ikManager.statesByName[ "WalkingState" ] );
			}
			else
				_o.transform.position = Vector3.MoveTowards( _o.transform.position, _o.handTarget, speed );

			_o.newLookAtTarget = _o.handTransform.position - ( _o.handTransform.forward * 2f );
		}

		//Move hand
		if ( _o.oldLookAtTarget != _o.newLookAtTarget )
		{
			if ( speed > Vector3.Distance( _o.oldLookAtTarget, _o.newLookAtTarget ) )
				_o.oldLookAtTarget = _o.newLookAtTarget;
			else
				_o.oldLookAtTarget = Vector3.MoveTowards( _o.oldLookAtTarget, _o.newLookAtTarget, speed );
		}
	}

	public override void ExitState( HandController _o )
	{
		_o.ikManager.handsAvailable++;
		_o.handTarget = Vector3.zero;
		_o.oldPosition = _o.handTransform.position;
		_o.currentPosition = _o.handTransform.position;
		_o.newPosition = _o.handTransform.position;
	}
}
