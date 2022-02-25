using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "HeadBob_System", menuName = "Systems/HeadBobSystem" )]
public class HeadBobSystem : ComponentSystem {

    public HeadBobModule hbm;

	public override void Init() {
		hbm.currentHeadBobSpeed = hbm.headBobSpeed;
	}

	public override void OnUpdate() {
		if ( hbm.adjustSpeedWhenSprinting ) {
			if ( hbm.jocManager.joc.sprinting ) {
				float speedDifferential;
				if ( hbm.jocManager.joc.relativeSprintSpeed != 0 )
					speedDifferential = hbm.jocManager.joc.speed * hbm.jocManager.joc.relativeSprintSpeed / hbm.jocManager.joc.speed;
				else
					speedDifferential = hbm.jocManager.joc.sprintSpeed / hbm.jocManager.joc.speed;

				hbm.currentHeadBobSpeed = hbm.headBobSpeed * speedDifferential;
			}
			else {
				hbm.currentHeadBobSpeed = hbm.headBobSpeed;
			}
		}

		if ( hbm.jocManager.joc.rawMovementVector != Vector2.zero && hbm.jocManager.joc.grounded ) {
			//Use a sine functions to move the camera up and down.
			hbm.jocManager.cam.transform.localPosition = new Vector3( 0.0f,
				hbm.jocManager.joc.currentCamHeight + ( Mathf.Sin( Time.fixedTime * Mathf.PI * hbm.headBobSpeed ) * hbm.headBobIntensity ), 0.0f );
		}
	}

}
