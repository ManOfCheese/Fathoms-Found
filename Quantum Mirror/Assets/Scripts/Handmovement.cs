using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Handmovement : MonoBehaviour
{
    public float XSensitivity;
    public float YSensitivity;
    public GameObject center;
    public GameObject circle;
    public GameObject idle;
    public GameObject handmodel;

    public GameObject[] lights;

    public GameObject[] Digits;
    public GameObject[] ClosedDigits;

    public GameObject FPSController;

    public bool gestureMode;

    private float lerp = 0;
    public Vector2 sensitivity;
    public Vector2 lookVector;
    public Vector3 punchdestination = new Vector3( 0, 0, 0.08f );

    private int i = 0;

    public int fingermode = 0;

    void Update()
    {
        if ( gestureMode == true )
        {
            //Hand rotates outward from the gesture circle
            Vector3 relativepos = handmodel.transform.InverseTransformPoint( center.transform.position );
            relativepos.y = 0;

            Vector3 targetPostition = handmodel.transform.TransformPoint( relativepos );

            handmodel.transform.LookAt( targetPostition, handmodel.transform.up );

            circle.SetActive( true );
            sensitivity.x = 0.1f;
            sensitivity.x = 0.2f;

            //Moving the hand with the mouse as long as it's in the circle, otherwise move it slightly back to center
            float distance = Vector3.Distance( center.transform.position, transform.position );

            if ( distance < 1 )
            {
                float xMove = lookVector.x * sensitivity.x;
                float yMove = lookVector.y * sensitivity.y;
                gameObject.transform.Translate(new Vector3( xMove, yMove, 0 ) );
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards( transform.position, center.transform.position, 0.005f );
            }
        }
        else
        {
            gameObject.transform.localPosition = idle.transform.localPosition;
            circle.SetActive( false );

            sensitivity.x = 2f;
            sensitivity.x = 2f;
        }
    }

    public void OnLook( InputAction.CallbackContext value )
	{
        Vector2 mouseLook = value.ReadValue<Vector2>();
        lookVector = new Vector2( mouseLook.y, mouseLook.x );
    }

    public void OnToggleGestureMode()
	{
        if ( !gestureMode )
            gestureMode = true;
        else
            gestureMode = false;
    }

    public void OnSwitchFingerMode()
    {
        if ( fingermode < 2 )
            fingermode += 1;
        else
            fingermode = 0;

		for ( int i = 0; i < lights.Length; i++ )
		{
            if ( i == fingermode )
                lights[ i ].SetActive( true );
            else
                lights[ i ].SetActive( false );
		}
	}

    public void OnUse( InputAction.CallbackContext value )
    {
        if ( value.performed )
        {
            if ( gameObject.transform.localPosition.z < 0.4f )
                gameObject.transform.Translate( punchdestination );
        }
        else if ( value.canceled )
        {
            if ( gameObject.transform.localPosition.z >= 0f )
                gameObject.transform.Translate( -punchdestination );
        }
    }

    public void OnScroll( InputAction.CallbackContext value )
	{
        float mouseDelta = value.ReadValue<float>();

        switch ( fingermode )
        {
            case 0:
                if ( mouseDelta < 0 && i <= 5 )
                {
                    Debug.Log( i );
                    Digits[ i ].SetActive( false );
                    ClosedDigits[ i ].SetActive( true );
                    i += 1;
                }

                if ( mouseDelta > 0 && i > 0 )
                {
                    Debug.Log( i );
                    Digits[ i ].SetActive( true );
                    ClosedDigits[ i ].SetActive( false );
                    i -= 1;
                }
                break;

            case 1:
                if ( mouseDelta < 0 && i <= 5 )
                {
                    Debug.Log( i );
                    foreach ( GameObject digit in Digits )
                    {
                        digit.SetActive( true );
                    }
                    foreach ( GameObject digit in ClosedDigits )
                    {
                        digit.SetActive( false );
                    }

                    Digits[ i ].SetActive( false );
                    ClosedDigits[ i ].SetActive( true );
                    i += 1;
                }

                if ( mouseDelta > 0 && i > 0 )
                {
                    Debug.Log( i );
                    foreach ( GameObject digit in Digits )
                    {
                        digit.SetActive( true );
                    }
                    foreach ( GameObject digit in ClosedDigits )
                    {
                        digit.SetActive( false );
                    }

                    Digits[ i ].SetActive( false );
                    ClosedDigits[ i ].SetActive( true );
                    i -= 1;
                }
                break;

            case 2:
                if ( mouseDelta < 0 && i <= 5 )
                {
                    Debug.Log( i );
                    foreach ( GameObject digit in Digits )
                    {
                        digit.SetActive( false );
                    }
                    foreach ( GameObject digit in ClosedDigits )
                    {
                        digit.SetActive( true );
                    }

                    Digits[ i ].SetActive( true );
                    ClosedDigits[ i ].SetActive( false );
                    i += 1;
                }

                if ( mouseDelta > 0 && i > 0 )
                {
                    Debug.Log( i );
                    foreach ( GameObject digit in Digits )
                    {
                        digit.SetActive( false );
                    }
                    foreach ( GameObject digit in ClosedDigits )
                    {
                        digit.SetActive( true );
                    }

                    Digits[ i ].SetActive( true );
                    ClosedDigits[ i ].SetActive( false );
                    i -= 1;
                }
                break;

            default:
                if ( mouseDelta < 0 && i <= 5 )
                {
                    Debug.Log( i );
                    Digits[ i ].SetActive( false );
                    ClosedDigits[ i ].SetActive( true );
                    i += 1;
                }

                if ( mouseDelta > 0 && i > 0 )
                {
                    Debug.Log( i );
                    Digits[ i ].SetActive( true );
                    ClosedDigits[ i ].SetActive( false );
                    i -= 1;
                }
                break;
        }
    }

}

