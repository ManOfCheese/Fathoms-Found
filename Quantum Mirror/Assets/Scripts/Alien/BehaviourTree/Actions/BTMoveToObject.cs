using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMoveToObject : BTActionNode
{

    public RunTimeSet<Transform> objects;
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    protected override void OnStart() {
        Transform closestObject = null;
        float closestDist = 0f;
		for ( int i = 0; i < objects.Items.Count; i++ )
		{
            float dist = Vector3.Distance( objects.Items[ i ].transform.position, context.manager.transform.position );
            if ( closestObject == null )
			{
                closestObject = objects.Items[ i ];
                closestDist = dist;
            }
            else if ( dist < closestDist )
			{
                closestObject = objects.Items[ i ];
                closestDist = dist;
            }
            Debug.Log( closestDist );
        }
        if ( closestObject != null )
            context.moveController.agent.destination = closestObject.transform.position;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if ( context.moveController.agent.pathPending )
        {
            return State.Running;
        }
        else if ( context.moveController.agent.remainingDistance < tolerance )
        {
            blackboard.AddData( "moveToPosition", blackboard.vector3s, Vector3.zero );
            AlienBlackboard alienBlackboard = blackboard as AlienBlackboard;
            alienBlackboard.moveToPosition = blackboard.GetData( "moveToPosition", blackboard.vector3s, new Vector3() );
            return State.Success;
        }
        else if ( context.moveController.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid )
        {
            return State.Failure;
        }

        return State.Running;
    }
}
