using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour {

    public int interactionDistance;
    public float changeDestinationTime;
    public float wanderRange;
    public Transform wanderTarget;
    public AlienGestureController gestureController;
    public GameObject player;
    public Image image;
    public NavMeshAgent agent;
    public List<ObjectType> objects;
    public ActionSequence idleSeq;
    public ActionSequence alertSeq;
    public Move_Action approachPlayer;
    public Move_Action runAway;
    public MoveTowards_Action moveTowards;

    public List<ActionSequence> objectPickUpReactions;
    public List<ActionSequence> objectUseEnvReactions;
    public List<ActionSequence> objectUseNPCReactions;

    private List<Action> currentActions;
    private float idleDestinationTimestamp;

	private void Awake() 
    {
        Idle();
        idleDestinationTimestamp = Time.time;
        moveTowards.target = wanderTarget;
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
        if ( Time.time - idleDestinationTimestamp > changeDestinationTime && currentActions == idleSeq.actions )
		{
            wanderTarget.transform.position = transform.position + GetRandomPosOnNavMesh();
            idleDestinationTimestamp = Time.time;
		}
	}

    public void Idle() 
    {
        currentActions = idleSeq.actions;
        wanderTarget.transform.position = GetRandomPosOnNavMesh();
    }

    public void Alert() 
    {
        currentActions = alertSeq.actions;
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
                    currentActions = alertSeq.actions;
                    agent.destination = transform.position;
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
                    currentActions = objectUseNPCReactions[ i ].actions;
            }
        }
        else {
            for ( int i = 0; i < objects.Count; i++ ) 
            {
                if ( objects[ i ] == obj )
                    currentActions = objectUseEnvReactions[ i ].actions;
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

    public Vector3 GetRandomPosOnNavMesh()
	{
        Vector3 randomTarget = Random.insideUnitSphere * wanderRange;
        NavMeshHit hit;
        NavMesh.SamplePosition( randomTarget, out hit, wanderRange, 1 );
        Vector3 finalPosition = hit.position;
        return finalPosition;
    }
}
