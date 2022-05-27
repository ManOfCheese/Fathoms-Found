using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMachine;

public class AlienManager : MonoBehaviour
{

    [Header( "References" )]
    public RunTimeSet<GestureCircle> gestureCircles;
    public RunTimeSet<Door> doors;

    [Header( "Settings" )]
    public Transform player;
    [Tooltip( "How long does an alien investigate a tremor before moving on." )]
    public float tremorInterestDuration;
    [Tooltip( "How long does an alien investigate a gesture before moving on." )]
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
    [ReadOnly] public TremorInfo lastHeardTremor;
    [ReadOnly] public GestureSignal gestureSignal;
    [ReadOnly] public GestureCircle gestureCircle;

    [HideInInspector] public AlienGestureController gc;
    [HideInInspector] public AlienMovementController mc;

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

    public TheKiwiCoder.BTNode.State FindGestureCircle()
	{
        GestureCircle closestCircle = null;
        for ( int i = 0; i < gestureCircles.Items.Count; i++ )
        {
            float shortestDistance = 0f;

            float dist = Vector3.Distance( gestureCircles.Items[ i ].transform.position, this.transform.position );
            if ( closestCircle == null )
            {
                if ( dist < interactDistance )
                {
                    closestCircle = gestureCircles.Items[ i ];
                    shortestDistance = dist;
                }
            }
            else if ( Vector3.Distance( gestureCircles.Items[ i ].transform.position, this.transform.position ) < shortestDistance )
            {
                closestCircle = gestureCircles.Items[ i ];
                shortestDistance = dist;
            }
        }
        gestureCircle = closestCircle;

        if ( gestureCircle != null )
            return TheKiwiCoder.BTNode.State.Success;
        else
            return TheKiwiCoder.BTNode.State.Failure;
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

    public void OnTremor( Vector3 _position, float _intensity )
	{
        lastHeardTremor.intensity = _intensity;
        lastHeardTremor.timeStamp = Time.time;
        lastHeardTremor.position = _position;
    }

    public void OnWord( GestureCircle _gestureCircle, Gesture _word )
	{

	}

    public void OnSentence( GestureCircle _gestureCircle )
	{
        gestureSignal.gestureCircle = _gestureCircle;
        gestureSignal.timeStamp = Time.time;
        this.gestureCircle = _gestureCircle;
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