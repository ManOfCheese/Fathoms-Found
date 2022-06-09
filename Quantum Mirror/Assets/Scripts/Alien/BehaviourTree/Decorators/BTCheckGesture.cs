using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTCheckGesture : BTDecoratorNode
{

    public string gCode;
    private string playerGesture;

    protected override void OnStart() {
        if ( context.manager.gestureCircle != null )
            playerGesture = context.manager.gestureCircle.sentence;
    }

    protected override void OnStop() {
        playerGesture = "";
    }

    protected override State OnUpdate()
	{
		if ( gCode == playerGesture )
            return child.Update();
        return State.Failure;
    }
}
