using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddComponentModifier : Modifier
{

	public GameObject component;
	public GameObject logicObject;

	public override void OnThresholdCross()
	{
		base.OnThresholdCross();
		Instantiate( component, logicObject.transform.position, logicObject.transform.rotation, logicObject.transform );
	}

}
