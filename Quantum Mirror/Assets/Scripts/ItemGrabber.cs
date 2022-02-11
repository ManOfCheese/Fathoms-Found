using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour {

    public int pickUpRange;
    public int dropRange;
    public Vector3 dropPosition;
    public LayerMask pickUpMask;
    public LayerMask dropMask;
    public Transform handPosition;
    public PickUpObject pickUpObject;
    public PickUpObject objectInHand;
    public GameObject cam;
    public GameObject pickUpPrompt;
    public GameObject dropPrompt;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        Debug.DrawRay( cam.transform.position, cam.transform.forward * pickUpRange, Color.yellow, 0 );

        //Pick up
        if ( objectInHand == null ) {
            RaycastHit hit;
            if ( !pickUpPrompt.activeSelf && Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, pickUpRange, pickUpMask ) ) {
                pickUpPrompt.gameObject.SetActive( true );
                pickUpObject = hit.transform.GetComponent<PickUpObject>();
            }
            else if ( pickUpPrompt.activeSelf && !Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, pickUpRange, pickUpMask ) ) {
                pickUpPrompt.gameObject.SetActive( false );
                pickUpObject = null;
            }

            if ( pickUpPrompt.activeSelf ) {
                if ( Input.GetKeyDown( KeyCode.E ) ) {
                    pickUpObject.transform.parent = this.transform;
                    pickUpObject.transform.localPosition = handPosition.localPosition;
                    pickUpObject.GetComponent<Rigidbody>().isKinematic = true;
                    objectInHand = pickUpObject;
                    pickUpObject = null;
                    pickUpPrompt.SetActive( false );
                }
            }
        }
		else {
            RaycastHit hit;
            if ( !dropPrompt.activeSelf && Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, dropRange, dropMask ) ) {
                dropPrompt.gameObject.SetActive( true );
                dropPosition = hit.point;
            }
            else if ( dropPrompt.activeSelf && !Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, dropRange, dropMask ) ) {
                dropPrompt.gameObject.SetActive( false );
                dropPosition = Vector3.zero;
            }

            if ( dropPrompt.activeSelf ) {
                if ( Input.GetKeyDown( KeyCode.E ) ) {
                    objectInHand.transform.parent = null;
                    objectInHand.transform.localPosition = dropPosition + new Vector3( 0, objectInHand.transform.localScale.y, 0 );
                    objectInHand.GetComponent<Rigidbody>().isKinematic = false;
                    objectInHand = null;
                    dropPrompt.SetActive( false );
                }
            }
        }
    }
}
