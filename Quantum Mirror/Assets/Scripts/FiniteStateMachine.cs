using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {
    public class StateMachine<T> {

        public State<T> CurrentState {
            get; private set;
        } 
        public T Owner;

        public StateMachine( T _owner ) {
            Owner = _owner;
            CurrentState = null;
        }

        public void ChangeState( State<T> _newState ) {
            if ( CurrentState != null ) {
                CurrentState.ExitState( Owner );
            }
            //Debug.Log( "Switched from " + CurrentState + " to " + _newState );
            CurrentState = _newState;
            CurrentState.EnterState( Owner );
        }

        public void Update() {
            if ( CurrentState != null ) {
                CurrentState.UpdateState( Owner );
            }
        }
    }

    public abstract class State<T> {
        public string stateName;
        public abstract void EnterState( T _owner );
        public abstract void ExitState( T _owner );
        public abstract void UpdateState( T _owner );
    }
}