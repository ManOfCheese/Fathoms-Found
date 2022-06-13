using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTLockMovement : BTActionNode
{
    protected override void OnStart() {
        context.mc.agent.isStopped = true;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
