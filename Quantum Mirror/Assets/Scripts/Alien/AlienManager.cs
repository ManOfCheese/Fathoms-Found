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
    public float tremorInterestDuration;
    public float gestureInterestDuration;
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
    public TremorInfo lastHeardTremor;
    public GestureSignal gestureSignal;

    private void Awake()
    {
        gc = GetComponent<AlienGestureController>();
        gc.alienManager = this;
        mc = GetComponent<AlienMovementController>();
    }

	private void Update()
	{
        if ( Time.time - gestureSignal.timeStamp > tremorInterestDuration )
        {
            gestureSignal.gestureCircle = null;
            gestureSignal.timeStamp = 0f;
        }

        if ( lastHeardTremor == null ) { return; }
        if ( Time.time - lastHeardTremor.timeStamp > gestureInterestDuration )
		{
            lastHeardTremor.intensity = 0f;
            lastHeardTremor.timeStamp = 0f;
            lastHeardTremor.position = Vector3.zero;
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
        if ( !focused ) 
        {
            lastHeardTremor.intensity = intensity;
            lastHeardTremor.timeStamp = Time.time;
            lastHeardTremor.position = position;
        }
    }

    public void OnWord( GestureCircle gestureCircle, Gesture word )
	{

	}

    public void OnSentence( GestureCircle gestureCircle )
	{
        gestureSignal.gestureCircle = gestureCircle;
        gestureSignal.timeStamp = Time.time;
        gc.gestureCircle = gestureCircle;
    }
}

[System.Serializable]
public class TremorInfo
{   
    public float intensity;
    public float timeStamp;
    public Vector3 position;
}

[System.Serializable] 
public class GestureSignal
{
    public GestureCircle gestureCircle;
    public float timeStamp;
}