using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractObject : MonoBehaviour
{
    public bool isInteractable;
    public Animator animator;
    float t = 0;
    public GameObject ovenObj;
    public bool isInOven = false;

    void Update(){
        if (isInOven){
            t += Time.deltaTime / 5.0f; // Divided by 5 to make it 5 seconds.
            Debug.Log(t);
            ovenObj.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.cyan, t);
        }
    }
    public void ActivateObject()
    {
        if (!isInteractable)
        {
            isInteractable = true;
            animator.SetBool("isActivated", isInteractable);

        }
        else if (isInteractable)
        {
            isInteractable = false;
            animator.SetBool("isActivated", isInteractable);
        }
    }
    public void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "ReddoMode"){
            isInOven = true;
            ovenObj = collision.gameObject;
        }
    }
}
