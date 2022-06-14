using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTSetBBPositionList : BTBBActionNode
{

    public List<Vector3> positionListValue = new List<Vector3>();
	public RunTimeSet<Transform> objectTargets;

    protected override void OnStart() {
    }

    protected override void OnStop() {
		positionListValue = new List<Vector3>();
    }

    protected override State OnUpdate() {
		if ( blackboard != null )
		{
			if ( objectTargets != null )
			{
				for ( int i = 0; i < objectTargets.Items.Count; i++ )
					positionListValue.Add( objectTargets.Items[ i ].position );
				blackboard.AddData( key, positionListValue );
			}
			else
				blackboard.AddData( key, new List<Vector3>() );
			return State.Success;
		}
		return State.Failure;
	}
}
