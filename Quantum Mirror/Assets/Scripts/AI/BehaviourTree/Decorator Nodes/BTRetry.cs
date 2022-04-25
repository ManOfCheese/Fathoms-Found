using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//keeps ticking the child node until it returns success.
public class BTRetry : BTDecorator
{
	
	public int maxTicks;
	private int ticks;

	public override TaskState Tick( Blackboard blackboard )
	{
		bool breakLoop = false;
		while ( ticks < maxTicks )
		{
			TaskState taskState = childNode.Tick( blackboard );
			switch ( childNode.Tick( blackboard ) )
			{
				case TaskState.Success:
					breakLoop = true;
					break;
				case TaskState.Failure:;
					break;
				case TaskState.Running:
					return TaskState.Running;
				default:
					break;
			}

			if ( breakLoop )
				break;
		}
		return TaskState.Success;
	}

}
