using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTForgetGestureCircle : BTActionNode
{
    protected override void OnStart() {
        context.manager.gestureCircle = null;
        if ( context.ikManager.gestureHand != null )
        {
            context.ikManager.gestureHand.stateMachine.ChangeState( context.ikManager.statesByName[ "WalkingState" ] );
            context.ikManager.gestureHand = null;
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
