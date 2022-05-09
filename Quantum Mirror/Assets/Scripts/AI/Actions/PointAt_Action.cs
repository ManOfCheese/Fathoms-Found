using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Action", menuName = "Action/PointAt_Action" )]
public class PointAt_Action : Action
{

	public RunTimeSet<Transform> targetObjects;

	public override void ExecuteAction( AlienManager alienManager )
	{
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
		alienManager.pointTarget = targetObjects.Items[ closestObjectIndex ];
		
		//Initiate point.
		int closestHand = alienManager.gc.FindClosestHand( alienManager.pointTarget );
		alienManager.gc.pointHandIndex = closestHand;
		alienManager.gc.pointing = true;
		Vector3 pointVector = alienManager.pointTarget.position - alienManager.transform.position;
		if ( pointVector.magnitude > alienManager.gc.maxPointDistance )
			pointVector = pointVector.normalized * alienManager.gc.maxPointDistance;
		alienManager.gc.handTarget = alienManager.transform.position + pointVector;
	}

}
