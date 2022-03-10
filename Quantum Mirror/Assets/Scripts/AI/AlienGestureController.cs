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
	public float gCircleDiameter;

	public GestureSequence_Set gestures;
	public GestureSequence_Set responses;
	public float gestureSpeed = 1f;
	public float holdPosFor = 1f;

	[ReadOnly] public bool gesture;
	[ReadOnly] public bool waiting = false;
	[ReadOnly] public bool startGesture;
	[ReadOnly] public bool endGesture;
	[ReadOnly] public float waitTimeStamp;
	[ReadOnly] public int handIndex;
	[ReadOnly] public int sentenceIndex;
	[ReadOnly] public int wordIndex = 0;
	[ReadOnly] public Vector3 preGestureHandPos;

	[HideInInspector] public List<Gesture> playerGestures = new List<Gesture>();
	[HideInInspector] public List<Gesture> respondTo = new List<Gesture>();

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
				respondTo.Clear();
				for ( int i = 0; i < playerGestures.Count; i++ ) {
					respondTo.Add( playerGestures[ i ] );
				}
				playerGestures.Clear();

				//Find the player's sentence in the library and save the id.
				bool sentenceFound = false;
				for ( int i = 0; i < gestures.Items.Count; i++ ) {
					//Debug.Log( gestures.Items[ i ].words[ 0 ].circle + "-" + gestures.Items[ i ].words[ 0 ].fingers[ 0 ] + "-"
					//	+ gestures.Items[ i ].words[ 0 ].fingers[ 1 ] + "-" + gestures.Items[ i ].words[ 0 ].fingers[ 2 ] + " | " 
					//	+ respondTo[ 0 ].circle + "-" + respondTo[ 0 ].fingers[ 0 ] + "-" + respondTo[ 0 ].fingers[ 1 ] + "-" 
					//	+ respondTo[ 0 ].fingers[ 2 ] );
					if ( gestures.Items[ i ].words[ 0 ].circle == respondTo[ 0 ].circle &&
						gestures.Items[ i ].words[ 0 ].fingers[ 0 ] == respondTo[ 0 ].fingers[ 0 ] &&
						gestures.Items[ i ].words[ 0 ].fingers[ 1 ] == respondTo[ 0 ].fingers[ 1 ] &&
						gestures.Items[ i ].words[ 0 ].fingers[ 2 ] == respondTo[ 0 ].fingers[ 2 ] ) {
						sentenceIndex = i;
						sentenceFound = true;
					}
				}
				Debug.Log( "Known Sentence?: " + sentenceFound + " (" + respondTo.Count + ")" );

				if ( sentenceFound ) {
					int closestHand = FindClosestHand( alienManager.player );

					handIndex = closestHand;
					hands[ handIndex ].enabled = false;
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