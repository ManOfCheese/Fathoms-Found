using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using TheKiwiCoder;

public class AlienIKManager : GestureSender
{

	public Transform body;
	public List<HandController> allHands;
	public State<HandController>[] states;
	public GestureSequence_Set gestureLibrary;
	public GestureSequence_Set responses;
	public GestureSequence standardResponse;

	public int maxHandsUseSimultaneously;

	[HideInInspector] public int handsAvailable;
	[HideInInspector] public AlienManager alienManager;
	[HideInInspector] public Dictionary<string, State<HandController>> statesByName;

	private bool changingHeight;
	private bool useSlerp;
	private float p = 0f;
	private float changeHeightSpeed;
	private float startHeight;
	private float targetHeight;

	protected override void Awake()
	{
		base.Awake();
		handsAvailable = maxHandsUseSimultaneously;
		for ( int i = 0; i < allHands.Count; i++ )
			allHands[ i ].handIndex = i;

		for ( int i = 0; i < states.Length; i++ )
			statesByName.Add( states[ i ].stateName, states[ i ] );
	}

	private void Update()
	{
		for ( int i = 0; i < allHands.Count; i++ )
			allHands[ i ].stateMachine.Update();

		if ( changingHeight )
		{
			p += changeHeightSpeed * Time.deltaTime;
			if ( p < 1 )
			{
				if ( useSlerp )
				{
					body.transform.localPosition = Vector3.Slerp( new Vector3( body.localPosition.x, startHeight, body.localPosition.z ),
						new Vector3( body.localPosition.x, targetHeight, body.localPosition.z ), p );
				}
				else
				{
					body.transform.localPosition = Vector3.Lerp( new Vector3( body.localPosition.x, startHeight, body.localPosition.z ),
						new Vector3( body.localPosition.x, targetHeight, body.localPosition.z ), p );
				}
			}
			else if ( p >= 1 )
				changingHeight = false;
		}
	}

	public void ChangeAlienHeight( bool _useSlerp, float _speed, float _targetHeight )
	{
		if ( !changingHeight )
		{
			changingHeight = true;
			p = 0f;
			useSlerp = _useSlerp;
			changeHeightSpeed = _speed;
			startHeight = body.transform.localPosition.y;
			targetHeight = _targetHeight;
		}
	}

	public bool FindGestureHands()
	{
		if ( handsAvailable >= 1 )
		{
			HandController closestHand = FindClosestHand( alienManager.gestureCircle.transform );
			if ( closestHand != null )
			{
				closestHand.gestureCircle = alienManager.gestureCircle;
				closestHand.moveState = MoveState.Starting;
				closestHand.stateMachine.ChangeState( statesByName[ "GestureState" ] );
				FindGesture( closestHand );
				return true;
			}
		}
		return false;
	}

	public BTNode.State FindPointHands( Transform[] objectsToPointAt )
	{
		int pointingHandCount = 0;
		for ( int i = 0; i < Mathf.Min( objectsToPointAt.Length, handsAvailable ); i++ )
		{
			HandController closestHand = FindClosestHand( objectsToPointAt[ i ] );
			closestHand.moveState = MoveState.Starting;
			closestHand.stateMachine.ChangeState( statesByName[ "PointingState" ] );
			pointingHandCount++;
		}

		if ( pointingHandCount > 0 )
			return BTNode.State.Success;
		else
			return BTNode.State.Failure;
	}

	public void FindGesture( HandController hand )
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
					hand.sentence = responses.Items[ i ];
					sentenceFound = true;
					break;
				}
			}
		}

		Debug.Log( "Known Sentence?: " + sentenceFound + " ( " + alienManager.gestureCircle.sentence + " = " + gestureLibrary.Items[ hand.sentenceIndex ].gCode + " )" );
		if ( !sentenceFound )
			hand.sentence = standardResponse;
	}

	public HandController FindClosestHand( Transform _respondTo )
	{
		HandController closestHand = null;
		float shortestDist = 0f;

		for ( int i = 0; i < allHands.Count; i++ )
		{
			if ( allHands[ i ].stateMachine.CurrentState != statesByName[ "GestureState" ] && 
				allHands[ i ].stateMachine.CurrentState != statesByName[ "PointingState" ] )
			{
				float dist = Vector3.Distance( _respondTo.position, allHands[ i ].handTransform.position );
				if ( dist < shortestDist || i == 0 )
				{
					closestHand = allHands[ i ];
					shortestDist = dist;
				}
			}
		}
		return closestHand;
	}

	public Transform[] FindClosestObjectsInList( RunTimeSet<Transform> _targetObjects, int _maxObjects, float cutOffDistance )
	{
		Dictionary<int, float> closestByDistance = new Dictionary<int, float>();
		for ( int i = 0; i < _targetObjects.Items.Count; i++ )
		{
			float dist = Vector3.Distance( _targetObjects.Items[ i ].transform.position, body.transform.position );

			if ( dist < cutOffDistance )
			{
				if ( closestByDistance.Count < _maxObjects )
					closestByDistance.Add( i, dist );
				else
				{
					foreach ( var item in closestByDistance )
					{
						if ( dist < item.Value )
						{
							closestByDistance.Add( i, dist );
							closestByDistance.Remove( item.Key );
						}
					}
				}

			}
		}

		List<Transform> closestObjects = new List<Transform>();
		foreach ( var item in closestByDistance )
		{
			closestObjects.Add( _targetObjects.Items[ item.Key ] );
		}
		return closestObjects.ToArray();
	}
}
