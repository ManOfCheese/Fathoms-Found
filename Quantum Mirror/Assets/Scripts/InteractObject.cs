using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractObject : MonoBehaviour
{
    public bool isInteractable;
    public Animator animator;

    public void ActivateObject()
    {
        if (!isInteractable)
        {
            isInteractable = true;
            animator.SetBool("isActivated", isInteractable);
        }
    }
}
