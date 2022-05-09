using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Graph : MonoBehaviour
{

	[Header( "Settings" )]
	public Text propertyText;
	public RectTransform graphContainer;
	public GameObject graphPoint;
	public GameObject graphLine;
    public FloatValue[] properties;
    public int pointCount;
    public float measureInterval;
	public float minValue;
	public float maxValue;
	public float scrollTolerance;

	private int propertyIndex;
	private float timeStamp;
	private float pointDistance;
    private float[] valuesOverTime;
	private RectTransform[] points;
	private RectTransform[] lines;
	private RectTransform graphPointRT;

	private void Awake()
	{
		propertyIndex = 0;
		propertyText.text = properties[ propertyIndex ].displayName;
		timeStamp = Time.time;
		graphPointRT = graphPoint.GetComponent<RectTransform>();

		valuesOverTime = new float[ pointCount ];
		points = new RectTransform[ pointCount ];
		lines = new RectTransform[ pointCount - 1 ];
		pointDistance = ( graphContainer.sizeDelta.x - graphPointRT.sizeDelta.x ) / ( pointCount - 1f );

		for ( int i = 0; i < pointCount; i++ )
		{
			points[ i ] = Instantiate( graphPoint, graphContainer.transform ).GetComponent<RectTransform>();
			points[ i ].anchoredPosition = new Vector3( ( graphPointRT.sizeDelta.x / 2f ) + ( i * pointDistance ), 0f, 0f );
		}
		for ( int i = 0; i < lines.Length; i++ )
		{
			lines[ i ] = Instantiate( graphLine, graphContainer.transform ).GetComponent<RectTransform>();
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
					valuesOverTime[ i ] = Sam.Math.Map( Mathf.Clamp( properties[ propertyIndex ].Value, minValue, maxValue ), 
						minValue, maxValue, 0f, graphContainer.sizeDelta.y );
			}
			for ( int i = 0; i < points.Length; i++ )
				points[ i ].anchoredPosition = new Vector2( points[ i ].anchoredPosition.x, valuesOverTime[ i ] );
			for ( int i = 0; i < lines.Length; i++ )
				lines[ i ] = UpdateLinePos( lines[ i ], points[ i ], points[ i + 1 ] );
			timeStamp = Time.time;
		}
	}

	public void CycleProperty( InputAction.CallbackContext value )
	{
		int oldIndex = propertyIndex;

		if ( value.ReadValue<float>() > scrollTolerance )
		{
			propertyIndex++;
			propertyIndex = Mathf.Clamp( propertyIndex, 0, properties.Length - 1 );
		}
		else if ( value.ReadValue<float>() < -scrollTolerance )
		{
			propertyIndex--;
			propertyIndex = Mathf.Clamp( propertyIndex, 0, properties.Length - 1 );
		}

		if ( oldIndex != propertyIndex )
		{
			for ( int i = 0; i < valuesOverTime.Length; i++ )
				valuesOverTime[ i ] = 0f;
			for ( int i = 0; i < points.Length; i++ )
				points[ i ].anchoredPosition = new Vector3( ( graphPointRT.sizeDelta.x / 2f ) + ( i * pointDistance ), 0f, 0f );
			for ( int i = 0; i < lines.Length; i++ )
				lines[ i ] = UpdateLinePos( lines[ i ], points[ i ], points[ i + 1 ] );
			propertyText.text = properties[ propertyIndex ].displayName;
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

	private void OnValidate()
	{
		if ( pointCount < 2 )
			pointCount = 2;

		if ( points != null && lines != null && graphContainer != null && graphPointRT != null )
		{
			for ( int i = 0; i < points.Length; i++ )
				Destroy( points[ i ].gameObject );
			for ( int i = 0; i < lines.Length; i++ )
				Destroy( lines[ i ].gameObject );

			valuesOverTime = null;
			valuesOverTime = new float[ pointCount ];

			points = null;
			points = new RectTransform[ pointCount ];

			lines = null;
			lines = new RectTransform[ pointCount - 1 ];

			pointDistance = ( graphContainer.sizeDelta.x - graphPointRT.sizeDelta.x ) / ( pointCount - 1f );

			for ( int i = 0; i < pointCount; i++ )
			{
				points[ i ] = null;
				points[ i ] = Instantiate( graphPoint, graphContainer.transform ).GetComponent<RectTransform>();
				points[ i ].anchoredPosition = new Vector3( ( graphPointRT.sizeDelta.x / 2f ) + ( i * pointDistance ), 0f, 0f );
			}
			for ( int i = 0; i < lines.Length; i++ )
			{
				lines[ i ] = null;
				lines[ i ] = Instantiate( graphLine, graphContainer.transform ).GetComponent<RectTransform>();
				lines[ i ] = UpdateLinePos( lines[ i ], points[ i ], points[ i + 1 ] );
			}
		}
	}
}
