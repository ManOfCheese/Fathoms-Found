using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTRepeatFor : BTDecoratorNode
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
        Debug.Log( Time.time - startTime + " - " + duration );
        if ( Time.time - startTime < duration )
            return State.Running;
        else
            return child.Update();
    }
}
