using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTWander : BTMove
{

    public bool interruptable = false;

    protected override void OnStart()
    {
        if ( context.mc.agent.remainingDistance < context.mc.tolerance && context.mc.movementMode != MovementMode.Static )
            blackboard.AddData( "moveToPosition", context.mc.Wander() );
        base.OnStart();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if ( context.mc.movementMode != MovementMode.Static )
		{
            State state = context.mc.EvaluateWander();

            if ( interruptable )
            {
                if ( state == State.Running )
                    state = State.Success;
            }
            return state;
        }
		else
		{
            context.mc.agent.destination = context.mc.transform.position;
            return State.Failure;
        }
    }
}
