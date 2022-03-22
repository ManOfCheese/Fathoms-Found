using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AerialMovementSettings {
    FullMovement,
    FullCameraMovement,
    LimitedCameraMovement,
    NoMovement
}

public class JackOfController : MonoBehaviour {

    public JackOfControllerSystem system;

    [Header( "VariableObjects" )]
    public BoolValue inGestureMode;
    public BoolValue legsBroken;

    [Header( "Camera Settings" )]
    [Tooltip( "When true camera controls will be inverted meaning moving left will move the camera to the right." )]
    public bool inverted;
    [Tooltip( "Determines how much the camera moves relative to the input." )]
    [Range( 0.0f, 1.0f )]
    public float sensitivity;
    [Tooltip( "How many degrees the camera can rotate upwards before locking in place." )]
    public float xRotationLimitsUp = -90f;
    [Tooltip( "How many degrees the camera can rotate downwards before locking in place" )]
    public float xRotationLimitsDown = 60f;

    [Header( "Movement Settings" )]
    [Tooltip("Speed of movement in units per second")]
    public float speed = 5f;
    [Tooltip( "How fast does the player fall" )]
    public float gravity;
    public float stickToGroundForce = 1f;

    [Header( "Sprint Settings" )]
    [Tooltip( "Is sprinting enabled" )]
    public bool sprintAllowed = true;
    //[Tooltip( "Should the FOV momentarily increase when you start sprinting" )]
    //public bool FOVBurst = true;
    //[Tooltip( "Should the FOV increase when you start sprinting" )]
    //public bool FOVBoost = true;
    [Tooltip( "Sprinting speed in units per second" )]
    public float sprintSpeed = 10f;
    [Tooltip( "Sprinting speed relative to the walking speed" )]
    public float relativeSprintSpeed = 2f;
    //public AnimationCurve startSprintCurve;
    //public AnimationCurve endSprintCurve;

    [Header( "Jump Settings" )]
    [Tooltip( "Height of the jump" )]
    public float jumpHeight = 5f;
    [Tooltip( "How many times the player can jump" )]
    public int jumps = 1;
    [Tooltip( "How much freedom of movement should the player have in mid-air?" )]
    public AerialMovementSettings aerialMovement;
    [Tooltip( "How fast can the player change direction in mid-air with limited camera movement" )]
    public float aerialTurnSpeed;
    public bool midAirSprint;

    [Header( "Groundcheck Settings" )]
    public float groundDistance;
    public LayerMask groundMask;

    [Header( "Audio" )]
    public AudioSource tracksStartSource;
    public Fader tracksStartFader;
    public AudioClip[] tracksStart;
    public AudioSource tracksGoingSource;
    public Fader tracksFader;
    public AudioClip[] tracks;
    public AudioSource tracksSprintSource;
    public AudioClip[] tracksSprint;
    public AudioSource tracksStopSource;
    public Fader tracksStopFader;
    public AudioClip[] tracksStop;

    [HideInInspector] public JackOfManager jom;
    [HideInInspector] public Camera cam;
    [HideInInspector] public CharacterController cc;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public GroundedState groundedState;
    [HideInInspector] public AirborneState airborneState;

    //Startup
    [ReadOnly] public float playerStartHeight;
    [ReadOnly] public float camStartHeight;
    [ReadOnly] public float startSensitivity;

    //Runtime
    [ReadOnly] public bool sprinting = false;
    [ReadOnly] public bool jump = false;
    [ReadOnly] public bool grounded = true;
    [ReadOnly] public bool gestureMode = false;
    [ReadOnly] public float currentSpeed;
    [ReadOnly] public float currentCamHeight;
    [ReadOnly] public float xCamRotation = 0.0f;
    [ReadOnly] public float yCamRotation = 0.0f;
    [ReadOnly] public int jumpCount;
    [ReadOnly] public Vector2 lookVector;
    [ReadOnly] public Vector2 rawMoveVector;
    [ReadOnly] public Vector3 velocity;
    [ReadOnly] public Vector3 velocityOnJump;

    private Vector2 prevRawMoveVector;
    private bool moving;

    private void Awake() {
        system.joc = this;
    }

    public void ChangeSensitivity( float newSensitivity )
	{
        sensitivity = newSensitivity;
	}

    #region Input
	public void OnLook( InputAction.CallbackContext value ) 
    {
        if ( !inGestureMode.Value )
		{
            Vector2 mouseLook = value.ReadValue<Vector2>();
            lookVector = new Vector2( mouseLook.y, mouseLook.x );
        }
    }

    public void OnMove( InputAction.CallbackContext value ) 
    {
        if ( !legsBroken.Value )
        {
            prevRawMoveVector = rawMoveVector;
            rawMoveVector = value.ReadValue<Vector2>();
        }

        //Sound
        if ( value.performed && !moving )
		{
            moving = true;
            tracksStartSource.clip = tracksStart[ Random.Range( 0, tracksStart.Length - 1 ) ];
            tracksStartSource.Play();
            tracksGoingSource.Play();
        }
        else if ( value.canceled && moving )
		{
            moving = false;
            tracksStopSource.clip = tracksStop[ Random.Range( 0, tracksStart.Length - 1 ) ];
            tracksStopSource.Play();
            tracksGoingSource.Stop();
        }
    }

    public void OnSprint( InputAction.CallbackContext value ) 
    {
        if ( value.started )
        {
            sprinting = true;
            if ( moving )
			{
                tracksFader.Crossfade( tracksGoingSource, tracksSprintSource, tracksGoingSource.volume, 0f, 1f );
            }
        }
        if ( value.canceled ) 
        {
            sprinting = false;
            currentSpeed = speed;
            if ( moving )
            {
                tracksFader.Crossfade( tracksSprintSource, tracksGoingSource, tracksSprintSource.volume, 0f, 1f );
            }
        }
    }

    public void OnJump( InputAction.CallbackContext value ) 
    {
        if ( value.performed && ( grounded || jumpCount < jumps ) ) jump = true;
    }
	#endregion

	#region Movement
    public void CameraLook() 
    {
        xCamRotation -= sensitivity * lookVector.x;
        yCamRotation += sensitivity * lookVector.y;

        xCamRotation %= 360;
        yCamRotation %= 360;
        xCamRotation = Mathf.Clamp( xCamRotation, xRotationLimitsUp, xRotationLimitsDown );
        cam.transform.eulerAngles = new Vector3( xCamRotation, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z );
        cc.transform.eulerAngles = new Vector3( jom.cc.transform.eulerAngles.x, yCamRotation, 
            cc.transform.eulerAngles.z );
    }

    public void Walk() 
    {
        if ( grounded || aerialMovement == AerialMovementSettings.FullMovement ) 
        {
            Vector3 relativeMovementVector = rawMoveVector.x * cc.transform.right + rawMoveVector.y * cc.transform.forward;
            Vector3 finalMovementVector = new Vector3( relativeMovementVector.x * currentSpeed, velocity.y, 
                relativeMovementVector.z * currentSpeed );
            cc.Move( finalMovementVector * Time.deltaTime );

   //         Debug.Log( prevRawMoveVector.magnitude + " | " + rawMoveVector.magnitude );
   //         if ( prevRawMoveVector.magnitude == 0 && rawMoveVector.magnitude != 0 )
			//{
   //             tracksStartSource.clip = tracksStart[ Random.Range( 0, tracksStart.Length - 1 ) ];
   //             tracksStartSource.Play( 0 );
			//}
   //         else if ( prevRawMoveVector.magnitude > 0 && rawMoveVector.magnitude == 0 )
			//{
   //             tracksStopSource.clip = tracksStop[ Random.Range( 0, tracksStart.Length - 1 ) ];
   //             tracksStopSource.Play( 0 );
   //         }
        }
    }

    public void Jump() 
    {
        if ( jump && ( grounded || jumpCount < jumps ) ) {
            velocity.y = Mathf.Sqrt( jumpHeight * -2 * gravity );
            jump = false;
            if ( aerialMovement != AerialMovementSettings.FullMovement ) velocityOnJump = cc.velocity;
            jumpCount++;
        }
        cc.Move( ( velocityOnJump + velocity ) * Time.deltaTime );
    }

    public void LookMove() 
    {
        if ( aerialMovement == AerialMovementSettings.FullCameraMovement || aerialMovement == AerialMovementSettings.LimitedCameraMovement ) 
        {
            Vector3 camVector = new Vector3( cam.transform.forward.x, 0f, cam.transform.forward.z );

            if ( aerialMovement == AerialMovementSettings.FullCameraMovement )
                velocityOnJump = camVector * currentSpeed;
            if ( aerialMovement == AerialMovementSettings.LimitedCameraMovement )
                velocityOnJump = ( ( ( camVector * aerialTurnSpeed ) + velocityOnJump ) / 2f ).normalized * currentSpeed;
        }
	}

    public void Gravity() 
    {
        velocity.y += gravity * Time.deltaTime;
    }

    public void StickToGround() 
    {
        velocity.y = stickToGroundForce;
    }
	#endregion

	#region Checks
	public void CheckGround() 
    {
        bool newGrounded;

        float radius = cc.height / 4f;
        newGrounded = Physics.CheckSphere( new Vector3( cc.transform.position.x, cc.transform.position.y - radius, cc.transform.position.z ), 
            groundDistance, groundMask );

        if ( newGrounded != grounded ) {
            if ( newGrounded && jom.stateMachine.CurrentState != jom.statesByName[ "GroundedState" ] ) 
            {
                jom.stateMachine.ChangeState( jom.statesByName[ "GroundedState" ] );
                velocityOnJump = Vector3.zero;
                jumpCount = 0;
            }
            if ( !newGrounded && jom.stateMachine.CurrentState != jom.statesByName[ "AirborneState" ] ) 
                jom.stateMachine.ChangeState( jom.statesByName[ "AirborneState" ] );
        }

        grounded = newGrounded;
    }

    public void CheckSprint() 
    {
        if ( sprintAllowed && ( grounded || midAirSprint ) ) 
        {
            if ( sprinting ) 
            {
                if ( sprintSpeed != 0f )
                    currentSpeed = sprintSpeed;
                else
                    currentSpeed = speed * relativeSprintSpeed;
            }
        }
    }
	#endregion

	public void OnDrawGizmos() 
    {
        float radius = playerStartHeight / 4;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere( new Vector3( cc.transform.position.x, cc.transform.position.y - radius, 
            cc.transform.position.z ), groundDistance );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere( new Vector3( cc.transform.position.x, cc.transform.position.y + radius, 
            cc.transform.position.z ), groundDistance );
    }
}
