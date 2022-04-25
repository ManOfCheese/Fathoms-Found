using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Calls the nodes in order returning failure if on of the nodes fails and success if all the nodes succeed.
public class BTSequence : BTOrder
{

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
					return TaskState.Failure;
				case TaskState.Running:
					return TaskState.Running;
				default:
					break;
			}
		}

		if ( succesCount == childNodes.Count )
			return TaskState.Success;
		else
			return TaskState.Failure;
	}

}
