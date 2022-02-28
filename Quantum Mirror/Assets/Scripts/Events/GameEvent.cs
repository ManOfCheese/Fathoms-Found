using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "GameEvent", menuName = "GameEvent/VoidEvent" )]
public class GameEvent : PersistentSetElement
{

	public EventListener[] listeners;

	public virtual void CallEvent()
	{

	}

}

public abstract class Info
{

}
