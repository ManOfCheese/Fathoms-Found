using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This node always returns failure.
public class BTForceFailure : BTDecorator
{
	public override TaskState Tick( Blackboard blackboard )
	{
		switch ( childNode.Tick( blackboard ) )
		{
			case TaskState.Success:
				return TaskState.Failure;
			case TaskState.Failure:
				return TaskState.Failure;
			case TaskState.Running:
				return TaskState.Running;
			default:
				break;
		}
		return TaskState.Success;
	}

}
