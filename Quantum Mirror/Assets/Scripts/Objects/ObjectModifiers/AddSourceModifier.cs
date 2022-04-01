using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSourceModifier : ObjModifier
{

	public GameObject sourcePrefab;
	public GameObject parentUnderObject;
    public Property becomeSourceOf;
	public float radius;
    public float valueAtCentre;
    public AnimationCurve fallOff;

	public override void OnThresholdCross()
	{
		base.OnThresholdCross();
		GameObject newSource = Instantiate( sourcePrefab, parentUnderObject.transform.position, parentUnderObject.transform.rotation, parentUnderObject.transform.parent );
		newSource.GetComponent<SphereCollider>().radius = radius;
		Source source = newSource.GetComponent<Source>();
		source.sourceOf = becomeSourceOf;
		source.valueAtCentre = valueAtCentre;
		source.fallOff = fallOff;
		Destroy( this.gameObject );
	}

}
