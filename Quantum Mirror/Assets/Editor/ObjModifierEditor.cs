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

        EditorGUILayout.LabelField( "Base Settings", EditorStyles.boldLabel );
        EditorGUILayout.PropertyField( property_Prop, new GUIContent( "Property" ) );
        EditorGUILayout.PropertyField( thresholdLogic_Prop, new GUIContent( "Threshold Logic" ) );
        EditorGUILayout.PropertyField( threshold_Prop, new GUIContent( "Threshold" ) );

        EditorGUILayout.Space( 10 );
        EditorGUILayout.LabelField( "Audio Settings", EditorStyles.boldLabel );
        EditorGUILayout.PropertyField( crossFadeDuration_prop, new GUIContent( "Cross Fade Duration" ) );
        EditorGUILayout.PropertyField( crossFadeDuration_prop, new GUIContent( "Fade Duration" ) );
        EditorGUILayout.PropertyField( passiveSource_Prop, new GUIContent( "Passive Source" ) );
        EditorGUILayout.PropertyField( changeSource_Prop, new GUIContent( "Change Source" ) );
        EditorGUILayout.PropertyField( thresholdSource_Prop, new GUIContent( "Threshold Source" ) );
    }
}

