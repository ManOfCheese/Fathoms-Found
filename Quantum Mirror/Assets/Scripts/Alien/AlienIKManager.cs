using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using TheKiwiCoder;
using UnityEngine.AI;

public class AlienIKManager : GestureSender
{
	[Header( "References" )]
	public Transform body;
	public GestureSequence_Set gestureLibrary;
	public GestureSequence_Set responses;
	public GestureSequence standardResponse;
	public List<HandController> allHands;

	[Header( "Settings" )]
	public int maxHandsUseSimultaneously;
	public bool defaultFingerPos = true;

	[Header( "Gesture Settings" )]
	public float gestureDistance = 0.1f;
	public float gestureSpeed = 1f;
	public float holdGestureFor = 1f;

	[Header( "Point Settings" )]
	public float pointSpeed = 1f;
	public float holdPointFor = 3f;

	[Header( "Walk Settings" )]
	public LayerMask terrainLayer = default;
	public float heightOffset = default;
	public float walkSpeed = 1;
	public float openHandSpeed = 1;
	public float closeFingerTreshold = 0.2f;
	public float openFingerTreshold = 0.8f;
	public Vector2 randomOffsetRange = Vector2.one;

	[Space( 10 )]
	public float stepDistance = 4;
	public Vector2 stepDistRandomization = Vector2.zero;

	[Space( 10 )]
	public float stepLength = 4;
	public Vector2 stepLengthRandomization = Vector2.zero;

	[Space( 10 )]
	public float stepHeight = 1;
	public Vector2 stepHeightRandomization = Vector2.zero;

	[HideInInspector] public int handsAvailable;
	[HideInInspector] public AlienManager alienManager;
	[HideInInspector] public List<State<HandController>> states = new List<State<HandController>>();
	[HideInInspector] public Dictionary<string, State<HandController>> statesByName = new Dictionary<string, State<HandController>>();
	[HideInInspector] public NavMeshAgent agent;

	private bool changingHeight;
	private bool useSlerp;
	private float p = 0f;
	private float changeHeightSpeed;
	private float startHeight;
	private float targetHeight;

	protected override void Awake()
	{
		base.Awake();
		alienManager = GetComponent<AlienManager>();
		agent = GetComponentInParent<NavMeshAgent>();
		handsAvailable = maxHandsUseSimultaneously;

		states.Add( GesturingState.Instance );
		states[ states.Count - 1 ].stateName = "GesturingState";
		states.Add( PointingState.Instance );
		states[ states.Count - 1 ].stateName = "PointingState";
		states.Add( WalkingState.Instance );
		states[ states.Count - 1 ].stateName = "WalkingState";
		for ( int i = 0; i < states.Count; i++ )
			statesByName.Add( states[ i ].stateName, states[ i ] );

		for ( int i = 0; i < allHands.Count; i++ )
		{
			allHands[ i ].ikManager = this;
			allHands[ i ].handIndex = i;
			allHands[ i ].SyncSettingsWithManager();
			allHands[ i ].stateMachine = new StateMachine<HandController>( allHands[ i ] );
			allHands[ i ].stateMachine.ChangeState( statesByName[ "WalkingState" ] );
		}
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

	private void OnValidate()
	{
		for ( int i = 0; i < allHands.Count; i++ )
		{
			if ( allHands[ i ].ikManager == null )
				allHands[ i ].ikManager = this;
			allHands[ i ].SyncSettingsWithManager();
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

	public bool FindGestureHands( List<GestureCircle> gestureCircles )
	{
		for ( int i = 0; i < Mathf.Min( gestureCircles.Count, handsAvailable ); i++ )
		{
			HandController closestHand = FindClosestHand( gestureCircles[ i ].transform );
			if ( closestHand != null )
			{
				closestHand.gestureCircle = gestureCircles[ i ];
				closestHand.moveState = MoveState.Starting;
				closestHand.stateMachine.ChangeState( statesByName[ "GesturingState" ] );
				FindGesture( closestHand );
				return true;
			}
		}
		return false;
	}

	public bool FindPointHands( Transform[] objectsToPointAt )
	{
		int pointingHandCount = 0;
		for ( int i = 0; i < Mathf.Min( objectsToPointAt.Length, handsAvailable ); i++ )
		{
			HandController closestHand = FindClosestHand( objectsToPointAt[ i ] );
			closestHand.pointAt = objectsToPointAt[ i ];
			closestHand.moveState = MoveState.Starting;
			closestHand.stateMachine.ChangeState( statesByName[ "PointingState" ] );
			pointingHandCount++;
		}

		if ( pointingHandCount > 0 )
			return true;
		else
			return false;
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
				for ( int k = 0; k < alienManager.gestureCircles.Count; k++ )
				{
					if ( gestureLibrary.Items[ i ].gCode == alienManager.gestureCircles[ k ].sentence )
					{
						hand.sentence = responses.Items[ i ];
						sentenceFound = true;
						break;
					}
					Debug.Log( "Known Sentence?: " + sentenceFound + " ( " + alienManager.gestureCircles[ k ].sentence + " = " + 
						gestureLibrary.Items[ hand.sentenceIndex ].gCode + " )" );
				}
			}
		}

		if ( !sentenceFound )
			hand.sentence = standardResponse;
	}

	public HandController FindClosestHand( Transform _respondTo )
	{
		HandController closestHand = null;
		float shortestDist = 0f;

		for ( int i = 0; i < allHands.Count; i++ )
		{
			if ( allHands[ i ].stateMachine.CurrentState != statesByName[ "GesturingState" ] && 
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
