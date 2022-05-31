using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Handmovement : MonoBehaviour
{
    
    [Header( "Settings" )]
    public bool resetHandOnConfirm;
    public float gestureModeLookSensitivity;
    public float handLength;
    public float ikPoleLerpSpeed = 0.05f;
    public float snapToRange = 5f;
    public LayerMask snapToMask;
    public LayerMask gestureMask;
    public Vector3 punchdestination = new Vector3( 0, 0, 0.08f );

    [Header("Variables")]
    public BoolArrayValue fingers;
    public BoolValue isInGestureMode;
    public BoolValue confirmGesture;
    public BoolValue isUsingGestureCircle;

    [Header( "References" )]
    public Animator[] fingerAnimators;
    public Animator Draweranim;
    public Animator Armanim;
    public GameObject armModeUI;
    public GameObject camModeUI;
    public GameObject armIK;
    public Transform gestureCircle;
    public JackOfController controller;
    public Transform worldSpaceCursor;
    public Transform hand;
    public Transform handmodel;
    public Transform IKpole;

    [Header( "Default Positions" )]
    public Transform center;
    public Transform idle;
    public Transform IKpoleIdle;

    [Header( "Read Only" )]
    [SerializeField, ReadOnly] private Vector2 lookVector;
    [SerializeField, ReadOnly] private Vector3 mousePos;
    [SerializeField, ReadOnly] private Vector3 mouseWorldPos;
    [SerializeField, ReadOnly] private Vector3 IKpoleNew;

    private RaycastHit hitData;
    private float p = 0f;

	private void OnEnable()
	{
        isInGestureMode.onValueChanged += OnGestureModeChange;
	}

	private void OnDisable()
	{
        isInGestureMode.onValueChanged -= OnGestureModeChange;
    }

	void Update()
    {
        //The new arm controls based on the actual mouse position

        Vector3 handStartPos = hand.position;

        hand.position = Vector3.Lerp( handStartPos, worldSpaceCursor.transform.position, 0.4f );

        if ( isInGestureMode.Value == true )
        {
            //Raycast looking for snapping planes            

            mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay( mousePos );

            //If we find a snap plane.
            if ( Physics.Raycast( ray, out hitData, snapToRange, snapToMask ) )
            {
                mouseWorldPos = hitData.point;
                Vector3 planeIKpole = hitData.collider.gameObject.transform.GetChild( 0 ).gameObject.transform.position;
                IKpoleNew = planeIKpole;

                if ( hitData.collider.gameObject.name == "DrawerContactPlane" )
                    Draweranim.SetBool( "isinInventory", true );
                
                if ( p < 1 )
                    p += ikPoleLerpSpeed;

                handmodel.rotation = hitData.collider.gameObject.transform.rotation;
            }
            else 
            {
                if ( Draweranim.GetBool( "isInInventoy" ) )
                    Draweranim.SetBool( "isinInventory", false );

                mouseWorldPos = Camera.main.ScreenToWorldPoint( new Vector3( Mouse.current.position.ReadValue().x,
                    Mouse.current.position.ReadValue().y, gestureCircle.localPosition.z ) );
                
                if ( p > 0f )
                    p -= ikPoleLerpSpeed;

                handmodel.rotation = gestureCircle.rotation;
            }

            IKpole.position = Vector3.Lerp( IKpoleIdle.position, IKpoleNew, p );

            worldSpaceCursor.transform.position = mouseWorldPos + ( hand.transform.forward * -handLength );

            float distance = Vector3.Distance( center.position, hand.position );
        }
		else
		{
            worldSpaceCursor.transform.position = idle.position;
            handmodel.rotation = gestureCircle.transform.rotation;
            IKpoleNew = IKpoleIdle.localPosition;
        }
    }

    public void OnGestureModeChange( bool value )
	{
		if ( value )
		{
            Cursor.lockState = CursorLockMode.None;
            armIK.SetActive( true );
			Armanim.SetBool( "Retract", false );
            armModeUI.SetActive( true );
            camModeUI.SetActive( false );
        }
		else
		{
            Cursor.lockState = CursorLockMode.Locked;
            armIK.SetActive( false );
            Armanim.SetBool( "Retract", true );
            armModeUI.SetActive( false );
            camModeUI.SetActive( true );
            Draweranim.SetBool( "isinInventory", false );
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
            if ( !isInGestureMode.Value )
			{
                isInGestureMode.Value = true;
                //Mouse.current.WarpCursorPosition(handEntryPos.transform.position);
                controller.ChangeSensitivity( gestureModeLookSensitivity );
            }
			else
			{
                isInGestureMode.Value = false;
                controller.ChangeSensitivity( controller.startSensitivity );
            }
        }
    }

    public void OnUseHand( InputAction.CallbackContext value )
    {
        if ( value.performed )
        {
            //grabber.GetComponent<Collider>().enabled = true;

			if ( resetHandOnConfirm )
			{
                for ( int i = 0; i < fingerAnimators.Length; i++ )
                    fingerAnimators[ i ].SetBool( "FingerOpen", true );
            }

            if ( gameObject.transform.localPosition.z < 0.4f ) {
                worldSpaceCursor.transform.Translate( punchdestination );

                if ( isUsingGestureCircle.Value )
                    confirmGesture.Value = true;
            }
        }
        else if ( value.canceled )
        {
            //grabber.GetComponent<Collider>().enabled = false;

            if ( isUsingGestureCircle.Value )
                confirmGesture.Value = false;

            if ( resetHandOnConfirm )
            {
                for ( int i = 0; i < fingerAnimators.Length; i++ )
                    fingerAnimators[ i ].SetBool( "FingerOpen", false );
                for ( int i = 0; i < fingers.Value.Length; i++ )
                    fingers.Value[ i ] = false;
            }

            if ( gameObject.transform.localPosition.z >= 0f )
                gameObject.transform.Translate( -punchdestination );
        }
    }

    public void OnFinger1( InputAction.CallbackContext value )
    {
        if ( value.performed )
        {
            fingerAnimators[ 0 ].SetBool( "FingerOpen", true );
            fingers.Value[ 0 ] = true;
        }
        if ( value.canceled )
        {
            fingerAnimators[ 0 ].SetBool( "FingerOpen", false );
            fingers.Value[ 0 ] = false;
        }
        
    }

    public void OnFinger2( InputAction.CallbackContext value )
    {
        if ( value.performed )
        {
            fingerAnimators[ 1 ].SetBool( "FingerOpen", true );
            fingers.Value[ 1 ] = true;
        }
        if (value.canceled)
        {
            fingerAnimators[ 1 ].SetBool( "FingerOpen", false );
            fingers.Value[ 1 ] = false;
        }

    }

    public void OnFinger3( InputAction.CallbackContext value )
    {
        if (value.performed)
        {
            fingerAnimators[ 2 ].SetBool( "FingerOpen", true );
            fingers.Value[ 2 ] = true;
        }
        if (value.canceled)
        {
            fingerAnimators[ 2 ].SetBool( "FingerOpen", false );
			fingers.Value[ 2 ] = false;
        }

	}
}

