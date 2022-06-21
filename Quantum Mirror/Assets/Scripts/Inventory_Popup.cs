using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Popup : MonoBehaviour
{
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Claw_Base")
        anim.SetBool("IsOpen", true);       
    }
    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("IsOpen", false);
    }


}
