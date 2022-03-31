using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThresholdLogic
{
	MoreThan,
	LessThan
}

public class ObjModifier : MonoBehaviour
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

	protected AudioSource[] audioInfos;
	protected Detector detector;
	protected AudioSource currentSource;
	protected float t;
	protected bool thresholdCrossed;

	[HideInInspector] public bool canUseSpeed;

	[HideInInspector] protected Fader fader;
	[HideInInspector] protected AudioInfo[] sources;
		 
	private void Awake()
	{
		fader = GetComponent<Fader>();
		audioInfos = GetComponents<AudioSource>();
		sources = new AudioInfo[ 3 ] { passiveSource, changeSource, thresholdSource };

		for ( int i = 0; i < Mathf.Min( sources.Length, audioInfos.Length ); i++ )
		{
			sources[ i ].source = audioInfos[ i ];
			audioInfos[ i ].clip = sources[ i ].clip;
			audioInfos[ i ].volume = sources[ i ].startVolume;
			audioInfos[ i ].loop = sources[ i ].loop;
		}
	}

	public virtual void OnStart( Object obj )
	{
		for ( int i = 0; i < obj.detectors.Length; i++ )
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

	}

	public virtual void WhileThresholdNotCrossed()
	{

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