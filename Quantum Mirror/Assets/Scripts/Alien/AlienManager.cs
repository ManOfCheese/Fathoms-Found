using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;

public class AlienManager : MonoBehaviour
{

    [Header( "References" )]
    public BoolValue paused;
    public RunTimeSet<GestureCircle> allGestureCircles;
    public RunTimeSet<Transform> allDoorPanels;

    [Header( "Settings" )]
    [Tooltip( "How long does an alien investigate a tremor before moving on." )]
    public float tremorInterestDuration;
    [Tooltip( "How long does an alien investigate a gesture before moving on." )]
    public float gestureInterestDuration;
    [Tooltip( "From how far away can the alien interact with objects." )]
    public float interactDistance;

    [Header( "Runtime" )]
    [ReadOnly] public TremorInfo lastHeardTremor;
    [ReadOnly] public GestureSignal gestureSignal;
    [ReadOnly] public GestureCircle gestureCircle;

    [HideInInspector] public AlienMovementController mc;
    [HideInInspector] public AlienIKManager ikManager;

    private AudioSource audioSource;

    private void Awake()
    {
        mc = GetComponent<AlienMovementController>();
        ikManager = GetComponent<AlienIKManager>();
        audioSource = GetComponent<AudioSource>();
    }

	private void Update()
	{
        if ( paused.Value ) { return; }

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

    public BTNode.State FindGestureCircle()
	{
        GestureCircle closestCircle = null;
        for ( int i = 0; i < allGestureCircles.Items.Count; i++ )
        {
            float shortestDistance = 0f;

            float dist = Vector3.Distance( allGestureCircles.Items[ i ].transform.position, this.transform.position );
            if ( ( closestCircle == null || Vector3.Distance( allGestureCircles.Items[ i ].transform.position, this.transform.position ) < shortestDistance ) &&
                dist < interactDistance )
            {
                closestCircle = allGestureCircles.Items[ i ];
                shortestDistance = dist;
            }
        }
        if ( closestCircle != null )
		{
            gestureCircle = closestCircle;
            return BTNode.State.Success;
        }
        else
            return BTNode.State.Failure;
    }

    public void OnTremor( Vector3 _position, float _intensity )
	{
        lastHeardTremor.intensity = _intensity;
        lastHeardTremor.timeStamp = Time.time;
        lastHeardTremor.position = _position;
    }

    public void OnWord( int _senderID, GestureCircle _gestureCircle, Gesture _word )
	{

	}

    public void OnSentence( int _senderID, GestureCircle _gestureCircle )
	{
        if ( _senderID != ikManager.ID )
		{
            gestureSignal.gestureCircle = _gestureCircle;
            gestureSignal.timeStamp = Time.time;
            gestureCircle = _gestureCircle;
        }
    }

    public void PlaySound( AudioClip _clip )
	{
        audioSource.clip = _clip;
        audioSource.Play();
	}

	private void OnDrawGizmos()
	{
        if ( lastHeardTremor != null )
		{
            Gizmos.color = Color.white;
            Gizmos.DrawSphere( lastHeardTremor.position, 1f );
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

[System.Serializable] 
public class GestureSignal
{
    public GestureCircle gestureCircle;
    public float timeStamp;
}