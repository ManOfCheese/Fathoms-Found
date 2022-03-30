using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceModifier : ObjModifier
{

	public GameObject topObject; 
	public GameObject replaceWith;

	public override void OnThresholdCross()
	{
		base.OnThresholdCross();
		GameObject newObject = Instantiate( replaceWith, topObject.transform.position, topObject.transform.rotation, topObject.transform.parent );
		newObject.transform.localScale = topObject.transform.localScale;
		Destroy( topObject );
	}

}
