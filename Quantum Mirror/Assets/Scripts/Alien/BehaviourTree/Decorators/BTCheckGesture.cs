using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTCheckGesture : BTDecoratorNode
{

    public string gCode;
    private string playerGesture;

    protected override void OnStart() {
        playerGesture = context.gestureController.respondGCode;
    }

    protected override void OnStop() {
        playerGesture = "";
    }

    protected override State OnUpdate()
	{
		if ( gCode == playerGesture )
		{
            return child.Update();
		}
        return State.Failure;
    }
}
