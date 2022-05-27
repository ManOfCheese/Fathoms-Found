using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTCheckGestureSignals : BTActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if ( context.manager.gestureSignal.gestureCircle != null )
        {
            blackboard.AddData( "moveToPosition", blackboard.vector3s, context.manager.gestureSignal.gestureCircle.gesturePosition.position );
            return State.Success;
        }
        else
        {
            blackboard.AddData( "gestureSignalDetected", blackboard.bools, false );
            return State.Failure;
        }
    }
}
