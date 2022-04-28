using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericSensor : MonoBehaviour
{
    [Header( "References" )]
    public GameObject cam;
    public PropertyInfo[] propertiestoDetect;
    public Text sensorText;
    public float detectionRange;
    public string prefix;
    public string suffix;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if ( Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, detectionRange ) ) {
            if ( hit.transform.GetComponentInChildren<Source>() )
                sensorText.text = prefix + hit.transform.GetComponentInChildren<Source>().sourceOf.propertyName + suffix;
            else if ( hit.transform.GetComponentInChildren<Object>() )
			{
                Object obj = hit.transform.GetComponentInChildren<Object>();
                string propertiesText = "";
				for ( int i = 0; i < obj.currentValues.Count; i++ )
				{
                    propertiesText += obj.currentValues[ i ].property.propertyName + " = " + obj.currentValues[ i ].value + "\n";
				}
                sensorText.text = propertiesText;
			}
            else
                sensorText.text = "Nothing Detected";
        }
    }
}
