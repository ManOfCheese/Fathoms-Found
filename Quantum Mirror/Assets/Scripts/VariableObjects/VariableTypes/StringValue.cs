using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "StringValue", menuName = "Variables/StringValue" )]
public class StringValue : VariableObject
{
    [Header( "Value" )]
    [SerializeField] private string _value;
    public string Value
    {
        get
        {
            return _value;
        }
        set
        {
            if ( _value != value )
            {
                _value = value;
                changedThisFrame = true;
                onValueChanged?.Invoke( _value );
            }
        }
    }

    [Space( 10 )]
    public string defaultValue;

    public delegate void OnValueChanged( string value );
    public OnValueChanged onValueChanged;

    public override void ResetToDefault()
    {
        _value = defaultValue;
        onValueChanged?.Invoke( _value );
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
