using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu( fileName = "JackOfController_System", menuName = "Systems/JackOfControllerSystem" )]
public class JackOfControllerSystem : ComponentSystem {

    public JackOfController joc;

	public override void Init() {
        joc.audioInfos = new AudioInfo[ 3 ] { joc.tracksGoingSource, joc.tracksGoingSource2, joc.collisionSource };
        joc.audioSources = joc.GetComponentsInChildren<AudioSource>();
        for ( int i = 0; i < Mathf.Min( joc.audioSources.Length, joc.audioInfos.Length ); i++ )
        {
            joc.audioInfos[ i ].source = joc.audioSources[ i ];
            joc.audioSources[ i ].clip = joc.audioInfos[ i ].clip;
            joc.audioSources[ i ].volume = joc.audioInfos[ i ].startVolume;
            joc.audioSources[ i ].loop = joc.audioInfos[ i ].loop;
        }

        joc.cam = joc.jom.cam;
        joc.cc = joc.jom.cc;
        joc.xCamRotation = joc.head.transform.eulerAngles.x;
        joc.yCamRotation = joc.cc.transform.eulerAngles.y;

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
        joc.startSensitivity = joc.sensitivity;
        joc.currentCamHeight = joc.camStartHeight;
        Cursor.lockState = CursorLockMode.Locked;
    }

	public override void OnUpdate()
	{

    }

}
