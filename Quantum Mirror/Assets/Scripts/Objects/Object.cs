using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{

	public bool wa;

	[Header( "References" )]
	public Properties_Set allProperties;
	public MeshRenderer meshRenderer;
	public GameObject detectorPrefab;
	public GameObject sourcePrefab;
	public List<GameObject> modifierPrefabs;

	[Header( "Settings" )]
	[Tooltip( "The higher and objects hardness the better the aliens can detect movement through it" )]
	public float hardness;
	public AudioClip collisonSound;
	public AnimationCurve defaultCurve;
	public PropertyInfo[] baseValues;
	public SourceInfo[] sourceSettings;

	[HideInInspector] public List<PropertyInfo> currentValues;
    [HideInInspector] public List<Modifier> modifiers = new List<Modifier>();
	[HideInInspector] public List<Detector> detectors;
	[HideInInspector] public List<Source> sources;

	private bool updateProperties;
	private bool updateModifiers;
	private Dictionary<Detector, PropertyInfo> propertyInfoByDetector;
	private Dictionary<Source, PropertyInfo> propertyInfoBySource;
	private List<Property> sourceProperties;
	private GameObject detectorContainer;
	private GameObject sourceContainer;
	private GameObject modifierContainer;

	private void Awake()
	{
		detectorContainer = transform.Find( "Detectors" ).gameObject;
		sourceContainer = transform.Find( "Sources" ).gameObject;
		modifierContainer = transform.Find( "Modifiers" ).gameObject;

		//Initialize property info.
		currentValues = new List<PropertyInfo>();
		for ( int i = 0; i < allProperties.Items.Count; i++ )
			currentValues.Add( new PropertyInfo( allProperties.Items[ i ], 0f ) );

		//Initialize detectors and sources.
		detectors = new List<Detector>();
		sources = new List<Source>();
		int index = -1;
		for ( int i = 0; i < allProperties.Items.Count; i++ )
		{
			if ( !allProperties.Items[ i ].isInherent )
			{
				index++;
				detectors.Add( Instantiate( detectorPrefab, detectorContainer.transform ).GetComponent<Detector>() );
				detectors[ index ].propertyToDetect = allProperties.Items[ i ];
			}
		}

		for ( int i = 0; i < sourceSettings.Length; i++ )
		{
			sources.Add( Instantiate( sourcePrefab, sourceContainer.transform ).GetComponent<Source>() );
			sources[ i ].sourceOf = sourceSettings[ i ].property;
			sources[ i ].valueAtCentre = sourceSettings[ i ].valueAtCentre;
			sources[ i ].fallOff = sourceSettings[ i ].fallOff;
			SphereCollider sphereCollider = sources[ i ].gameObject.AddComponent<SphereCollider>();
			sources[ i ].sphereCollider = sphereCollider;
			sphereCollider.isTrigger = true;
			sphereCollider.radius = sourceSettings[ i ].radius;
		}

		//Initialize Dictionaries
		propertyInfoByDetector = new Dictionary<Detector, PropertyInfo>();
		propertyInfoBySource = new Dictionary<Source, PropertyInfo>();
		for ( int i = 0; i < currentValues.Count; i++ )
		{
			if ( !currentValues[ i ].property.isInherent )
			{
				for ( int j = 0; j < detectors.Count; j++ )
				{
					if ( detectors[ j ].propertyToDetect.propertyName == currentValues[ i ].property.propertyName )
						propertyInfoByDetector.Add( detectors[ j ], currentValues[ i ] );
				}
				for ( int j = 0; j < sources.Count; j++ )
				{
					if ( sources[ j ].sourceOf.propertyName == currentValues[ i ].property.propertyName )
						propertyInfoBySource.Add( sources[ j ], currentValues[ i ] );
				}
			}
		}

		//Get modifiers references.
		Modifier[] mods = modifierContainer.GetComponentsInChildren<Modifier>();
		for ( int i = 0; i < mods.Length; i++ )
		{
			modifiers.Add( mods[ i ] );
			modifiers[ i ].OnStart( this );
		}
	}

	private void Update()
	{
		if ( updateModifiers )
		{
			for ( int i = 0; i < modifiers.Count; i++ )
				modifiers[ i ].UpdateProperty();
		}

		//Update values.
		if ( updateProperties )
		{
			for ( int i = 0; i < detectors.Count; i++ )
				propertyInfoByDetector[ detectors[ i ] ].value = detectors[ i ].propertyValue;
			for ( int i = 0; i < sources.Count; i++ )
				propertyInfoBySource[ sources[ i ] ].value += sources[ i ].valueAtCentre;
		}
	}

	public void Seal()
	{
		for ( int i = 0; i < sources.Count; i++ )
			sources[ i ].gameObject.SetActive( false );
		for ( int i = 0; i < detectors.Count; i++ )
			detectors[ i ].enabled = false;
		updateModifiers = false;
		updateProperties = false;
	}

	public void Unseal()
	{
		for ( int i = 0; i < sources.Count; i++ )
			sources[ i ].gameObject.SetActive( true );
		for ( int i = 0; i < detectors.Count; i++ )
			detectors[ i ].enabled = true;
		updateModifiers = true;
		updateProperties = true;
	}

	private void OnValidate()
	{
		if ( allProperties == null ) { return; }

		if ( baseValues == null )
		{
			baseValues = new PropertyInfo[ allProperties.Items.Count ];
			for ( int i = 0; i < baseValues.Length; i++ )
				baseValues[ i ] = new PropertyInfo( allProperties.Items[ i ] );
		}
		else
		{
			PropertyInfo[] newValues = new PropertyInfo[ allProperties.Items.Count ];
			for ( int i = 0; i < newValues.Length; i++ )
				newValues[ i ] = new PropertyInfo( allProperties.Items[ i ] );

			for ( int i = 0; i < newValues.Length; i++ )
			{
				for ( int j = 0; j < baseValues.Length; j++ )
				{
					if ( newValues[ i ].property.propertyName == baseValues[ j ].property.propertyName )
						newValues[ i ].value = baseValues[ j ].value;
				}
			}
			baseValues = newValues;
		}

		sourceProperties = new List<Property>();
		sourceProperties.Clear();
		for ( int i = 0; i < baseValues.Length; i++ )
		{
			if ( !baseValues[ i ].property.isInherent && baseValues[ i ].value > 0 )
				sourceProperties.Add( allProperties.Items[ i ] );
		}

		if ( sourceSettings.Length != sourceProperties.Count )
		{
			sourceSettings = new SourceInfo[ sourceProperties.Count ];
			for ( int i = 0; i < sourceSettings.Length; i++ )
			{
				sourceSettings[ i ] = new SourceInfo( sourceProperties[ i ] );
				sourceSettings[ i ].property = sourceProperties[ i ];
				sourceSettings[ i ].fallOff = defaultCurve;
			}
		}
		for ( int i = 0; i < sourceSettings.Length; i++ )
		{
			for ( int j = 0; j < baseValues.Length; j++ )
			{
				if ( baseValues[ j ].property.propertyName == sourceProperties[ i ].propertyName )
					sourceSettings[ i ].valueAtCentre = baseValues[ j ].value;
			}
		}
	}

}

[System.Serializable]
public class PropertyInfo
{
	public PropertyInfo( Property _property )
	{
		property = _property;
	}

	public PropertyInfo( Property _property, float _value )
	{
		property = _property;
		value = _value;
	}
	
	[ReadOnly] public Property property;
	public float value;
}

[System.Serializable]
public class SourceInfo
{
	public SourceInfo( Property _property )
	{
		property = _property;
	}

	[ReadOnly] public Property property;
	[ReadOnly] public float valueAtCentre;
	public AnimationCurve fallOff;
	public float radius;
}
