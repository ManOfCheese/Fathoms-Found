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
	}

	public override void UpdateState( HandController _o )
	{
		for ( int j = 0; j < _o.fingerAnimators.Length; j++ )
			_o.fingerAnimators[ j ].SetBool( "FingerOpen", false );

		if ( _o.moveState == MoveState.Holding )
		{
			if ( Time.time - _o.holdTimeStamp > _o.holdPointFor )
				_o.moveState = MoveState.Moving;
		}
		else if ( _o.moveState == MoveState.Moving )
		{
			float speed = _o.pointSpeed * Time.deltaTime;
			if ( speed > Vector3.Distance( _o.handTransform.position, _o.handTarget ) )
			{
				_o.transform.position = _o.handTarget;

				if ( _o.handTarget != _o.idleHandTarget.position )
				{
					_o.holdTimeStamp = Time.time;
					_o.moveState = MoveState.Holding;
					_o.handTarget = _o.idleHandTarget.position;
				}
			}
			else
				_o.handTransform.position = Vector3.MoveTowards( _o.handTransform.position, _o.handTarget, speed );
		}
	}

	public override void ExitState( HandController _o )
	{
		_o.ikManager.handsAvailable++;
	}
}
