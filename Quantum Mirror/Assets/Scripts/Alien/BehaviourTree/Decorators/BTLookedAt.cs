using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTLookedAt : BTDecoratorNode
{

    public ObserverType agentType;
    public string agentName;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Failure;
    }
}
