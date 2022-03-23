using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSpinner : MonoBehaviour
{
    public float rotationSpeed;

    private OxygenDetector oxygenDetector;

    void Start()
    {
        oxygenDetector = GetComponent<OxygenDetector>();
    }

    void Update()
    {
        rotationSpeed = oxygenDetector.oxygenLevels;
        this.transform.Rotate( 0f, 0f, rotationSpeed / 20f, Space.Self );
    }
}
