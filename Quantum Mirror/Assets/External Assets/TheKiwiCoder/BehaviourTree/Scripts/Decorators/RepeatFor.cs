using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RepeatFor : DecoratorNode
{
    public float duration;
    private float startTime;

    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        switch ( child.Update() )
        {
            case State.Running:
                break;
            case State.Failure:
                if ( Time.time - startTime < duration )
                    return State.Running;
                else
                    return State.Failure;
            case State.Success:
                if ( Time.time - startTime < duration )
                    return State.Running;
                else
                    return State.Success;
        }
        return State.Running;
    }
}
