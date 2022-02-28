using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour {

    public EventLinker[] events;

	private void Update()
	{
		for ( int i = 0; i < events.Length; i++ )
		{
			events[ i ].
		}
	}

}

[System.Serializable]
public class EventLinker
{
    public GameEvent gameEvent;
    public UnityEvent unityEvent;
}
