using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningMessage : MonoBehaviour
{
    public Animator Errorpopup;

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject.CompareTag( "Player" ) )
        {
            Debug.Log("STOP");
            Errorpopup.SetBool( "Warning", true );
        }
           
    }
    private void OnCollisionExit( Collision collision )
    {
        if ( collision.gameObject.CompareTag( "Player" ) )
            Errorpopup.SetBool( "Warning", false ) ;
    }

}
