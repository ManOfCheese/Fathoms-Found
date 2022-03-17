using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
     
    public RunTimeSet<Transform> set;

	private void Start()
	{
		set.Add( transform );
	}

}
