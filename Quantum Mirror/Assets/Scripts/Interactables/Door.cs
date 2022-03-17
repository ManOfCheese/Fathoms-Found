using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{

	public RunTimeSet<Door> doors;
	public Animator animator;

	private void Awake()
	{
		doors.Add( this );
	}

	public void Open()
	{
		animator.SetTrigger( "Open" );
	}

}
