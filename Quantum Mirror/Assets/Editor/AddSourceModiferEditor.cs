using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( AddSourceModifier ) ), CanEditMultipleObjects]
public class AddSourceModifierEditor : ObjModifierEditor
{
    public SerializedProperty
        sourcePrefab_Prop,
        parentUnderObject_Prop,
        removeSourceWhenUnderThreshold_Prop,
        sourceOf_Prop,
        radius_Prop,
        valueAtCentre_Prop,
        fallOff_Prop;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Setup the SerializedProperties
        sourcePrefab_Prop = serializedObject.FindProperty( "sourcePrefab" );
        parentUnderObject_Prop = serializedObject.FindProperty( "parentUnderObject" );
        removeSourceWhenUnderThreshold_Prop = serializedObject.FindProperty( "removeSourceWhenUnderThreshold" );
        sourceOf_Prop = serializedObject.FindProperty( "sourceOf" );
        radius_Prop = serializedObject.FindProperty( "radius" );
        valueAtCentre_Prop = serializedObject.FindProperty( "valueAtCentre" );
        fallOff_Prop = serializedObject.FindProperty( "fallOff" );
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space( 10 );
        EditorGUILayout.LabelField( "Add Source Settings", EditorStyles.boldLabel );
        EditorGUILayout.PropertyField( sourcePrefab_Prop, new GUIContent( "Source Prefab" ) );
        EditorGUILayout.PropertyField( parentUnderObject_Prop, new GUIContent( "Parent Under Object" ) );
        EditorGUILayout.PropertyField( removeSourceWhenUnderThreshold_Prop, new GUIContent( "Remove Source When Under Threshold" ) );
        EditorGUILayout.PropertyField( sourceOf_Prop, new GUIContent( "Source Of" ) );
        EditorGUILayout.PropertyField( radius_Prop, new GUIContent( "Radius" ) );
        EditorGUILayout.PropertyField( valueAtCentre_Prop, new GUIContent( "Value At Centre" ) );
        EditorGUILayout.PropertyField( fallOff_Prop, new GUIContent( "Fall Off" ) );

        serializedObject.ApplyModifiedProperties();
    }
}
