using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTForgetGestureCircle : BTActionNode
{
    protected override void OnStart() {
        context.manager.gestureCircle = null;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}