using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( ReplaceModifier ) ), CanEditMultipleObjects]
public class ReplaceModifierEditor : ObjModifierEditor
{
    public SerializedProperty
        topObject_Prop,
        replaceWith_Prop;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Setup the SerializedProperties
        topObject_Prop = serializedObject.FindProperty( "topObject" );
        replaceWith_Prop = serializedObject.FindProperty( "replaceWith" );
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space( 10 );
        EditorGUILayout.LabelField( "Replace Settings", EditorStyles.boldLabel );
        EditorGUILayout.PropertyField( topObject_Prop, new GUIContent( "Top Object" ) );
        EditorGUILayout.PropertyField( replaceWith_Prop, new GUIContent( "Replace With" ) );

        serializedObject.ApplyModifiedProperties();
    }
}
