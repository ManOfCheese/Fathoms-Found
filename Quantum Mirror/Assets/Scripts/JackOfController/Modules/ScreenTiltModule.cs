using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenTiltModule : Module {

	public ScreenTiltSystem system;

	[Tooltip( "How many degrees does the screen tilt." )]
	public float maximumTilt;
	[Tooltip( "How fast does the tilting happen." )]
	public float tiltSpeed;

	[HideInInspector] public Vector2 rawMovementVector;

	[ReadOnly] public float tilt;

	protected override void Awake() {
		system.stm = this;
	}

	public void OnMove( InputAction.CallbackContext value ) {
		rawMovementVector = value.ReadValue<Vector2>();
	}

	public float Map( float value, float inputFrom, float inputTo, float outputFrom, float outputTo ) {
		return ( value - inputFrom ) / ( inputTo - inputFrom ) * ( outputTo - outputFrom ) + outputFrom;
	}
}
