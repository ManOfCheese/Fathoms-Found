using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StateMachine;

public class Module : MonoBehaviour {

    [HideInInspector] public JackOfManager jocManager;
    public string moduleName;

    //State
    [HideInInspector] public State<JackOfManager> state;

    private PlayerInput playerInput;

    protected virtual void Awake() {
        playerInput = GetComponent<PlayerInput>();
        playerInput.ActivateInput();
        playerInput.SwitchCurrentActionMap( "Player" );
    }

}
