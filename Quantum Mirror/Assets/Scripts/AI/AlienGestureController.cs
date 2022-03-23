using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlienGestureController : MonoBehaviour
{

	[Header( "References" )]
	public GestureListener gestureListener;

    public AlienIKHandler[] hands;
	public GestureCircle[] gestureCircles;
	public Transform[] idleHandTargets;

	public GestureSequence_Set gestureLibrary;
	public GestureSequence_Set responses;
	public Action_Set actionResponses;
	public float gestureSpeed = 1f;
	public float holdGestureFor = 1f;
	public float holdPointFor = 3f;

	[ReadOnly] public bool repositioning;
	[ReadOnly] public bool pointing = false;
	[ReadOnly] public bool gesturing = false;
	[ReadOnly] public bool waiting = false;
	[ReadOnly] public bool startGesture = false;
	[ReadOnly] public bool endGesture = false;
	[ReadOnly] public bool standardGesture = false;
	[ReadOnly] public float waitTimeStamp = 0f;
	[ReadOnly] public int gestureHandIndex = -1;
	[ReadOnly] public int pointHandIndex = -1;
	[ReadOnly] public int sentenceIndex = 0;
	[ReadOnly] public int wordIndex = 0;
	[ReadOnly] public Vector3 preGestureHandPos;

	[HideInInspector] public List<int> repositionedLegs = new List<int>();
	[HideInInspector] public List<Gesture> playerGestures = new List<Gesture>();
	[HideInInspector] public List<int> respondTo = new List<int>();

	[HideInInspector] public AlienManager alienManager;

	[HideInInspector] public Vector3 handTarget;

	private void OnEnable() {
		gestureListener.onSentence += OnSentence;
	}

	private void OnDisable() {
		gestureListener.onSentence += OnSentence;
	}

	public int FindClosestHand( Transform respondTo )
	{
		int closestHand = 0;
		float shortestDist = 0f;

		for ( int i = 0; i < gestureCircles.Length; i++ )
		{
			float dist = Vector3.Distance( respondTo.position, gestureCircles[ i ].transform.position );
			if ( dist < shortestDist || i == 0 )
			{
				closestHand = i;
				shortestDist = dist;
			}
		}
		return closestHand;
	}

	public void OnSentence( List<int> sentenceCode ) {
		if ( alienManager.stateMachine.CurrentState == AttentionState.Instance ) {
			string respondGCode = "";
			for ( int i = 0; i < sentenceCode.Count; i++ )
				respondGCode += sentenceCode[ i ];

			//Find the player's sentence in the library and save the id.
			bool sentenceFound = false;
			for ( int i = 0; i < gestureLibrary.Items.Count; i++ ) 
			{
				//Check if the gesture codes match.
				for ( int j = 0; j < gestureLibrary.Items[ i ].gestureCode.Length; j++ ) 
				{
					if ( gestureLibrary.Items[ i ].gCode == respondGCode ) 
					{
						sentenceIndex = i;
						sentenceFound = true;
						break;
					}
				}
			}

			Debug.Log( "Known Sentence?: " + sentenceFound + " ( " + respondGCode + " = " + gestureLibrary.Items[ sentenceIndex ].gCode + " )" );
			if ( sentenceFound ) {
				if ( responses.Items[ sentenceIndex ] != null )
				{
					int closestHand = FindClosestHand( alienManager.player );

					gestureHandIndex = closestHand;
					gesturing = true;
					wordIndex = -1;
					startGesture = true;
					waiting = false;
					preGestureHandPos = hands[ gestureHandIndex ].transform.position;
				}
				else if ( actionResponses.Items[ sentenceIndex ] != null )
					actionResponses.Items[ sentenceIndex ].ExecuteAction( alienManager );
			}
			else {
				int closestHand = FindClosestHand( alienManager.player );

				gestureHandIndex = closestHand;
				gesturing = true;
				wordIndex = -1;
				startGesture = true;
				waiting = false;
				standardGesture = true;
				preGestureHandPos = hands[ gestureHandIndex ].transform.position;
			}
		}
	}

	private void OnDrawGizmos() {
		Gizmos.DrawCube( handTarget, Vector3.one );
	}
}