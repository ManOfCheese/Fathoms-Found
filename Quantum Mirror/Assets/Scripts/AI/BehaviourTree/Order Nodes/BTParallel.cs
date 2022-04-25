using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Executes all nodes "in parallel" returning success if N number of nodes succeed, otherwise it returns failure.
public class BTParallel : BTOrder
{

    public int minSuccesCount;

	public override TaskState Tick( Blackboard blackboard )
	{
		int succesCount = 0;
		for ( int i = 0; i < childNodes.Count; i++ )
		{
			switch ( childNodes[ i ].Tick( blackboard ) )
			{
				case TaskState.Success:
					succesCount++;
					break;
				case TaskState.Failure:
					break;
				case TaskState.Running:
					return TaskState.Running;
				default:
					break;
			}
		}

		if ( succesCount >= minSuccesCount )
			return TaskState.Success;
		else
			return TaskState.Failure;
	}

}
