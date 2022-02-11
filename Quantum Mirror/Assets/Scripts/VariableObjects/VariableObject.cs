using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VariableObject : PersistentSetElement {

	[Header( "Settings" )]
	public string displayName;
	public bool applyDefaultOnStartup;
	public bool useLimits;
	[HideInInspector] public bool changedThisFrame;

	protected virtual void Awake() {
		ResetToDefault();
	}

	protected virtual void OnEnable() {
		if ( applyDefaultOnStartup ) {
			ResetToDefault();
		}
	}

	public abstract void ResetToDefault();

}
