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
    public bool rotateHand;

    [Header( "References" )]
    public BoolArrayValue fingers;
    public IntValue handPos;
    public BoolValue isInGestureMode;
    public BoolValue confirmGesture;
    
    public GameObject center;
    public GameObject idle;
    public GameObject hand;
    public GameObject handmodel;
    public GameObject reticle;
    public GameObject[] lights;
    public GameObject[] Digits;
    public GameObject[] ClosedDigits;
    public GestureCircle gestureCircle;
    public SphereCollider[] subCircles;
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
            float distance = Vector3.Distance(center.transform.position, hand.transform.position);

            if ( rotateHand )
            { 
            //Hand rotates outward from the gesture circle
            Vector3 relativepos = handmodel.transform.InverseTransformPoint( center.transform.position );
            relativepos.y = 0;

            Vector3 targetPostition = handmodel.transform.TransformPoint( relativepos );

            //if in outside ring, lerp to face hand outward
            if (distance > 0.25)
                handmodel.transform.LookAt(targetPostition, handmodel.transform.up);
            else
                handmodel.transform.localRotation = Quaternion.Euler(90,0,0);
            }

            reticle.SetActive( false );
            gestureCircle.gameObject.SetActive( true );

            //Moving the hand with the mouse as long as it's in the circle, otherwise move it slightly back to center
            
            //if ( distance < 0.8 )
            //{
                float xMove = lookVector.normalized.y * handSensitivity * Time.deltaTime;
                float yMove = lookVector.normalized.x * handSensitivity * Time.deltaTime;
                hand.transform.Translate(new Vector3( xMove, yMove, 0 ) );
            //}
            //else
            //{
                //hand.transform.position = Vector3.MoveTowards( hand.transform.position, center.transform.position, 0.05f );
            //}
        }
        else
        {
            hand.transform.localPosition = idle.transform.localPosition;
            reticle.SetActive( true );
            gestureCircle.gameObject.SetActive( false );
        }

        //Check in which area of the gesture circle the hand is.
        bool foundPos = false;
		for ( int i = 0; i < subCircles.Length; i++ ) {
            if ( Vector3.Distance( hand.transform.position, subCircles[ i ].transform.position ) < 
                ( subCircles[ i ].radius * subCircles[ i ].transform.localScale.x ) ) {
                handPos.Value = i + 1;
                foundPos = true;
			}
            if ( i == subCircles.Length - 1 && !foundPos ) {
                handPos.Value = 0;
			}
		}
    }

    public void OnLook( InputAction.CallbackContext value )
	{
        if ( Gamepad.current != null ) 
        {
            if ( Gamepad.current.rightStick.IsActuated( 0f ) )
            {
                if ( value.performed )
                {
                    var handpos = Gamepad.current.rightStick.ReadValue();
                    hand.transform.localPosition = new Vector3( handpos.x * 0.6f, handpos.y * 0.6f, 0f );
                }
            }
            else
            {
                Vector2 mouseLook = value.ReadValue<Vector2>();
                lookVector = new Vector2( mouseLook.y, mouseLook.x );
            }
        }
        else
        {
            Vector2 mouseLook = value.ReadValue<Vector2>();
            lookVector = new Vector2( mouseLook.y, mouseLook.x );
        }
    }


    public void OnToggleGestureMode( InputAction.CallbackContext value)
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
                for ( int i = 0; i < gestureCircle.fingerSprites.Length; i++ )
                    gestureCircle.fingerSprites[ i ].SetActive( false );
            }
        }
    }

    /*public void OnSwitchFingerMode( InputAction.CallbackContext value )
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
	}*/

    public void OnUseHand( InputAction.CallbackContext value )
    {
        if ( value.performed )
        {
            if ( handPos.Value == 0 )
			{
				for ( int i = 0; i < gestureCircle.fingerSprites.Length; i++ )
				{
                    gestureCircle.fingerSprites[ i ].SetActive( false );
				}
			}
			else
			{
                gestureCircle.fingerSprites[ ( handPos.Value - 1 ) * 4 ].SetActive( true );
                gestureCircle.fingerSprites[ ( handPos.Value - 1 ) * 4 + 1 ].SetActive( fingers.Value[ 0 ] );
                gestureCircle.fingerSprites[ ( handPos.Value - 1 ) * 4 + 2 ].SetActive( fingers.Value[ 1 ] );
                gestureCircle.fingerSprites[ ( handPos.Value - 1 ) * 4 + 3 ].SetActive( fingers.Value[ 2 ] );
            }

			for ( int i = 0; i < Digits.Length; i++ )
                Digits[ i ].GetComponent<Animator>().SetBool( "FingerOpen", true );
            if ( gameObject.transform.localPosition.z < 0.4f ) {
                gameObject.transform.Translate( punchdestination );
                confirmGesture.Value = true;
            }
        }
        else if ( value.canceled )
        {
            for ( int i = 0; i < Digits.Length; i++ )
                Digits[ i ].GetComponent<Animator>().SetBool( "FingerOpen", false );

			for ( int i = 0; i < fingers.Value.Length; i++ )
                fingers.Value[ i ] = false;

            if ( gameObject.transform.localPosition.z >= 0f ) {
                gameObject.transform.Translate( -punchdestination );
                confirmGesture.Value = false;
            }
        }
    }

    public void OnFinger1(InputAction.CallbackContext value)
    {
        if ( value.performed )
        {
            Digits[0].GetComponent<Animator>().SetBool("FingerOpen", true);
            //Digits[1].SetActive(false);
            //ClosedDigits[1].SetActive(true);
            fingers.Value[ 0 ] = true;
        }

        if ( value.canceled )
        {
            Digits[0].GetComponent<Animator>().SetBool("FingerOpen", false);
            //Digits[1].SetActive(true);
            //ClosedDigits[1].SetActive(false);
            fingers.Value[ 0 ] = false;
        }
        
    }

    public void OnFinger2(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Digits[1].GetComponent<Animator>().SetBool("FingerOpen", true);
            //Digits[2].SetActive(false);
            //ClosedDigits[2].SetActive(true);
            fingers.Value[ 1 ] = true;
        }

        if (value.canceled)
        {
            Digits[1].GetComponent<Animator>().SetBool("FingerOpen", false);
            //Digits[2].SetActive(true);
            //ClosedDigits[2].SetActive(false);
            fingers.Value[ 1 ] = false;
        }

    }

    public void OnFinger3(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Digits[2].GetComponent<Animator>().SetBool("FingerOpen", true);
            //Digits[3].SetActive(false);
            //ClosedDigits[3].SetActive(true);
            fingers.Value[ 2 ] = true;
        }

        if (value.canceled)
        {
            Digits[2].GetComponent<Animator>().SetBool("FingerOpen", false);
            //Digits[3].SetActive(true);
            //ClosedDigits[3].SetActive(false);
            fingers.Value[ 2 ] = false;
        }

    }


    /*public void OnArticaluteFingers( InputAction.CallbackContext value )
	{
        float mouseDelta = value.ReadValue<float>();

        switch ( fingermode )
        {
            case 0:
                if ( mouseDelta < scrollSensitivity && i <= 3 )
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
                if ( mouseDelta < scrollSensitivity && i <= 3 )
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
                if ( mouseDelta < scrollSensitivity && i <= 3 )
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

                if ( mouseDelta > scrollSensitivity && i > 3 )
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
                if ( mouseDelta < 0 && i <= 3 )
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
    }*/

}

