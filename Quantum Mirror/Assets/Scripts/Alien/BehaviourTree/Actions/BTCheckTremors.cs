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
            blackboard.AddData( "moveToPosition", blackboard.vector3s, context.manager.lastHeardTremor.position );
            //blackboard.AddData( "tremorDetected", blackboard.bools, true );
            //AlienBlackboard alienBlackboard = blackboard as AlienBlackboard;
            //alienBlackboard.moveToPosition = blackboard.GetData( "moveToPosition", blackboard.vector3s, new Vector3() );
            //alienBlackboard.tremorDetected = blackboard.GetData( "tremorDetected", blackboard.bools, new bool() );
            return State.Success;
        }
		else
		{
            //blackboard.AddData( "tremorDetected", blackboard.bools, false );
            //AlienBlackboard alienBlackboard = blackboard as AlienBlackboard;
            //alienBlackboard.tremorDetected = blackboard.GetData( "tremorDetected", blackboard.bools, new bool() );
            return State.Failure;
        }
    }
}
