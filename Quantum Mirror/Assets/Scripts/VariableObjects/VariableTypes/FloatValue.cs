using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "FloatValue", menuName = "Variables/FloatValue" )]
public class FloatValue : VariableObject {

	[Header( "Value" )]
	[SerializeField] private float _value;
	public float Value {
        get {
            return _value;
        }
        set {
            if ( _value != value ) {
                if ( useLimits ) {
                    _value = Mathf.Clamp( value, minValue, maxValue );
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
	public float defaultValue;
	public float minValue;
    public float maxValue;

    public delegate void OnValueChanged( float value );
    public OnValueChanged onValueChanged;

    public override void ResetToDefault() {
        _value = defaultValue;
        onValueChanged?.Invoke( _value );
    }

    protected override void OnEnable() {
        base.OnEnable();
    }
}
