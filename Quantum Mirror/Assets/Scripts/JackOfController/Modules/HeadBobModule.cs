using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobModule : Module {

	public HeadBobSystem system;

	[Tooltip( "HWhen true the head bob speed will change when sprinting." )]
	public bool adjustSpeedWhenSprinting;
    [Tooltip( "How quickly the camera moves up and down." )]
    public float headBobSpeed;
    [Tooltip( "How far up and down the camera moves, if  this is higher it the camera will also need to move faster to cover the distance." )]
    public float headBobIntensity;

	[HideInInspector] public float currentHeadBobSpeed;

	protected override void Awake() {
		system.hbm = this;
	}

}
