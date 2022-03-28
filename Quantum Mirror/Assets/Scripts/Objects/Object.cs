using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{

	public GameObject detectorsObject;
    public List<ObjModifier> modifiers = new List<ObjModifier>();

	[HideInInspector] public Detector[] detectors;

	private void Start()
	{
		detectors = detectorsObject.GetComponents<Detector>();

		for ( int i = 0; i < modifiers.Count; i++ )
		{
			modifiers[ i ].OnStart( this );
		}
	}

	private void Update()
	{
		for ( int i = 0; i < modifiers.Count; i++ )
		{
			modifiers[ i ].UpdateProperty();
		}
	}

}
