using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( AddComponentModifier ) ), CanEditMultipleObjects]
public class AddComponentModifierEditor : ObjModifierEditor
{
    public SerializedProperty
        component_Prop,
        logicObject_Prop;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Setup the SerializedProperties
        component_Prop = serializedObject.FindProperty( "component" );
        logicObject_Prop = serializedObject.FindProperty( "logicObject" );
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space( 10 );
        EditorGUILayout.LabelField( "Add Component Settings", EditorStyles.boldLabel );
        EditorGUILayout.PropertyField( component_Prop, new GUIContent( "Component" ) );
        EditorGUILayout.PropertyField( logicObject_Prop, new GUIContent( "Logic Object" ) );

        serializedObject.ApplyModifiedProperties();
    }
}
