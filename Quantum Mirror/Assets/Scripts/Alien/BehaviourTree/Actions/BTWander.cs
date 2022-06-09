using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTWander : BTMove
{

    public bool interruptable = false;

    protected override void OnStart()
    {
        if ( context.moveController.agent.remainingDistance < context.moveController.tolerance && context.moveController.movementMode != MovementMode.Static )
            blackboard.AddData( "moveToPosition", blackboard.vector3s, context.moveController.Wander() );
        base.OnStart();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if ( context.moveController.movementMode != MovementMode.Static )
		{
            State state = context.moveController.EvaluateWander();

            if ( interruptable )
            {
                if ( state == State.Running )
                    state = State.Success;
            }
            return state;
        }
		else
		{
            context.moveController.agent.destination = context.moveController.transform.position;
            return State.Failure;
        }
    }
}
