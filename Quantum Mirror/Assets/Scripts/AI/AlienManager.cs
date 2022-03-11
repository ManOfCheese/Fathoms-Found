using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMachine;

public class AlienManager : MonoBehaviour
{

    public Transform player;
    public float attentionDistance;
    public bool lookAtForAttention;

    [HideInInspector] public AlienGestureController gc;
    [HideInInspector] public AlienMovementController mc;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public StateMachine<AlienManager> stateMachine;

    [ReadOnly] public bool looking;

    private void Awake()
    {
        gc = GetComponent<AlienGestureController>();
        gc.alienManager = this;
        mc = GetComponent<AlienMovementController>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine<AlienManager>( this );
    }

	private void Start()
	{
        stateMachine.ChangeState( WanderState.Instance );
    }

	// Update is called once per frame
	void Update()
    {
        stateMachine.Update();

        if ( Vector3.Distance( transform.position, player.position ) < attentionDistance ) {
            if ( lookAtForAttention && looking && stateMachine.CurrentState != AttentionState.Instance ) {
                stateMachine.ChangeState( AttentionState.Instance );
			}
		}
        else if ( stateMachine.CurrentState == AttentionState.Instance ) {
            stateMachine.ChangeState( WanderState.Instance );
        }
    }

    public void OnLookAt() {
        looking = true;
    }

    public void OnLookAway() {
        looking = false;

        if ( lookAtForAttention && stateMachine.CurrentState == AttentionState.Instance ) {
            stateMachine.ChangeState( WanderState.Instance );
        }
    }
}
