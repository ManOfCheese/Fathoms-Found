using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMove : BTActionNode
{

    public bool overrideAgentSettings;
    public float tolerance;
    public float speed = 5;

    protected override void OnStart() {
        if ( overrideAgentSettings )
            context.mc.agent.speed = speed;
        context.mc.agent.destination = blackboard.GetData( "moveToPosition", new Vector3() );
    }

    protected override void OnStop() {
        if ( overrideAgentSettings )
            context.mc.agent.speed = context.ikManager.walkSpeed;
    }

    protected override State OnUpdate() {
        if ( context.mc.agent.pathPending )
        {
            return State.Running;
        }
        else if ( context.mc.agent.remainingDistance < tolerance )
        {
            blackboard.AddData( "moveToPosition", Vector3.zero );
            context.mc.agent.destination = context.manager.transform.position;
            return State.Success;
        }
        else if ( context.mc.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid )
        {
            return State.Failure;
        }

        return State.Running;
    }
}
