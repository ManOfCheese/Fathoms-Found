using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSpinner : MonoBehaviour
{
    public GameObject theDetector;
    public float rotationSpeed;
    private OxygenDetector oxygenDetector;


    // Start is called before the first frame update
    void Start()
    {
        oxygenDetector = theDetector.GetComponent<OxygenDetector>();
    }

    // Update is called once per frame
    void Update()
    {

        rotationSpeed = oxygenDetector.oxygenLevels;
        this.transform.Rotate(0, 0, (rotationSpeed/20), Space.Self);


    }
}
