using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractor : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject cam;
    public UnityEvent interactAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!interactPrompt.activeSelf &&
               Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickUpRange, pickUpMask))
        {
            interactPrompt.gameObject.SetActive(true);
            pickUpObject = hit.transform.GetComponent<PickUpObject>();
        }
        else if (interactPrompt.activeSelf &&
            !Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickUpRange, pickUpMask))
        {
            interactPrompt.gameObject.SetActive(false);
            pickUpObject = null;
        }
    }
}
