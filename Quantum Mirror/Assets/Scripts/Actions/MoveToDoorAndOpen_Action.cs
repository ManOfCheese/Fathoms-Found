using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "MoveToDoorAndOpen_Action", menuName = "Action/MoveToDoorAndOpen_Action" )]
public class MoveToDoorAndOpen_Action : Action
{
	
	public RunTimeSet<Door> doors;

	public override void ExecuteAction( AlienManager alienManager )
	{
		if ( doors.Items.Count > 0 )
		{
			//Find closest object.
			float shortestDist = 0f;
			int closestObjectIndex = 0;
			for ( int i = 0; i < doors.Items.Count; i++ )
			{
				if ( i == 0 )
				{
					shortestDist = Vector3.Distance( doors.Items[ i ].transform.position, alienManager.transform.position );
					closestObjectIndex = 0;
				}
				else
				{
					float dist = Vector3.Distance( doors.Items[ i ].transform.position, alienManager.transform.position );
					if ( dist < shortestDist )
					{
						shortestDist = dist;
						closestObjectIndex = i;
					}
				}
			}
			
			alienManager.doorToOpen = doors.Items[ closestObjectIndex ];
			alienManager.mc.agent.destination = doors.Items[ closestObjectIndex ].transform.position;
			alienManager.stateMachine.ChangeState( InterestState.Instance );
		}
	}
}
