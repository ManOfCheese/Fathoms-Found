using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( ScaleModifier ) ), CanEditMultipleObjects]
public class ScaleModifierEditor : ObjModifierEditor
{
    public SerializedProperty
        objTransform_Prop,
        linkDetector_Prop,
        linkedScaleModifier_Prop,
        minScale_Prop,
        maxScale_Prop;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Setup the SerializedProperties
        objTransform_Prop = serializedObject.FindProperty( "objTransform" );
        linkDetector_Prop = serializedObject.FindProperty( "linkToDetector" );
        linkedScaleModifier_Prop = serializedObject.FindProperty( "linkedScaleModifier" );
        minScale_Prop = serializedObject.FindProperty( "minScale" );
        maxScale_Prop = serializedObject.FindProperty( "maxScale" );
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField( linkDetector_Prop, new GUIContent( "Link To Detector" ) );
        bool linkToDetector = linkDetector_Prop.boolValue;
        if ( !linkToDetector )
		{
            EditorGUILayout.PropertyField( changeMode_Prop, new GUIContent( "Change Mode" ) );
            ChangeMode cm = ( ChangeMode )changeMode_Prop.enumValueIndex;
            switch ( cm )
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
		else
		{
            EditorGUILayout.PropertyField( linkedScaleModifier_Prop, new GUIContent( "Linked Scale Modifier" ) );
        }

        EditorGUILayout.Space( 10 );
        EditorGUILayout.LabelField( "Scale Settings", EditorStyles.boldLabel );
        EditorGUILayout.PropertyField( objTransform_Prop, new GUIContent( "Object Transform" ) );
        EditorGUILayout.PropertyField( minScale_Prop, new GUIContent( "Min Scale" ) );
        EditorGUILayout.PropertyField( maxScale_Prop, new GUIContent( "Max Scale" ) );

        serializedObject.ApplyModifiedProperties();
    }
}
