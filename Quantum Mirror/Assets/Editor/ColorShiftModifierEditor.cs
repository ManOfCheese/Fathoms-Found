using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( ColorShiftModifier ) ), CanEditMultipleObjects]
public class ColorShiftModifierEditor : ObjModifierEditor
{
    public SerializedProperty
        objRenderer_Prop,
        lerpTo_Prop;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Setup the SerializedProperties
        objRenderer_Prop = serializedObject.FindProperty( "objRenderer" );
        lerpTo_Prop = serializedObject.FindProperty( "lerpTo" );
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space( 10 );
        EditorGUILayout.LabelField( "Color Shift Settings", EditorStyles.boldLabel );
        EditorGUILayout.PropertyField( changeDuration_Prop, new GUIContent( "Change Duration" ) );
        EditorGUILayout.PropertyField( objRenderer_Prop, new GUIContent( "Object Renderer" ) );
        EditorGUILayout.PropertyField( lerpTo_Prop, new GUIContent( "Lerp To Color" ) );

        serializedObject.ApplyModifiedProperties();
    }
}
