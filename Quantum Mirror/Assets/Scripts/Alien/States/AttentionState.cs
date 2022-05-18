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

		stateName = "AttentionState";
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
		_owner.gc.repositioning = true;
		_owner.gc.repositionedLegs.Clear();

		for ( int i = 0; i < _owner.gc.hands.Length; i++ ) {
			_owner.gc.hands[ i ].ikHandler.enabled = false;
		}
	}

	public override void UpdateState( AlienManager _owner )
	{
		_owner.TryDoor();

		if ( _owner.gc.repositioning ) {
			for ( int i = 0; i < _owner.gc.hands.Length; i++ ) 
			{
				if ( !_owner.gc.repositionedLegs.Contains( i ) ) 
				{
					Transform handTransform = _owner.gc.hands[ i ].ikHandler.transform;
					handTransform.position = Vector3.MoveTowards( handTransform.position, _owner.gc.idleHandTargets[ i ].position, 
						_owner.mc.speed * Time.deltaTime );

					if ( handTransform.position == _owner.gc.idleHandTargets[ i ].position )
						_owner.gc.repositionedLegs.Add( i );
				}
			}

			if ( _owner.gc.repositionedLegs.Count == _owner.gc.hands.Length )
				_owner.gc.repositioning = false;
		}

		if ( _owner.gc.gesturing )
		{
			GestureCircle gestureCircle = _owner.gc.gestureCircles[ _owner.gc.gestureHandIndex ];
			AlienIKHandler hand = _owner.gc.hands[ _owner.gc.gestureHandIndex ].ikHandler;

			List<Gesture> gestures;
			if ( _owner.gc.standardGesture )
			{
				gestures = new List<Gesture>();
				for ( int i = 0; i < _owner.gc.standardResponse.words.Count; i++ )
				{
					gestures.Add( _owner.gc.standardResponse.words[ i ] );
				}
			}
			else
				gestures = _owner.gc.responses.Items[ _owner.gc.sentenceIndex ].words;

			//Hold Gesture
			if ( _owner.gc.waiting )
			{
				if ( Time.time - _owner.gc.gestureHoldTimeStamp > _owner.gc.holdGestureFor )
					_owner.gc.waiting = false;
			}
			else
			{
				if ( _owner.gc.startGesture )
				{
					gestureCircle.gameObject.SetActive( true );
					_owner.gc.handTarget = gestureCircle.transform.position;
					_owner.gc.startGesture = false;
				}
				//Check if we have reached our new hand target.
				else
				{
					float speed = _owner.gc.gestureSpeed * Time.deltaTime;
					//Debug.Log( speed + " > " + Vector3.Distance( hand.transform.position, _owner.gc.handTarget ) + " | " + _owner.gc.waiting );
					if ( speed > Vector3.Distance( hand.transform.position, _owner.gc.handTarget ) )
					{
						if ( _owner.gc.endGesture )
						{
							_owner.gc.gesturing = false;
							gestureCircle.gameObject.SetActive( false );
							_owner.gc.waiting = false;
							_owner.gc.gestureHandIndex = -1;
							_owner.gc.endGesture = false;
							if ( _owner.gc.standardGesture ) _owner.gc.standardGesture = false;
							for ( int i = 0; i < gestureCircle.subCircles.Length; i++ )
							{
								for ( int j = 0; j < gestureCircle.subCircles[ i ].fingerSprites.Length; j++ )
									gestureCircle.subCircles[ i ].fingerSprites[ j ].SetActive( false );
							}
						}
						//Set new hand target.
						else
						{
							hand.transform.position = _owner.gc.handTarget;
							if ( _owner.gc.holdStart || _owner.gc.wordIndex >= 0 && !_owner.gc.holdStart )
							{
								_owner.gc.gestureHoldTimeStamp = Time.time;
								_owner.gc.waiting = true;
							}

							if ( _owner.gc.wordIndex >= 0 )
							{
								Gesture gesture = gestures[ _owner.gc.wordIndex ];
								gestureCircle.subCircles[ gesture.circle - 1 ].fingerSprites[ 0 ].SetActive( true );
								for ( int i = 0; i < gesture.fingers.Length; i++ )
									gestureCircle.subCircles[ gesture.circle - 1 ].fingerSprites[ i + 1 ].SetActive( gesture.fingers[ 0 ] );
							}

							_owner.gc.wordIndex++;
							//Set target as our start position.
							if ( _owner.gc.wordIndex > gestures.Count - 1 )
							{
								_owner.gc.handTarget = _owner.gc.idleHandTargets[ _owner.gc.gestureHandIndex ].position;
								_owner.gc.endGesture = true;
							}
							//Set target as the next word in the sentence.
							else
							{
								_owner.gc.handTarget = gestureCircle.subCircles[ gestures[ _owner.gc.wordIndex ].circle - 1 ].transform.position;
								//Debug.Log( Vector3.Distance( hand.transform.position, _owner.gc.handTarget ) + " | " + _owner.gc.waiting );
							}
						}
					}
					//Move towards hand target.
					else
						hand.transform.position = Vector3.MoveTowards( hand.transform.position, _owner.gc.handTarget, speed );
				}
			}
		}
	}

	public override void ExitState( AlienManager _owner )
	{
		for ( int i = 0; i < _owner.gc.hands.Length; i++ )
			_owner.gc.hands[ i ].ikHandler.enabled = true;
		for ( int i = 0; i < _owner.gc.gestureCircles.Length; i++ )
			_owner.gc.gestureCircles[ i ].gameObject.SetActive( false );
		_owner.gc.gesturing = false;
		_owner.gc.waiting = false;
		_owner.gc.gestureHandIndex = -1;
		_owner.gc.repositioning = false;
		_owner.gc.endGesture = false;
	}
}
