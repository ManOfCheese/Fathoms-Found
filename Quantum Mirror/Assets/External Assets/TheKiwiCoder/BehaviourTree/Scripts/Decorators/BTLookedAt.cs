using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTLookedAt : BTDecoratorNode
{

    public AgentType agentType;
    public string agentName;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
		for ( int i = 0; i < context.agent.lookedAtBy.Count; i++ )
		{
            if ( agentName != "" )
			{
                if ( context.agent.lookedAtBy[ i ].agentName == agentName )
                    return child.Update();
			}
            else if ( context.agent.lookedAtBy[ i ].agentType == agentType )
			{
                return child.Update();
            }
		}
        return State.Failure;
    }
}
