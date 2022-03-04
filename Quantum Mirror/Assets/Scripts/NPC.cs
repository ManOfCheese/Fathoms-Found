using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour {

    [Header( "Settings" )]
    public int interactionDistance;

    [Header( "References" )]
    public Transform wanderTarget;
    public Wander wander;
    public NavMeshAgent agent;
    public AlienGestureController gestureController;
    public GameObject player;
    public Image image;

    [Header( "Actions" )]
    public ActionSequence idleSeq;
    public ActionSequence alertSeq;
    public Move_Action approachPlayer;
    public Move_Action runAway;

    [Header( "Object Interaction" )]
    public List<ObjectType> objects;
    public List<ActionSequence> objectPickUpReactions;
    public List<ActionSequence> objectUseEnvReactions;
    public List<ActionSequence> objectUseNPCReactions;

    private List<Action> currentActions;

	private void Awake() 
    {
        Idle();
    }

	private void Update() 
    {
        if ( currentActions != null ) 
        {
            for ( int i = 0; i < currentActions.Count; i++ ) 
            {
                currentActions[ i ].ExecuteAction( this );
            }
        }
	}

    public void Idle() 
    {
        currentActions = idleSeq.actions;
        wander.OnIdle();
    }

    public void Alert() 
    {
        currentActions = alertSeq.actions;
        wander.idling = false;
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
                wander.idling = false;
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
                    currentActions = alertSeq.actions;
                    agent.destination = transform.position;
                    wander.idling = false;
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
                    wander.idling = false;
                }
            }
        }
        else {
            for ( int i = 0; i < objects.Count; i++ ) 
            {
                if ( objects[ i ] == obj )
				{
                    currentActions = objectUseEnvReactions[ i ].actions;
                    wander.idling = false;
                }
            }
        }
    }

    public void OnCommunicate( InputAction.CallbackContext value ) 
    {
        if ( value.performed && Vector3.Distance( player.transform.position, transform.position ) < interactionDistance )
            gestureController.OnGesture( player.transform );
        wanderTarget.transform.position = transform.position;
	}

	private void OnDrawGizmos()
	{
        Debug.DrawLine( agent.destination, agent.destination + new Vector3( 0f, 10f, 0f ), Color.red );
	}
}
