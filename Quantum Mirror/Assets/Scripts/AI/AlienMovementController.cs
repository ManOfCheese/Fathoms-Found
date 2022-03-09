using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MovementMode
{
    PointToPoint,
    Timed
}

public enum WanderShape
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
    public MovementMode movementMode;
    public WanderShape wanderShape;
    public float speed;
    public float changeDestinationTime;
    public float minDistance;
    public float maxDistance;
    public float destinationReachedWindow;

    [Space(10)]
    public Transform wanderTorusCenter;
    public float torusInnerRadius;
    public float torusOuterRadius;

    [Space( 10 )]
    public Transform wandercircleCenter;
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
        switch ( wanderShape )
        {
            case WanderShape.Torus:
                return GetRandomPos( wanderTorusCenter.transform.position, torusInnerRadius, torusOuterRadius );
            case WanderShape.Circle:
                return GetRandomPos( wandercircleCenter.transform.position, 0f, circleRadius );
            case WanderShape.None:
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
        if ( wandercircleCenter )
            Gizmos.DrawWireSphere( wandercircleCenter.transform.position, circleRadius );
        if ( wanderTorusCenter ) {
            Gizmos.DrawWireSphere( wanderTorusCenter.transform.position, torusInnerRadius );
            Gizmos.DrawWireSphere( wanderTorusCenter.transform.position, torusOuterRadius );
        }
	}
}
