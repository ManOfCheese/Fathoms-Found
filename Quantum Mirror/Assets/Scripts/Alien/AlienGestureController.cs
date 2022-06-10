using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GestureState
{
	Repositioning,
	Pointing,
	Gesturing,
	HoldingGesture,
	StartGesture,
	EndGesture,
	Ended
}

public class AlienGestureController : GestureSender
{

	[Header( "References" )]
	public Transform alienBody;
	public HandInfo[] hands;
	public Transform[] idleHandTargets;
	public GestureSequence_Set gestureLibrary;
	public GestureSequence_Set responses;

	[Header( "Settings" )]
	public GestureSequence standardResponse;
	[Tooltip( "Should the alien hold at the centre of the circle before starting with the gestures?" )]
	public bool holdStart;
	public bool defaultFingerPos = true;
	public float gestureDistance;
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
	[ReadOnly] public List<Gesture> gesturesToMake = new List<Gesture>();
	[ReadOnly] public GestureCircle gestureCircle;
	[ReadOnly] public Animator[] fingerAnimators;
	[ReadOnly] public GestureState gestureState;
	[ReadOnly] public bool standardGesture = false;
	[ReadOnly] public float gestureHoldTimeStamp = 0f;
	[ReadOnly] public float pointHoldTimeStamp = 0f;
	[ReadOnly] public int gestureHandIndex = -1;
	[ReadOnly] public int pointHandIndex = -1;
	[ReadOnly] public int sentenceIndex = 0;
	[ReadOnly] public int wordIndex = 0;
	[ReadOnly] public Vector3 preGestureHandPos;
	[ReadOnly] public Vector3 handTarget;

	[HideInInspector] public AlienManager alienManager;
	[HideInInspector] public List<Gesture> playerGestures = new List<Gesture>();
	[HideInInspector] public List<int> respondTo = new List<int>();
	[HideInInspector] public HandInfo hand;



	private List<int> repositionedHands = new List<int>();
	private float alienStartHeight;

	protected override void Awake()
	{
		base.Awake();
		alienStartHeight = alienBody.localPosition.y;
	}

	public int FindClosestHand( Transform respondTo, bool isForPointing )
	{
		int closestHand = 0;
		float shortestDist = 0f;

		for ( int i = 0; i < hands.Length; i++ )
		{
			if ( isForPointing && i == gestureHandIndex ) { continue; }
			if ( !isForPointing && i == pointHandIndex ) { continue; }
			float dist = Vector3.Distance( respondTo.position, hands[ i ].handTransform.position );
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
		for ( int i = 0; i < hand.fingerAnimators.Length; i++ )
		{
			hand.fingerAnimators[ i ].SetBool( "FingerOpen", false );
		}

		if ( gestureState == GestureState.HoldingGesture )
		{
			if ( Time.time - pointHoldTimeStamp > holdPointFor )
				gestureState = GestureState.Gesturing;
		}
		else if ( gestureState == GestureState.Gesturing )
		{
			float speed = pointSpeed * Time.deltaTime;
			if ( speed > Vector3.Distance( hand.transform.position, handTarget ) )
			{
				hand.transform.position = handTarget;

				if ( handTarget != idleHandTargets[ pointHandIndex ].position )
				{
					pointHoldTimeStamp = Time.time;
					gestureState = GestureState.HoldingGesture;
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

	public TheKiwiCoder.BTNode.State ChangeAlienHeight( bool useSlerp, float startTimeStamp, float duration, float startHeight, float targetHeight )
	{
		float p = ( Time.time - startTimeStamp ) / duration;
		if ( p < 1 )
		{
			if ( useSlerp )
			{
				alienBody.transform.localPosition = Vector3.Slerp( new Vector3( alienBody.localPosition.x, startHeight, alienBody.localPosition.z ),
					new Vector3( alienBody.localPosition.x, targetHeight, alienBody.localPosition.z ), p );
			}
			else
			{
				alienBody.transform.localPosition = Vector3.Lerp( new Vector3( alienBody.localPosition.x, startHeight, alienBody.localPosition.z ),
					new Vector3( alienBody.localPosition.x, targetHeight, alienBody.localPosition.z ), p );
			}
			return TheKiwiCoder.BTNode.State.Running;
		}
		else if ( p >= 1 )
		{
			return TheKiwiCoder.BTNode.State.Success;
		}
		return TheKiwiCoder.BTNode.State.Failure;
	}

	public void FindGesture()
	{
		//Find the player's sentence in the library and save the id.
		bool sentenceFound = false;
		for ( int i = 0; i < gestureLibrary.Items.Count; i++ )
		{
			//Check if the gesture codes match.
			for ( int j = 0; j < gestureLibrary.Items[ i ].gestureCode.Length; j++ )
			{
				if ( gestureLibrary.Items[ i ].gCode == alienManager.gestureCircle.sentence )
				{
					sentenceIndex = i;
					sentenceFound = true;
					break;
				}
			}
		}

		Debug.Log( "Known Sentence?: " + sentenceFound + " ( " + alienManager.gestureCircle.sentence + " = " + gestureLibrary.Items[ sentenceIndex ].gCode + " )" );
		if ( !sentenceFound )
			standardGesture = true;
	}


	public void EnterGestureMode()
	{
		int closestHand = FindClosestHand( alienManager.gestureCircle.transform, false );
		gestureHandIndex = closestHand;
		repositionedHands.Add( gestureHandIndex );

		for ( int i = 0; i < hands.Length; i++ )
			hands[ i ].ikHandler.enabled = false;

		gestureState = GestureState.Repositioning;
	}

	public void ExitGestureMode()
	{
		gestureHandIndex = -1;
		repositionedHands.Clear();

		for ( int i = 0; i < hands.Length; i++ )
			hands[ i ].ikHandler.enabled = true;
	}

	public TheKiwiCoder.BTNode.State Gesture( GestureSequence sentence, bool clearcircle, bool startAtCentre, bool endAtCentre, bool returnToIdle, 
		bool inputGesture, bool fingerPosAtCentre, float repositionSpeed )
	{
		Debug.DrawLine( hand.handTransform.transform.position, hand.handTransform.transform.position + gestureCircle.transform.up * 5f );

		//Hold Gesture
		if ( gestureState == GestureState.Repositioning )
		{
			for ( int i = 0; i < hands.Length; i++ )
			{
				if ( !repositionedHands.Contains( i ) )
				{
					float speed = repositionSpeed * Time.deltaTime;
					if ( speed > Vector3.Distance( hands[ i ].ikHandler.transform.position, idleHandTargets[ i ].position ) )
					{
						hands[ i ].ikHandler.transform.position = idleHandTargets[ i ].position;
						repositionedHands.Add( i );
					}
					else
						hands[ i ].ikHandler.transform.position = Vector3.MoveTowards( hands[ i ].ikHandler.transform.position, idleHandTargets[ i ].position, speed );
				}
			}
			if ( repositionedHands.Count >= hands.Length )
				gestureState = GestureState.StartGesture;
		}
		else if ( gestureState == GestureState.HoldingGesture )
		{
			if ( Time.time - gestureHoldTimeStamp > holdGestureFor )
			{
				gestureState = GestureState.Gesturing;
				for ( int i = 0; i < fingerAnimators.Length; i++ )
					fingerAnimators[ i ].SetBool( "FingerOpen", false );
			}
		}
		else
		{
			if ( gestureState == GestureState.StartGesture )
			{
				handTarget = gestureCircle.subCircles[ gesturesToMake[ wordIndex ].circle ].transform.position + ( gestureCircle.transform.up * gestureDistance );
				gestureState = GestureState.Gesturing;

				for ( int i = 0; i < fingerAnimators.Length; i++ )
				{
					if ( fingerAnimators[ i ].GetBool( "FingerOpen" ) == defaultFingerPos )
						fingerAnimators[ i ].SetBool( "FingerOpen", !defaultFingerPos );
				}
			}
			//Check if we have reached our new hand target.
			else
			{
				float speed = gestureSpeed * Time.deltaTime;
				//Debug.Log( speed + " > " + Vector3.Distance( hand.transform.position, _owner.gc.handTarget ) + " | " + _owner.gc.waiting );
				if ( speed > Vector3.Distance( hand.ikHandler.transform.position, handTarget ) )
				{
					if ( gestureState == GestureState.EndGesture )
					{
						if ( standardGesture ) standardGesture = false;
						gestureState = GestureState.Ended;
						return TheKiwiCoder.BTNode.State.Success;
					}
					//Set new hand target.
					else if ( gestureState == GestureState.Gesturing )
					{
						hand.ikHandler.transform.position = handTarget;
						if ( holdStart || wordIndex >= 0 && !holdStart )
						{
							//Manipulate hand to make the gesture.
							hand.handTransform.transform.LookAt( hand.handTransform.transform.position + gestureCircle.transform.up );
							for ( int i = 0; i < fingerAnimators.Length; i++ )
							{
								if ( gesturesToMake[ wordIndex ].fingers[ i ] && !fingerAnimators[ i ].GetBool( "FingerOpen" ) )
									fingerAnimators[ i ].SetBool( "FingerOpen", true );
							}

							//Input the gesture into the circle.
							if ( inputGesture )
							{
								gestureCircle.subCircles[ gesturesToMake[ wordIndex ].circle ].ConfirmGestureTwoWay( ID, gesturesToMake[ wordIndex ].circle,
									gesturesToMake[ wordIndex ].fingers );
							}

							gestureHoldTimeStamp = Time.time;
							if ( holdGestureFor > 0f ) 
								gestureState = GestureState.HoldingGesture;
						}

						wordIndex++;
						//Set target as our start position.
						if ( wordIndex > gesturesToMake.Count - 1 )
						{
							if ( returnToIdle)
								handTarget = idleHandTargets[ gestureHandIndex ].position + ( gestureCircle.transform.up * gestureDistance );

							gestureState = GestureState.EndGesture;
						}
						//Set target as the next word in the sentence.
						else
							handTarget = gestureCircle.subCircles[ gesturesToMake[ wordIndex ].circle ].transform.position + ( gestureCircle.transform.up * gestureDistance );
					}
				}
				//Move towards hand target.
				else
					hand.ikHandler.transform.position = Vector3.MoveTowards( hand.ikHandler.transform.position, handTarget, speed );
			}
		}
		return TheKiwiCoder.BTNode.State.Running;
	}

	private void OnDrawGizmos() {
		Gizmos.DrawCube( handTarget, Vector3.one );
	}
}

[System.Serializable]
public class HandInfo
{
	public Transform handTransform;
	public AlienIKHandler ikHandler;
}