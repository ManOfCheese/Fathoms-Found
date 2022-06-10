using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTPointAtObject : BTActionNode
{

    public RunTimeSet<Transform> objects;
    public float pointSpeed;
    public float holdPointFor;

    protected override void OnStart() {
        Transform obj = context.gc.FindClosetObjectInList( objects );
        context.gc.handTarget = obj.transform.position;
        context.gc.pointHandIndex = context.gc.FindClosestHand( obj, true );
        context.gc.gestureState = GestureState.Gesturing;
        context.gc.pointSpeed = pointSpeed;
        context.gc.holdPointFor = holdPointFor;

        for ( int i = 0; i < context.gc.hands.Length; i++ )
            context.gc.hands[ i ].ikHandler.enabled = false;
    }

    protected override void OnStop() {
        for ( int i = 0; i < context.gc.hands.Length; i++ )
            context.gc.hands[ i ].ikHandler.enabled = true;
        context.gc.pointHandIndex = -1;
    }

    protected override State OnUpdate() {
        return context.gc.Point();
    }
}
