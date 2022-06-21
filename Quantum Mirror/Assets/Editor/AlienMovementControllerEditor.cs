using UnityEngine;
using UnityEditor;

[CustomEditor(typeof( AlienMovementController ) ), CanEditMultipleObjects]
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
         wanderCentre_Prop,
         wanderRadius_Prop,
         torusInnerRadius_Prop;

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
        wanderCentre_Prop = serializedObject.FindProperty( "wanderCentre" );
        wanderRadius_Prop = serializedObject.FindProperty( "wanderRadius" );
        torusInnerRadius_Prop = serializedObject.FindProperty( "torusInnerRadius" );
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
                EditorGUILayout.PropertyField( tolerance_Prop, new GUIContent( "tolerance" ) );
                DisplayWanderShape();
                break;

            case MovementMode.Timed:
                EditorGUILayout.PropertyField( speed_Prop, new GUIContent( "speed" ) );
                EditorGUILayout.PropertyField( changeDestinationTime_Prop, new GUIContent( "changeDestinationTime" ) );
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
                EditorGUILayout.PropertyField( wanderCentre_Prop, new GUIContent( "wanderCentre" ) );
                EditorGUILayout.PropertyField( wanderRadius_Prop, new GUIContent( "wanderRadius" ) );
                EditorGUILayout.PropertyField( torusInnerRadius_Prop, new GUIContent( "torusInnerRadius" ) );
                break;

            case MovementShape.Circle:
                EditorGUILayout.PropertyField( wanderCentre_Prop, new GUIContent( "wanderCentre" ) );
                EditorGUILayout.PropertyField( wanderRadius_Prop, new GUIContent( "wanderRadius" ) );
                break;

        }
    }
}

