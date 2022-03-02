using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Handmovement : MonoBehaviour
{

    [Header( "Settings" )]
    public float gestureModeLookSensitivity;
    public float handSensitivity;
    public float scrollSensitivity;

    [Header( "References" )]
    public GameObject center;
    public GameObject circle;
    public GameObject idle;
    public GameObject hand;
    public GameObject handmodel;
    public GameObject reticle;
    public GameObject[] lights;
    public GameObject[] Digits;
    public GameObject[] ClosedDigits;
    public JackOfController controller;

    [Header( "Read Only" )]
    [ReadOnly] public bool gestureMode;
    [ReadOnly] public int fingermode = 0;
    [ReadOnly] public Vector2 lookVector;
    [ReadOnly] public Vector3 punchdestination = new Vector3( 0, 0, 0.08f );

    private int i = 0;

    void Update()
    {
        if ( gestureMode == true )
        {
            //Hand rotates outward from the gesture circle
            Vector3 relativepos = handmodel.transform.InverseTransformPoint( center.transform.position );
            relativepos.y = 0;

            Vector3 targetPostition = handmodel.transform.TransformPoint( relativepos );

            handmodel.transform.LookAt( targetPostition, handmodel.transform.up );

            reticle.SetActive( false );
            circle.SetActive( true );

            //Moving the hand with the mouse as long as it's in the circle, otherwise move it slightly back to center
            float distance = Vector3.Distance( center.transform.position, hand.transform.position );

            if ( distance < 1 )
            {
                float xMove = lookVector.normalized.y * handSensitivity;
                float yMove = lookVector.normalized.x * handSensitivity;
                hand.transform.Translate(new Vector3( xMove, yMove, 0 ) );
            }
            else
            {
                hand.transform.position = Vector3.MoveTowards( hand.transform.position, center.transform.position, 0.005f );
            }
        }
        else
        {
            gameObject.transform.localPosition = idle.transform.localPosition;
            reticle.SetActive( true );
            circle.SetActive( false );
            
        }
    }

    public void OnLook( InputAction.CallbackContext value )
	{
        Vector2 mouseLook = value.ReadValue<Vector2>();
        lookVector = new Vector2( mouseLook.y, mouseLook.x );
    }

    public void OnToggleGestureMode( InputAction.CallbackContext value )
	{
        if ( value.performed )
		{
            if ( !gestureMode )
			{
                gestureMode = true;
                controller.ChangeSensitivity( gestureModeLookSensitivity );
            }
			else
			{
                gestureMode = false;
                controller.ChangeSensitivity( controller.startSensitivity );
            }
        }
    }

    public void OnSwitchFingerMode( InputAction.CallbackContext value )
    {
        if ( value.performed )
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
	}

    public void OnUseHand( InputAction.CallbackContext value )
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

    public void OnArticaluteFingers( InputAction.CallbackContext value )
	{
        float mouseDelta = value.ReadValue<float>();

        switch ( fingermode )
        {
            case 0:
                if ( mouseDelta < scrollSensitivity && i <= 5 )
                {
                    Debug.Log( i );
                    Digits[ i ].SetActive( false );
                    ClosedDigits[ i ].SetActive( true );
                    i += 1;
                }

                if ( mouseDelta > scrollSensitivity && i > 0 )
                {
                    Debug.Log( i );
                    Digits[ i ].SetActive( true );
                    ClosedDigits[ i ].SetActive( false );
                    i -= 1;
                }
                break;

            case 1:
                if ( mouseDelta < scrollSensitivity && i <= 5 )
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

                if ( mouseDelta > scrollSensitivity && i > 0 )
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
                if ( mouseDelta < scrollSensitivity && i <= 5 )
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

                if ( mouseDelta > scrollSensitivity && i > 0 )
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

