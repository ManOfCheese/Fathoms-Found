using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMachine;

public enum MoveState
{
    Moving,
    Holding,
    Starting,
    Ending,
    Ended
}

public class HandController : MonoBehaviour
{
	
    [ReadOnly] public int handIndex;

    [Header( "References" )]
    public NavMeshAgent agent;
    public Animator[] fingerAnimators;
    public Transform handTransform;
    public Transform idleHandTarget;
    [SerializeField] private Transform body = default;

    [HideInInspector] public StateMachine<HandController> stateMachine;

    [Header( "Settings" )]
    public bool defaultFingerPos = true;

    [Header( "Gesture Settings" )]
    public GestureSequence sentence;
    public bool clearCircle;
    public bool startAtCentre;
    public bool holdStart;
    public bool endAtCentre;
	public bool returnToIdle;
    public bool inputGesture;
    public bool fingerPosAtCentre;
    public float gestureDistance;
    public float gestureSpeed = 1f;
    public float holdGestureFor = 1f;

    [Header( "Point Settings" )]
    public float pointSpeed = 1f;
    public float holdPointFor = 3f;

    [Header( "Walk Settings" )]
    public LayerMask terrainLayer = default;
    public float heightOffset = default;
    public float speed = 1;
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

    [HideInInspector] public AlienIKManager ikManager;
    [HideInInspector] public Vector3 footSpacing;
    [HideInInspector] public Vector3 oldPosition, currentPosition, newPosition;
    [HideInInspector] public float heightRandomization;
    [HideInInspector] public float lerp;
    [HideInInspector] public bool fingersOpen = true;

    [Header( "Runtime" )]
    [ReadOnly] public List<Gesture> gesturesToMake = new List<Gesture>();
    [ReadOnly] public GestureCircle gestureCircle;
    [ReadOnly] public MoveState moveState;
    [ReadOnly] public float holdTimeStamp = 0f;
    [ReadOnly] public int sentenceIndex = 0;
    [ReadOnly] public int wordIndex = 0;
    [ReadOnly] public Vector3 preGestureHandPos;
    [ReadOnly] public Vector3 handTarget;
}
