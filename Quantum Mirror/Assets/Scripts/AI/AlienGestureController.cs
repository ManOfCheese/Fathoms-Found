using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlienGestureController : MonoBehaviour
{

	[Header( "References" )]
	public BoolValue confirmGesture;
	public BoolValue isInGestureMode;
	public IntValue handPos;
	public BoolArrayValue fingers;

	public UI_Text_Changer textChanger;
    public AlienIKHandler[] hands;
	public GestureCircle[] gestureCircles;
	public Transform[] idleHandTargets;
	public float gCircleDiameter;

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
	[ReadOnly] public int handIndex = -1;
	[ReadOnly] public int sentenceIndex = 0;
	[ReadOnly] public int wordIndex = 0;
	[ReadOnly] public Vector3 preGestureHandPos;

	[HideInInspector] public List<int> repositionedLegs = new List<int>();
	[HideInInspector] public List<Gesture> playerGestures = new List<Gesture>();
	[HideInInspector] public List<int> respondTo = new List<int>();

	[HideInInspector] public AlienManager alienManager;

	[HideInInspector] public Vector3 handTarget;

	private void OnEnable() {
		isInGestureMode.onValueChanged += OnPlayerTogglesGestureMode;
		confirmGesture.onValueChanged += OnConfirmGesture;
	}

	private void OnDisable() {
		isInGestureMode.onValueChanged -= OnPlayerTogglesGestureMode;
		confirmGesture.onValueChanged += OnConfirmGesture;
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

	public void OnPlayerTogglesGestureMode( bool isInGesturemode ) {
		playerGestures.Clear();
	}

	public void OnConfirmGesture( bool confirmGesture ) {
		if ( confirmGesture && alienManager.stateMachine.CurrentState == AttentionState.Instance ) {
			if ( handPos.Value == 0 ) {
				//Generate gCode for player gesture.
				respondTo.Clear();

				respondTo.Add( playerGestures.Count );
				for ( int i = 0; i < playerGestures.Count; i++ ) 
					respondTo.Add( playerGestures[ i ].circle );

				for ( int i = 0; i < playerGestures.Count; i++ ) 
				{
					for ( int j = 0; j < playerGestures[ i ].fingers.Length; j++ )
						respondTo.Add( playerGestures[ i ].fingers[ j ] ? 1 : 0 );
				}

				string respondGCode = "";
				for ( int i = 0; i < respondTo.Count; i++ )
					respondGCode += respondTo[ i ];
				playerGestures.Clear();

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
				//Debug.Log( "Known Sentence?: " + sentenceFound + " ( " + respondGCode + " = " + gestureLibrary.Items[ sentenceIndex ].gCode + " )" );

				if ( sentenceFound ) {
					if ( responses.Items[ sentenceIndex ] != null )
					{
						int closestHand = FindClosestHand( alienManager.player );

						handIndex = closestHand;
						gesturing = true;
						wordIndex = -1;
						startGesture = true;
						waiting = false;
						preGestureHandPos = hands[ handIndex ].transform.position;
					}
					else if ( actionResponses.Items[ sentenceIndex ] != null )
					{
						actionResponses.Items[ sentenceIndex ].ExecuteAction( alienManager );
					}

					//textChanger.OnAlienInteract();
				}
				else {
					int closestHand = FindClosestHand( alienManager.player );

					handIndex = closestHand;
					gesturing = true;
					wordIndex = -1;
					startGesture = true;
					waiting = false;
					standardGesture = true;
					preGestureHandPos = hands[ handIndex ].transform.position;
				}
			}
			else {
				playerGestures.Add( new Gesture( handPos.Value, fingers.Value ) );
			}
		}
	}

	private void OnDrawGizmos() {
		Gizmos.DrawCube( handTarget, Vector3.one );
	}
}