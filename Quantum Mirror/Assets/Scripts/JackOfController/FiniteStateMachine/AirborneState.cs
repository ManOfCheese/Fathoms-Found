using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class AirborneState : State<JackOfManager> {

	#region singleton
	//Create a single instance of this state for all state machines.
	private static AirborneState _instance;

	private AirborneState() {
		if ( _instance != null ) {
			return;
		}

		_instance = this;
	}

	public static AirborneState Instance {
		get {
			if ( _instance == null ) {
				new AirborneState();
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
		_owner.joc.LookMove();
		_owner.joc.Gravity();
		_owner.joc.Jump();
		_owner.joc.CheckGround();
	}

	public override void ExitState( JackOfManager _owner ) {

	}
}
