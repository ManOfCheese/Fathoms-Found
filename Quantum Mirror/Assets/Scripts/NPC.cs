using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC : MonoBehaviour {

    public int interactionDistance;
    public GameObject player;
    public Image image;
    public NavMeshAgent agent;
    public List<ObjectType> objects;
    public ActionSequence idleSeq;
    public ActionSequence alertSeq;
    public Move_Action approachPlayer;
    public Move_Action runAway;

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
	}

    public void Alert() 
    {
        currentActions = alertSeq.actions;
	}

	public void OnLookAtNPC() 
    {
        transform.LookAt( player.transform.position );
        transform.rotation = Quaternion.Euler( new Vector3( 0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z ) );
	}

    public void OnLookAway() 
    {
        transform.LookAt( transform.forward );
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

    public void OnCommunicate( Sprite sprite ) 
    {

	}
}
