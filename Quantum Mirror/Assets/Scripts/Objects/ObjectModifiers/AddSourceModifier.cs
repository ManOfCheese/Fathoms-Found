using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSourceModifier : ObjModifier
{

	public GameObject logicObject;
    public Property becomeSourceOf;
    public float valueAtCentre;
    public AnimationCurve fallOff;

	public override void OnThresholdCross()
	{
		base.OnThresholdCross();
		Source source = logicObject.AddComponent<Source>();
		source.sourceOf = becomeSourceOf;
		source.valueAtCentre = valueAtCentre;
		source.fallOff = fallOff;
		Destroy( this.gameObject );
	}

}
