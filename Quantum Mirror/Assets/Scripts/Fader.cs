using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
	public void Fade( AudioSource source, float startVolume, float endVolume, float duration, float delay )
	{
		StartCoroutine( FadeCoroutine( source, startVolume, endVolume, duration, delay ) );
	}

	public void Fade( AudioSource source, float startVolume, float endVolume, float duration )
	{
		StartCoroutine( FadeCoroutine( source, startVolume, endVolume, duration, 0 ) );
	}

	public void Crossfade( AudioSource sourceOff, AudioSource sourceOn, float startVolume, float endVolume, float duration, float delay )
	{
		StartCoroutine( FadeCoroutine( sourceOff, startVolume, endVolume, duration, delay ) );
		StartCoroutine( FadeCoroutine( sourceOn, endVolume, startVolume, duration, delay ) );
	}

	public void Crossfade( AudioSource sourceOff, AudioSource sourceOn, float startVolume, float endVolume, float duration )
	{
		StartCoroutine( FadeCoroutine( sourceOff, startVolume, endVolume, duration, 0 ) );
		StartCoroutine( FadeCoroutine( sourceOn, endVolume, startVolume, duration, 0 ) );
	}

	public IEnumerator FadeCoroutine( AudioSource source, float startVolume, float endVolume, float duration, float delay )
	{
		yield return new WaitForSeconds( delay );
		if ( !source.isPlaying )
			source.Play();
		float startTimeStamp = Time.time;
		source.volume = startVolume;

		while ( true )
		{
			float elapsedTime = Time.time - startTimeStamp;
			source.volume = Mathf.Lerp( startVolume, endVolume, elapsedTime / duration );
			Debug.Log( source.volume );

			if ( elapsedTime >= duration )
			{
				source.volume = endVolume;
				if ( endVolume == 0 )
					source.Stop();
				break;
			}
			yield return null;
		}
	}
}
