using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//keeps ticking the child node until it returns failure.
public class BTRepeat : BTDecorator
{

	public int maxTicks;
	private int ticks;

	public override TaskState Tick( Blackboard blackboard )
	{
		bool breakLoop = false;
		while ( ticks < maxTicks ) { 
			TaskState taskState = childNode.Tick( blackboard );
			switch ( childNode.Tick( blackboard ) )
			{
				case TaskState.Success:
					break;
				case TaskState.Failure:
					breakLoop = true;
					break;
				case TaskState.Running:
					return TaskState.Running;
				default:
					break;
			}

			if ( breakLoop )
				break;
		}
		return TaskState.Failure;
	}

}
