using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "JackOfManager_System", menuName = "Systems/JackOfManagerSystem" )]
public class JackOfManagerSystem : ComponentSystem {

	public JackOfManager jom;

	public override void Init() {
		Module[] modules = jom.GetComponents<Module>();

		jom.modulesByName = new Dictionary<string, Module>();
		for ( int i = 0; i < modules.Length; i++ ) {
			jom.modulesByName.Add( modules[ i ].moduleName, modules[ i ] );
			modules[ i ].jocManager = jom;
		}

		List<State<JackOfManager>> states = new List<State<JackOfManager>>();
		states.Add( jom.joc.groundedState );
		states.Add( jom.joc.airborneState );
		for ( int i = 0; i < modules.Length; i++ ) {
			if ( modules[ i ].state != null ) states.Add( modules[ i ].state );
		}

		jom.statesByName = new Dictionary<string, State<JackOfManager>>();
		for ( int i = 0; i < states.Count; i++ ) {
			jom.statesByName.Add( states[ i ].stateName, states[ i ] );
		}

		jom.stateMachine.ChangeState( jom.statesByName[ "GroundedState" ] );
	}

	public override void OnUpdate() {
		jom.stateMachine.Update();
	}

}
