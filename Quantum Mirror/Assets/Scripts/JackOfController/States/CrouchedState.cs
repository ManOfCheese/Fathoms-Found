using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class CrouchedState : State<JackOfManager> {

	#region singleton
	//Create a single instance of this state for all state machines.
	private static CrouchedState _instance;

	private CrouchedState() {
		if ( _instance != null ) {
			return;
		}

		_instance = this;
	}

	public static CrouchedState Instance {
		get {
			if ( _instance == null ) {
				new CrouchedState();
			}

			return _instance;
		}
	}
	#endregion

	public override void EnterState( JackOfManager _owner ) {
		JackOfController joc = _owner.joc;
		CrouchModule cm = _owner.modulesByName[ "CrouchModule" ] as CrouchModule;

		_owner.cam.transform.localPosition = new Vector3( _owner.cam.transform.localPosition.x, cm.crouchCamHeight, 
			_owner.cam.transform.localPosition.z );
		joc.currentCamHeight = cm.crouchCamHeight;
		_owner.cc.height = cm.crouchPlayerHeight;
		_owner.cc.Move( Vector3.down );

		if ( cm.crouchSpeed != 0f )
			_owner.joc.currentSpeed = cm.crouchSpeed;
		else
			_owner.joc.currentSpeed = _owner.joc.speed * cm.relativeCrouchSpeed;
	}

	public override void UpdateState( JackOfManager _owner ) {
		CrouchModule cm = _owner.modulesByName[ "CrouchModule" ] as CrouchModule;

		_owner.joc.CameraLook();
		_owner.joc.Walk();
		_owner.joc.Gravity();
		_owner.joc.CheckGround();
		if ( cm.crouchCanceled ) {
			if ( !cm.CheckCeiling() ) {
				_owner.stateMachine.ChangeState( _owner.statesByName[ "GroundedState" ] );
				cm.crouchCanceled = false;
			}
		}
	}

	public override void ExitState( JackOfManager _owner ) {
		_owner.cam.transform.localPosition = new Vector3( _owner.cam.transform.localPosition.x,
			_owner.joc.camStartHeight, _owner.cam.transform.localPosition.z );
		_owner.joc.currentCamHeight = _owner.joc.camStartHeight;
		_owner.cc.height = _owner.joc.playerStartHeight;
		_owner.joc.currentSpeed = _owner.joc.speed;
	}
}
