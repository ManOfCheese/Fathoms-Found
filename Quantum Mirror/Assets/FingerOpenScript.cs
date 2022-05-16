using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerOpenScript : MonoBehaviour
{
    public Animator m_Animator;
    public void OnTriggerStay(Collider other)
    {

        if (other.gameObject.name == "AlienCenter")
        {
            Debug.Log("OPen");

            m_Animator.SetBool("FingerOpen", true);
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "AlienCenter")
        {
            Debug.Log("Close");

            m_Animator.SetBool("FingerOpen", false);
        }
    }


}
