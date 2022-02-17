using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractor : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject cam;
    public int interactRange;
    public InteractObject interactObject;
    public LayerMask interactMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactRange, Color.red, 0);


        RaycastHit hit;
        if (!interactPrompt.activeSelf &&
               Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, interactMask))
        {
            interactPrompt.gameObject.SetActive(true);
            interactObject = hit.transform.GetComponent<InteractObject>();
        }
        else if (interactPrompt.activeSelf &&
            !Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, interactMask))
        {
            interactPrompt.gameObject.SetActive(false);
            interactObject = null;
        }

        if (interactPrompt.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactPrompt.SetActive(false);
            }
        }
    }
}
