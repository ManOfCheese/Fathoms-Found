using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class JackOfManager : MonoBehaviour {

	public JackOfManagerSystem system;
	public BoolValue paused;
	[Space( 10 )]
	public ComponentSystem[] systemInitOrder;
	public ComponentSystem[] systemUpdateOrder;

	[ReadOnly] public string currentState;

	[HideInInspector] public JackOfController joc;
	[HideInInspector] public Camera cam;
	[HideInInspector] public CharacterController cc;
	[HideInInspector] public StateMachine<JackOfManager> stateMachine;
	[HideInInspector] public Dictionary<string, Module> modulesByName;
	[HideInInspector] public Dictionary<string, State<JackOfManager>> statesByName;

	private void Awake() {
		system.jom = this;
		joc.jom = this;
		cam = transform.parent.GetComponentInChildren<Camera>();
		cc = transform.parent.GetComponent<CharacterController>();
		stateMachine = new StateMachine<JackOfManager>( this );
	}

	private void Start() {
		for ( int i = 0; i < systemInitOrder.Length; i++ ) {
			systemInitOrder[ i ].Init();
		}
	}

	private void Update() {
		if ( !paused.Value )
		{
			for ( int i = 0; i < systemUpdateOrder.Length; i++ )
			{
				systemUpdateOrder[ i ].OnUpdate();
			}
			currentState = stateMachine.CurrentState.stateName;
		}
	}

}
