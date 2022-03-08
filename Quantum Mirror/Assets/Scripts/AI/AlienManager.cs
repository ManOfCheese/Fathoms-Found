using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMachine;

public class AlienManager : MonoBehaviour
{

    public Transform player;

    [HideInInspector] public AlienGestureController gc;
    [HideInInspector] public AlienMovementController mc;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public StateMachine<AlienManager> stateMachine;

    private void Awake()
    {
        gc = GetComponent<AlienGestureController>();
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
    }
}
