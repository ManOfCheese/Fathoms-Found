using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMachine;

public class AlienManager : MonoBehaviour
{

    [Header( "Settings" )]
    public Transform player;
    [Tooltip( "From how far away can the alien interact with objects." )]
    public float interactDistance;
    [Tooltip( "From how far away in units should the player be able to get the alien's attention by looking at it." )]
    public float attentionDistance;

    [Header( "Runtime" )]
    [ReadOnly] public Door doorToOpen;
    [ReadOnly] public string currentState;
    [ReadOnly] public float interestTimeStamp;
    [ReadOnly] public bool interest;
    [ReadOnly] public bool looking;

    [HideInInspector] public AlienGestureController gc;
    [HideInInspector] public AlienMovementController mc;

    private void Awake()
    {
        gc = GetComponent<AlienGestureController>();
        gc.alienManager = this;
        mc = GetComponent<AlienMovementController>();
    }

    public void TryDoor()
	{
        if ( doorToOpen != null && Vector3.Distance( doorToOpen.transform.position, transform.position ) < interactDistance )
		{
            Debug.Log( "Open Door" );
            doorToOpen.Open();
            interest = false;
            doorToOpen = null;
        }
	}
}
