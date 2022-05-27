using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMakeGesture : BTActionNode
{
    protected override void OnStart() {
        context.gestureController.SetGesture();
        context.moveController.agent.destination = context.moveController.agent.transform.position;
        context.gestureController.repositioning = true;
        context.gestureController.repositionedLegs.Clear();

        for ( int i = 0; i < context.gestureController.hands.Length; i++ )
            context.gestureController.hands[ i ].ikHandler.enabled = false;
    }

    protected override void OnStop() {
        context.gestureController.gesturing = false;
        context.gestureController.holdingGesture = false;
        context.gestureController.gestureHandIndex = -1;
        context.gestureController.repositioning = false;
        context.gestureController.endGesture = false;

        for ( int i = 0; i < context.gestureController.hands.Length; i++ )
            context.gestureController.hands[ i ].ikHandler.enabled = true;

    }

    protected override State OnUpdate() {
        return context.gestureController.Gesture();
    }
}
