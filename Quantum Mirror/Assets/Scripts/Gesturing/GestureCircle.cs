using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestureCircle : MonoBehaviour
{

	[Header( "References" )]
	public SubCircle[] subCircles;

	[Header( "Settings" )]
	public List<PasswordAction> passwordActions;
	[Space( 10 )]
	public BoolValue usePartialConfirmation;
	public UnityEvent confirmationAction;

	public Sprite[] playerClawSprite;
	public Sprite[] alienClawSprite;
	public string playerClawTag;
	public string alienClawTag;

	[ReadOnly] public List<Gesture> words = new List<Gesture>();
	[ReadOnly] public string sentence;

	public delegate void OnWord( Gesture word );
	public OnWord onWord;

	public delegate void OnSentence( List<Gesture> sentenceCode );
	public OnSentence onSentence;

	public bool clawInCircle;
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
				lastClawInCircle = other.gameObject;
				for ( int i = 0; i < subCircles.Length; i++ )
				{
					for ( int j = 0; j < subCircles[ i ].fingerSprites.Length - 1; j++ )
						subCircles[ i ].fingerSprites[ j + 1 ].enabled = false;
					subCircles[ i ].fingerSprites[ 0 ].enabled = false;
					Debug.Log( "Don't touch my fingies" );
				}
			}

			if ( other.gameObject.tag == playerClawTag )
			{
				clawInCircle = true;
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
			clawInCircle = false;
	}

	public void ConfirmWord( Gesture word )
	{
		if ( usePartialConfirmation )
		{
			for ( int i = 0; i < passwordActions.Count; i++ )
			{
				for ( int j = 0; j < passwordActions[ i ].sentence.words.Count; j++ )
				{
					if ( passwordActions[ i ].sentence.words[ j ].circle == word.circle &&
						passwordActions[ i ].sentence.words[ j ].fingers[ 0 ] == word.fingers[ 0 ] &&
						passwordActions[ i ].sentence.words[ j ].fingers[ 1 ] == word.fingers[ 1 ] &&
						passwordActions[ i ].sentence.words[ j ].fingers[ 2 ] == word.fingers[ 2 ] )
					{
						confirmationAction?.Invoke();
					}
				}
			}
		}
	}

	public void ConfirmSentence( List<Gesture> sentence )
	{

	}

}

[System.Serializable]
public class PasswordAction
{
	public bool useForPartialConfirmation;
	public GestureSequence sentence;
	public UnityEvent<List<int>> action;
}