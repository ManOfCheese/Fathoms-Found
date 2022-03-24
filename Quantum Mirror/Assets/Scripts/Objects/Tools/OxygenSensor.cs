using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenSensor : OxygenDetector
{
    
    [Header( "References" )]
    public Text oxygenText;
    public string prefix;
    public string suffix;

    // Update is called once per frame
    void Update()
    {
        oxygenText.text = prefix + Mathf.Round( oxygenLevels ).ToString() + suffix;
    }
}
