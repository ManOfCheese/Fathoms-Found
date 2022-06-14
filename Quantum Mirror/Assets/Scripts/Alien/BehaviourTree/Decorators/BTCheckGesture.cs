using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTCheckGesture : BTDecoratorNode
{

    public string gCode;
    private string playerGestures;

    protected override void OnStart() {
        if ( context.manager.gestureCircle != null )
		{
            playerGestures = context.manager.gestureCircle.sentence;
		}
    }

    protected override void OnStop() {
        playerGestures = "";
    }

    protected override State OnUpdate()
	{
        if ( gCode == playerGestures )
            return child.Update();
        return State.Failure;
    }
}
