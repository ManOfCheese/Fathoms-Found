using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorResponse : MonoBehaviour
{
    public bool startOpen;
    public bool isOpen = false;
    
    private Animator animator;
    public Animator wire;
    public Animator solution;
    public Animator panel;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if ( startOpen )
            Open();
    }

    public void Open() 
    {

        if ( isOpen == true )
        {
            animator.SetTrigger( "Error" );
        }

        if ( isOpen == false )
        {
            StartCoroutine(CorrectInput());
        }
    }
    IEnumerator CorrectInput()
    {
        panel.SetBool( "Active", true );
        wire.SetBool("Active", true);
        yield return new WaitForSeconds(3);
        solution.SetBool("Active", true);
        yield return new WaitForSeconds(1);
        animator.SetBool( "Open", true );
        
        isOpen = true;
    }

    public void Close()
    {
        if ( isOpen == false )
        {
            animator.SetTrigger( "Error" );
        }

        if ( isOpen == true )
        {
            animator.SetBool( "Open", false );
            wire.SetBool("Active", false );
            panel.SetBool("Active", false );
            isOpen = false;
        }
    }
}
