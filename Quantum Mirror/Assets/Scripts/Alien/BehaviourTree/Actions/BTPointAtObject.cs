using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTPointAtObject : BTActionNode
{

    public RunTimeSet<Transform> objects;
    public int maxObjects;
    public float cutOffDistance;
    public float pointSpeed;
    public float holdPointFor;

    protected override void OnStart() {
        //Find hand and set point targets.
        Transform[] objectsToPointAt = context.ikManager.FindClosestObjectsInList( objects, maxObjects, cutOffDistance );
        context.ikManager.FindPointHands( objectsToPointAt );
        Transform obj = context.gc.FindClosestObjectInList( objects );
        context.gc.handTarget = obj.transform.position;
        context.gc.pointHandIndex = context.gc.FindClosestHand( obj ).handIndex;
        context.ikManager.RequestHand( context.gc.pointHandIndex, context.ikManager.pointingHands );

        context.gc.gestureState = MoveState.Moving;
        context.gc.pointSpeed = pointSpeed;
        context.gc.holdPointFor = holdPointFor;
    }

    protected override void OnStop() {
		for ( int i = 0; i < context.ikManager.pointingHands.Count; i++ )
            context.ikManager.ReleaseHand( context.ikManager.pointingHands[ i ].handIndex, context.ikManager.pointingHands );
        context.gc.pointHandIndex = -1;
    }

    protected override State OnUpdate() {
        return context.gc.Point();
    }
}
