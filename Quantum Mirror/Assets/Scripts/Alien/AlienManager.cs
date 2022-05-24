using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMachine;

public class AlienManager : MonoBehaviour
{

    [Header( "Settings" )]
    public Transform player;
    [Tooltip( "Modifies how quickly an alien loses interest in a sound, a higher value means it loses interest faster." )]
    public float interestDuration;
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
    [ReadOnly] public bool focused;

    [HideInInspector] public AlienGestureController gc;
    [HideInInspector] public AlienMovementController mc;
    public TremorInfo tremorSource;

    private void Awake()
    {
        gc = GetComponent<AlienGestureController>();
        gc.alienManager = this;
        mc = GetComponent<AlienMovementController>();
    }

	private void Update()
	{
        if ( tremorSource == null ) { return; }
        if ( Time.time - tremorSource.timeStamp > interactDistance )
		{
            tremorSource.intensity = 0f;
            tremorSource.timeStamp = 0f;
            tremorSource.position = Vector3.zero;
        }
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

    public void OnTremor( Vector3 position, float intensity )
	{
        if ( !focused && intensity > tremorSource.intensity ) 
        {
            tremorSource.intensity = intensity;
            tremorSource.timeStamp = Time.time;
            tremorSource.position = position;
        }
    }
}

[System.Serializable]
public class TremorInfo
{   
    public float intensity;
    public float timeStamp;
    public Vector3 position;
}