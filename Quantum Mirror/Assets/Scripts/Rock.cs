using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : InteractableObject
{

    public float spawnDistanceFromRock;
    public GameObject gem;
    public string objectPrereqisuite;
    public Transform gemSpawnPlace;

    public override void Interact( Interactor player )
    {
        if ( player.objectInHand )
		{
            if ( player.objectInHand.objectName == objectPrereqisuite )
            {
                Vector3 rockPerimeter = ( player.transform.position - transform.position ).normalized * ( transform.localScale.y / 2f );

                Instantiate( gem, rockPerimeter.normalized * ( rockPerimeter.magnitude + spawnDistanceFromRock ), Quaternion.identity );
            }
        }
    }

}
