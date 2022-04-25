using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskState
{
    Success,
    Failure,
    Running
}

//Abstract node.
public abstract class ABTNode : ScriptableObject
{
    
    protected TaskState state;

    public virtual TaskState Tick( Blackboard blackboard )
	{
        return state;
	}

}
