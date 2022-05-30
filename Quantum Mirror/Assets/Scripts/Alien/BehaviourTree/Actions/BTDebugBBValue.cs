using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTDebugBBValue : BTBBActionNode
{

	protected override void OnStart()
	{
	}

	protected override void OnStop()
	{
	}

	protected override State OnUpdate()
	{
		if ( blackboard != null )
		{
			switch ( valueType )
			{
				case Blackboard.BlackboardValueType.GameObject:
					if ( !blackboard.gameObjects.ContainsKey( key ) ) return State.Failure;
					GameObject go = new GameObject();
					go = blackboard.GetData( key, blackboard.gameObjects, go );
					Debug.LogError( $"{ key } is currently {go.name}" );
					break;
				case Blackboard.BlackboardValueType.Bool:
					if ( !blackboard.bools.ContainsKey( key ) ) return State.Failure;
					bool b = blackboard.GetData( key, blackboard.bools, true );
					Debug.LogError( $"{ key } is currently {b}" );
					break;
				case Blackboard.BlackboardValueType.String:
					if ( !blackboard.strings.ContainsKey( key ) ) return State.Failure;
					string s = blackboard.GetData( key, blackboard.strings, "" );
					Debug.LogError( $"{ key } is currently {s}" );
					break;
				case Blackboard.BlackboardValueType.Int:
					if ( !blackboard.ints.ContainsKey( key ) ) return State.Failure;
					int bbint = blackboard.GetData( key, blackboard.ints, new int() );
					Debug.LogError( $"{ key } is currently {bbint}" );
					break;
				case Blackboard.BlackboardValueType.Float:
					if ( !blackboard.floats.ContainsKey( key ) ) return State.Failure;
					float f = blackboard.GetData( key, blackboard.floats, new float() );
					Debug.LogError( $"{ key } is currently {f}" );
					break;
				case Blackboard.BlackboardValueType.Vector3:
					if ( !blackboard.vector3s.ContainsKey( key ) ) return State.Failure;
					Vector3 vector3 = blackboard.GetData( key, blackboard.vector3s, new Vector3() );
					Debug.LogError( $"{ key } is currently {vector3}" );
					break;
				case Blackboard.BlackboardValueType.Vector2:
					if ( !blackboard.vector2s.ContainsKey( key ) ) return State.Failure;
					Vector2 vector2 = blackboard.GetData( key, blackboard.vector2s, new Vector2() );
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
