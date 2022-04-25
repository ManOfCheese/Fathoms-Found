using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Turns Failure into Success and Success into Failure.
public class BTInvert : BTDecorator
{

	public override TaskState Tick( Blackboard blackboard )
	{
		switch ( childNode.Tick( blackboard ) )
		{
			case TaskState.Success:
				return TaskState.Failure;
			case TaskState.Failure:
				return TaskState.Success;
			case TaskState.Running:
				return TaskState.Running;
			default:
				break;
		}
		return TaskState.Failure;
	}

}
