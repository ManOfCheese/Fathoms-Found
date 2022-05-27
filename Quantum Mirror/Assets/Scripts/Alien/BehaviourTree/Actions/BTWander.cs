using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTWander : BTMoveToPosition
{

    public bool interruptable = false;
    public MovementMode movementMode = MovementMode.PointToPoint;
    public MovementShape movementShape = MovementShape.Circle;

    protected override void OnStart()
    {
        context.moveController.movementMode = movementMode;
        context.moveController.movementShape = movementShape;
        if ( context.moveController.agent.remainingDistance < tolerance )
            blackboard.AddData( "moveToPosition", blackboard.vector3s, context.moveController.Wander() );
        base.OnStart();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        State state = context.moveController.EvaluateWander();

        if ( interruptable )
		{
            if ( state == State.Running )
                state = State.Success;
		}
        return state;
    }
}
