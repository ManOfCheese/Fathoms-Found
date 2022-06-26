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
					blackboard.AddData( key, gameObjectValue );
					break;
				case Blackboard.BlackboardValueType.Bool:
					blackboard.AddData( key, boolValue );
					break;
				case Blackboard.BlackboardValueType.String:
					blackboard.AddData( key, stringValue );
					break;
				case Blackboard.BlackboardValueType.Int:
					blackboard.AddData( key, intValue );
					break;
				case Blackboard.BlackboardValueType.Float:
					blackboard.AddData( key, floatValue );
					break;
				case Blackboard.BlackboardValueType.Vector3:
					blackboard.AddData( key, vector3Value );
					break;
				case Blackboard.BlackboardValueType.Vector2:
					blackboard.AddData( key, vector2Value );
					break;
				default:
					break;
			}
			return State.Success;
		}
		return State.Failure;
    }
}
