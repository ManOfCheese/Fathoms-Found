using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTExitGestureMode : BTActionNode
{
    protected override void OnStart() {
        context.gc.ExitGestureMode();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
