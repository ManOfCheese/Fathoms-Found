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
	}

	public override void UpdateState( HandController _o )
	{
		Debug.DrawLine( _o.transform.position, _o.transform.position + _o.gestureCircle.transform.up * 5f );

		//Hold Gesture
		if ( _o.moveState == GestureState.Holding )
		{
			if ( Time.time - _o.holdTimeStamp > _o.holdGestureFor )
			{
				_o.moveState = GestureState.Moving;
				for ( int i = 0; i < _o.fingerAnimators.Length; i++ )
					_o.fingerAnimators[ i ].SetBool( "FingerOpen", false );
			}
		}
		else
		{
			if ( _o.moveState == GestureState.Starting )
			{
				_o.handTarget = _o.gestureCircle.subCircles[ _o.gesturesToMake[ _o.wordIndex ].circle ].transform.position + 
					( _o.gestureCircle.transform.up * _o.handToPanelDistance );
				_o.moveState = GestureState.Moving;

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
				if ( speed > Vector3.Distance( _o.transform.position, _o.handTarget ) )
				{
					//Set new hand target.
					if ( _o.moveState == GestureState.Moving )
					{
						_o.transform.position = _o.handTarget;
						if ( _o.holdStart || _o.wordIndex >= 0 && !_o.holdStart )
						{
							//Manipulate hand to make the gesture.
							_o.handTransform.LookAt( _o.handTransform.transform.position + _o.gestureCircle.transform.up );
							for ( int i = 0; i < _o.fingerAnimators.Length; i++ )
							{
								if ( _o.gesturesToMake[ _o.wordIndex ].fingers[ i ] && !_o.fingerAnimators[ i ].GetBool( "FingerOpen" ) )
									_o.fingerAnimators[ i ].SetBool( "FingerOpen", true );
							}

							//Input the gesture into the circle.
							if ( _o.inputGesture )
							{
								_o.gestureCircle.subCircles[ _o.gesturesToMake[ _o.wordIndex ].circle ].ConfirmGestureTwoWay( _o.ikManager.ID, 
									_o.gesturesToMake[ _o.wordIndex ].circle, _o.gesturesToMake[ _o.wordIndex ].fingers );
							}

							_o.holdTimeStamp = Time.time;
							if ( _o.holdGestureFor > 0f )
								_o.moveState = GestureState.Holding;
						}

						_o.wordIndex++;
						//Set target as our start position.
						if ( _o.wordIndex > _o.gesturesToMake.Count - 1 )
						{
							if ( _o.returnToIdle )
								_o.handTarget = _o.idleHandTarget.position + ( new Vector3( 0f, 1f, 0f ) * _o.heightOffset );

							_o.stateMachine.ChangeState( _o.ikManager.statesByName[ "WalkingState" ] );
						}
						//Set target as the next word in the sentence.
						else
							_o.handTarget = _o.gestureCircle.subCircles[ _o.gesturesToMake[ _o.wordIndex ].circle ].transform.position + 
								( _o.gestureCircle.transform.up * _o.handToPanelDistance );
					}
				}
				//Move towards hand target.
				else
					_o.transform.position = Vector3.MoveTowards( _o.transform.position, _o.handTarget, speed );
			}
		}
	}

	public override void ExitState( HandController _o )
	{
		_o.ikManager.handsAvailable++;
		_o.gesturesToMake.Clear();
		_o.gestureCircle = null;
		_o.holdTimeStamp = 0f;
		_o.wordIndex = 0;
		_o.preGestureHandPos = Vector3.zero;
		_o.handTarget = Vector3.zero;
		_o.oldPosition = _o.handTransform.position;
		_o.currentPosition = _o.handTransform.position;
		_o.newPosition = _o.handTransform.position;
	}
}
