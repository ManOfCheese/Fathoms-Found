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
        isOpen = true;
        if (panel != null)
        panel.SetBool ( "Active", true );

        if (wire != null)
        wire.SetBool("Active", true);
        yield return new WaitForSeconds(3);

        if(solution != null && isOpen == true)
        solution.SetBool("Active", true);
        yield return new WaitForSeconds(1);

        if(animator != null && isOpen == true)
        animator.SetBool( "Open", true );
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
            
            if (wire != null)
            wire.SetBool("Active", false );

            if (solution != null)
            solution.SetBool("Active", false);

            if (panel != null)
            panel.SetBool("Active", false );

            isOpen = false;
        }
    }
}
