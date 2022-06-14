using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTAnimateHeight : BTActionNode
{

    public bool useSlerp;
    public float duration;
    public float targetHeight;

    private float startTimeStamp;

    protected override void OnStart() {
        startTimeStamp = Time.time;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return context.gc.ChangeAlienHeight( useSlerp, startTimeStamp, duration, context.gc.alienBody.transform.localPosition.y, 
            targetHeight );
    }
}
