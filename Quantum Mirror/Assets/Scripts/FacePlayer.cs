using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour {

    public GameObject player;

    // Update is called once per frame
    void Update() 
    {
        transform.LookAt( player.transform.position );
    }
}
