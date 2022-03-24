using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenProperty : ObjectProperty
{

	[Header( "References" )]
	public OxygenDetector oxygenDetector;
	public List<ObjModifier> modifiers;

	[Header( "Settings" )]
	public float oxydizationPoint;
	public float deoxydizationPoint;
	public float oxydizationDuration;
	public float deoxydizationDuration;
	public float crossFadeDuration;

	[Header( "Sounds" )]
	public Fader fader;
	public AudioSource passiveSource;
	public AudioSource oxydizingSource;
	public AudioSource deoxydizingSource;

	private AudioSource currentSource;
	private float t;
	private bool oxydizing;

	private void Start()
	{
		currentSource = passiveSource;
	}

	private void Update()
	{
		if ( oxygenDetector.oxygenLevels > oxydizationPoint )
		{
			if ( !oxydizing )
			{
				oxydizing = true;
				fader.Crossfade( currentSource, oxydizingSource, 1f, 0f, crossFadeDuration );
				currentSource = oxydizingSource;
			}
			t += Time.deltaTime / oxydizationDuration;
		}
		else if ( oxygenDetector.oxygenLevels < deoxydizationPoint )
		{
			if ( oxydizing )
			{
				oxydizing = false;
				fader.Crossfade( currentSource, deoxydizingSource, 1f, 0f, crossFadeDuration );
				currentSource = deoxydizingSource;
			}
			t -= Time.deltaTime / deoxydizationDuration;
		}
		else
		{
			fader.Crossfade( currentSource, passiveSource, 1f, 0f, crossFadeDuration );
			currentSource = passiveSource;
		}
		Mathf.Clamp01( t );

		for ( int i = 0; i < modifiers.Count; i++ )
		{
			modifiers[ i ].ModifyObject( t );
		}
	}
}
