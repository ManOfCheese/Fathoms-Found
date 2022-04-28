using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSourceModifier : Modifier
{

	public GameObject sourcePrefab;
	public GameObject parentUnderObject;
	public bool removeSourceWhenUnderThreshold;
    public Property sourceOf;
	public float radius;
    public float valueAtCentre;
    public AnimationCurve fallOff;

	private GameObject sourceObject;

	public override void OnThresholdCross()
	{
		base.OnThresholdCross();
		if ( sourceObject == null )
		{
			sourceObject = Instantiate( sourcePrefab, parentUnderObject.transform.position, parentUnderObject.transform.rotation, parentUnderObject.transform.parent );
			sourceObject.GetComponent<SphereCollider>().radius = radius;
			Source source = sourceObject.GetComponent<Source>();
			obj.sources.Add( source );
			source.sourceOf = sourceOf;
			source.valueAtCentre = valueAtCentre;
			source.fallOff = fallOff;
		}

		if ( !removeSourceWhenUnderThreshold )
			Destroy( this.gameObject );
	}

	public override void OnThresholdUncross()
	{
		base.OnThresholdUncross();

		if ( removeSourceWhenUnderThreshold )
		{
			obj.sources.Remove( sourceObject.GetComponent<Source>() );
			Destroy( sourceObject );
		}
	}

}
