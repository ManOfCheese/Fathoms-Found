using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class PointAt : ActionNode
{

    private Transform pointTarget;

    protected override void OnStart() {
        pointTarget = context.gestureController.FindClosetObjectInList( blackboard.targetObjects );
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return context.gestureController.Point();
    }
}
