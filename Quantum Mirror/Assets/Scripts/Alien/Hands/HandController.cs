using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMachine;

public enum MoveState
{
    Moving,
    Holding,
    Starting
}

public class HandController : MonoBehaviour
{
	
    [ReadOnly] public int handIndex;

    [Header( "References" )]
    public Animator[] fingerAnimators;
    public Transform handTransform;
    public Transform idleHandTarget;

    [HideInInspector] public bool defaultFingerPos = true;
    [HideInInspector] public AlienIKManager ikManager;
    [HideInInspector] public StateMachine<HandController> stateMachine;

    //Walking Settings
    [HideInInspector] public float walkSpeed = 1;
    [HideInInspector] public float openHandSpeed = 1;
    [HideInInspector] public float closeFingerTreshold = 0.2f;
    [HideInInspector] public float openFingerTreshold = 0.8f;
    [HideInInspector] public float stepDistance = 4;
    [HideInInspector] public float stepLength = 4;
    [HideInInspector] public float stepHeight = 1;
    [HideInInspector] public float heightOffset = default;
    [HideInInspector] public LayerMask terrainLayer = default;
    [HideInInspector] public Vector2 stepDistRandomization = Vector2.zero;
    [HideInInspector] public Vector2 stepLengthRandomization = Vector2.zero;
    [HideInInspector] public Vector2 stepHeightRandomization = Vector2.zero;

    [HideInInspector] public bool fingersOpen = true;
    [HideInInspector] public float heightRandomization;
    [HideInInspector] public float lerp;
    [HideInInspector] public Vector2 randomOffsetRange = Vector2.one;
    [HideInInspector] public Vector3 footSpacing;
    [HideInInspector] public Vector3 oldPosition, currentPosition, newPosition;

    //Gesturing Settings
    [HideInInspector] public GestureSequence sentence;
    [HideInInspector] public bool clearCircle;
    [HideInInspector] public bool startAtCentre;
    [HideInInspector] public bool holdStart;
    [HideInInspector] public bool endAtCentre;
    [HideInInspector] public bool returnToIdle;
    [HideInInspector] public bool inputGesture;
    [HideInInspector] public bool fingerPosAtCentre;
    [HideInInspector] public float gestureDistance;
    [HideInInspector] public float gestureSpeed = 1f;
    [HideInInspector] public float holdGestureFor = 1f;

    //Pointing Settings
    [HideInInspector] public float pointSpeed = 1f;
    [HideInInspector] public float holdPointFor = 3f;

    [Header( "Runtime" )]
    [ReadOnly] public string currentState;
    [ReadOnly] public Transform pointAt;
    [ReadOnly] public List<Gesture> gesturesToMake = new List<Gesture>();
    [ReadOnly] public GestureCircle gestureCircle;
    [ReadOnly] public MoveState moveState;
    [ReadOnly] public float holdTimeStamp = 0f;
    [ReadOnly] public int sentenceIndex = 0;
    [ReadOnly] public int wordIndex = 0;
    [ReadOnly] public Vector3 preGestureHandPos;
    [ReadOnly] public Vector3 handTarget;

	private void Awake()
	{
        footSpacing = transform.localPosition;
        currentPosition = newPosition = oldPosition = transform.position;
        lerp = 1f;
    }

	public void SyncSettingsWithManager()
	{
        walkSpeed = ikManager.walkSpeed;
        openHandSpeed = ikManager.openHandSpeed;
        closeFingerTreshold = ikManager.closeFingerTreshold;
        openFingerTreshold = ikManager.openFingerTreshold;
        stepDistance = ikManager.stepDistance;
        stepLength = ikManager.stepLength;
        stepHeight = ikManager.stepHeight;
        heightOffset = ikManager.heightOffset;
        terrainLayer = ikManager.terrainLayer;
        stepDistRandomization = ikManager.stepDistRandomization;
        stepLengthRandomization = ikManager.stepDistRandomization;
        stepHeightRandomization = ikManager.stepHeightRandomization;

        gestureSpeed = ikManager.gestureSpeed;
        holdGestureFor = ikManager.holdGestureFor;

        pointSpeed = ikManager.pointSpeed;
        holdPointFor = ikManager.holdPointFor;
	}

	private void Update()
	{
        currentState = stateMachine.CurrentState.stateName;
	}

	private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere( newPosition, 0.5f );

        Gizmos.color = Color.red;
        Gizmos.DrawSphere( oldPosition, 0.5f );

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere( currentPosition, 0.5f );
    }

}
