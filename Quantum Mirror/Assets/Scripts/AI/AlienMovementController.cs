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
    public NavMeshAgent agent;
    public AlienIKHandler[] hands;

    [Header( "Settings" )]
    [Tooltip( "How should the alien move, if at all." )]
    public MovementMode movementMode;
    [Tooltip( "What kind of pattern should be used to determine destinations." )]
    public MovementShape movementShape;
    [Tooltip( "How fast in units does the alien walk." )]
    public float speed;
    [Tooltip( "How often in seconds should the alien switch destination" )]
    public float changeDestinationTime;
    [Tooltip( "How close in units should the alien be to it's destination to consider it reached, this is from center to center." )]
    public float destinationReachedWindow;

    //None mode
    [Tooltip( "What is the minimum distance in units between the current position and the new destination." )]
    public float minDistance;
    [Tooltip( "What is the maximum distance in units between the current position and the new destination." )]
    public float maxDistance;

    //Torus mode
    public Transform torusCenter;
    [Tooltip( "What is the inner radius in units of the torus." )]
    public float torusInnerRadius;
    [Tooltip( "What is the outer radius in units of the torus." )]
    public float torusOuterRadius;

    //Circle mode
    public Transform circleCenter;
    [Tooltip( "What is the radius in units of the circle." )]
    public float circleRadius;

    [Header( "Runtime" )]
    [ReadOnly] public float bodyHeight;
    [ReadOnly] public Vector3 destination;

    [HideInInspector] public float idleDestinationTimestamp;

	private void Awake()
	{
        agent.speed = speed;
        agent.destination = transform.position;

		for ( int i = 0; i < hands.Length; i++ )
		{
            hands[ i ].speed = speed;
		}
	}

	private void OnValidate()
	{
        if ( agent != null )
            agent.speed = speed;

        for ( int i = 0; i < hands.Length; i++ )
        {
            hands[ i ].speed = speed;
        }
    }

	public void EvaluateMovement( bool isFirstDestination )
	{
        switch ( movementMode )
        {
            case MovementMode.Static:
                break;
            case MovementMode.PointToPoint:
                if ( CheckDestinationReached() || isFirstDestination )
                {
                    SetDestination( CheckWanderShape() );
                }
                break;
            case MovementMode.Timed:
                if ( Time.time - idleDestinationTimestamp > changeDestinationTime || isFirstDestination )
                {
                    SetDestination( CheckWanderShape() );
                    idleDestinationTimestamp = Time.time;
                }
                break;
            default:
                break;
        }
    }

    public Vector3 CheckWanderShape()
	{
        switch ( movementShape )
        {
            case MovementShape.Torus:
                return GetRandomPos( torusCenter.transform.position, torusInnerRadius, torusOuterRadius );
            case MovementShape.Circle:
                return GetRandomPos( circleCenter.transform.position, 0f, circleRadius );
            case MovementShape.None:
                return GetRandomPos( transform.position, minDistance, maxDistance );
            default:
                return GetRandomPos( transform.position, minDistance, maxDistance );
        }
    }

    public void SetDestination( Vector3 _destination )
    {
        destination = _destination;
        agent.destination = _destination;
    }

    public bool CheckDestinationReached()
    {
        if ( Vector3.Distance( destination, transform.position ) < destinationReachedWindow )
            return true;
        else
            return false;
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

	private void OnDrawGizmos()
	{
        Gizmos.DrawSphere( destination, 2 );
        if ( circleCenter )
            Gizmos.DrawWireSphere( circleCenter.transform.position, circleRadius );
        if ( torusCenter ) {
            Gizmos.DrawWireSphere( torusCenter.transform.position, torusInnerRadius );
            Gizmos.DrawWireSphere( torusCenter.transform.position, torusOuterRadius );
        }
	}
}
