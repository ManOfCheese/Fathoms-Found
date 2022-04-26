using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{

	public RectTransform grapContainer;
	public GameObject graphPoint;
	public GameObject graphLine;
    public FloatValue data;
    public int pointCount;
    public float measureInterval;
	public float minValue;
	public float maxValue;

	private float timeStamp;
	private float pointDistance;
    private float[] valuesOverTime;
	private RectTransform[] points;
	private RectTransform[] lines;
	private RectTransform graphPointRT;

	private void Awake()
	{
		timeStamp = Time.time;
		valuesOverTime = new float[ pointCount ];
		points = new RectTransform[ pointCount ];
		lines = new RectTransform[ pointCount - 1 ];

		graphPointRT = graphPoint.GetComponent<RectTransform>();
		pointDistance = ( grapContainer.sizeDelta.x - graphPointRT.sizeDelta.x ) / ( pointCount - 1f );

		for ( int i = 0; i < pointCount; i++ )
		{
			points[ i ] = Instantiate( graphPoint, grapContainer.transform ).GetComponent<RectTransform>();
			points[ i ].anchoredPosition = new Vector3( ( graphPointRT.sizeDelta.x / 2f ) + ( i * pointDistance ), 0f, 0f );
		}
		for ( int i = 0; i < lines.Length; i++ )
		{
			lines[ i ] = Instantiate( graphLine, grapContainer.transform ).GetComponent<RectTransform>();
			lines[ i ] = UpdateLinePos( lines[ i ], points[ i ], points[ i + 1 ] );
		}
	}

	private void Update()
	{
		if ( Time.time - timeStamp > measureInterval )
		{
			for ( int i = 0; i < valuesOverTime.Length; i++ )
			{
				if ( i != valuesOverTime.Length - 1 )
					valuesOverTime[ i ] = valuesOverTime[ i + 1 ];
				else
					valuesOverTime[ i ] = Sam.Math.Map( Mathf.Clamp( data.Value, minValue, maxValue ), minValue, maxValue, 0f, grapContainer.sizeDelta.y );
			}
			for ( int i = 0; i < points.Length; i++ )
				points[ i ].anchoredPosition = new Vector2( points[ i ].anchoredPosition.x, valuesOverTime[ i ] );
			for ( int i = 0; i < lines.Length; i++ )
				lines[ i ] = UpdateLinePos( lines[ i ], points[ i ], points[ i + 1 ] );
			timeStamp = Time.time;
		}
	}

	private RectTransform UpdateLinePos( RectTransform line, RectTransform point, RectTransform nextPoint )
	{
		line.anchoredPosition = point.anchoredPosition;
		float a = Mathf.Abs( point.anchoredPosition.x - nextPoint.anchoredPosition.x );
		float b = point.anchoredPosition.y - nextPoint.anchoredPosition.y;
		float c = Mathf.Sqrt( ( a * a ) + ( b * b ) );
		line.sizeDelta = new Vector2( line.sizeDelta.x, c );
		float angle = Mathf.Rad2Deg * Mathf.Atan2( a, b );
		line.rotation = Quaternion.Euler( 0f, 0f, angle );
		return line;
	}
}
