using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTCheckTremors : BTActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if ( context.manager.lastHeardTremor.position != Vector3.zero )
		{
            blackboard.AddData( "moveToPosition", context.manager.lastHeardTremor.position );
            return State.Success;
        }
		else
		{
            return State.Failure;
        }
    }
}
