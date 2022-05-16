using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlienGestureController : MonoBehaviour
{

	[Header( "References" )]
    public HandInfo[] hands;
	public GestureCircle[] gestureCircles;
	public Transform[] idleHandTargets;
	public GestureListener gestureListener;
	public GestureSequence_Set gestureLibrary;
	public GestureSequence_Set responses;
	public Action_Set actionResponses;

	[Header( "Settings" )]
	public GestureSequence standardResponse;
	[Tooltip( "Should the alien hold at the centre of the circle before starting with the gestures?" )]
	public bool holdStart;
	[Tooltip( "How fast in units per second should the alien gestures." )]
	public float gestureSpeed = 1f;
	[Tooltip( "How long should the alien hold a gestures before moving on to the next one." )]
	public float holdGestureFor = 1f;
	[Tooltip( "How fast in units per second should the alien point." )]
	public float pointSpeed = 1f;
	[Tooltip( "How far away from the alien's center should the point target be allowed to be in units, if set low the arm may not full extend." )]
	public float maxPointDistance = 5f;
	[Tooltip( "How long should the alien hold a point before lowering it's arm again." )]
	public float holdPointFor = 3f;

	[Header( "Runtime" )]
	[ReadOnly] public bool repositioning;
	[ReadOnly] public bool pointing = false;
	[ReadOnly] public bool gesturing = false;
	[ReadOnly] public bool waiting = false;
	[ReadOnly] public bool startGesture = false;
	[ReadOnly] public bool endGesture = false;
	[ReadOnly] public bool standardGesture = false;
	[ReadOnly] public float gestureHoldTimeStamp = 0f;
	[ReadOnly] public float pointHoldTimeStamp = 0f;
	[ReadOnly] public int gestureHandIndex = -1;
	[ReadOnly] public int pointHandIndex = -1;
	[ReadOnly] public int sentenceIndex = 0;
	[ReadOnly] public int wordIndex = 0;
	[ReadOnly] public string respondGCode;
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

	public Transform FindClosetObjectInList( RunTimeSet<Transform> targetObjects )
	{
		float shortestDist = 0f;
		int closestObjectIndex = 0;
		for ( int i = 0; i < targetObjects.Items.Count; i++ )
		{
			if ( i == 0 )
			{
				shortestDist = Vector3.Distance( targetObjects.Items[ i ].transform.position, alienManager.transform.position );
				closestObjectIndex = 0;
			}
			else
			{
				float dist = Vector3.Distance( targetObjects.Items[ i ].transform.position, alienManager.transform.position );
				if ( dist < shortestDist )
				{
					shortestDist = dist;
					closestObjectIndex = i;
				}
			}
		}
		return targetObjects.Items[ closestObjectIndex ].transform;
	}

	public TheKiwiCoder.BTNode.State Point()
	{
		AlienIKHandler hand = hands[ pointHandIndex ].ikHandler;

		if ( waiting )
		{
			if ( Time.time - pointHoldTimeStamp > holdPointFor )
				waiting = false;
		}
		else
		{
			float speed = pointSpeed * Time.deltaTime;
			if ( speed > Vector3.Distance( hand.transform.position, handTarget ) )
			{
				hand.transform.position = handTarget;

				if ( handTarget != idleHandTargets[ pointHandIndex ].position )
				{
					pointHoldTimeStamp = Time.time;
					waiting = true;
					handTarget = idleHandTargets[ pointHandIndex ].position;
				}
				else
					return TheKiwiCoder.BTNode.State.Success;
			}
			else
				hand.transform.position = Vector3.MoveTowards( hand.transform.position, handTarget, speed );
		}
		return TheKiwiCoder.BTNode.State.Running;
	}

	public void SetGesture()
	{
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
		if ( sentenceFound )
		{
			if ( responses.Items[ sentenceIndex ] != null )
			{
				ResetGestureSettings();
			}
			else if ( actionResponses.Items[ sentenceIndex ] != null )
				actionResponses.Items[ sentenceIndex ].ExecuteAction( alienManager );
		}
		else
		{
			ResetGestureSettings();
			standardGesture = true;
		}
	}

	private void ResetGestureSettings()
	{
		int closestHand = FindClosestHand( alienManager.player );

		gestureHandIndex = closestHand;
		gesturing = true;
		wordIndex = -1;
		startGesture = true;
		waiting = false;
		preGestureHandPos = hands[ gestureHandIndex ].ikHandler.transform.position;
	}

	public TheKiwiCoder.BTNode.State Gesture()
	{
		GestureCircle gestureCircle = gestureCircles[ gestureHandIndex ];
		AlienIKHandler hand = hands[ gestureHandIndex ].ikHandler;

		List<Gesture> gestures;
		if ( standardGesture )
		{
			gestures = new List<Gesture>();
			for ( int i = 0; i < standardResponse.words.Count; i++ )
			{
				gestures.Add( standardResponse.words[ i ] );
			}
		}
		else
			gestures = responses.Items[ sentenceIndex ].words;

		//Hold Gesture
		if ( waiting )
		{
			if ( Time.time - gestureHoldTimeStamp > holdGestureFor )
				waiting = false;
		}
		else
		{
			if ( startGesture )
			{
				gestureCircle.gameObject.SetActive( true );
				handTarget = gestureCircle.transform.position;
				startGesture = false;
			}
			//Check if we have reached our new hand target.
			else
			{
				float speed = gestureSpeed * Time.deltaTime;
				//Debug.Log( speed + " > " + Vector3.Distance( hand.transform.position, _owner.gc.handTarget ) + " | " + _owner.gc.waiting );
				if ( speed > Vector3.Distance( hand.transform.position, handTarget ) )
				{
					if ( endGesture )
					{
						gesturing = false;
						gestureCircle.gameObject.SetActive( false );
						waiting = false;
						gestureHandIndex = -1;
						endGesture = false;
						if ( standardGesture ) standardGesture = false;
						for ( int i = 0; i < gestureCircle.subCircles.Length; i++ )
						{
							for ( int j = 0; j < gestureCircle.subCircles[ i ].fingerSprites.Length; j++ )
								gestureCircle.subCircles[ i ].fingerSprites[ j ].SetActive( false );
						}
						return TheKiwiCoder.BTNode.State.Success;
					}
					//Set new hand target.
					else
					{
						hand.transform.position = handTarget;
						if ( holdStart || wordIndex >= 0 && !holdStart )
						{
							gestureHoldTimeStamp = Time.time;
							waiting = true;
						}

						if ( wordIndex >= 0 )
						{
							Gesture gesture = gestures[ wordIndex ];
							gestureCircle.subCircles[ gesture.circle - 1 ].fingerSprites[ 0 ].SetActive( true );
							for ( int i = 0; i < gesture.fingers.Length; i++ )
								gestureCircle.subCircles[ gesture.circle - 1 ].fingerSprites[ i + 1 ].SetActive( gesture.fingers[ 0 ] );
						}

						wordIndex++;
						//Set target as our start position.
						if ( wordIndex > gestures.Count - 1 )
						{
							handTarget = idleHandTargets[ gestureHandIndex ].position;
							endGesture = true;
						}
						//Set target as the next word in the sentence.
						else
						{
							handTarget = gestureCircle.subCircles[ gestures[ wordIndex ].circle - 1 ].transform.position;
							//Debug.Log( Vector3.Distance( hand.transform.position, _owner.gc.handTarget ) + " | " + _owner.gc.waiting );
						}
					}
				}
				//Move towards hand target.
				else
					hand.transform.position = Vector3.MoveTowards( hand.transform.position, handTarget, speed );
			}
		}
		return TheKiwiCoder.BTNode.State.Running;
	}

	public void OnSentence( List<int> sentenceCode ) {
		respondGCode = "";
		for ( int i = 0; i < sentenceCode.Count; i++ )
			respondGCode += sentenceCode[ i ];
	}

	private void OnDrawGizmos() {
		Gizmos.DrawCube( handTarget, Vector3.one );
	}
}

[System.Serializable]
public class HandInfo
{
	public AlienIKHandler ikHandler;
	public Animator[] fingerAnimators;
}