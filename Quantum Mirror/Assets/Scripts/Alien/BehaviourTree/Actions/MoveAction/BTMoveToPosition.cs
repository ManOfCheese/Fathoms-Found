using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMoveToPosition : BTMove
{

    protected override void OnStart() {
        base.OnStart();
    }

    protected override void OnStop() {

    }

    protected override State OnUpdate() {
        if ( context.moveController.agent.destination != blackboard.GetData( "moveToPosition", blackboard.vector3s, new Vector3() ) )
            context.moveController.agent.destination = blackboard.GetData( "moveToPosition", blackboard.vector3s, new Vector3() );

        return base.OnUpdate();
    }

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawSphere( context.moveController.agent.destination, 10f );
    }
}