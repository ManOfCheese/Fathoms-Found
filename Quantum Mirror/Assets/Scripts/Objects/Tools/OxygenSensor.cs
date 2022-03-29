using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenSensor : Detector
{
    
    [Header( "References" )]
    public Text oxygenText;
    public string prefix;
    public string suffix;

    // Update is called once per frame
    void Update()
    {
        oxygenText.text = prefix + Mathf.Round( propertyValue ).ToString() + suffix;
    }
}
