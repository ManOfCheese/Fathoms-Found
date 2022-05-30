using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorResponse : MonoBehaviour
{
    public bool isOpen = false;
    private Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void OpenDoor() 
    {

        if (isOpen == true)
        {
            animator.SetTrigger("Error");
        }

        if (isOpen == false)
        {
            animator.SetBool("Open", true);
            isOpen = true;
        }
 
    }

    public void CloseDoor()
    {
        if (isOpen == false)
        {
            animator.SetTrigger("Error");
        }

        if (isOpen == true)
        {
            animator.SetBool("Open", false);
            isOpen = false;
        }
    }
}
