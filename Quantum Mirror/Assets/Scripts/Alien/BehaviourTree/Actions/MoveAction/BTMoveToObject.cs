using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTMoveToObject : BTMove
{

    public RunTimeSet<Transform> objects;

    protected override void OnStart() {
        Transform closestObject = null;
        float closestDist = 0f;
		for ( int i = 0; i < objects.Items.Count; i++ )
		{
            float dist = Vector3.Distance( objects.Items[ i ].transform.position, context.manager.transform.position );
            if ( closestObject == null )
			{
                closestObject = objects.Items[ i ];
                closestDist = dist;
            }
            else if ( dist < closestDist )
			{
                closestObject = objects.Items[ i ];
                closestDist = dist;
            }
        }
        if ( closestObject != null )
            blackboard.AddData( "moveToPosition", closestObject.transform.position );

        base.OnStart();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return base.OnUpdate();
    }
}
