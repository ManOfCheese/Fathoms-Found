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
    [Tooltip( "From how far away can the alien interact with objects." )]
    public float interactDistance;
    [Tooltip( "From how far away in units should the player be able to get the alien's attention by looking at it." )]
    public float attentionDistance;
    [Tooltip( "How long in seconds should the alien remain at objects of interest before moving on." )]
    public float interestDuration;
    [Tooltip( "Should the player have to look at the alien to trigger it's attention state." )]
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
  //      currentState = stateMachine.CurrentState.stateName;
  //      stateMachine.Update();

  //      if ( Vector3.Distance( transform.position, player.position ) < attentionDistance ) 
  //      {
  //          if ( ( !lookAtForAttention || lookAtForAttention && looking ) && stateMachine.CurrentState != AttentionState.Instance &&
  //              ( stateMachine.CurrentState != InterestState.Instance || interest ) ) 
  //          {
  //              stateMachine.ChangeState( AttentionState.Instance );
		//	}
		//}
  //      else if ( stateMachine.CurrentState == AttentionState.Instance && stateMachine.CurrentState != InterestState.Instance && 
  //          !gc.gesturing && !gc.repositioning ) 
  //      {
  //          stateMachine.ChangeState( WanderState.Instance );
  //      }
    }

    public void OnLookAt() {
        looking = true;
    }

    public void OnLookAway() {
        looking = false;

        if ( lookAtForAttention && stateMachine.CurrentState == AttentionState.Instance && !gc.gesturing && !gc.repositioning ) 
            stateMachine.ChangeState( WanderState.Instance );
    }

    public void TryDoor()
	{
        if ( doorToOpen != null && Vector3.Distance( doorToOpen.transform.position, transform.position ) < interactDistance )
		{
            Debug.Log( "Open Door" );
            doorToOpen.Open();
            interest = false;
            doorToOpen = null;
            stateMachine.ChangeState( WanderState.Instance );
        }
	}
}
