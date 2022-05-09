using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Action", menuName = "Action/ResponseAction" )]
public class Respond_Action : Action {

	public Sprite response;

    public override void ExecuteAction( AlienManager alienManager ) {
		//npc.image.sprite = response;
	}

}
