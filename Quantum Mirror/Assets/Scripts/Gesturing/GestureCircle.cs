using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestureCircle : MonoBehaviour
{

	[Header( "References" )]
	public BoolValue isUsingGestureCircle;
	public SubCircle[] subCircles;
	public BoolValue usePartialConfirmation;
	public BoolValue confirmOnWord;

	[Header( "Settings" )]
	public string playerClawTag;
	public string alienClawTag;
	public Color standardColor;
	public Color confirmationColor;
	[Space( 10 )]
	public List<PasswordAction> passwordActions;
	public Sprite[] playerClawSprite;
	public Sprite[] alienClawSprite;

	[ReadOnly] public List<Gesture> words = new List<Gesture>();
	[ReadOnly] public string sentence;
	[ReadOnly] public bool clawInCircle;

	public delegate void OnWord( Gesture word );
	public OnWord onWord;

	public delegate void OnSentence( List<Gesture> sentenceCode );
	public OnSentence onSentence;

	private GameObject lastClawInCircle;

	private void Awake()
	{
		for ( int i = 0; i < subCircles.Length; i++ )
			subCircles[ i ].gestureCircle = this;
	}

	private void OnEnable()
	{
		onSentence += ConfirmSentence;
		onWord += ConfirmWord;
	}

	private void OnDisable()
	{
		onSentence -= ConfirmSentence;
		onWord -= ConfirmWord;
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.tag == playerClawTag || other.gameObject.tag == alienClawTag )
		{
			if ( lastClawInCircle != other.gameObject && lastClawInCircle != null )
			{
				for ( int i = 0; i < subCircles.Length; i++ )
				{
					for ( int j = 0; j < subCircles[ i ].fingerSprites.Length - 1; j++ )
						subCircles[ i ].fingerSprites[ j + 1 ].enabled = false;
					subCircles[ i ].fingerSprites[ 0 ].enabled = false;

					for ( int j = 0; j < subCircles[ i ].fingerSprites.Length; j++ )
						subCircles[ i ].fingerSprites[ j ].color = standardColor;
				}
			}
			lastClawInCircle = other.gameObject;

			if ( other.gameObject.tag == playerClawTag )
			{
				clawInCircle = true;
				isUsingGestureCircle.Value = true;
				for ( int i = 0; i < subCircles.Length; i++ )
				{
					if ( !subCircles[ i ].isCentreCircle )
					{
						for ( int j = 0; j < subCircles[ i ].fingerSprites.Length; j++ )
							subCircles[ i ].fingerSprites[ j ].sprite = playerClawSprite[ j ];
					}
				}
			}
			else if ( other.gameObject.tag == alienClawTag )
			{
				for ( int i = 0; i < subCircles.Length; i++ )
				{
					if ( !subCircles[ i ].isCentreCircle )
					{
						for ( int j = 0; j < subCircles[ i ].fingerSprites.Length; i++ )
							subCircles[ i ].fingerSprites[ j ].sprite = playerClawSprite[ j ];
					}
				}
			}
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.gameObject.tag == playerClawTag )
		{
			isUsingGestureCircle.Value = false;
			clawInCircle = false;
		}
	}

	public void ConfirmWord( Gesture word )
	{
		if ( usePartialConfirmation && confirmOnWord.Value )
		{
			PartialConfirmation( word );
		}
	}

	public void ConfirmSentence( List<Gesture> sentence )
	{
		for ( int i = 0; i < passwordActions.Count; i++ )
		{
			if ( Gestures.GestureLogic.GestureListToGCode( sentence ) == Gestures.GestureLogic.GestureSequenceToGCode( passwordActions[ i ].sentence ) )
				passwordActions[ i ].action?.Invoke();
		}
		if ( usePartialConfirmation && !confirmOnWord.Value )
		{
			for ( int i = 0; i < sentence.Count; i++ )
				PartialConfirmation( sentence[ i ] );
		}
	}

	public void PartialConfirmation( Gesture word )
	{
		for ( int i = 0; i < passwordActions.Count; i++ )
		{
			for ( int j = 0; j < passwordActions[ i ].sentence.words.Count; j++ )
			{
				if ( passwordActions[ i ].useForPartialConfirmation &&
					passwordActions[ i ].sentence.words[ j ].circle == word.circle &&
					passwordActions[ i ].sentence.words[ j ].fingers[ 0 ] == word.fingers[ 0 ] &&
					passwordActions[ i ].sentence.words[ j ].fingers[ 1 ] == word.fingers[ 1 ] &&
					passwordActions[ i ].sentence.words[ j ].fingers[ 2 ] == word.fingers[ 2 ] )
				{
					for ( int k = 0; k < subCircles[ word.circle ].fingerSprites.Length; k++ )
						subCircles[ word.circle ].fingerSprites[ k ].color = confirmationColor;
				}
			}
		}
	}

}

[System.Serializable]
public class PasswordAction
{
	public bool useForPartialConfirmation;
	public GestureSequence sentence;
	public UnityEvent action;
}