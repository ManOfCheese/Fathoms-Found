using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gestures;

public class GestureListener : MonoBehaviour
{

	[Header( "References" )]
	public BoolValue confirmGesture;
	public BoolValue isInGestureMode;
	public IntValue handPos;
	public BoolArrayValue fingers;

	public List<Gesture> words = new List<Gesture>();
	public List<int> playerSentence;

	public delegate void OnWord( Gesture word );
	public OnWord onWord;

	public delegate void OnSentence( List<int> sentenceCode );
	public OnSentence onSentence;

	private void OnEnable()
	{
		isInGestureMode.onValueChanged += OnPlayerTogglesGestureMode;
		confirmGesture.onValueChanged += OnConfirmGesture;
	}

	private void OnDisable()
	{
		isInGestureMode.onValueChanged -= OnPlayerTogglesGestureMode;
		confirmGesture.onValueChanged -= OnConfirmGesture;
	}

	public void OnPlayerTogglesGestureMode( bool isInGesturemode )
	{
		words.Clear();
	}

	public void OnConfirmGesture( bool confirmGesture )
	{
		if ( confirmGesture )
		{
			if ( handPos.Value != 0 )
			{
				Gesture word = new Gesture( handPos.Value, fingers.Value );
				onWord?.Invoke( word );

				//Check if a word was already submitted in this circle.
				bool replacedWord = false;
				for ( int i = 0; i < words.Count; i++ )
				{
					if ( word.circle == words[ i ].circle )
					{
						words[ i ] = word;
						replacedWord = true;
					}
				}
				if ( !replacedWord )
					words.Add( word );

				words.Sort( ( g1, g2 ) => g1.circle.CompareTo( g2.circle ) );
				playerSentence = Gestures.GestureLogic.WordListToCode( words );
			}
			else
			{
				onSentence?.Invoke( playerSentence );
			}
		}
	}
}
