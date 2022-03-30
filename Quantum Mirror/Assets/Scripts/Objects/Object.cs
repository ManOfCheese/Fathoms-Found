using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{

    [HideInInspector] public List<ObjModifier> modifiers = new List<ObjModifier>();
	[HideInInspector] public Detector[] detectors;

	private void Start()
	{
		detectors = GetComponentsInParent<Detector>();
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
	}

}
