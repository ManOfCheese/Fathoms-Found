using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
     
    public RunTimeSet<Transform> set;
	public Transform actualTarget;

	private void Start()
	{
		if ( actualTarget == null )
			set.Add( transform );
		else
			set.Add( actualTarget );
	}

	private void OnApplicationQuit()
	{
		set.Items.Clear();
	}

}
