using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "ScreenTilt_System", menuName = "Systems/ScreenTiltSystem" )]
public class ScreenTiltSystem : ComponentSystem {

    public ScreenTiltModule stm;

	public override void OnUpdate() {
		float tilt = stm.tilt;

		if ( stm.rawMovementVector.x != 0 ) {
			float zRotation = stm.Map( stm.rawMovementVector.x, -1, 1, -stm.tiltSpeed, stm.tiltSpeed );
			tilt += zRotation * Time.deltaTime;
			tilt = Mathf.Clamp( tilt, -stm.maximumTilt, stm.maximumTilt );

			Camera cam = stm.jocManager.cam;
			cam.transform.eulerAngles = new Vector3( cam.transform.rotation.x, cam.transform.rotation.y, -tilt );
		}
		else {
			if ( stm.tilt != 0 ) {
				float adjustedTilt = stm.tiltSpeed * Time.deltaTime;
				if ( tilt > 0 ) {
					if ( tilt - adjustedTilt < 0 )
						tilt = 0;
					else
						tilt -= adjustedTilt;
				}
				else {
					if ( tilt + adjustedTilt > 0 )
						tilt = 0;
					else
						tilt += adjustedTilt;
				}
			}
			Camera cam = stm.jocManager.cam;
			cam.transform.eulerAngles = new Vector3( cam.transform.rotation.x, cam.transform.rotation.y, -tilt );
		}

		stm.tilt = tilt;
	}

}
