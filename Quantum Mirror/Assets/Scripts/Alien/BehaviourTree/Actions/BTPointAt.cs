using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTPointAt : BTActionNode
{

    public RunTimeSet<Transform> objects;

    protected override void OnStart() {
        Transform obj = context.gc.FindClosetObjectInList( objects );
        context.gc.handTarget = obj.transform.position;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return context.gc.Point();
    }
}
