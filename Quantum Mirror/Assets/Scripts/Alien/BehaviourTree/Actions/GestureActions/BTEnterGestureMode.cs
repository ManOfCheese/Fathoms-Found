using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTEnterGestureMode : BTActionNode
{
    protected override void OnStart() {
        context.gc.EnterGestureMode();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
