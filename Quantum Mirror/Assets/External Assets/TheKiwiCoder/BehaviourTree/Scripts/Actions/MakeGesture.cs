using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MakeGesture : ActionNode
{
    protected override void OnStart() {
        context.gestureController.SetGesture();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return context.gestureController.Gesture();
    }
}