using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTExitAnimatorMode : BTActionNode
{

    public float handSpeed;

    private List<int> handsRepositioned = new List<int>();

    protected override void OnStart() {
        context.animator.enabled = false;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        float speed = handSpeed * Time.deltaTime;
        for ( int i = 0; i < context.ikManager.allHands.Count; i++ )
        {
            if ( !handsRepositioned.Contains( i ) )
			{
                if ( speed > Vector3.Distance( context.ikManager.allHands[ i ].transform.position, context.ikManager.allHands[ i ].idleHandTarget.position ) )
                {
                    context.ikManager.allHands[ i ].transform.position = context.ikManager.allHands[ i ].idleHandTarget.position;
                    handsRepositioned.Add( i );
                }
                else
                {
                    context.ikManager.allHands[ i ].transform.position = Vector3.MoveTowards( context.ikManager.allHands[ i ].transform.position,
                        context.ikManager.allHands[ i ].idleHandTarget.position, speed );
                }
            }
        }
        if ( handsRepositioned.Count >= context.ikManager.allHands.Count )
		{
            context.ikManager.enabled = true;
            for ( int i = 0; i < context.ikManager.allHands.Count; i++ )
			{
                context.ikManager.allHands[ i ].enabled = true;
                context.ikManager.allHands[ i ].oldPosition = context.ikManager.allHands[ i ].handTransform.position;
                context.ikManager.allHands[ i ].currentPosition = context.ikManager.allHands[ i ].handTransform.position;
                context.ikManager.allHands[ i ].newPosition = context.ikManager.allHands[ i ].handTransform.position;
            }
            return State.Success;
        }
        else
            return State.Running;
    }
}
