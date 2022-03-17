using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMachine;

public class AlienManager : MonoBehaviour
{

    [Header( "References" )]
    public Transform player;
    public Transform moveTarget;
    public Transform pointTarget;

    [Header( "Settings" )]
    public float attentionDistance;
    public float interestDuration;
    public bool lookAtForAttention;

    [Header( "Runtime" )]
    [ReadOnly] public Door doorToOpen;
    [ReadOnly] public string currentState;
    [ReadOnly] public float interestTimeStamp;
    [ReadOnly] public bool interest;
    [ReadOnly] public bool looking;

    [HideInInspector] public AlienGestureController gc;
    [HideInInspector] public AlienMovementController mc;
    [HideInInspector] public StateMachine<AlienManager> stateMachine;

    private void Awake()
    {
        gc = GetComponent<AlienGestureController>();
        gc.alienManager = this;
        mc = GetComponent<AlienMovementController>();
        stateMachine = new StateMachine<AlienManager>( this );
    }

	private void Start()
	{
        stateMachine.ChangeState( WanderState.Instance );
    }

	// Update is called once per frame
	void Update()
    {
        currentState = stateMachine.CurrentState.stateName;
        stateMachine.Update();

        if ( Vector3.Distance( transform.position, player.position ) < attentionDistance ) {
            if ( ( !lookAtForAttention || lookAtForAttention && looking ) && stateMachine.CurrentState != AttentionState.Instance &&
                ( stateMachine.CurrentState != InterestState.Instance || interest ) ) {
                stateMachine.ChangeState( AttentionState.Instance );
			}
		}
        else if ( stateMachine.CurrentState == AttentionState.Instance && stateMachine.CurrentState != InterestState.Instance && !gc.gesturing && 
            !gc.repositioning ) {
            stateMachine.ChangeState( WanderState.Instance );
        }
    }

    public void OnLookAt() {
        looking = true;
    }

    public void OnLookAway() {
        looking = false;

        if ( lookAtForAttention && stateMachine.CurrentState == AttentionState.Instance && !gc.gesturing && !gc.repositioning ) {
            stateMachine.ChangeState( WanderState.Instance );
        }
    }

    public void TryDoor()
	{
        if ( doorToOpen != null && Vector3.Distance( doorToOpen.transform.position, transform.position ) < mc.destinationReachedWindow )
		{
            doorToOpen.Open();
            interest = false;
            doorToOpen = null;
            stateMachine.ChangeState( WanderState.Instance );
        }
	}
}
