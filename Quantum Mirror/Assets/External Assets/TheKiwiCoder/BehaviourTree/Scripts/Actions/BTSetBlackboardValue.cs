using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTSetBlackboardValue : BTBlackBoardActionNode
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
        if ( targetBlackboard != null )
		{
			switch ( valueType )
			{
				case Blackboard.BlackboardValueType.GameObject:
					if ( !targetBlackboard.AddData( key, targetBlackboard.gameObjects, gameObjectValue ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.Bool:
					if ( !targetBlackboard.AddData( key, targetBlackboard.bools, boolValue ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.String:
					if ( !targetBlackboard.AddData( key, targetBlackboard.strings, stringValue ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.Int:
					if ( !targetBlackboard.AddData( key, targetBlackboard.ints, intValue ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.Float:
					if ( !targetBlackboard.AddData( key, targetBlackboard.floats, floatValue ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.Vector3:
					if ( !targetBlackboard.AddData( key, targetBlackboard.vector3s, vector3Value ) ) return State.Failure;
					break;
				case Blackboard.BlackboardValueType.Vector2:
					if ( !targetBlackboard.AddData( key, targetBlackboard.vector2s, vector2Value ) ) return State.Failure;
					break;
				default:
					break;
			}
			return State.Success;
		}
		return State.Failure;
    }
}
