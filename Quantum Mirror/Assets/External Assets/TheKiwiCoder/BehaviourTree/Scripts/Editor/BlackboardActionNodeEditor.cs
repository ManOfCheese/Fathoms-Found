using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TheKiwiCoder;

public class BlackboardActionNodeEditor : Editor
{

    private List<string> _hiddenProperties = new List<string>();
    public List<string> hiddenProperties
	{
        get { return _hiddenProperties; }
        set { _hiddenProperties = value; }
	}

	public override void OnInspectorGUI()
	{

		_hiddenProperties.Add( "m_Script" );
		_hiddenProperties.Add( "targetBlackboard" );
		_hiddenProperties.Add( "valueType" );
		_hiddenProperties.Add( "key" );

		serializedObject.Update();

		BTBlackBoardActionNode bban = ( BTBlackBoardActionNode )target;
		BlackboardManager bbManager = bban.bbManager;

		SerializedProperty p = serializedObject.GetIterator();
		while ( p.NextVisible( true ) )
		{
			if ( hiddenProperties.Contains( p.propertyPath ) ) continue;
			SerializedProperty property = serializedObject.FindProperty( p.name );
			if ( property != null ) EditorGUILayout.PropertyField( property, true );
		}
		GUILayout.Space( 30 );

		if ( bbManager != null )
			TargetBlackboardDropdown( bban, bbManager );
		else
		{
			GUIStyle red = new GUIStyle( EditorStyles.label );
			red.normal.textColor = Color.red;
			red.fontSize = 16;
			red.wordWrap = true;
			GUILayout.Label( "No Blackboard Manager! Add one before you can use this.", red );
		}

		serializedObject.ApplyModifiedProperties();

		if ( serializedObject.hasModifiedProperties )
			AssetDatabase.SaveAssets();
	}

	protected static void TargetBlackboardDropdown( BTBlackBoardActionNode bban, BlackboardManager bbManager )
	{
		string[] bbtypes = new string[ bbManager.blackboards.Count ];
		int[] indices = new int[ bbManager.blackboards.Count ];
		int i = 0;

		foreach ( Blackboard bb in bbManager.blackboards )
		{
			bbtypes[ i ] = bb.GetType().Name;
			indices[ i ] = bbManager.blackboards.IndexOf( bbManager.GetBlackboardByType( bb.GetType() ) );
			i++;
		}

		int currentTargetBlackboard = bbManager.blackboards.IndexOf( bban.targetBlackboard );
		if ( bbtypes.Length > 0 ) 
			bban.targetBlackboard = bbManager.blackboards[ EditorGUILayout.IntPopup( "Target Blackboard: ", 
				currentTargetBlackboard == -1 ? 0 : currentTargetBlackboard, bbtypes, indices ) ];
		else
		{
			GUIStyle red = new GUIStyle( EditorStyles.label );
			red.normal.textColor = Color.red;
			red.fontSize = 16;
			red.wordWrap = true;
			GUILayout.Label( "No blackboards! Add one before you can use this.", red );
		}
		bban.valueType = ( Blackboard.BlackboardValueType )EditorGUILayout.EnumPopup( "Value Type: ", bban.valueType );
		bban.key = EditorGUILayout.TextField( "Key: ", bban.key );
	}
}
