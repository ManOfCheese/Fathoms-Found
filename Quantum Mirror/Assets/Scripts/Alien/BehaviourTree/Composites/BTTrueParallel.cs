using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTTrueParallel : BTCompositeNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
		for ( int i = 0; i < children.Count; i++ )
		{
            children[ i ].Update();
		}
        return State.Running;
    }
}
