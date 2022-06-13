using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTCheckGesture : BTDecoratorNode
{

    public string gCode;
    private List<string> playerGestures = new List<string>();

    protected override void OnStart() {
        if ( context.manager.gestureCircles.Count > 0 )
		{
			for ( int i = 0; i < context.manager.gestureCircles.Count; i++ )
                playerGestures.Add( context.manager.gestureCircles[ i ].sentence );
		}
    }

    protected override void OnStop() {
        playerGestures.Clear();
    }

    protected override State OnUpdate()
	{
		for ( int i = 0; i < playerGestures.Count; i++ )
		{
            if ( gCode == playerGestures[ i ] )
                return child.Update();

        }
        return State.Failure;
    }
}
