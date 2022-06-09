using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMakeGesture : BTActionNode
{

    public GestureSequence sentence;
    public float gestureSpeed = 1f;
    public float holdGestureFor = 1f;
    public bool clearCircle;
    public bool startAtCentre;
    public bool endAtCentre;
    public bool inputGesture;

    protected override void OnStart() {
        if ( sentence == null )
            context.gestureController.FindGesture();
        context.moveController.agent.destination = context.moveController.agent.transform.position;

        for ( int i = 0; i < context.gestureController.hands.Length; i++ )
            context.gestureController.hands[ i ].ikHandler.enabled = false;

        context.gestureController.gestureSpeed = gestureSpeed;
        context.gestureController.holdGestureFor = holdGestureFor;
        context.gestureController.ResetGestureSettings();
    }

    protected override void OnStop() {
        context.gestureController.gestureHandIndex = -1;

        for ( int i = 0; i < context.gestureController.hands.Length; i++ )
            context.gestureController.hands[ i ].ikHandler.enabled = true;
    }

    protected override State OnUpdate() {
        if ( context.manager.gestureCircle != null )
            return context.gestureController.Gesture( sentence, clearCircle, startAtCentre, endAtCentre, inputGesture );
        else
            return State.Failure;
    }
}
