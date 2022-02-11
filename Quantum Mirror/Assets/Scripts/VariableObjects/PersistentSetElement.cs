using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSetElement : ScriptableObject {

	public int ID {
		get {
			return _ID;
		}
		set {
			if ( value != _ID ) {
				_ID = value;
			}
		}
	}
	private int _ID = -1;

	public virtual void RegisterToSet() {

	}

}
