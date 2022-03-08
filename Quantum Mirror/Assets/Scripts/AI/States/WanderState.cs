using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class WanderState : State<AlienManager>
{
	#region singleton
	//Create a single instance of this state for all state machines.
	private static WanderState _instance;

	private WanderState()
	{
		if ( _instance != null )
		{
			return;
		}

		_instance = this;
	}

	public static WanderState Instance
	{
		get
		{
			if ( _instance == null )
			{
				new WanderState();
			}

			return _instance;
		}
	}
	#endregion

	public override void EnterState( AlienManager _owner )
	{
		_owner.moveController.EvaluateMovement( true );
	}

	public override void UpdateState( AlienManager _owner )
	{
		_owner.moveController.EvaluateMovement( false );
	}

	public override void ExitState( AlienManager _owner )
	{

	}
}
