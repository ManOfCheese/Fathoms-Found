using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMachine;

public class AlienManager : MonoBehaviour
{

    [HideInInspector] public AlienGestureController gestureController;
    [HideInInspector] public AlienMovementController moveController;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public StateMachine<AlienManager> stateMachine;

    private void Awake()
    {
        gestureController = GetComponent<AlienGestureController>();
        moveController = GetComponent<AlienMovementController>();
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
    }
}
