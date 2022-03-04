using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum WanderMode
{
    TorusMode,
    RandomMode
}

public class Wander : MonoBehaviour
{

    [Header( "References" )]
    public Transform torusCenter;
    public Transform wanderTarget;
    public MoveTowards_Action moveTowards;
    public NPC npc;
    public NavMeshAgent agent;

    [Header( "Settings" )]
    public WanderMode wanderMode;
    public float changeDestinationTime;

    [Header( "Torus Mode Settings" )]
    public float innerTorusRadius;
    public float outerTorusRadius;

    [Header( "Free Range Settings" )]
    public float wanderRange;

    private float idleDestinationTimestamp;
    [ReadOnly] public bool idling;

    private Vector3 finalDestination;

	private void Awake()
	{
        idleDestinationTimestamp = Time.time;
        moveTowards.target = wanderTarget;
    }

	private void Update()
	{
        if ( Time.time - idleDestinationTimestamp > changeDestinationTime && idling )
        {
            switch ( wanderMode )
            {
                case WanderMode.TorusMode:
                    wanderTarget.transform.position = GetRandomPosOnTorus();
                    break;
                case WanderMode.RandomMode:
                    wanderTarget.transform.position = GetRandomPosOnNavMesh();
                    break;
                default:
                    break;
            }
            idleDestinationTimestamp = Time.time;
        }

        Debug.DrawLine( torusCenter.position, torusCenter.position + finalDestination );
        Debug.Log( finalDestination.magnitude );
        Debug.DrawLine( agent.destination, agent.destination + transform.up );
    }

    public void OnIdle()
	{
		switch ( wanderMode )
		{
			case WanderMode.TorusMode:
                wanderTarget.transform.position = GetRandomPosOnTorus();
                break;
			case WanderMode.RandomMode:
                wanderTarget.transform.position = GetRandomPosOnNavMesh();
                break;
			default:
				break;
		}
        idling = true;
    }

    public Vector3 GetRandomPosOnNavMesh()
    {
        Vector3 randomTarget = Random.insideUnitSphere * wanderRange;
        NavMeshHit hit;
        NavMesh.SamplePosition( randomTarget, out hit, wanderRange, 1 );
        Vector3 finalPosition = hit.position;
        return finalPosition;
    }

    public Vector3 GetRandomPosOnTorus()
	{
        Vector2 randomDir = new Vector2( Random.Range( -1f, 1f ), Random.Range( -1f, 1f ) );
        float magnitude = Random.Range( innerTorusRadius, outerTorusRadius );
        finalDestination = new Vector3( randomDir.normalized.x * magnitude, 0f, randomDir.normalized.y * magnitude );

        return torusCenter.position + finalDestination;
	}

}
