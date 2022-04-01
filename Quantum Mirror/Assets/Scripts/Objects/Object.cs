using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{

	public PropertyInfo[] properties;

    [HideInInspector] public List<ObjModifier> modifiers = new List<ObjModifier>();
	[HideInInspector] public Detector[] detectors;
	[HideInInspector] public List<Source> sources;

	private void Start()
	{
		detectors = GetComponentsInParent<Detector>();
		Source[] sourcesArray = GetComponentsInChildren<Source>();
		for ( int i = 0; i < sourcesArray.Length; i++ )
			sources.Add( sourcesArray[ i ] );

		ObjModifier[] mods = GetComponentsInChildren<ObjModifier>();
		for ( int i = 0; i < mods.Length; i++ )
		{
			modifiers.Add( mods[ i ] );
			modifiers[ i ].OnStart( this );
		}
	}

	private void Update()
	{
		for ( int i = 0; i < modifiers.Count; i++ )
			modifiers[ i ].UpdateProperty();

		for ( int i = 0; i < properties.Length; i++ )
		{
			bool detector = false;
			
			for ( int j = 0; j < detectors.Length; j++ )
			{
				if ( detectors[ j ].propertyToDetect.propertyName == properties[ i ].property.propertyName )
				{
					detector = true;
					properties[ i ].value = detectors[ j ].propertyValue;
				}
			}
			for ( int j = 0; j < sources.Count; j++ )
			{
				if ( sources[ j ].sourceOf.propertyName == properties[ i ].property.propertyName )
				{
					if ( detector )
						properties[ i ].value += sources[ j ].valueAtCentre;
					else
						properties[ i ].value = sources[ j ].valueAtCentre;
				}

			}


		}
	}

}

[System.Serializable]
public class PropertyInfo
{
	public Property property;
	public float value;
}
