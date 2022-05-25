using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BlackboardManager : ScriptableObject
{

    public List<Blackboard> blackboards;
    public Blackboard main;

    public Blackboard GetBlackboardByType( System.Type type )
	{
		foreach ( Blackboard bb in blackboards )
		{
			if ( bb.GetType() == type )
			{
				return bb;
			}
		}
		return null;
	}

}
