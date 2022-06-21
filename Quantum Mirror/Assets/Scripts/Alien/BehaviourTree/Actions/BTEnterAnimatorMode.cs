using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTEnterAnimatorMode : BTActionNode
{

    public float handSpeed;
    public Vector3[] animatorHandStartPositions;

    private List<int> handsRepositioned = new List<int>();

    protected override void OnStart() {
        context.ikManager.enabled = false;
        for ( int i = 0; i < context.ikManager.allHands.Count; i++ )
            context.ikManager.allHands[ i ].enabled = false;
    }

    protected override void OnStop() {
        handsRepositioned.Clear();
    }

    protected override State OnUpdate() {
        float speed = handSpeed * Time.deltaTime;
        for ( int i = 0; i < context.ikManager.allHands.Count; i++ )
		{
            if ( !handsRepositioned.Contains( i ) )
			{
                if ( speed > Vector3.Distance( context.ikManager.allHands[ i ].transform.localPosition, animatorHandStartPositions[ i ] ) )
                {
                    context.ikManager.allHands[ i ].transform.localPosition = animatorHandStartPositions[ i ];
                    handsRepositioned.Add( i );
                }
                else
                {
                    context.ikManager.allHands[ i ].transform.localPosition = Vector3.MoveTowards( context.ikManager.allHands[ i ].transform.localPosition,
                        animatorHandStartPositions[ i ], speed );
                }
            }
		}

        if ( handsRepositioned.Count >= context.ikManager.allHands.Count )
		{
            context.animator.enabled = true;
            return State.Success;
        }
        else
            return State.Running;
    }
}
