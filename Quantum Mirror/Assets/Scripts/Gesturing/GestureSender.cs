using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureSender : MonoBehaviour
{
    
    public RunTimeSet<GestureSender> gestureSenders;
	[ReadOnly] public int ID;

	protected virtual void Awake()
	{
		gestureSenders.Add( this );
		//Use count because ID 0 is always the player.
		ID = gestureSenders.Items.Count;
	}

}
