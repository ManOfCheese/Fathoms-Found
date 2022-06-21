using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MovementMode
{
    PointToPoint,
    Timed,
    Static
}

public enum MovementShape
{
    Torus,
    Circle,
    None
}

public class AlienMovementController : MonoBehaviour
{

    [Header( "References" )]
    public AlienIKHandler[] hands;

    [Header( "Settings" )]
    [Tooltip( "How should the alien move, if at all." )]
    public MovementMode movementMode;
    [Tooltip( "What kind of pattern should be used to determine destinations." )]
    public MovementShape movementShape;
    [Tooltip( "How close in units should the alien be to it's destination to consider it reached, this is from center to center." )]
    public float tolerance;

    [Tooltip( "How often in seconds should the alien switch destination" )]
    [SerializeField] private float changeDestinationTime;
    [Tooltip( "How fast in units does the alien walk." )]
    [SerializeField] private float speed;

    [Header( "None Settings" )]
    [Tooltip( "What is the minimum distance in units between the current position and the new destination." )]
    public float minDistance;
    [Tooltip( "What is the maximum distance in units between the current position and the new destination." )]
    public float maxDistance;

    [Header( "Torus Settings" )]
    [Tooltip( "What is the inner radius in units of the torus." )]
    public float torusInnerRadius;

    [Header( "Circle Settings" )]
    public Transform wanderCentre;
    [Tooltip( "What is the radius in units of the circle." )]
    public float wanderRadius;

    [HideInInspector] public NavMeshAgent agent;
    private float idleDestinationTimestamp;

	private void Awake()
	{
        agent = GetComponentInParent<NavMeshAgent>();
        
        agent.speed = speed;
        agent.destination = transform.position;

		//for ( int i = 0; i < hands.Length; i++ )
        //    hands[ i ].speed = speed;
	}

	private void OnValidate()
	{
        if ( agent != null )
            agent.speed = speed;

        //for ( int i = 0; i < hands.Length; i++ )
        //    hands[ i ].speed = speed;
    }

    public Vector3 Wander()
	{
        switch ( movementShape )
        {
            case MovementShape.Torus:
                return GetRandomPos( wanderCentre.transform.position, torusInnerRadius, wanderRadius );
            case MovementShape.Circle:
                if ( wanderCentre != null )
                    return GetRandomPos( wanderCentre.transform.position, 0f, wanderRadius );
                else
                    return GetRandomPos( Vector3.zero, 0f, wanderRadius );
            case MovementShape.None:
                return GetRandomPos( transform.position, minDistance, maxDistance );
            default:
                return GetRandomPos( transform.position, minDistance, maxDistance );
        }
    }

    public TheKiwiCoder.BTNode.State EvaluateWander()
    {
        switch ( movementMode )
        {
            case MovementMode.Static:
                return TheKiwiCoder.BTNode.State.Success;
            case MovementMode.PointToPoint:
                if ( agent.remainingDistance < tolerance )
                    return TheKiwiCoder.BTNode.State.Success;
                else if ( agent.pathStatus == NavMeshPathStatus.PathInvalid )
                    return TheKiwiCoder.BTNode.State.Failure;
                else
                    return TheKiwiCoder.BTNode.State.Running;
            case MovementMode.Timed:
                if ( Time.time - idleDestinationTimestamp > changeDestinationTime )
                {
                    idleDestinationTimestamp = Time.time;
                    return TheKiwiCoder.BTNode.State.Success;
                }
                return TheKiwiCoder.BTNode.State.Running;
            default:
                return TheKiwiCoder.BTNode.State.Success;
        }
    }

    public Vector3 GetRandomPos( Vector3 center, float min, float max )
    {
        Vector2 randomTarget = new Vector3( Random.Range( 0f, 1f ), Random.Range( 0f, 1f ) );
        float magnitude = Random.Range( min, max );

        NavMeshHit hit;
        NavMesh.SamplePosition( center + RandomizeDirection( randomTarget.normalized * magnitude ), out hit, 500, 1 );
        return hit.position;
    }

    public Vector3 RandomizeDirection( Vector2 randomTarget )
    {
        int invertX = Random.Range( 0, 2 );
        int invertY = Random.Range( 0, 2 );

        Vector3 finalDestination = Vector3.zero;
        if ( invertX == 0 )
            finalDestination = new Vector3( randomTarget.x, 0f, randomTarget.y );
        else
            finalDestination = new Vector3( randomTarget.x * -1f, 0f, randomTarget.y );

        if ( invertY == 0 )
            finalDestination = new Vector3( finalDestination.x, 0f, randomTarget.y );
        else
            finalDestination = new Vector3( finalDestination.x, 0f, randomTarget.y * -1f );

        return finalDestination;
    }

    public void MoveToObjectOfType()
	{

	}

	private void OnDrawGizmos()
	{
        if ( agent != null )
            Gizmos.DrawSphere( agent.destination, 2 );
        if ( movementShape == MovementShape.Circle )
            Gizmos.DrawWireSphere( wanderCentre.transform.position, wanderRadius );
        if ( movementShape == MovementShape.Torus ) {
            Gizmos.DrawWireSphere( wanderCentre.transform.position, torusInnerRadius );
            Gizmos.DrawWireSphere( wanderCentre.transform.position, wanderRadius );
        }
	}
}
