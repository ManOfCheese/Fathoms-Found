using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTPointAtObject : BTActionNode
{

    public RunTimeSet<Transform> objects;
    public int maxObjects;
    public float cutOffDistance;
    public float pointSpeed;
    public float holdPointFor;

    protected override void OnStart() {
        //Find hand and set point targets.
        Transform[] objectsToPointAt = context.ikManager.FindClosestObjectsInList( objects, maxObjects, cutOffDistance );
        
        if ( context.ikManager.FindPointHands( objectsToPointAt ) )
		{
            for ( int i = 0; i < context.ikManager.allHands.Count; i++ )
            {
                if ( context.ikManager.allHands[ i ].stateMachine.CurrentState == context.ikManager.statesByName[ "PointingState" ] )
                {
                    HandController hand = context.ikManager.allHands[ i ];

                    hand.moveState = MoveState.Starting;
                    hand.pointSpeed = pointSpeed;
                    hand.holdPointFor = holdPointFor;
                }
            }
        }
    }

    protected override void OnStop() {

    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
