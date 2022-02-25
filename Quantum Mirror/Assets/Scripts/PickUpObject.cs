using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour {

    public string objectName;
    public ObjectType objectType;
    public Transform pickUpPoint;
    public Vector3 inHandOrientation;
    public float temperature;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider[] colliders;

	private void Awake() {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponents<Collider>();
	}

}
