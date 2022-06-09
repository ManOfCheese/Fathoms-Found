using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCircle : MonoBehaviour
{

	[HideInInspector] public GestureCircle gestureCircle;

	[Header( "References" )]
	public IntValue handPos;
	public BoolArrayValue fingers;
	public BoolValue confirmGesture;
	public SpriteRenderer[] fingerSprites;

	[Header( "Settings" )]
	public bool isCentreCircle;
	public bool showSpritesInCircle;
	public int circleNumber;

	private void OnEnable()
	{
		confirmGesture.onValueChanged += OnPlayerGesture;
	}

	private void OnDisable()
	{
		confirmGesture.onValueChanged -= OnPlayerGesture;
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.tag == gestureCircle.playerClawTag )
			handPos.Value = circleNumber;;
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.gameObject.tag == gestureCircle.playerClawTag )
		{
			if ( handPos.Value == circleNumber )
				handPos.Value = -1;
		}
	}

	public void OnPlayerGesture( bool _confirmGesture )
	{
		if ( _confirmGesture && handPos.Value == circleNumber && gestureCircle.clawInCircle )
			ConfirmGestureTwoWay( 0, handPos.Value, fingers.Value );
	}

	public void ConfirmGestureTwoWay( int _senderID, int _circleNumber, bool[] _fingers )
	{
		ConfirmGesture( _senderID, gestureCircle, _circleNumber, _fingers );
		if ( gestureCircle.twoWayCircle )
			ConfirmGesture( _senderID, gestureCircle.otherCircle, _circleNumber, _fingers );
	}

	public void ConfirmGesture( int _senderID, GestureCircle _gestureCircle, int _circleNumber, bool[] _fingers )
	{
		if ( circleNumber != 0 )
		{
			Gesture word = new Gesture( _circleNumber, _fingers );
			ShowGestureSprites( word );

			//Check if a word was already submitted in this circle.
			bool replacedWord = false;
			for ( int i = 0; i < _gestureCircle.words.Count; i++ )
			{
				if ( word.fingers[ 0 ] == false && word.fingers[ 1 ] == false && word.fingers[ 2 ] == false )
				{
					if ( word.circle == _gestureCircle.words[ i ].circle )
					{
						_gestureCircle.words.RemoveAt( i );
						replacedWord = true;
					}
				}
				else
				{
					if ( word.circle == _gestureCircle.words[ i ].circle )
					{
						_gestureCircle.words[ i ] = word;
						replacedWord = true;
					}
				}

			}
			if ( !replacedWord )
				_gestureCircle.words.Add( word );

			_gestureCircle.words.Sort( ( g1, g2 ) => g1.circle.CompareTo( g2.circle ) );
			_gestureCircle.sentence = Sam.Gesturing.GestureListToGCode( _gestureCircle.words );
			_gestureCircle.onWord?.Invoke( _senderID, _gestureCircle, word );
		}
		else
		{
			_gestureCircle.onSentence?.Invoke( _senderID, _gestureCircle );
		}
	}

	public void ShowGestureSprites( Gesture word )
	{
		if ( !isCentreCircle )
		{
			bool anyFingerOpen = false;
			for ( int i = 0; i < fingerSprites.Length - 1; i++ )
			{
				fingerSprites[ i + 1 ].enabled = word.fingers[ i ];
				if ( word.fingers[ i ] )
					anyFingerOpen = true;
			}
			if ( anyFingerOpen )
				fingerSprites[ 0 ].enabled = true;
			else
				fingerSprites[ 0 ].enabled = false;
		}
	}
}
