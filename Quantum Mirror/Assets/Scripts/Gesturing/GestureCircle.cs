using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestureCircle : MonoBehaviour
{

	[Header( "References" )]
	public RunTimeSet<GestureCircle> set;
	[Tooltip( "Where the alien will stand to use the gesture circle" )]
	public Transform gesturePosition;
	public BoolValue isUsingGestureCircle;
	public BoolValue usePartialConfirmation;
	public BoolValue confirmOnWord;
	public SubCircle[] subCircles;

	[Header( "Settings" )]
	public bool twoWayCircle;
	public GestureCircle otherCircle;
	public GestureSequence presetSentence;
	public string playerClawTag;
	public string alienClawTag;
	public Color standardColor;
	public Color confirmationColor;
	[Space( 10 )]
	public List<PasswordAction> passwordActions;
	public Sprite[] playerClawSprite;
	public Sprite[] alienClawSprite;

	public List<Gesture> words = new List<Gesture>();
	[ReadOnly] public string sentence;
	[ReadOnly] public bool clawInCircle;

	public delegate void OnWord( int senderID, GestureCircle gestureCircle, Gesture word );
	public OnWord onWord;

	public delegate void OnSentence( int senderID, GestureCircle gestureCircle );
	public OnSentence onSentence;

	private GameObject lastClawInCircle;
	private PasswordAction activePassword;

	private void Awake()
	{
		for ( int i = 0; i < subCircles.Length; i++ )
		{
			for ( int j = 0; j < subCircles[ i ].fingerSprites.Length; j++ )
				subCircles[ i ].fingerSprites[ j ].color = standardColor;
		}
		for ( int i = 0; i < subCircles.Length; i++ )
			subCircles[ i ].gestureCircle = this;

		if ( presetSentence != null )
		{
			for ( int i = 0; i < presetSentence.words.Count; i++ )
				words.Add( presetSentence.words[ i ] );
			sentence = Sam.Gesturing.GestureSequenceToGCode( presetSentence );
			for ( int i = 0; i < subCircles.Length; i++ )
			{
				for ( int j = 0; j < presetSentence.words.Count; j++ )
				{
					if ( i == presetSentence.words[ j ].circle )
						subCircles[ i ].ShowGestureSprites( presetSentence.words[ j ] );
				}
			}
		}
	}

	private void OnEnable()
	{
		set.Add( this );
		onWord += ConfirmWord;
		onSentence += ConfirmSentence;
		if ( twoWayCircle )
		{
			otherCircle.onWord += ConfirmWord;
			otherCircle.onSentence += ConfirmSentence;
		}
	}

	private void OnDisable()
	{
		set.Remove( this );
		onSentence -= ConfirmSentence;
		onWord -= ConfirmWord;
		if ( twoWayCircle )
		{
			otherCircle.onWord -= ConfirmWord;
			otherCircle.onSentence -= ConfirmSentence;
		}
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

	public void ConfirmWord( int senderID, GestureCircle gestureCircle, Gesture word )
	{
		if ( activePassword != null )
		{
			if ( Sam.Gesturing.GestureListToGCode( words ) != Sam.Gesturing.GestureSequenceToGCode( activePassword.sentence ) )
			{
				activePassword.onRemoved?.Invoke();
				activePassword = null;
			}
		}

		if ( gestureCircle != this )
		{
			subCircles[ word.circle ].ShowGestureSprites( word );
		}
		if ( usePartialConfirmation && confirmOnWord.Value )
		{
			PartialConfirmation( word );
		}
	}

	public void ConfirmSentence( int senderID, GestureCircle gestureCircle )
	{
		if ( activePassword != null )
		{
			if ( Sam.Gesturing.GestureListToGCode( words ) != Sam.Gesturing.GestureSequenceToGCode( activePassword.sentence ) )
			{
				activePassword.onRemoved?.Invoke();
				activePassword = null;
			}
		}
		
		for ( int i = 0; i < passwordActions.Count; i++ )
		{
			Debug.Log( Sam.Gesturing.GestureListToGCode( words ) + " == " + Sam.Gesturing.GestureSequenceToGCode( passwordActions[ i ].sentence ) );
			if ( Sam.Gesturing.GestureListToGCode( words ) == Sam.Gesturing.GestureSequenceToGCode( passwordActions[ i ].sentence ) )
			{
				passwordActions[ i ].onInput?.Invoke();
				activePassword = passwordActions[ i ];
			}
		}
		if ( usePartialConfirmation && !confirmOnWord.Value )
		{
			for ( int i = 0; i < words.Count; i++ )
				PartialConfirmation( words[ i ] );
		}
	}

	public void Clear()
	{
		words.Clear();
		sentence = "";
		for ( int i = 0; i < subCircles.Length; i++ )
			subCircles[ i ].ShowGestureSprites( new Gesture( 0, new bool[ 3 ] { false, false, false } ) );
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
	public UnityEvent onInput;
	public UnityEvent onRemoved;
}