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
			List<Gesture> gestures = _owner.gc.responses.Items[ _owner.gc.sentenceIndex ].words;
			AlienIKHandler hand = _owner.gc.hands[ _owner.gc.handIndex ];

			if ( _owner.gc.startGesture )
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
				Debug.Log( speed + " > " + Vector3.Distance( hand.transform.position, _owner.gc.handTarget ) + " | " + _owner.gc.waiting );
				if ( speed > Vector3.Distance( hand.transform.position, _owner.gc.handTarget ) )
				{
					if ( _owner.gc.endGesture ) 
					{
						_owner.gc.gesture = false;
						hand.enabled = true;
						circle.SetActive( false );
						_owner.gc.waiting = false;
					}
					else {
						hand.transform.position = _owner.gc.handTarget;
						_owner.gc.waitTimeStamp = Time.time;
						_owner.gc.waiting = true;

						_owner.gc.wordIndex++;
						//Debug.Log( _owner.gc.waitTimeStamp + " | " + _owner.gc.wordIndex );
						if ( _owner.gc.wordIndex > _owner.gc.responses.Items[ _owner.gc.sentenceIndex ].words.Count - 1 ) {
							_owner.gc.handTarget = _owner.gc.preGestureHandPos;
							_owner.gc.endGesture = true;
							
						}
						else {
							_owner.gc.handTarget = hand.subCircles[ gestures[ _owner.gc.wordIndex ].circle ].transform.position;
							Debug.Log( Vector3.Distance( hand.transform.position, _owner.gc.handTarget ) + " | " + _owner.gc.waiting );
						}
					}
				}
				else
				{
					hand.transform.position = Vector3.MoveTowards( hand.transform.position, _owner.gc.handTarget, speed );
					Debug.Log( "Moving" );
				}
			}
		}
	}

	public override void ExitState( AlienManager _owner )
	{

	}
}
