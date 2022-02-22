using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class GroundedState : State<JackOfManager> {

	#region singleton
	//Create a single instance of this state for all state machines.
	private static GroundedState _instance;

	private GroundedState() {
		if ( _instance != null ) {
			return;
		}

		_instance = this;
	}

	public static GroundedState Instance {
		get {
			if ( _instance == null ) {
				new GroundedState();
			}

			return _instance;
		}
	}
	#endregion

	public override void EnterState( JackOfManager _owner ) {

	}

	public override void UpdateState( JackOfManager _owner ) {
		_owner.joc.CameraLook();
		_owner.joc.CheckSprint();
		_owner.joc.Walk();
		_owner.joc.Gravity();
		_owner.joc.Jump();
		_owner.joc.CheckGround();
	}

	public override void ExitState( JackOfManager _owner ) {

	}
}
