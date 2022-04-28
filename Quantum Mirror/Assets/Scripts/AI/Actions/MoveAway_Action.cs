using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Action", menuName = "Action/MoveAwayAction" )]
public class MoveAway_Action : Move_Action {

	public override void ExecuteAction( AlienManager alienManager ) {
		alienManager.mc.agent.destination = alienManager.transform.position - alienManager.moveTarget.position;
	}

}
