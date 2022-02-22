using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu( fileName = "JackOfController_System", menuName = "Systems/JackOfControllerSystem" )]
public class JackOfControllerSystem : ComponentSystem {

    public JackOfController joc;

	public override void Init() {
        joc.cam = joc.jom.cam;
        joc.cc = joc.jom.cc;

        joc.playerInput = joc.GetComponent<PlayerInput>();
        joc.playerInput.ActivateInput();
        joc.playerInput.SwitchCurrentActionMap( "Player" );

        joc.groundedState = GroundedState.Instance;
        joc.groundedState.stateName = "GroundedState";
        joc.airborneState = AirborneState.Instance;
        joc.airborneState.stateName = "AirborneState";

        joc.currentSpeed = joc.speed;
        joc.playerStartHeight = joc.jom.cc.height;
        joc.camStartHeight = joc.jom.cam.transform.localPosition.y;
        joc.currentCamHeight = joc.camStartHeight;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
