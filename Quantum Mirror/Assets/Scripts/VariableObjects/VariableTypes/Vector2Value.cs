using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Vector2Value", menuName = "Variables/Vector2Value" )]
public class Vector2Value : VariableObject {

	[Header( "Value" )]
	[SerializeField] private Vector2 _value;
	public Vector2 Value {
		get {
			return _value;
		}
		set {
			if ( _value != value ) {
				if ( useLimits ) {
					_value = new Vector2( Mathf.Clamp( value.x, minValue.x, maxValue.x ), Mathf.Clamp( value.y, minValue.y, maxValue.y ) );
				}
				else {
					_value = value;
				}
				_value = value;
				changedThisFrame = true;
				onValueChanged?.Invoke( _value );
			}
		}
	}

	[Space( 10 )]
	public Vector2 defaultValue;
	public Vector2 minValue;
	public Vector2 maxValue;

	public delegate void OnValueChanged( Vector2 value );
	public OnValueChanged onValueChanged;

	public override void ResetToDefault() {
		_value = defaultValue;
		onValueChanged?.Invoke( _value );
	}

	protected override void OnEnable() {
		base.OnEnable();
	}

}
