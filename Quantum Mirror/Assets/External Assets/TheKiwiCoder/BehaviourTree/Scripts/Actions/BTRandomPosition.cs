using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTRandomPosition : BTBlackBoardActionNode
{
    public Vector2 min = Vector2.one * -10;
    public Vector2 max = Vector2.one * 10;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        targetBlackboard.AddData( "MoveToPosition", targetBlackboard.vector3s, new Vector3( Random.Range(min.x, max.x), 0f, Random.Range( min.y, max.y ) ) );
        return State.Success;
    }
}
