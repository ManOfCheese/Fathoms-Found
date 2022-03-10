using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGestureController : MonoBehaviour
{

	[Header( "References" )]
	public BoolValue confirmGesture;
	public BoolValue isInGestureMode;
	public IntValue handPos;
	public BoolArrayValue fingers;

	public UI_Text_Changer textChanger;
    public AlienIKHandler[] hands;
	public GameObject[] gestureCircles;
	public float gCircleDiameter;

	public Vector2[] gesturePoints;
	public float gestureSpeed = 1f;
	public float holdPosFor = 1f;

	[HideInInspector] public AlienManager alienManager;
	[HideInInspector] public int handIndex;
	[HideInInspector] public int gestureIndex = 0;
	[HideInInspector] public float waitTimeStamp;
	[HideInInspector] public bool gesture;
	[HideInInspector] public bool startGesture;
	[HideInInspector] public bool waiting = false;
	[HideInInspector] public Vector3 handTarget;

	private void Update()
	{
		if ( confirmGesture.Value ) {
			gesture = false;
			hands[ handIndex ].enabled = true;
			gestureCircles[ handIndex ].SetActive( false );

			int closestHand = FindClosestHand( alienManager.player );

			hands[ closestHand ].enabled = false;
			hands[ closestHand ].gameObject.transform.position = gestureCircles[ closestHand ].transform.position;
			handIndex = closestHand;
			gesture = true;
			gestureIndex = 0;
			startGesture = false;
			waiting = false;
			//textChanger.OnAlienInteract();
		}
	}

	public int FindClosestHand( Transform respondTo )
	{
		int closestHand = 0;
		float shortestDist = 0f;

		for ( int i = 0; i < gestureCircles.Length; i++ )
		{
			float dist = Vector3.Distance( respondTo.position, gestureCircles[ i ].transform.position );
			if ( dist < shortestDist || i == 0 )
			{
				closestHand = i;
				shortestDist = dist;
			}
		}
		return closestHand;
	}

}
