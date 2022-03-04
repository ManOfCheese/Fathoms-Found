using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGestureController : MonoBehaviour
{

	public UI_Text_Changer textChanger;
    public AlienIKHandler[] alienHands;
	public GameObject[] gestureCircles;
	public float gestureCircleDiameter;

	public Vector2[] gesturePoints;
	public float gestureSpeed = 1f;
	public float holdPositionDuration = 1f;

	private int handIndex;
	private int gestureIndex = 0;
	private float waitTimeStamp;
	private bool gesture;
	private bool startGesture;
	private bool waiting = false;
	private Vector3 handTarget;

	private void Update()
	{
		if ( gesture )
		{
			if ( !startGesture )
			{
				gestureCircles[ handIndex ].SetActive( true );
				Vector3 left = gestureCircles[ handIndex ].transform.right * gesturePoints[ gestureIndex ].x;
				Vector3 up = gestureCircles[ handIndex ].transform.up * gesturePoints[ gestureIndex ].y;
				handTarget = gestureCircles[ handIndex ].transform.position + ( ( left + up ) * gestureCircleDiameter );
				startGesture = true;
			}

			if ( waiting )
			{
				if ( Time.time - waitTimeStamp > holdPositionDuration )
				{
					waiting = false;
				}
			}
			else
			{
				float magnitude = gestureSpeed * Time.deltaTime;
				float dist = Vector3.Distance( alienHands[ handIndex ].transform.position, handTarget );
				if ( dist < magnitude )
				{
					alienHands[ handIndex ].transform.position += ( handTarget - alienHands[ handIndex ].transform.position ).normalized * dist;
					waitTimeStamp = Time.time;
					waiting = true;

					gestureIndex++;
					if ( gestureIndex >= gesturePoints.Length )
					{
						gesture = false;
						alienHands[ handIndex ].enabled = true;
						gestureCircles[ handIndex ].SetActive( false );
					}
					else
					{
						handTarget = gestureCircles[ handIndex ].transform.position + new Vector3( gesturePoints[ gestureIndex ].x * gestureCircleDiameter, 0f,
							gesturePoints[ gestureIndex ].y * gestureCircleDiameter );
					}
				}
				else
				{
					alienHands[ handIndex ].transform.position += ( handTarget - alienHands[ handIndex ].transform.position ).normalized * magnitude;
				}
			}
		}
	}

	public void OnGesture( Transform respondTo )
	{
		gesture = false;
		alienHands[ handIndex ].enabled = true;
		gestureCircles[ handIndex ].SetActive( false );

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

		alienHands[ closestHand ].enabled = false;
		alienHands[ closestHand ].gameObject.transform.position = gestureCircles[ closestHand ].transform.position;
		handIndex = closestHand;
		gesture = true;
		gestureIndex = 0;
		startGesture = false;
		waiting = false;
		textChanger.OnAlienInteract();
	}

}
