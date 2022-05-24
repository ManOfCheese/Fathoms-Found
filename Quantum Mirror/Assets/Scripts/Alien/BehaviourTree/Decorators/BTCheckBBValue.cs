using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTCheckBBValue : BTDecoratorNode
{

    public Blackboard.BlackboardValueType valueType;
	public bool isNot;
    public string key;
	[Space( 10 )]
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
			if ( isNot )
			{
				switch ( valueType )
				{
					case Blackboard.BlackboardValueType.GameObject:
						if ( blackboard.gameObjects[ key ] != gameObjectValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Bool:
						if ( blackboard.bools[ key ] != boolValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.String:
						if ( blackboard.strings[ key ] != stringValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Int:
						if ( blackboard.ints[ key ] != intValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Float:
						if ( blackboard.floats[ key ] != floatValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Vector3:
						if ( blackboard.vector3s[ key ] != vector3Value ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Vector2:
						if ( blackboard.vector2s[ key ] != vector2Value ) return child.Update();
						break;
					default:
						break;
				}
			}
			else
			{
				switch ( valueType )
				{
					case Blackboard.BlackboardValueType.GameObject:
						if ( blackboard.gameObjects[ key ] == gameObjectValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Bool:
						if ( blackboard.bools[ key ] == boolValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.String:
						if ( blackboard.strings[ key ] == stringValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Int:
						if ( blackboard.ints[ key ] == intValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Float:
						if ( blackboard.floats[ key ] == floatValue ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Vector3:
						if ( blackboard.vector3s[ key ] == vector3Value ) return child.Update();
						break;
					case Blackboard.BlackboardValueType.Vector2:
						if ( blackboard.vector2s[ key ] == vector2Value ) return child.Update();
						break;
					default:
						break;
				}
			}
		}
		return State.Failure;
	}
}
