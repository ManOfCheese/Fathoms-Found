using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCircle : MonoBehaviour
{

	[Header( "References" )]
	public IntValue handPos;
	public BoolArrayValue fingers;
	public BoolValue showSpritesInCircle;
	public GameObject[] fingerSprites;

	[Header( "Settings" )]
	public bool isCentreCircle;
	public int circleNumber;
	public string clawTag;

	private void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.tag == clawTag )
			handPos.Value = circleNumber;
	}

	private void OnTriggerStay( Collider other )
	{
		if ( other.tag == clawTag )
		{
			if ( showSpritesInCircle.Value )
			{
				fingerSprites[ 0 ].SetActive( true );
				if ( !isCentreCircle )
				{
					for ( int i = 0; i < fingerSprites.Length - 1; i++ )
						fingerSprites[ i + 1 ].SetActive( fingers.Value[ i ] );
				}
			}
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.gameObject.tag == clawTag )
		{
			if ( showSpritesInCircle.Value )
			{
				for ( int i = 0; i < fingerSprites.Length; i++ )
					fingerSprites[ i ].SetActive( false );
			}
			if ( handPos.Value == circleNumber )
				handPos.Value = -1;
		}
	}

}
