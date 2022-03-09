using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class AttentionState : State<AlienManager>
{
	#region singleton
	//Create a single instance of this state for all state machines.
	private static AttentionState _instance;

	private AttentionState()
	{
		if ( _instance != null )
		{
			return;
		}

		_instance = this;
	}

	public static AttentionState Instance
	{
		get
		{
			if ( _instance == null )
			{
				new AttentionState();
			}

			return _instance;
		}
	}
	#endregion

	public override void EnterState( AlienManager _owner )
	{
		_owner.mc.agent.destination = _owner.transform.position; 
	}

	public override void UpdateState( AlienManager _owner )
	{
		if ( _owner.gc.gesture )
		{
			GameObject circle = _owner.gc.gestureCircles[ _owner.gc.handIndex ];
			AlienIKHandler hand = _owner.gc.hands[ _owner.gc.handIndex ];
			Vector3 handPos = hand.transform.position;
			Vector2 gesturePoint = _owner.gc.gesturePoints[ _owner.gc.gestureIndex ];

			if ( !_owner.gc.startGesture )
			{
				circle.SetActive( true );
				Vector3 left = circle.transform.right * gesturePoint.x;
				Vector3 up = circle.transform.up * gesturePoint.y;
				_owner.gc.handTarget = circle.transform.position + ( ( left + up ) * _owner.gc.gCircleDiameter );
				_owner.gc.startGesture = true;
			}

			if ( _owner.gc.waiting )
			{
				if ( Time.time - _owner.gc.waitTimeStamp > _owner.gc.holdPosFor )
					_owner.gc.waiting = false;
			}
			else
			{
				float magnitude = _owner.gc.gestureSpeed * Time.deltaTime;
				float dist = Vector3.Distance( handPos, _owner.gc.handTarget );
				if ( dist < magnitude )
				{
					handPos += ( _owner.gc.handTarget - handPos ).normalized * dist;
					_owner.gc.waitTimeStamp = Time.time;
					_owner.gc.waiting = true;

					_owner.gc.gestureIndex++;
					if ( _owner.gc.gestureIndex >= _owner.gc.gesturePoints.Length )
					{
						_owner.gc.gesture = false;
						hand.enabled = true;
						circle.SetActive( false );
					}
					else
					{
						_owner.gc.handTarget = circle.transform.position + new Vector3( gesturePoint.x * _owner.gc.gCircleDiameter, 0f, 
							gesturePoint.y * _owner.gc.gCircleDiameter );
					}
				}
				else
				{
					handPos += ( _owner.gc.handTarget - handPos ).normalized * magnitude;
				}
			}
		}
	}

	public override void ExitState( AlienManager _owner )
	{

	}
}
