using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTPointAtObject : BTActionNode
{

    public int maxObjects;

    public bool overrideStandardSettings;
    public float pointSpeed;
    public float holdPointFor;
    [Space( 10 )]
    public float maxDistance;

    protected override void OnStart() {
        //Find hand and set point targets.
        Vector3[] objectsToPointAt = context.ikManager.FindClosestObjectsInList( blackboard.GetData( "objectTargets", new List<Vector3>() ), 
            Mathf.Min( context.ikManager.handsAvailable, maxObjects ) );
        if ( context.ikManager.FindPointHands( objectsToPointAt, maxDistance ) )
		{
            for ( int i = 0; i < context.ikManager.allHands.Count; i++ )
            {
                if ( context.ikManager.allHands[ i ].stateMachine.CurrentState == context.ikManager.statesByName[ "PointingState" ] )
                {
                    HandController hand = context.ikManager.allHands[ i ];

                    if ( overrideStandardSettings )
                    {
                        hand.pointSpeed = pointSpeed;
                        hand.holdPointFor = holdPointFor;
                    }
					else
					{
                        hand.pointSpeed = context.ikManager.pointSpeed;
                        hand.holdPointFor = context.ikManager.holdPointFor;
                    }
                }
            }
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        int pointingHands = 0;
        for ( int i = 0; i < context.ikManager.allHands.Count; i++ )
        {
            if ( context.ikManager.allHands[ i ].stateMachine.CurrentState.stateName == "PointingState" )
                pointingHands++;
        }

        if ( pointingHands > 0 )
            return State.Running;
        else
            return State.Success;
    }
}
