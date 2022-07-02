using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestureCircle : MonoBehaviour
{

	[Header( "References" )]
	public Animator buttonAnimator;
	public RunTimeSet<GestureCircle> set;
	[Tooltip( "Where the alien will stand to use the gesture circle" )]
	public Transform gesturePosition;
	public BoolValue isUsingGestureCircle;
	public BoolValue usePartialConfirmation;
	public BoolValue confirmOnWord;
	public SubCircle[] subCircles;

	[Header( "Settings" )]
	public bool twoWayCircle;
	public bool usePasswordActionsForConfirmation;
	public GestureCircle otherCircle;
	public GestureSequence presetSentence;
	public string playerClawTag;
	public string alienClawTag;
	public Color standardColor;
	public Color confirmationColor;
	[Space( 10 )]
	public List<GestureSequence> sentencesToConfirm;
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

	[HideInInspector] public bool usePlayerSprites;
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
		if ( other.gameObject.tag == playerClawTag )
		{
			clawInCircle = true;
			isUsingGestureCircle.Value = true;
			for ( int i = 0; i < subCircles.Length; i++ )
				usePlayerSprites = true;
		}
		else if ( other.gameObject.tag == alienClawTag )
		{
			for ( int i = 0; i < subCircles.Length; i++ )
				usePlayerSprites = false;
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
				buttonAnimator.SetTrigger( "TurnOff" );
				activePassword = null;
			}
		}

		if ( gestureCircle != this )
			subCircles[ word.circle ].ShowGestureSprites( word );
		if ( usePartialConfirmation && confirmOnWord.Value )
			PartialConfirmation( word );
		else
			Deconfirm( word );
	}

	public void ConfirmSentence( int senderID, GestureCircle gestureCircle )
	{
		bool correct = false;

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
			if ( Sam.Gesturing.GestureListToGCode( words ) == Sam.Gesturing.GestureSequenceToGCode( passwordActions[ i ].sentence ) )
			{
				passwordActions[ i ].onInput?.Invoke();
				buttonAnimator.SetTrigger( "TurnOn" );
				correct = true;
				activePassword = passwordActions[ i ];
			}
		}

		if ( usePartialConfirmation && !confirmOnWord.Value )
		{
			for ( int i = 0; i < words.Count; i++ )
				PartialConfirmation( words[ i ] );
		}

		if ( !correct )
			buttonAnimator.SetTrigger( "Incorrect" );
	}

	public void Clear()
	{
		Debug.Log( "Clear Words" );
		words.Clear();
		sentence = "";
		for ( int i = 0; i < subCircles.Length; i++ )
			subCircles[ i ].ShowGestureSprites( new Gesture( 0, new bool[ 3 ] { false, false, false } ) );
	}

	public void PartialConfirmation( Gesture word )
	{
		List<int> circlesConfirmed = new List<int>();
		if ( usePasswordActionsForConfirmation )
		{
			for ( int i = 0; i < passwordActions.Count; i++ )
			{
				for ( int j = 0; j < passwordActions[ i ].sentence.words.Count; j++ )
				{
					if ( passwordActions[ i ].useForPartialConfirmation &&
						passwordActions[ i ].sentence.words[ j ].circle == word.circle )
					{
						if ( passwordActions[ i ].sentence.words[ j ].fingers[ 0 ] == word.fingers[ 0 ] &&
							passwordActions[ i ].sentence.words[ j ].fingers[ 1 ] == word.fingers[ 1 ] &&
							passwordActions[ i ].sentence.words[ j ].fingers[ 2 ] == word.fingers[ 2 ] )
						{
							for ( int k = 0; k < subCircles[ word.circle ].fingerSprites.Length; k++ )
								subCircles[ word.circle ].fingerSprites[ k ].color = confirmationColor;
							circlesConfirmed.Add( word.circle );
						}
						else if ( !circlesConfirmed.Contains( word.circle ) )
						{
							for ( int k = 0; k < subCircles[ word.circle ].fingerSprites.Length; k++ )
								subCircles[ word.circle ].fingerSprites[ k ].color = standardColor; ;
						}
					}
				}
			}
		}
		else
		{
			for ( int i = 0; i < sentencesToConfirm.Count; i++ )
			{
				for ( int j = 0; j < sentencesToConfirm[ i ].words.Count; j++ )
				{
					if ( sentencesToConfirm[ i ].words[ j ].circle == word.circle )
					{
						if ( sentencesToConfirm[ i ].words[ j ].fingers[ 0 ] == word.fingers[ 0 ] &&
							sentencesToConfirm[ i ].words[ j ].fingers[ 1 ] == word.fingers[ 1 ] &&
							sentencesToConfirm[ i ].words[ j ].fingers[ 2 ] == word.fingers[ 2 ] )
						{
							for ( int k = 0; k < subCircles[ word.circle ].fingerSprites.Length; k++ )
								subCircles[ word.circle ].fingerSprites[ k ].color = confirmationColor;
							circlesConfirmed.Add( word.circle );
						}
						else if ( !circlesConfirmed.Contains( word.circle ) )
						{
							for ( int k = 0; k < subCircles[ word.circle ].fingerSprites.Length; k++ )
								subCircles[ word.circle ].fingerSprites[ k ].color = standardColor; ;
						}
					}
				}
			}
		}
	}

	public void Deconfirm( Gesture word )
	{
		for ( int i = 0; i < passwordActions.Count; i++ )
		{
			for ( int j = 0; j < passwordActions[ i ].sentence.words.Count; j++ )
			{
				if ( passwordActions[ i ].sentence.words[ j ].circle == word.circle )
				{
					if ( passwordActions[ i ].sentence.words[ j ].fingers[ 0 ] != word.fingers[ 0 ] ||
						passwordActions[ i ].sentence.words[ j ].fingers[ 1 ] != word.fingers[ 1 ] ||
						passwordActions[ i ].sentence.words[ j ].fingers[ 2 ] != word.fingers[ 2 ] )
					{
						for ( int k = 0; k < subCircles[ word.circle ].fingerSprites.Length; k++ )
							subCircles[ word.circle ].fingerSprites[ k ].color = standardColor;
					}
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