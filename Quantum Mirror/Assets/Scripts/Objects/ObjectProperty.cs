using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChangeMode
{
	Speed,
	Duration
}

public class ObjectProperty : MonoBehaviour
{

    public Property property;

    public Detector detector;
    public List<ObjModifier> lowValueModifiers = new List<ObjModifier>();
    public List<ObjModifier> highValueModifiers = new List<ObjModifier>();
	public float startValue;
    public float changeIfUnderThisValue;
    public float changeIfOverThisValue;
	public ChangeMode changeMode;
    public float changeSpeed;
    public float changeDuration;

	[Header( "Sounds" )]
	public float crossFadeDuration;
    public Fader fader;
    public AudioSource passiveSource;
    public AudioSource lowValueChangeSource;
    public AudioSource highValueChangeSource;

    private AudioSource currentSource;
    private float t;
    private bool valueIsOver;

	public void StartSample()
	{
		for ( int i = 0; i < highValueModifiers.Count; i++ )
		{
			if ( highValueModifiers[ i ].canUseSpeed || changeMode == ChangeMode.Duration )
			{
				highValueModifiers[ i ].ModifyObjectPerc( t );
			}
		}

		for ( int i = 0; i < lowValueModifiers.Count; i++ )
		{
			if ( lowValueModifiers[ i ].canUseSpeed || changeMode == ChangeMode.Duration )
			{
				lowValueModifiers[ i ].ModifyObjectPerc( t );
			}
		}
	}

    public void UpdateSample()
	{
		if ( detector.propertyValue > changeIfOverThisValue )
		{
			if ( !valueIsOver )
			{
				valueIsOver = true;
				fader.Crossfade( currentSource, highValueChangeSource, 1f, 0f, crossFadeDuration );
				currentSource = highValueChangeSource;
			}

			for ( int i = 0; i < highValueModifiers.Count; i++ )
			{
				if ( highValueModifiers[ i ].canUseSpeed )
				{
					if ( changeMode == ChangeMode.Duration )
					{
						t += Time.deltaTime / changeDuration;
						Mathf.Clamp01( t );
						highValueModifiers[ i ].ModifyObjectPerc( t );
					}
					else if ( changeMode == ChangeMode.Speed )
					{
						t += Time.deltaTime * changeSpeed;
						highValueModifiers[ i ].ModifyObjectStep( t );
					}
				}
				else
				{
					t += Time.deltaTime / changeDuration;
					Mathf.Clamp01( t );
					highValueModifiers[ i ].ModifyObjectPerc( t );
				}
			}
		}
		else if ( detector.propertyValue < changeIfUnderThisValue )
		{
			if ( valueIsOver )
			{
				valueIsOver = false;
				fader.Crossfade( currentSource, lowValueChangeSource, 1f, 0f, crossFadeDuration );
				currentSource = lowValueChangeSource;
			}

			for ( int i = 0; i < lowValueModifiers.Count; i++ )
			{
				if ( lowValueModifiers[ i ].canUseSpeed )
				{
					if ( changeMode == ChangeMode.Duration )
					{
						t += Time.deltaTime / changeDuration;
						Mathf.Clamp01( t );
						lowValueModifiers[ i ].ModifyObjectPerc( t );
					}
					else if ( changeMode == ChangeMode.Speed )
					{
						t += Time.deltaTime * changeSpeed;
						lowValueModifiers[ i ].ModifyObjectStep( t );
					}
				}
				else
				{
					t += Time.deltaTime / changeDuration;
					Mathf.Clamp01( t );
					lowValueModifiers[ i ].ModifyObjectPerc( t );
				}
			}
		}
		else
		{
			fader.Crossfade( currentSource, passiveSource, 1f, 0f, crossFadeDuration );
			currentSource = passiveSource;
		}
	}

}
