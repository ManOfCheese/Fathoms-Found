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
			List<Gesture> gestures = _owner.gc.responses.Items[ _owner.gc.gestureIndex ].words;

			if ( !_owner.gc.startGesture )
			{
				circle.SetActive( true );
				_owner.gc.handTarget = circle.transform.position;
				_owner.gc.startGesture = false;
			}

			if ( _owner.gc.waiting )
			{
				if ( Time.time - _owner.gc.waitTimeStamp > _owner.gc.holdPosFor )
					_owner.gc.waiting = false;
			}
			else
			{
				float speed = _owner.gc.gestureSpeed * Time.deltaTime;
				float dist = Vector3.Distance( hand.transform.position, _owner.gc.handTarget );
				if ( dist < speed )
				{
					hand.transform.position += ( _owner.gc.handTarget - hand.transform.position ).normalized * dist;
					_owner.gc.waitTimeStamp = Time.time;
					_owner.gc.waiting = true;

					_owner.gc.gestureIndex++;
					if ( _owner.gc.gestureIndex >= gestures.Count )
					{
						_owner.gc.gesture = false;
						hand.enabled = true;
						circle.SetActive( false );
					}
					else
					{
						_owner.gc.handTarget = hand.subCircles[ gestures[ _owner.gc.gestureIndex ].circle ].transform.position;
					}
				}
				else
				{
					hand.transform.position += ( _owner.gc.handTarget - hand.transform.position ).normalized * speed;
				}
			}
		}
	}

	public override void ExitState( AlienManager _owner )
	{

	}
}
