using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof( AlienMovementController ) ), CanEditMultipleObjects]
public class AlienMovementControllerEditor : Editor
{
    public SerializedProperty
         agent_Prop,
         hands_Prop,
         movementMode_Prop,
         movementShape_Prop,
         speed_Prop,
         changeDestinationTime_Prop,
         tolerance_Prop,
         minDistance_Prop,
         maxDistance_Prop,
         torusCenter_Prop,
         torusInnerRadius_Prop,
         torusOuterRadius_Prop,
         circleCenter_Prop,
         circleRadius_Prop;

    void OnEnable() {
        // Setup the SerializedProperties
        hands_Prop = serializedObject.FindProperty( "hands" );
        movementMode_Prop = serializedObject.FindProperty( "movementMode" );
        movementShape_Prop = serializedObject.FindProperty( "movementShape" );
        speed_Prop = serializedObject.FindProperty( "speed" );
        changeDestinationTime_Prop = serializedObject.FindProperty( "changeDestinationTime" );
        tolerance_Prop = serializedObject.FindProperty( "tolerance" );
        minDistance_Prop = serializedObject.FindProperty( "minDistance" );
        maxDistance_Prop = serializedObject.FindProperty( "maxDistance" );
        torusCenter_Prop = serializedObject.FindProperty( "torusCenter" );
        torusInnerRadius_Prop = serializedObject.FindProperty( "torusInnerRadius" );
        torusOuterRadius_Prop = serializedObject.FindProperty( "torusOuterRadius" );
        circleCenter_Prop = serializedObject.FindProperty( "circleCenter" );
        circleRadius_Prop = serializedObject.FindProperty( "circleRadius" );
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField( hands_Prop, new GUIContent( "hands" ) );

        EditorGUILayout.PropertyField( movementMode_Prop );

        MovementMode mm = ( MovementMode )movementMode_Prop.enumValueIndex;

        switch ( mm ) {
            case MovementMode.Static:
                break;

            case MovementMode.PointToPoint:
                EditorGUILayout.PropertyField( speed_Prop, new GUIContent( "speed" ) );
                DisplayWanderShape();
                break;

            case MovementMode.Timed:
                EditorGUILayout.PropertyField( speed_Prop, new GUIContent( "speed" ) );
                EditorGUILayout.PropertyField( changeDestinationTime_Prop, new GUIContent( "changeDestinationTime" ) );
                EditorGUILayout.PropertyField( tolerance_Prop, new GUIContent( "tolerance" ) );
                DisplayWanderShape();
                break;

        }

        serializedObject.ApplyModifiedProperties();
    }

    public void DisplayWanderShape() {
        EditorGUILayout.PropertyField( movementShape_Prop );

        MovementShape ms = ( MovementShape )movementShape_Prop.enumValueIndex;

        switch ( ms ) {
            case MovementShape.None:
                EditorGUILayout.PropertyField( minDistance_Prop, new GUIContent( "minDistance" ) );
                EditorGUILayout.PropertyField( maxDistance_Prop, new GUIContent( "maxDistance" ) );
                break;

            case MovementShape.Torus:
                EditorGUILayout.PropertyField( torusCenter_Prop, new GUIContent( "torusCenter" ) );
                EditorGUILayout.PropertyField( torusInnerRadius_Prop, new GUIContent( "torusInnerRadius" ) );
                EditorGUILayout.PropertyField( torusOuterRadius_Prop, new GUIContent( "torusOuterRadius" ) );
                break;

            case MovementShape.Circle:
                EditorGUILayout.PropertyField( circleCenter_Prop, new GUIContent( "circleCenter" ) );
                EditorGUILayout.PropertyField( circleRadius_Prop, new GUIContent( "circleRadius" ) );
                break;

        }
    }
}

