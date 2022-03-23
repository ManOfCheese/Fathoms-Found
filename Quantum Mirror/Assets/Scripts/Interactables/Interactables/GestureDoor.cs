using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDoor : InteractableObject
{

	public Door door;
	public GestureSequence gesturePassword;

	public override void Interact( Interactor interactor )
	{
		base.Interact( interactor );
	}


}
