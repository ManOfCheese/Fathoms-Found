using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureBroadcaster : MonoBehaviour
{

	public GestureCircle gestureCircle;
	public List<AlienManager> aliens;

	private void OnTriggerEnter( Collider other )
	{
		if ( other.GetComponent<AlienManager>() )
		{
			AlienManager alien = other.GetComponent<AlienManager>();
			gestureCircle.onWord += alien.OnWord;
			gestureCircle.onSentence += alien.OnSentence;
			aliens.Add( alien );
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.GetComponent<AlienManager>() )
		{
			AlienManager alien = other.GetComponent<AlienManager>();
			gestureCircle.onWord += alien.OnWord;
			gestureCircle.onSentence += alien.OnSentence;
			aliens.Remove( alien );
		}
	}

}
