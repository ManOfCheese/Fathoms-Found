using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( AddSourceModifier ) ), CanEditMultipleObjects]
public class AddSourceModifierEditor : ObjModifierEditor
{
    public SerializedProperty
        logicObject_Prop,
        becomeSourceOf_Prop,
        valueAtCentre_Prop,
        fallOff_Prop;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Setup the SerializedProperties
        logicObject_Prop = serializedObject.FindProperty( "logicObject" );
        becomeSourceOf_Prop = serializedObject.FindProperty( "becomeSourceOf" );
        valueAtCentre_Prop = serializedObject.FindProperty( "valueAtCentre" );
        fallOff_Prop = serializedObject.FindProperty( "fallOff" );
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space( 10 );
        EditorGUILayout.LabelField( "Add Source Settings", EditorStyles.boldLabel );
        EditorGUILayout.PropertyField( logicObject_Prop, new GUIContent( "Logic Object" ) );
        EditorGUILayout.PropertyField( becomeSourceOf_Prop, new GUIContent( "Become Source Of" ) );
        EditorGUILayout.PropertyField( valueAtCentre_Prop, new GUIContent( "Value At Centre" ) );
        EditorGUILayout.PropertyField( fallOff_Prop, new GUIContent( "Fall Off" ) );

        serializedObject.ApplyModifiedProperties();
    }
}
