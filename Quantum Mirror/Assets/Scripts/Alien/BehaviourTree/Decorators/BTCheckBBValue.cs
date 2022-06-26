using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTCheckBBValue : BTDecoratorNode
{

    public Blackboard.BlackboardValueType valueType;
    public string key;
	public bool notNull;
	[Space( 10 )]
	public List<Vector3> positionListValue = new List<Vector3>();
	public GameObject gameObjectValue;
	public bool boolValue;
    public int intValue;
    public string stringValue;
    public float floatValue;
    public Vector3 vector3Value;
    public Vector2 vector2Value;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
		if ( blackboard != null )
		{
			if ( notNull )
			{
				switch ( valueType )
				{
					case Blackboard.BlackboardValueType.PositionList:
						if ( blackboard.GetData( key, new List<Vector3>() ) != null ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.GameObject:
						if ( blackboard.GetData( key, new GameObject() ) != null ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Bool:
						if ( blackboard.GetData( key, new bool() ) != false ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.String:
						if ( blackboard.GetData( key, "" ) != "" ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Int:
						if ( blackboard.GetData( key, new int() ) != 0 ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Float:
						if ( blackboard.GetData( key, new float() ) != 0f ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Vector3:
						if ( blackboard.GetData( key, new Vector3() ) != Vector3.zero ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Vector2:
						if ( blackboard.GetData( key, new Vector2() ) != Vector2.zero ) return child.Update();
						break;
					default:
						break;
				}
			}
			else
			{
				switch ( valueType )
				{
					case Blackboard.BlackboardValueType.PositionList:
						if ( blackboard.GetData( key, new List<Vector3>() ) == positionListValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.GameObject:
						if ( blackboard.GetData( key, new GameObject() ) == gameObjectValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Bool:
						if ( blackboard.GetData( key, new bool() ) == boolValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.String:
						if ( blackboard.GetData( key, "" ) == stringValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Int:
						if ( blackboard.GetData( key, new int() ) == intValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Float:
						if ( blackboard.GetData( key, new float() ) == floatValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Vector3:
						if ( blackboard.GetData( key, new Vector3() ) == vector3Value ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Vector2:
						if ( blackboard.GetData( key, new Vector2() ) == vector2Value ) return child.Update();
						break;
					default:
						break;
				}
			}
		}
		return State.Failure;
	}
}
