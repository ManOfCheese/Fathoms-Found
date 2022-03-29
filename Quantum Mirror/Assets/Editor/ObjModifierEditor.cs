using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( ObjModifier ) ), CanEditMultipleObjects]
public class ObjModifierEditor : Editor
{
    public SerializedProperty
        property_Prop,
        hands_Prop,
        thresholdLogic_Prop,
        threshold_Prop,
        changeMode_Prop,
        changeSpeed_Prop,
        changeDuration_Prop,
        crossFadeDuration_prop,
        fadeDuration_prop,
        fader_Prop,
        passiveSource_Prop,
        changeSource_Prop,
        thresholdSource_Prop,
        canUseSpeed_Prop;

    protected virtual void OnEnable()
    {
        // Setup the SerializedProperties
        property_Prop = serializedObject.FindProperty( "property" );
        thresholdLogic_Prop = serializedObject.FindProperty( "thresholdLogic" );
        threshold_Prop = serializedObject.FindProperty( "threshold" );
        changeMode_Prop = serializedObject.FindProperty( "changeMode" );
        changeSpeed_Prop = serializedObject.FindProperty( "changeSpeed" );
        changeDuration_Prop = serializedObject.FindProperty( "changeDuration" );
        crossFadeDuration_prop = serializedObject.FindProperty( "crossFadeDuration" );
        fadeDuration_prop = serializedObject.FindProperty( "fadeDuration" );
        fader_Prop = serializedObject.FindProperty( "fader" );
        passiveSource_Prop = serializedObject.FindProperty( "passiveSource" );
        changeSource_Prop = serializedObject.FindProperty( "changeSource" );
        thresholdSource_Prop = serializedObject.FindProperty( "thresholdSource" );
        canUseSpeed_Prop = serializedObject.FindProperty( "canUseSpeed" );
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField( property_Prop, new GUIContent( "property" ) );
        EditorGUILayout.PropertyField( thresholdLogic_Prop, new GUIContent( "thresholdLogic" ) );
        EditorGUILayout.PropertyField( threshold_Prop, new GUIContent( "threshold" ) );

        bool canUseSpeed = canUseSpeed_Prop.boolValue;
        if ( canUseSpeed )
		{
            EditorGUILayout.PropertyField( changeMode_Prop );
            ChangeMode mm = ( ChangeMode )changeMode_Prop.enumValueIndex;
            switch ( mm )
            {
                case ChangeMode.Duration:
                    EditorGUILayout.PropertyField( changeDuration_Prop, new GUIContent( "changeDuration" ) );
                    break;

                case ChangeMode.Speed:
                    EditorGUILayout.PropertyField( changeSpeed_Prop, new GUIContent( "changeSpeed" ) );
                    break;
            }
        }
		else
		{
            EditorGUILayout.PropertyField( changeDuration_Prop, new GUIContent( "changeDuration" ) );
        }

        EditorGUILayout.PropertyField( crossFadeDuration_prop, new GUIContent( "crossFadeDuration" ) );
        EditorGUILayout.PropertyField( crossFadeDuration_prop, new GUIContent( "fadeDuration" ) );
        EditorGUILayout.PropertyField( passiveSource_Prop, new GUIContent( "passiveSource" ) );
        EditorGUILayout.PropertyField( changeSource_Prop, new GUIContent( "changeSource" ) );
        EditorGUILayout.PropertyField( thresholdSource_Prop, new GUIContent( "thresholdSource" ) );
    }
}

