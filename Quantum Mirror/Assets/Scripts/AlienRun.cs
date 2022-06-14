using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AlienRun : MonoBehaviour
{

    public BehaviourTreeRunner btRunner;
    public AlienBlackboard blackboard;
	public Transform runTo;
	public DoorResponse doorToClose;
	public float doorCloseDelay;
	public float alienDissappearDelay;

	private bool doorClosed;
	private float triggerTimeStamp;

	private void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Claw" )
		{
			blackboard.AddData( "moveToPosition", runTo.transform.position );
			btRunner.enabled = true;
			triggerTimeStamp = Time.time;
		}
	}

	private void Update()
	{
		if ( triggerTimeStamp != 0f ){
			if ( Time.time - triggerTimeStamp > doorCloseDelay && !doorClosed )
			{
				doorToClose.Close();
				doorClosed = true;
			}
			if ( Time.time - triggerTimeStamp > alienDissappearDelay )
				btRunner.transform.parent.gameObject.SetActive( false );
		}
	}

}
