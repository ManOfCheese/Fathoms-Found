using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
	public void Fade( AudioSource source, float startVolume, float endVolume, float duration, float delay, bool fadePitch )
	{
		StartCoroutine( FadeCoroutine( source, startVolume, endVolume, duration, delay, fadePitch ) );
	}

	public void Fade( AudioSource source, float startVolume, float endVolume, float duration, bool fadePitch )
	{
		StartCoroutine( FadeCoroutine( source, startVolume, endVolume, duration, 0, fadePitch ) );
	}

	public void Crossfade( AudioSource sourceOff, AudioSource sourceOn, float startVolume, float endVolume, float duration, float delay, bool fadePitch )
	{
		StartCoroutine( FadeCoroutine( sourceOff, startVolume, endVolume, duration, delay, fadePitch ) );
		StartCoroutine( FadeCoroutine( sourceOn, endVolume, startVolume, duration, delay, fadePitch ) );
	}

	public void Crossfade( AudioSource sourceOff, AudioSource sourceOn, float startVolume, float endVolume, float duration, bool fadePitch )
	{
		StartCoroutine( FadeCoroutine( sourceOff, startVolume, endVolume, duration, 0, fadePitch ) );
		StartCoroutine( FadeCoroutine( sourceOn, endVolume, startVolume, duration, 0, fadePitch ) );
	}

	public IEnumerator FadeCoroutine( AudioSource source, float startValue, float endValue, float duration, float delay, bool fadePitch )
	{
		yield return new WaitForSeconds( delay );
		if ( source == null ) 
			yield return null;
		else if ( source.clip == null ) 
			yield return null;
		else
		{
			if ( !source.isPlaying )
				source.Play();
			float startTimeStamp = Time.time;
			if ( fadePitch )
				source.pitch = startValue;
			else
				source.volume = startValue;

			while ( true )
			{
				float elapsedTime = Time.time - startTimeStamp;
				if ( fadePitch )
					source.pitch = Mathf.Lerp( startValue, endValue, elapsedTime / duration );
				else
					source.volume = Mathf.Lerp( startValue, endValue, elapsedTime / duration );

				if ( elapsedTime >= duration )
				{
					if ( fadePitch )
						source.pitch = endValue;
					else
						source.volume = endValue;
					if ( endValue == 0 )
						source.Stop();
					break;
				}
				yield return null;
			}
		}
	}
}
