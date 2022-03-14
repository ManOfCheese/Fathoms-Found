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
	public GameObject[] gestureCircles;
	public Transform[] idleHandTargets;
	public float gCircleDiameter;

	public GestureSequence_Set gestureLibrary;
	public GestureSequence_Set responses;
	public float gestureSpeed = 1f;
	public float holdPosFor = 1f;

	[ReadOnly] public bool repositioning;
	[ReadOnly] public List<int> repositionedLegs = new List<int>();

	[ReadOnly] public bool gesture = false;
	[ReadOnly] public bool waiting = false;
	[ReadOnly] public bool startGesture = false;
	[ReadOnly] public bool endGesture = false;
	[ReadOnly] public float waitTimeStamp = 0f;
	[ReadOnly] public int handIndex = -1;
	[ReadOnly] public int sentenceIndex = 0;
	[ReadOnly] public int wordIndex = 0;
	[ReadOnly] public Vector3 preGestureHandPos;

	[HideInInspector] public List<Gesture> playerGestures = new List<Gesture>();
	[HideInInspector] public List<int> respondTo = new List<int>();

	[HideInInspector] public AlienManager alienManager;

	[HideInInspector] public Vector3 handTarget;

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

	public void OnConfirmGesture( InputAction.CallbackContext value ) {
		if ( value.performed && alienManager.stateMachine.CurrentState == AttentionState.Instance ) {
			if ( handPos.Value == 0 ) {
				//Generate gCode for player gesture.
				respondTo.Clear();

				respondTo.Add( playerGestures.Count );
				for ( int i = 0; i < playerGestures.Count; i++ ) {
					respondTo.Add( playerGestures[ i ].circle );
				}
				for ( int i = 0; i < playerGestures.Count; i++ ) {
					for ( int j = 0; j < playerGestures[ i ].fingers.Length; j++ ) {
						respondTo.Add( playerGestures[ i ].fingers[ j ] ? 1 : 0 );
					}
				}

				string respondGCode = "";
				for ( int i = 0; i < respondTo.Count; i++ ) {
					respondGCode += respondTo[ i ];
				}
				Debug.Log( respondGCode );
				playerGestures.Clear();

				//Find the player's sentence in the library and save the id.
				bool sentenceFound = false;
				for ( int i = 0; i < gestureLibrary.Items.Count; i++ ) {
					//Check if the gesture codes match.
					for ( int j = 0; j < gestureLibrary.Items[ i ].gestureCode.Length; j++ ) {
						//if ( gestureLibrary.Items[ i ].gestureCode.Length != respondTo.Count ) { continue; }
						//else if ( gestureLibrary.Items[ i ].gestureCode[ j ] != respondTo[ j ] ) { continue; }

						if ( gestureLibrary.Items[ i ].gCode == respondGCode ) {
							sentenceIndex = i;
							sentenceFound = true;
							break;
						}
					}
				}
				Debug.Log( "Known Sentence?: " + sentenceFound + " ( " + respondGCode + " = " + gestureLibrary.Items[ sentenceIndex ].gCode + " )" );

				if ( sentenceFound ) {
					int closestHand = FindClosestHand( alienManager.player );

					handIndex = closestHand;
					gesture = true;
					wordIndex = -1;
					startGesture = true;
					waiting = false;
					preGestureHandPos = hands[ handIndex ].transform.position;
					//textChanger.OnAlienInteract();
				}
				else {
					//Unknown sentence behaviour?
				}
			}
			else {
				playerGestures.Add( new Gesture( handPos.Value, fingers.Value ) );
				//Debug.Log( playerGestures[ playerGestures.Count - 1 ].circle + " | " + playerGestures[ playerGestures.Count - 1 ].fingers[ 0 ] + "-"
				//	+ playerGestures[ playerGestures.Count - 1 ].fingers[ 1 ] + "-" + playerGestures[ playerGestures.Count - 1 ].fingers[ 2 ] );
			}
		}
	}

	private void OnDrawGizmos() {
		Gizmos.DrawCube( handTarget, Vector3.one );
	}
}