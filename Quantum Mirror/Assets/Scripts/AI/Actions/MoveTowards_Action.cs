using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "MoveTowards_Action", menuName = "Action/MoveTowards_Action" )]
public class MoveTowards_Action : Move_Action {

	public RunTimeSet<Transform> targetObjects;

	public override void ExecuteAction( AlienManager alienManager ) {
		//Find closest object.
		float shortestDist = 0f;
		int closestObjectIndex = 0;
		for ( int i = 0; i < targetObjects.Items.Count; i++ )
		{
			if ( i == 0 )
			{
				shortestDist = Vector3.Distance( targetObjects.Items[ i ].transform.position, alienManager.transform.position );
				closestObjectIndex = 0;
			}
			else
			{
				float dist = Vector3.Distance( targetObjects.Items[ i ].transform.position, alienManager.transform.position );
				if ( dist < shortestDist )
				{
					shortestDist = dist;
					closestObjectIndex = i;
				}
			}
		}
		alienManager.moveTarget = targetObjects.Items[ closestObjectIndex ];

		alienManager.mc.agent.destination = alienManager.moveTarget.position;
		alienManager.stateMachine.ChangeState( InterestState.Instance );
	}

}
