using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{

	public AudioManager audioManager;
	public int audioIndex;

	public void OnTriggerEnter( Collider other )
	{
		if ( other.isTrigger == false )
			audioManager.SwapMusic( audioIndex );
	}

}
