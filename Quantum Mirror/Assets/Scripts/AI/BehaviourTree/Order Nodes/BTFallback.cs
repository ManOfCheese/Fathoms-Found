using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Calls nodes in order returning success if one of the nodes succeeds and failure is all the nodes fail.
public class BTFallback : BTOrder
{

	public override TaskState Tick( Blackboard blackboard )
	{
		int failCount = 0;
		for ( int i = 0; i < childNodes.Count; i++ )
		{
			switch ( childNodes[ i ].Tick( blackboard ) )
			{
				case TaskState.Success:
					return TaskState.Success;
				case TaskState.Failure:
					failCount++;
					break;
				case TaskState.Running:
					return TaskState.Running;
				default:
					break;
			}
		}

		if ( failCount == childNodes.Count )
			return TaskState.Failure;
		else
			return TaskState.Success;
	}

}
