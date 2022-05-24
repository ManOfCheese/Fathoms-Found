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
		_hiddenProperties.Add( "valueType" );
		_hiddenProperties.Add( "key" );

		serializedObject.Update();

		BTBBActionNode bban = ( BTBBActionNode )target;
		Blackboard blackboard = bban.blackboard;

		SerializedProperty p = serializedObject.GetIterator();
		while ( p.NextVisible( true ) )
		{
			if ( hiddenProperties.Contains( p.propertyPath ) ) continue;
			SerializedProperty property = serializedObject.FindProperty( p.name );
			if ( property != null ) EditorGUILayout.PropertyField( property, true );
		}
		GUILayout.Space( 30 );

		serializedObject.ApplyModifiedProperties();

		if ( serializedObject.hasModifiedProperties )
			AssetDatabase.SaveAssets();
	}
}
