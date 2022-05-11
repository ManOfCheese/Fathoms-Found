using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTDebugBlackboardValue : BTBlackBoardActionNode
{

	protected override void OnStart()
	{
	}

	protected override void OnStop()
	{
	}

	protected override State OnUpdate()
	{
		if ( targetBlackboard != null )
		{
			switch ( valueType )
			{
				case Blackboard.BlackboardValueType.GameObject:
					if ( !targetBlackboard.gameObjects.ContainsKey( key ) ) return State.Failure;
					GameObject go = new GameObject();
					go = targetBlackboard.GetData( key, targetBlackboard.gameObjects, go );
					Debug.LogError( $"{ key } is currently {go.name}" );
					break;
				case Blackboard.BlackboardValueType.Bool:
					if ( !targetBlackboard.bools.ContainsKey( key ) ) return State.Failure;
					bool b = targetBlackboard.GetData( key, targetBlackboard.bools, true );
					Debug.LogError( $"{ key } is currently {b}" );
					break;
				case Blackboard.BlackboardValueType.String:
					if ( !targetBlackboard.strings.ContainsKey( key ) ) return State.Failure;
					string s = targetBlackboard.GetData( key, targetBlackboard.strings, "" );
					Debug.LogError( $"{ key } is currently {s}" );
					break;
				case Blackboard.BlackboardValueType.Int:
					if ( !targetBlackboard.ints.ContainsKey( key ) ) return State.Failure;
					int bbint = targetBlackboard.GetData( key, targetBlackboard.ints, new int() );
					Debug.LogError( $"{ key } is currently {bbint}" );
					break;
				case Blackboard.BlackboardValueType.Float:
					if ( !targetBlackboard.floats.ContainsKey( key ) ) return State.Failure;
					float f = targetBlackboard.GetData( key, targetBlackboard.floats, new float() );
					Debug.LogError( $"{ key } is currently {f}" );
					break;
				case Blackboard.BlackboardValueType.Vector3:
					if ( !targetBlackboard.vector3s.ContainsKey( key ) ) return State.Failure;
					Vector3 vector3 = targetBlackboard.GetData( key, targetBlackboard.vector3s, new Vector3() );
					Debug.LogError( $"{ key } is currently {vector3}" );
					break;
				case Blackboard.BlackboardValueType.Vector2:
					if ( !targetBlackboard.vector2s.ContainsKey( key ) ) return State.Failure;
					Vector2 vector2 = targetBlackboard.GetData( key, targetBlackboard.vector2s, new Vector2() );
					Debug.LogError( $"{ key } is currently {vector2}" );
					break;
				default:
					break;
			}
			return State.Success;
		}
		return State.Failure;
	}
}
