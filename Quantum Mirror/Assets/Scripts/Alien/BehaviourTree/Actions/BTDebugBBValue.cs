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
			if ( !blackboard.values.ContainsKey( key ) ) return State.Failure;

			switch ( valueType )
			{
				case Blackboard.BlackboardValueType.GameObject:
					GameObject go = new GameObject();
					go = blackboard.GetData( key, go );
					Debug.LogError( $"{ key } is currently {go.name}" );
					break;
				case Blackboard.BlackboardValueType.Bool:
					bool b = blackboard.GetData( key, true );
					Debug.LogError( $"{ key } is currently {b}" );
					break;
				case Blackboard.BlackboardValueType.String:
					string s = blackboard.GetData( key, "" );
					Debug.LogError( $"{ key } is currently {s}" );
					break;
				case Blackboard.BlackboardValueType.Int:
					int bbint = blackboard.GetData( key, new int() );
					Debug.LogError( $"{ key } is currently {bbint}" );
					break;
				case Blackboard.BlackboardValueType.Float:
					float f = blackboard.GetData( key, new float() );
					Debug.LogError( $"{ key } is currently {f}" );
					break;
				case Blackboard.BlackboardValueType.Vector3:
					Vector3 vector3 = blackboard.GetData( key, new Vector3() );
					Debug.LogError( $"{ key } is currently {vector3}" );
					break;
				case Blackboard.BlackboardValueType.Vector2:
					Vector2 vector2 = blackboard.GetData( key, new Vector2() );
					Debug.LogError( $"{ key } is currently {vector2}" );
					break;
				case Blackboard.BlackboardValueType.PositionList:
					List<Vector3> positionList = blackboard.GetData( key, new List<Vector3>() );
					Debug.LogError( $"{ key } is currently {positionList}" );
					break;
				default:
					break;
			}
			return State.Success;
		}
		return State.Failure;
	}
}
