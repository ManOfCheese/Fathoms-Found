using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This node always returns success.
public class BTForceSuccess : BTDecorator
{

	public override TaskState Tick( Blackboard blackboard )
	{
		switch ( childNode.Tick( blackboard ) )
		{
			case TaskState.Success:
				return TaskState.Success;
			case TaskState.Failure:
				return TaskState.Success;
			case TaskState.Running:
				return TaskState.Running;
			default:
				break;
		}
		return TaskState.Success;
	}

}
