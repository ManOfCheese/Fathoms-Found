using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Action", menuName = "Action/MoveAwayAction" )]
public class MoveAway_Action : Move_Action {

	public override void ExecuteAction( NPC npc ) {
		//npc.agent.destination = npc.transform.position - target.position;
	}

}
