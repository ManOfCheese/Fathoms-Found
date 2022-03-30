using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSpinner : MonoBehaviour
{
    public float rotationSpeed;

    private Detector oxygenDetector;

    void Start()
    {
        oxygenDetector = GetComponent<Detector>();
    }

    void Update()
    {
        rotationSpeed = oxygenDetector.propertyValue;
        this.transform.Rotate( 0f, 0f, rotationSpeed / 20f, Space.Self );
    }
}
