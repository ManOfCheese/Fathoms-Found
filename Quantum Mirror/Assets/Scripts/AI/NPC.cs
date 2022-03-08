using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using StateMachine;

public enum WanderMode
{
    PointToPoint,
    Timed
}

public class NPC : MonoBehaviour {

    [Header( "Settings" )]
    public int interactionDistance;

    [Header( "References" )]
    public GameObject player;

    [Header( "Object Interaction" )]
    public List<ObjectType> objects;
    public List<ActionSequence> objectPickUpReactions;
    public List<ActionSequence> objectUseEnvReactions;
    public List<ActionSequence> objectUseNPCReactions;

    private List<Action> currentActions;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public AlienGestureController gc;
    [HideInInspector] public AlienMovementController moveController;

    [Header( "Runtime" )]
    public Vector3 wanderTarget;
    [HideInInspector] public float idleDestinationTimestamp;

    private void Awake() 
    {
        moveController = GetComponent<AlienMovementController>();
        gc = GetComponent<AlienGestureController>();
        agent = GetComponent<NavMeshAgent>();
    }

	private void Update() 
    {
        Debug.Log( agent.destination );
	}

	public void OnLookAtNPC() 
    {
        //transform.LookAt( player.transform.position );
        transform.rotation = Quaternion.Euler( new Vector3( 0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z ) );
	}

    public void OnLookAway() 
    {
        //transform.LookAt( transform.forward );
	}

    public void OnPickUpObject( ObjectType obj ) 
    {
		for ( int i = 0; i < objects.Count; i++ ) 
        {
            if ( objects[ i ] == obj ) 
            {
                currentActions = objectPickUpReactions[ i ].actions;
            }
		}
	}

    public void OnDropObject( ObjectType obj ) 
    {
        for ( int i = 0; i < objects.Count; i++ ) 
        {
            if ( objects[ i ] == obj ) 
            {
                if ( currentActions == objectPickUpReactions[ i ].actions ) 
                {
                    //currentActions = alertSeq.actions;
                    //agent.destination = transform.position;
                }
            }
        }
    }

    public void OnUseObject( ObjectType obj ) 
    {
        if ( Vector3.Distance( transform.position, player.transform.position ) <= interactionDistance ) 
        {
            for ( int i = 0; i < objects.Count; i++ ) 
            {
                if ( objects[ i ] == obj )
				{
                    currentActions = objectUseNPCReactions[ i ].actions;
                }
            }
        }
        else {
            for ( int i = 0; i < objects.Count; i++ ) 
            {
                if ( objects[ i ] == obj )
				{
                    currentActions = objectUseEnvReactions[ i ].actions;
                }
            }
        }
    }

    public void OnCommunicate( InputAction.CallbackContext value ) 
    {
        if ( value.performed && Vector3.Distance( player.transform.position, transform.position ) < interactionDistance )
            gc.OnGesture( player.transform );
        wanderTarget = transform.position;
	}

	private void OnDrawGizmos()
	{
        Debug.DrawLine( agent.destination, agent.destination + new Vector3( 0f, 10f, 0f ), Color.red );
	}
}
