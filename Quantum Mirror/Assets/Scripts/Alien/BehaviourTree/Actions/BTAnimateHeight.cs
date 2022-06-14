using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTAnimateHeight : BTActionNode
{

    public bool useSlerp;
    public float speed;
    public float targetHeight;

    private float startTimeStamp;

    protected override void OnStart() {
        startTimeStamp = Time.time;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return context.ikManager.ChangeAlienHeight( useSlerp, speed, targetHeight );
    }
}
