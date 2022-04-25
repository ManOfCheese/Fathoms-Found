using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{

    public BTRoot rootNode;
    public Blackboard blackboard;

	private void Awake()
	{
		blackboard = new Blackboard();
	}

}
