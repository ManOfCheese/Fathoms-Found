using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Wander : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    public MovementMode movementMode = MovementMode.PointToPoint;
    public MovementShape movementShape = MovementShape.Circle;

    protected override void OnStart()
    {
        context.moveController.agent.stoppingDistance = stoppingDistance;
        context.moveController.agent.speed = speed;
        context.moveController.agent.updateRotation = updateRotation;
        context.moveController.agent.acceleration = acceleration;
        context.moveController.movementMode = movementMode;
        context.moveController.movementShape = movementShape;
        context.moveController.Wander();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return context.moveController.EvaluateWander();
    }
}
