using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	[Header( "References" )]
	public Fader fader;
	public AudioSource[] audioSources;

	[Header( "Settings" )]
	public float fadeDuration;
	[Range( 0f, 1f )]
	public float volume;

	private int currentAudioIndex;

	private void Awake()
	{
		audioSources = GetComponents<AudioSource>();
		if ( audioSources.Length > 0 )
			audioSources[ 0 ].Play();
		currentAudioIndex = 0;
	}

	public void SwapMusic( int audioIndex )
	{
		if ( audioIndex != currentAudioIndex )
		{
			fader.Crossfade( audioSources[ currentAudioIndex ], audioSources[ audioIndex ], volume, 0f, fadeDuration, false );
			currentAudioIndex = audioIndex;
		}
	}
}
