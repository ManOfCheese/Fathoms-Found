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
        if ( context.manager.tremorSource.position != Vector3.zero )
		{
            blackboard.AddData( "moveToPosition", blackboard.vector3s, context.manager.tremorSource.position );
            Debug.Log( "Set moveToPosition to " + context.manager.tremorSource.position );
            Debug.Log( Vector3.Distance( context.manager.tremorSource.position, context.manager.transform.position ) );
            return State.Success;
        }
        return State.Failure;
    }
}
