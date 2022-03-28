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
	public float crossFadeDuration;
	public Fader fader;
	public AudioSource passiveSource;
	public AudioSource lowValueChangeSource;
	public AudioSource highValueChangeSource;

	protected Detector detector;
	protected AudioSource currentSource;
	protected float t;
	protected bool thresholdCrossed;

	[HideInInspector] public bool canUseSpeed;

	public virtual void OnStart( Object obj )
	{
		for ( int i = 0; i < obj.detectors.Length; i++ )
		{
			if ( obj.detectors[ i ].propertyToDetect.propertyName == property.propertyName )
				detector = obj.detectors[ i ];
		}
	}

	public virtual void OnThresholdCross()
	{

	}

	public virtual void UpdateProperty()
	{

	}

    public virtual void ModifyObjectPerc( float t )
	{

	}

	public virtual void ModifyObjectStep( float t )
	{

	}

}
