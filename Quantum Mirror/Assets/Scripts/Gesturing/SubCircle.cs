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
		confirmGesture.onValueChanged += OnConfirmGesture;
	}

	private void OnDisable()
	{
		confirmGesture.onValueChanged -= OnConfirmGesture;
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

	public void OnConfirmGesture( bool confirmGesture )
	{
		if ( confirmGesture && handPos.Value == circleNumber && gestureCircle.clawInCircle )
			ConfirmGesture();
	}

	public void ConfirmGesture()
	{
		if ( circleNumber != 0 )
		{
			Gesture word = new Gesture( circleNumber, fingers.Value );
			ShowGestureSprites( word );

			//Check if a word was already submitted in this circle.
			bool replacedWord = false;
			for ( int i = 0; i < gestureCircle.words.Count; i++ )
			{
				if ( word.fingers[ 0 ] == false && word.fingers[ 1 ] == false && word.fingers[ 2 ] == false )
				{
					if ( word.circle == gestureCircle.words[ i ].circle )
					{
						gestureCircle.words.RemoveAt( i );
						replacedWord = true;
					}
				}
				else
				{
					if ( word.circle == gestureCircle.words[ i ].circle )
					{
						gestureCircle.words[ i ] = word;
						replacedWord = true;
					}
				}

			}
			if ( !replacedWord )
				gestureCircle.words.Add( word );

			gestureCircle.words.Sort( ( g1, g2 ) => g1.circle.CompareTo( g2.circle ) );
			gestureCircle.sentence = Gestures.GestureLogic.GestureListToGCode( gestureCircle.words );
			gestureCircle.onWord?.Invoke( word );
		}
		else
		{
			gestureCircle.onSentence?.Invoke( gestureCircle.words );
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
