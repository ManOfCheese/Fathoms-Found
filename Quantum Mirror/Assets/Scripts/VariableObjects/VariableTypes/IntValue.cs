using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "IntValue", menuName = "Variables/IntValue" )]
public class IntValue : VariableObject {

	[Header( "Value" )]
	[SerializeField] private int _value;
	public int Value {
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
	public int defaultValue;
	public int minValue;
    public int maxValue;

    public delegate void OnValueChanged( int value );
    public OnValueChanged onValueChanged;

    public override void ResetToDefault() {
        _value = defaultValue;
        onValueChanged?.Invoke( _value );
    }

    protected override void OnEnable() {
        base.OnEnable();
    }

}
