using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "BoolValue", menuName = "Variables/BoolValue" )]
public class BoolValue : VariableObject {

	[Header( "Value" )]
	[SerializeField] private bool _value;
	public bool Value {
		get {
			return _value;
		}
		set {
			if ( _value != value ) {
				_value = value;
				changedThisFrame = true;
				onValueChanged?.Invoke( _value );
			}
		}
	}

	[Space( 10 )]
	public bool defaultValue;
	public int minValue;
	public int maxValue;

	public delegate void OnValueChanged( bool value );
	public OnValueChanged onValueChanged;

	public override void ResetToDefault() {
		_value = defaultValue;
		onValueChanged?.Invoke( _value );
	}

	protected override void OnEnable() {
		base.OnEnable();
	}

}
