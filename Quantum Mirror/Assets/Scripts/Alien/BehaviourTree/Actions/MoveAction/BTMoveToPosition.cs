using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMoveToPosition : BTMove
{

    public bool followTremors;
    public bool moveForwards;

    protected override void OnStart() {
        base.OnStart();
    }

    protected override void OnStop() {
        base.OnStop();
    }

    protected override State OnUpdate() {
        if ( followTremors )
		{
            if ( context.manager.lastHeardTremor.position != Vector3.zero )
			{
                blackboard.AddData( "moveToPosition", context.manager.lastHeardTremor.position );
                context.mc.agent.destination = context.manager.lastHeardTremor.position;
            }
        }
        //Vector3 toDestinationVector = ( context.mc.agent.destination - context.animator.transform.position ).normalized;
        //context.animator.transform.rotation = Quaternion.Euler( 0f, AngleBetweenVector2( Vector2.left, new Vector2( toDestinationVector.x, toDestinationVector.z ) ), 0f );

        return base.OnUpdate();
    }

    float AngleBetweenVector2( Vector2 vec1, Vector2 vec2 )
    {
        Vector2 vec1Rotated90 = new Vector2( -vec1.y, vec1.x );
        float sign = ( Vector2.Dot( vec1Rotated90, vec2 ) < 0 ) ? -1.0f : 1.0f;
        return Vector2.Angle( vec1, vec2 ) * sign;
    }
}
