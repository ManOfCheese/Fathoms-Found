using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( AnimationModifier ) ), CanEditMultipleObjects]
public class AnimationModifierEditor : ObjModifierEditor
{
    public SerializedProperty
    animator_Prop,
    parameterType_Prop,
    parameterName_Prop,
    boolParameter_Prop,
    changeOverTime_Prop,
    linkToDetector_Prop,
    floatParameter_Prop,
    intParameter_Prop,
    minValue_Prop,
    maxValue_Prop,
    minValueInt_Prop,
    maxValueInt_Prop;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Setup the SerializedProperties
        animator_Prop = serializedObject.FindProperty( "animator" );
        parameterType_Prop = serializedObject.FindProperty( "parameterType" );
        parameterName_Prop = serializedObject.FindProperty( "parameterName" );
        boolParameter_Prop = serializedObject.FindProperty( "boolParameter" );
        changeOverTime_Prop = serializedObject.FindProperty( "changeOverTime" );
        linkToDetector_Prop = serializedObject.FindProperty( "linkToDetector" );
        floatParameter_Prop = serializedObject.FindProperty( "floatParameter" );
        intParameter_Prop = serializedObject.FindProperty( "intParameter" );
        minValue_Prop = serializedObject.FindProperty( "minValue" );
        maxValue_Prop = serializedObject.FindProperty( "maxValue" );
        minValueInt_Prop = serializedObject.FindProperty( "minValueInt" );
        maxValueInt_Prop = serializedObject.FindProperty( "maxValueInt" );
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField( animator_Prop, new GUIContent( "Animator" ) );
        EditorGUILayout.PropertyField( parameterType_Prop, new GUIContent( "Parameter Type" ) );
        EditorGUILayout.PropertyField( parameterName_Prop, new GUIContent( "Parameter Name" ) );
        AnimParameterType atp = ( AnimParameterType )parameterType_Prop.enumValueIndex;
        bool changeOverTime = changeOverTime_Prop.boolValue;
        bool linkToDetector = linkToDetector_Prop.boolValue;
        switch ( atp )
        {
            case AnimParameterType.Trigger:
                break;
            case AnimParameterType.Bool:
                EditorGUILayout.PropertyField( boolParameter_Prop, new GUIContent( "Bool Parameter" ) );
                break;
            case AnimParameterType.Float:
                EditorGUILayout.Space( 10 );
                EditorGUILayout.PropertyField( linkToDetector_Prop, new GUIContent( "Link To Detector" ) );
                if ( !linkToDetector )
				{
                    EditorGUILayout.PropertyField( changeOverTime_Prop, new GUIContent( "Change Over Time" ) );

                    if ( !changeOverTime )
                        EditorGUILayout.PropertyField( floatParameter_Prop, new GUIContent( "Float Parameter" ) );
                    else
                    {
                        EditorGUILayout.PropertyField( changeMode_Prop );
                        ChangeMode mm = ( ChangeMode )changeMode_Prop.enumValueIndex;
                        switch ( mm )
                        {
                            case ChangeMode.Duration:
                                EditorGUILayout.PropertyField( changeDuration_Prop, new GUIContent( "Change Duration" ) );
                                break;
                            case ChangeMode.Speed:
                                EditorGUILayout.PropertyField( changeSpeed_Prop, new GUIContent( "Change Speed" ) );
                                break;
                            default:
                                break;
                        }
                    }

                    EditorGUILayout.Space( 10 );
                    EditorGUILayout.PropertyField( minValue_Prop, new GUIContent( "Min Value" ) );
                    EditorGUILayout.PropertyField( maxValue_Prop, new GUIContent( "Max Value" ) );
                }
                break;
            case AnimParameterType.Int:
                EditorGUILayout.Space( 10 );
                EditorGUILayout.PropertyField( linkToDetector_Prop, new GUIContent( "Link To Detector" ) );
                if ( !linkToDetector )
				{
                    EditorGUILayout.PropertyField( changeOverTime_Prop, new GUIContent( "Change Over Time" ) );

                    if ( !changeOverTime )
                        EditorGUILayout.PropertyField( intParameter_Prop, new GUIContent( "Int Parameter" ) );
                    else
                    {
                        EditorGUILayout.PropertyField( changeMode_Prop );
                        ChangeMode mm = ( ChangeMode )changeMode_Prop.enumValueIndex;
                        switch ( mm )
                        {
                            case ChangeMode.Duration:
                                EditorGUILayout.PropertyField( changeDuration_Prop, new GUIContent( "Change Duration" ) );
                                break;
                            case ChangeMode.Speed:
                                EditorGUILayout.PropertyField( changeSpeed_Prop, new GUIContent( "Change Speed" ) );
                                break;
                            default:
                                break;
                        }
                    }

                    EditorGUILayout.Space( 10 );
                    EditorGUILayout.PropertyField( minValueInt_Prop, new GUIContent( "Min Value" ) );
                    EditorGUILayout.PropertyField( maxValueInt_Prop, new GUIContent( "Max Value" ) );
                }
                break;
            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
