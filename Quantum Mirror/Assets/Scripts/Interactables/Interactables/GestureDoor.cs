using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDoor : InteractableObject
{

	public GestureListener gestureListener;
	public Door door;
	public GestureSequence passwordGesture;

	private string passwordGestureCode;

	private void Start()
	{
		passwordGestureCode = Gestures.GestureLogic.SequenceToString( passwordGesture );
	}

	public override void Interact( Interactor interactor )
	{
		base.Interact( interactor );
		if ( Gestures.GestureLogic.CodeToString( gestureListener.playerSentence ) == passwordGestureCode )
			door.Open();
	}

}
