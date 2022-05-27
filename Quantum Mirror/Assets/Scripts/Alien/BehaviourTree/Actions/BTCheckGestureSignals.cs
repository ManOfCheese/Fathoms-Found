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
            //blackboard.AddData( "gestureSignalDetected", blackboard.bools, true );
            //AlienBlackboard alienBlackboard = blackboard as AlienBlackboard;
            //alienBlackboard.moveToPosition = blackboard.GetData( "moveToPosition", blackboard.vector3s, new Vector3() );
            //alienBlackboard.tremorDetected = blackboard.GetData( "gestureSignalDetected", blackboard.bools, new bool() );
            return State.Success;
        }
        else
        {
            blackboard.AddData( "gestureSignalDetected", blackboard.bools, false );
            //AlienBlackboard alienBlackboard = blackboard as AlienBlackboard;
            //alienBlackboard.tremorDetected = blackboard.GetData( "gestureSignalDetected", blackboard.bools, new bool() );
            return State.Failure;
        }
    }
}
