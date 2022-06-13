using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class GesturingState : State<HandController>
{
	#region singleton
	//Create a single instance of this state for all state machines.
	private static GesturingState _instance;

	private GesturingState()
	{
		if ( _instance != null )
		{
			return;
		}

		_instance = this;
	}

	public static GesturingState Instance
	{
		get
		{
			if ( _instance == null )
			{
				new GesturingState();
			}

			return _instance;
		}
	}
	#endregion

	public override void EnterState( HandController _o )
	{
		_o.ikManager.handsAvailable--;

		//If circles need to be cleared.
		if ( _o.clearCircle )
		{
			for ( int i = 0; i < _o.gestureCircle.subCircles.Length; i++ )
			{
				bool foundGestureToClear = false;

				for ( int j = 0; j < _o.gestureCircle.subCircles[ i ].fingerSprites.Length; j++ )
				{
					if ( _o.gestureCircle.subCircles[ i ].fingerSprites[ j ].enabled == true && !foundGestureToClear )
					{
						_o.gesturesToMake.Add( new Gesture( i, new bool[ 3 ] { false, false, false } ) );
						foundGestureToClear = true;
					}
				}

			}
		}

		if ( _o.startAtCentre )
			_o.gesturesToMake.Add( new Gesture( 0, new bool[ 3 ] { _o.fingerPosAtCentre, _o.fingerPosAtCentre, _o.fingerPosAtCentre } ) );

		if ( _o.standardGesture )
		{
			for ( int i = 0; i < _o.standardResponse.words.Count; i++ )
				_o.gesturesToMake.Add( _o.standardResponse.words[ i ] );
		}
		else if ( _o.sentence != null )
		{
			for ( int i = 0; i < _o.sentence.words.Count; i++ )
				_o.gesturesToMake.Add( _o.sentence.words[ i ] );
		}
		else if ( _o.sentenceIndex < _o.responses.Items.Count - 1 )
		{
			for ( int i = 0; i < _o.responses.Items[ context.gc.sentenceIndex ].words.Count; i++ )
				_o.gesturesToMake.Add( _o.responses.Items[ _o.sentenceIndex ].words[ i ] );
		}
		else if ( _o.gesturesToMake.Count == 0 && !_o.endAtCentre )
			_o.endAtCentre = true;

		if ( _o.endAtCentre )
			_o.gesturesToMake.Add( new Gesture( 0, new bool[ 3 ] { _o.fingerPosAtCentre, _o.fingerPosAtCentre, _o.fingerPosAtCentre } ) );
	}

	public override void UpdateState( HandController _o )
	{
		Debug.DrawLine( _o.handTransform.transform.position, _o.handTransform.transform.position + _o.gestureCircle.transform.up * 5f );

		//Hold Gesture
		if ( _o.moveState == MoveState.Holding )
		{
			if ( Time.time - _o.holdTimeStamp > _o.holdGestureFor )
			{
				_o.moveState = MoveState.Moving;
				for ( int i = 0; i < _o.fingerAnimators.Length; i++ )
					_o.fingerAnimators[ i ].SetBool( "FingerOpen", false );
			}
		}
		else
		{
			if ( _o.moveState == MoveState.Starting )
			{
				_o.handTarget = _o.gestureCircle.subCircles[ _o.gesturesToMake[ _o.wordIndex ].circle ].transform.position + ( _o.gestureCircle.transform.up * _o.gestureDistance );
				_o.moveState = MoveState.Moving;

				for ( int i = 0; i < _o.fingerAnimators.Length; i++ )
				{
					if ( _o.fingerAnimators[ i ].GetBool( "FingerOpen" ) == _o.defaultFingerPos )
						_o.fingerAnimators[ i ].SetBool( "FingerOpen", !_o.defaultFingerPos );
				}
			}
			//Check if we have reached our new hand target.
			else
			{
				float speed = _o.gestureSpeed * Time.deltaTime;
				//Debug.Log( speed + " > " + Vector3.Distance( hand.transform.position, _owner.gc.handTarget ) + " | " + _owner.gc.waiting );
				if ( speed > Vector3.Distance( _o.handTransform.position, _o.handTarget ) )
				{
					if ( _o.moveState == MoveState.Ending )
					{
						if ( _o.standardGesture ) _o.standardGesture = false;
						_o.moveState = MoveState.Ended;
					}
					//Set new hand target.
					else if ( _o.moveState == MoveState.Moving )
					{
						_o.handTransform.position = _o.handTarget;
						if ( _o.holdStart || _o.wordIndex >= 0 && !_o.holdStart )
						{
							//Manipulate hand to make the gesture.
							_o.handTransform.transform.LookAt( _o.handTransform.transform.position + _o.gestureCircle.transform.up );
							for ( int i = 0; i < _o.fingerAnimators.Length; i++ )
							{
								if ( _o.gesturesToMake[ _o.wordIndex ].fingers[ i ] && !_o.fingerAnimators[ i ].GetBool( "FingerOpen" ) )
									_o.fingerAnimators[ i ].SetBool( "FingerOpen", true );
							}

							//Input the gesture into the circle.
							if ( _o.inputGesture )
							{
								_o.gestureCircle.subCircles[ _o.gesturesToMake[ _o.wordIndex ].circle ].ConfirmGestureTwoWay( _o.ID, _o.gesturesToMake[ _o.wordIndex ].circle,
									_o.gesturesToMake[ _o.wordIndex ].fingers );
							}

							_o.holdTimeStamp = Time.time;
							if ( _o.holdGestureFor > 0f )
								_o.moveState = MoveState.Holding;
						}

						_o.wordIndex++;
						//Set target as our start position.
						if ( _o.wordIndex > _o.gesturesToMake.Count - 1 )
						{
							if ( _o.returnToIdle )
								_o.handTarget = _o.idleHandTarget.position + ( _o.gestureCircle.transform.up * _o.gestureDistance );

							_o.moveState = MoveState.Ending;
						}
						//Set target as the next word in the sentence.
						else
							_o.handTarget = _o.gestureCircle.subCircles[ _o.gesturesToMake[ _o.wordIndex ].circle ].transform.position + 
								( _o.gestureCircle.transform.up * _o.gestureDistance );
					}
				}
				//Move towards hand target.
				else
					_o.handTransform.position = Vector3.MoveTowards( _o.handTransform.position, _o.handTarget, speed );
			}
		}
	}

	public override void ExitState( HandController _o )
	{
		_o.ikManager.handsAvailable++;
		_o.gesturesToMake.Clear();
		_o.gestureCircle = null;
	}
}
