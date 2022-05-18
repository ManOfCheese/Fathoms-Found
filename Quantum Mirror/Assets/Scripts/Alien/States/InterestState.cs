using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class InterestState : State<AlienManager>
{
	#region singleton
	//Create a single instance of this state for all state machines.
	private static InterestState _instance;

	private InterestState()
	{
		if ( _instance != null )
		{
			return;
		}

		stateName = "InterestState";
		_instance = this;
	}

	public static InterestState Instance
	{
		get
		{
			if ( _instance == null )
			{
				new InterestState();
			}

			return _instance;
		}
	}
	#endregion

	public override void EnterState( AlienManager _owner )
	{

	}

	public override void UpdateState( AlienManager _owner )
	{
		if ( _owner.mc.agent.remainingDistance == _owner.mc.tolerance )
		{
			_owner.interest = true;
			_owner.interestTimeStamp = Time.time;
		}

		if ( _owner.interest )
		{
			if ( Time.time - _owner.interestTimeStamp > _owner.interestDuration )
			{
				_owner.interest = false;
				_owner.stateMachine.ChangeState( WanderState.Instance );
			}
		}

		_owner.TryDoor();
	}

	public override void ExitState( AlienManager _owner )
	{

	}
}
