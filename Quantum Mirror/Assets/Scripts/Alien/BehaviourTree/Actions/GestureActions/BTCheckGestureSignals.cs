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
            if ( context.manager.gestureSignal.gestureCircle.twoWayCircle )
                blackboard.AddData( "moveToPosition", context.manager.gestureSignal.gestureCircle.otherCircle.gesturePosition.position );
            else
                blackboard.AddData( "moveToPosition", context.manager.gestureSignal.gestureCircle.gesturePosition.position );
            return State.Success;
        }
        else
        {
            blackboard.AddData( "gestureSignalDetected", false );
            return State.Failure;
        }
    }
}
