using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTUnlockMovement : BTActionNode
{
    protected override void OnStart() {
        context.mc.agent.isStopped = false;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
