using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Text_Changer : MonoBehaviour
{
    public GameObject missionObjectiveOne;
    public GameObject missionObjectiveTwo;
    public GameObject theDetector;
    private OxygenDetector oxygenDetector;
    public float oxygenCap;
    private Text missionOneText;
    private Text missionTwoText;


    // Start is called before the first frame update
    void Start()
    {
        oxygenDetector = theDetector.GetComponent<OxygenDetector>();
        missionOneText = missionObjectiveOne.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (oxygenDetector.oxygenLevels >= oxygenCap)
        {
            Debug.Log("gree");
            missionOneText.color = Color.green;
        }

        /* if (Creature says hallo)
        {
            missionTwoText.color = Color.green;
        }
        */
    }
}
