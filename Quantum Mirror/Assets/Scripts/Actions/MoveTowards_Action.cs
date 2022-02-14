using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Action", menuName = "Action/MoveTowardsAction" )]
public class MoveTowards_Action : Move_Action {

	public override void ExecuteAction( NPC npc ) {
		npc.agent.destination = target.position;
	}

}
