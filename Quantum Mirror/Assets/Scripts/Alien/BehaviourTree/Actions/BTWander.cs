using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTWander : BTActionNode
{
    
    public MovementMode movementMode = MovementMode.PointToPoint;
    public MovementShape movementShape = MovementShape.Circle;

    protected override void OnStart()
    {
        context.moveController.movementMode = movementMode;
        context.moveController.movementShape = movementShape;
        blackboard.AddData( "moveToPosition", blackboard.vector3s, context.moveController.Wander() );
        AlienBlackboard alienBlackboard = blackboard as AlienBlackboard;
        alienBlackboard.moveToPosition = blackboard.GetData( "moveToPosition", blackboard.vector3s, new Vector3() );
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return context.moveController.EvaluateWander();
    }
}
