using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using TheKiwiCoder;
using UnityEngine.AI;

public class AlienIKManager : GestureSender
{
	[Header( "References" )]
	public BoolValue paused;
	public Transform body;
	public GestureSequence_Set gestureLibrary;
	public GestureSequence_Set responses;
	public GestureSequence standardResponse;
	public List<HandController> allHands;

	[Header( "Settings" )]
	public float currentHeight;
	public int maxHandsUseSimultaneously;
	public bool defaultFingerPos = true;

	[Header( "Gesture Settings" )]
	public float handToPanelDistance = 0.1f;
	public float maxGestureDistance = 0.1f;
	public float gestureSpeed = 1f;
	public float holdGestureFor = 1f;

	[Header( "Point Settings" )]
	public float pointRadius = 2f;
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

	[ReadOnly] public int handsAvailable;

	[HideInInspector] public AlienManager alienManager;
	[HideInInspector] public List<State<HandController>> states = new List<State<HandController>>();
	[HideInInspector] public Dictionary<string, State<HandController>> statesByName = new Dictionary<string, State<HandController>>();
	[HideInInspector] public NavMeshAgent agent;
	[HideInInspector] public HandController gestureHand;

	private bool changingHeight;
	private bool useSlerp;
	private float p = 0f;
	private float changeHeightSpeed;
	private float startHeight;
	private float lerpStartHeight;
	private float lerpTargetHeight = 1f;

	protected override void Awake()
	{
		base.Awake();
		alienManager = GetComponent<AlienManager>();
		agent = GetComponentInParent<NavMeshAgent>();
		handsAvailable = maxHandsUseSimultaneously;
		startHeight = currentHeight;

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
			allHands[ i ].transform.position += new Vector3( 0f, 1f, 0f ) * allHands[ i ].heightOffset;
			allHands[ i ].SyncSettingsWithManager();
			allHands[ i ].stateMachine = new StateMachine<HandController>( allHands[ i ] );
			allHands[ i ].stateMachine.ChangeState( statesByName[ "WalkingState" ] );
		}
	}

	private void Update()
	{
		//if ( paused.Value ) {  return; }
		for ( int i = 0; i < allHands.Count; i++ )
			allHands[ i ].stateMachine.Update();

		Ray ray = new Ray( body.position, Vector3.down );
		Debug.DrawRay( ray.origin, ray.direction * 10f, Color.white );

		if ( Physics.Raycast( ray, out RaycastHit info, 100, terrainLayer ) )
		{
			body.transform.position = info.point + new Vector3( 0f, currentHeight, 0f );
			this.transform.position = info.point + new Vector3( 0f, currentHeight, 0f );
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

	public BTNode.State ChangeAlienHeight( float _speed, float _targetHeight )
	{
		if ( !changingHeight )
		{
			changingHeight = true;
			p = 0f;
			changeHeightSpeed = _speed;
			lerpStartHeight = currentHeight;
			lerpTargetHeight = startHeight + _targetHeight;
			return BTNode.State.Running;
		}
		else
		{
			p += changeHeightSpeed * Time.deltaTime;
			if ( p < 1 )
			{
				currentHeight = Mathf.Lerp( startHeight, lerpTargetHeight, p );
				return BTNode.State.Running;
			}
			else if ( p >= 1 )
			{
				p = 1f;
				currentHeight = Mathf.Lerp( startHeight, lerpTargetHeight, p );
				changingHeight = false;
				return BTNode.State.Success;
			}
			return BTNode.State.Failure;
		}
	}

	public bool FindGestureHand( GestureCircle _gestureCircle, float _maxDist )
	{
		HandController closestHand = FindClosestHand( _gestureCircle.transform.position, _maxDist );
		if ( closestHand != null )
		{
			gestureHand = closestHand;
			gestureHand.gestureCircle = _gestureCircle;
			gestureHand.moveState = GestureState.Starting;
			gestureHand.stateMachine.ChangeState( statesByName[ "GesturingState" ] );
			FindGesture( gestureHand );
			return true;
		}
		return false;
	}

	public bool FindPointHands( Vector3[] _objectsToPointAt, float _maxDist )
	{
		int pointingHandCount = 0;
		for ( int i = 0; i < _objectsToPointAt.Length; i++ )
		{
			HandController closestHand = FindClosestHand( _objectsToPointAt[ i ], _maxDist );
			if ( closestHand != null )
			{
				Vector3 pointVector = _objectsToPointAt[ i ] - body.transform.position;
				if ( Vector3.Distance( closestHand.transform.position, pointVector ) > pointRadius )
					closestHand.handTarget = body.transform.position + pointVector.normalized * pointRadius;
				else
					closestHand.handTarget = pointVector;
				closestHand.pointState = PointState.Starting;
				closestHand.stateMachine.ChangeState( statesByName[ "PointingState" ] );
				pointingHandCount++;
			}
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
				if ( gestureLibrary.Items[ i ].gCode == alienManager.gestureCircle.sentence )
				{
					hand.sentence = responses.Items[ i ];
					sentenceFound = true;
					break;
				}
				//Debug.Log( "Known Sentence?: " + sentenceFound + " ( " + alienManager.gestureCircle.sentence + " = " + 
				//	gestureLibrary.Items[ hand.sentenceIndex ].gCode + " )" );
			}
		}

		if ( !sentenceFound )
			hand.sentence = standardResponse;
	}

	public HandController FindClosestHand( Vector3 _respondTo, float _maxDist )
	{
		HandController closestHand = null;
		float shortestDist = 0f;

		if ( handsAvailable > 0 )
		{
			for ( int i = 0; i < allHands.Count; i++ )
			{
				if ( allHands[ i ].stateMachine.CurrentState != statesByName[ "GesturingState" ] &&
					allHands[ i ].stateMachine.CurrentState != statesByName[ "PointingState" ] )
				{
					float dist = Vector3.Distance( _respondTo, allHands[ i ].idleHandTarget.position + ( new Vector3( 0f, 1f, 0f ) * heightOffset ) );
					if ( ( dist < shortestDist || i == 0 ) && dist < _maxDist )
					{
						closestHand = allHands[ i ];
						shortestDist = dist;
					}
				}
			}
		}

		return closestHand;
	}

	public Vector3[] FindClosestObjectsInList( List<Vector3> _targetObjects, int _maxObjects )
	{
		Dictionary<int, float> closestByDistance = new Dictionary<int, float>();
		for ( int i = 0; i < _targetObjects.Count; i++ )
		{
			float dist = Vector3.Distance( _targetObjects[ i ], body.transform.position );

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
						break;
					}
				}
			}
		}

		List<Vector3> closestObjects = new List<Vector3>();
		foreach ( var item in closestByDistance )
			closestObjects.Add( _targetObjects[ item.Key ] );
		return closestObjects.ToArray();
	}
}
