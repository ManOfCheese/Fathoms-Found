using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMoveToPosition : BTBlackBoardActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    protected override void OnStart() {
        context.moveController.agent.stoppingDistance = stoppingDistance;
        context.moveController.agent.speed = speed;
        context.moveController.agent.destination = targetBlackboard.GetData( "MoveToPosition", targetBlackboard.vector3s, new Vector3() );
        context.moveController.agent.updateRotation = updateRotation;
        context.moveController.agent.acceleration = acceleration;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (context.moveController.agent.pathPending) {
            return State.Running;
        }

        if (context.moveController.agent.remainingDistance < tolerance) {
            return State.Success;
        }

        if (context.moveController.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
            return State.Failure;
        }

        return State.Running;
    }
}
