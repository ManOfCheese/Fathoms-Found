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

    private bool correctInput;
    private bool closeWhenDone;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if ( startOpen )
            Open();
    }

    public void Open() 
    {
        if ( isOpen == true && !animator.GetCurrentAnimatorStateInfo( 0 ).IsName( "CloseDoor" ) )
            animator.SetTrigger( "Error" );
        else
            StartCoroutine( CorrectInput() );
    }

    IEnumerator CorrectInput()
    {
        correctInput = true;

        if ( panel != null )
            panel.SetBool( "Active", true );

        if ( wire != null )
            wire.SetBool( "Active", true );
        yield return new WaitForSeconds( 3 );

        if ( solution != null )
            solution.SetBool( "Active", true );
        yield return new WaitForSeconds( 1 );

        if ( animator != null )
            animator.SetTrigger( "Open" );

        correctInput = false;
    }

    public void Close()
    {
        if ( correctInput )
        {
            StopCoroutine( CorrectInput() );
            if ( panel != null )
                panel.SetBool( "Active", false );

            if ( wire != null )
                wire.SetBool( "Active", false );

            if ( solution != null )
                solution.SetBool( "Active", false );
            correctInput = false;
            CloseDoor();
        }
        else if ( isOpen == false && !animator.GetCurrentAnimatorStateInfo( 0 ).IsName( "OpenDoor" ) )
            animator.SetTrigger( "Error" );
        else
            CloseDoor();
    }

    private void CloseDoor()
	{
        animator.SetTrigger( "Close" );

        if ( wire != null )
            wire.SetBool( "Active", false );

        if ( solution != null )
            solution.SetBool( "Active", false );

        if ( panel != null )
            panel.SetBool( "Active", false );
    }

    public void AnimationCompleted()
	{
        isOpen = !isOpen;
        animator.SetBool( "IsOpen", isOpen );
    }
}
