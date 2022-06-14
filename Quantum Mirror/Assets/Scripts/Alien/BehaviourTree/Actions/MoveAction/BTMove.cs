using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMove : BTActionNode
{

    public bool overrideAgentSettings;
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;

    protected override void OnStart() {
        if ( overrideAgentSettings )
		{
            context.moveController.agent.stoppingDistance = stoppingDistance;
            context.moveController.agent.speed = speed;
            context.moveController.agent.updateRotation = updateRotation;
            context.moveController.agent.acceleration = acceleration;
        }
        context.moveController.agent.destination = blackboard.GetData( "moveToPosition", blackboard.vector3s, new Vector3() );
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if ( context.moveController.agent.pathPending )
        {
            return State.Running;
        }
        else if ( context.moveController.agent.remainingDistance < context.moveController.tolerance )
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
