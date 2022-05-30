using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTSetBBValue : BTBBActionNode
{

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
			switch ( valueType )
			{
				case Blackboard.BlackboardValueType.GameObject:
					if ( !blackboard.AddData( key, blackboard.gameObjects, gameObjectValue ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.Bool:
					if ( !blackboard.AddData( key, blackboard.bools, boolValue ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.String:
					if ( !blackboard.AddData( key, blackboard.strings, stringValue ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.Int:
					if ( !blackboard.AddData( key, blackboard.ints, intValue ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.Float:
					if ( !blackboard.AddData( key, blackboard.floats, floatValue ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.Vector3:
					if ( !blackboard.AddData( key, blackboard.vector3s, vector3Value ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.Vector2:
					if ( !blackboard.AddData( key, blackboard.vector2s, vector2Value ) ) return State.Failure;
					break;
				default:
					break;
			}
			return State.Success;
		}
		return State.Failure;
    }
}
