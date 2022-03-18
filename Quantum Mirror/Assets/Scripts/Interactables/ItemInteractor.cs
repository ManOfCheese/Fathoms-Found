using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractor : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject cam;
    public int interactRange;
    public InteractableObject interactObject;
    public LayerMask interactMask;

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;    

        if ( !interactPrompt.activeSelf && Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, interactRange, interactMask ) ) {
            interactPrompt.gameObject.SetActive( true );
            Debug.DrawRay( cam.transform.position, cam.transform.forward * interactRange, Color.yellow );
            interactObject = hit.transform.GetComponent<InteractableObject>();
        }
        else if (interactPrompt.activeSelf && !Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, interactRange, interactMask ) )
        {
            Debug.DrawRay( cam.transform.position, cam.transform.forward * interactRange, Color.white );
            interactPrompt.gameObject.SetActive( false );
            interactObject = null;
        }

        if ( interactPrompt.activeSelf )
        {
            if (Input.GetKeyDown( KeyCode.E ) )
            {
                Debug.DrawRay( cam.transform.position, cam.transform.forward * interactRange, Color.white );
                //interactObject.Interact();
            }
        }
    }
}