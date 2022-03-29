using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Handmovement : MonoBehaviour
{

    [Header("Settings")]
    public float gestureModeLookSensitivity;
    public float handSensitivity;
    public float scrollSensitivity;
    public bool rotateHand;

    [Header("References")]
    public BoolArrayValue fingers;
    public IntValue handPos;
    public BoolValue isInGestureMode;
    public BoolValue confirmGesture;
    public GameObject handEntryPos;

    public GameObject cursor;

    public GameObject center;
    public GameObject idle;
    public GameObject hand;
    public GameObject handmodel;
    public GameObject reticle;
    public JackOfController controller;
    public LayerMask layerMask;
    public GameObject IKpole;
    public GameObject IKpoleIdle;

    public GameObject[] lights;
    public GameObject[] Digits;
    public GestureCircle gestureCircle;
    public SphereCollider[] subCircles;

    [Header("Read Only")]
    [ReadOnly] public bool gestureMode;
    [ReadOnly] public int fingermode = 0;
    [ReadOnly] public Vector2 lookVector;
    [ReadOnly] public Vector3 punchdestination = new Vector3(0, 0, 0.08f);
    [ReadOnly] public Vector3 mousePos;
    [ReadOnly] public Vector3 mouseWorldPos;
    [ReadOnly] public Collider planeCollider;
    [ReadOnly] public RaycastHit hitData;
    [ReadOnly] public Vector3 IKpoleNew;

    private int i = 0;

    public float l = 0f;

    private void Start()
    {
    }

    void Update()
    {
        Cursor.visible = false;

        //The new arm controls based on the actual mouse position

        Vector3 handStartPos = hand.transform.position;

        hand.transform.position = Vector3.Lerp(handStartPos, cursor.transform.position, 0.4f);


        
        

        if ( gestureMode == true )
        {
            Cursor.lockState = CursorLockMode.None;

            //Raycast looking for snapping planes            

            mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay( mousePos );

            if ( Physics.Raycast( ray, out hitData, 5, layerMask ) )
            {
                Debug.Log("Snap!");
                mouseWorldPos = hitData.point;
                Vector3 planeIKpole = hitData.collider.gameObject.transform.GetChild(0).gameObject.transform.position;
                IKpoleNew = planeIKpole;

                if (l < 1)
                    l += 0.05f;

                handmodel.transform.rotation = hitData.collider.gameObject.transform.rotation;
            }

            else 
            {
                Debug.Log("normal~");
                //mouseWorldPos.z = gestureCircle.transform.localPosition.z;
                mouseWorldPos = Camera.main.ScreenToWorldPoint( new Vector3(Mouse.current.position.ReadValue().x, 
                    Mouse.current.position.ReadValue().y, gestureCircle.transform.localPosition.z) );

                if (l > 0)
                    l -= 0.05f;

                handmodel.transform.rotation = gestureCircle.transform.rotation;
            }

            IKpole.transform.position = Vector3.Lerp(IKpoleIdle.transform.position, IKpoleNew, l);

            cursor.transform.position = mouseWorldPos;

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

           
        }
        else
        {
            cursor.transform.position = idle.transform.position;
            reticle.SetActive( true );
            gestureCircle.gameObject.SetActive( false );

            IKpoleNew = IKpoleIdle.transform.localPosition;
            Cursor.lockState = CursorLockMode.Locked;
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
                    hand.transform.localPosition = new Vector3( handpos.x * 0.8f, handpos.y * 0.8f, 0f );
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
                isInGestureMode.Value = true;
                //Mouse.current.WarpCursorPosition(handEntryPos.transform.position);
                controller.ChangeSensitivity( gestureModeLookSensitivity );
            }
			else
			{
                gestureMode = false;
                isInGestureMode.Value = false;
                controller.ChangeSensitivity( controller.startSensitivity );
                for ( int i = 0; i < gestureCircle.fingerSprites.Length; i++ )
                    gestureCircle.fingerSprites[ i ].SetActive( false );
            }
        }
    }

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
			else if ( gestureCircle.gameObject.activeSelf )
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
            fingers.Value[ 0 ] = true;
        }

        if ( value.canceled )
        {
            Digits[0].GetComponent<Animator>().SetBool("FingerOpen", false);
            fingers.Value[ 0 ] = false;
        }
        
    }

    public void OnFinger2(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Digits[1].GetComponent<Animator>().SetBool("FingerOpen", true);
            fingers.Value[ 1 ] = true;
        }

        if (value.canceled)
        {
            Digits[1].GetComponent<Animator>().SetBool("FingerOpen", false);
            fingers.Value[ 1 ] = false;
        }

    }

    public void OnFinger3(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Digits[2].GetComponent<Animator>().SetBool("FingerOpen", true);
            fingers.Value[ 2 ] = true;
        }

        if (value.canceled)
        {
            Digits[2].GetComponent<Animator>().SetBool("FingerOpen", false);
            fingers.Value[ 2 ] = false;
        }

    }

}

