using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThresholdLogic
{
	MoreThan,
	LessThan
}

public class Modifier : MonoBehaviour
{

	[Header( "Generic - References" )]
	public Property property;

	[Header( "Generic - Settings" )]
	public ThresholdLogic thresholdLogic;
	public float threshold;
	[Space( 10 )]
	public ChangeMode changeMode;
	public float changeSpeed;
	public float changeDuration;

	[Header( "Sounds" )]
	public float fadeDuration;
	public float crossFadeDuration;
	public AudioInfo passiveSource;
	public AudioInfo changeSource;
	public AudioInfo thresholdSource;

	protected AudioSource[] audioSources;
	protected Detector detector;
	protected AudioSource currentSource;
	protected float t;
	protected bool thresholdCrossed;

	[HideInInspector] public bool canUseSpeed;

	protected Object obj;
	protected Fader fader;
	protected AudioInfo[] audioInfos;
		 
	private void Awake()
	{
		obj = GetComponentInParent<Object>();
		fader = GetComponent<Fader>();
		if ( fader == null )
			fader = gameObject.AddComponent<Fader>();

		audioSources = GetComponents<AudioSource>();
		while ( audioSources.Length < 3 )
		{
			gameObject.AddComponent<AudioSource>();
			audioSources = GetComponents<AudioSource>();
		}

		for ( int i = 0; i < audioSources.Length; i++ )
			audioSources[ i ].playOnAwake = false;

		audioInfos = new AudioInfo[ 3 ] { passiveSource, changeSource, thresholdSource };

		for ( int i = 0; i < Mathf.Min( audioInfos.Length, audioSources.Length ); i++ )
		{
			audioInfos[ i ].source = audioSources[ i ];
			audioSources[ i ].clip = audioInfos[ i ].clip;
			audioSources[ i ].volume = audioInfos[ i ].startVolume;
			audioSources[ i ].loop = audioInfos[ i ].loop;
		}
	}

	public virtual void OnStart( Object obj )
	{
		for ( int i = 0; i < obj.detectors.Count; i++ )
		{
			if ( obj.detectors[ i ].propertyToDetect.propertyName == property.propertyName )
				detector = obj.detectors[ i ];
		}
	}

	public virtual void UpdateProperty()
	{
		if ( thresholdLogic == ThresholdLogic.LessThan )
		{
			if ( detector.propertyValue < threshold )
				WhileThresholdCrossed();
			else
				WhileThresholdNotCrossed();
		}
		else if ( thresholdLogic == ThresholdLogic.MoreThan )
		{
			if ( detector.propertyValue > threshold )
				WhileThresholdCrossed();
			else
				WhileThresholdNotCrossed();
		}
	}

	public virtual void WhileThresholdCrossed()
	{
		if ( !thresholdCrossed )
		{
			thresholdCrossed = true;
			OnThresholdCross();
		}
	}

	public virtual void WhileThresholdNotCrossed()
	{
		if ( thresholdCrossed )
		{
			thresholdCrossed = false;
			OnThresholdUncross();
		}
	}

	public virtual void OnThresholdCross()
	{
		fader.Crossfade( passiveSource.source, changeSource.source, passiveSource.startVolume, 0f, crossFadeDuration );

		if ( thresholdSource != null ) { if ( thresholdSource.clip != null ) thresholdSource.source.Play(); }
	}

	public virtual void OnThresholdUncross()
	{
		fader.Crossfade( changeSource.source, passiveSource.source, changeSource.startVolume, 0f, crossFadeDuration );

		if ( thresholdSource.source != null ) { if ( thresholdSource.clip != null ) thresholdSource.source.Stop(); }
	}

    public virtual void ModifyObjectPerc( float t )
	{

	}

	public virtual void ModifyObjectStep( float t )
	{

	}

}

[System.Serializable]
public class AudioInfo
{
	public AudioClip clip;
	[Range( 0f, 1f )]
	public float startVolume;
	public bool loop;

	[HideInInspector] public AudioSource source;
}