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

    #region Variables
    public JackOfControllerSystem system;
    public GameObject head;
    public TremorSource tremorSource;

    [Header( "VariableObjects" )]
    public BoolValue paused;
    public BoolValue inGestureMode;
    public BoolValue legsBroken;

    [Header( "Tremor Settings" )]
    public float tremorThreshold;
    public float jumpTremorIntensity;
    public float landTremorIntensity;
    public float walkTremorIntesity;
    public float runTremorIntesity;

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

    [Header( "Audio References" )]
    public float walkPitch;
    public float sprintPitch;
    public float walkSoundFadeInDuration = 0.25f;
    public float walkSoundFadeOutDuration = 0.25f;
    public float walkPitchFadeDuration = 0.1f;
    public AudioInfo tracksGoingSource;
    public AudioInfo tracksGoingSource2;
    public AudioInfo collisionSource;
    public Fader tracksFader;
    #endregion

    #region Non-public Variables
    [Header( "Start Values" )]
    [ReadOnly] public float playerStartHeight;
    [ReadOnly] public float camStartHeight;
    [ReadOnly] public float startSensitivity;

    [Header( "Movement Debug" )]
    [ReadOnly] public bool moving;
    [ReadOnly] public bool sprinting = false;
    [ReadOnly] public float currentSpeed;
    [ReadOnly] public Vector2 rawMoveVector;
    [ReadOnly] public Vector3 velocity;

    [Header( "Camera Debug" )]
    [ReadOnly] public float currentCamHeight;
    [ReadOnly] public float cameraStillTimer;
    [ReadOnly] public float xCamRotation = 0.0f;
    [ReadOnly] public float yCamRotation = 0.0f;
    [ReadOnly] public Vector2 lookVector;

    [Header( "Jumping Debug" )]
    [ReadOnly] public bool grounded = true;
    [ReadOnly] public bool jump = false;
    [ReadOnly] public int jumpCount;
    [ReadOnly] public Vector3 velocityOnJump;

    [HideInInspector] public bool looking = false;
    [HideInInspector] public JackOfManager jom;
    [HideInInspector] public Camera cam;
    [HideInInspector] public CharacterController cc;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public GroundedState groundedState;
    [HideInInspector] public AirborneState airborneState;
    [HideInInspector] public AudioSource[] audioSources;
    [HideInInspector] public AudioInfo[] audioInfos;
    #endregion

    private void Awake() {
        system.joc = this;
        tremorSource = GetComponent<TremorSource>();
    }

	private void OnEnable()
	{
        paused.onValueChanged += OnPause;
	}

	private void OnDisable()
	{
        paused.onValueChanged -= OnPause;
    }

	public void OnPause( bool pause ) 
    { 
        rawMoveVector = Vector3.zero;
    }

    #region Input
    public void ChangeSensitivity( float newSensitivity )
    {
        sensitivity = newSensitivity;
    }

    public void OnLook( InputAction.CallbackContext value ) 
    {
        if ( paused.Value ) { return; }
        if ( !inGestureMode.Value )
		{
            Vector2 mouseLook = value.ReadValue<Vector2>();
            lookVector = new Vector2( mouseLook.y, mouseLook.x );
        }
    }

    public void OnMove( InputAction.CallbackContext value ) 
    {
        if ( paused.Value ) { return; }
        
        if ( !legsBroken.Value )
            rawMoveVector = value.ReadValue<Vector2>();

        //Sound
        if ( value.performed && !moving )
		{
            moving = true;
            tracksFader.StopAllCoroutines();
            tracksFader.Fade( tracksGoingSource.source, 0f, tracksGoingSource.startVolume, walkSoundFadeInDuration, false );
            tracksFader.Fade( tracksGoingSource2.source, 0f, tracksGoingSource2.startVolume, walkSoundFadeInDuration, false );
        }
        else if ( value.canceled && moving )
		{
            moving = false;
            tracksFader.StopAllCoroutines();
            tracksFader.Fade( tracksGoingSource.source, tracksGoingSource.startVolume, 0f, walkSoundFadeOutDuration, false );
            tracksFader.Fade( tracksGoingSource2.source, tracksGoingSource2.startVolume, 0f, walkSoundFadeOutDuration, false );
        }
    }

    public void OnSprint( InputAction.CallbackContext value ) 
    {
        if ( paused.Value ) { return; }

        if ( value.started )
        {
            sprinting = true;
            tracksFader.Fade( tracksGoingSource.source, walkPitch, sprintPitch, walkPitchFadeDuration, true );
            tracksFader.Fade( tracksGoingSource2.source, walkPitch, sprintPitch, walkPitchFadeDuration, true );
            tremorSource.SetTremorIntesity( runTremorIntesity );
        }
        if ( value.canceled ) 
        {
            sprinting = false;
            currentSpeed = speed;
            tracksFader.Fade( tracksGoingSource.source, sprintPitch, walkPitch, walkPitchFadeDuration, true );
            tracksFader.Fade( tracksGoingSource2.source, sprintPitch, walkPitch, walkPitchFadeDuration, true );
            tremorSource.SetTremorIntesity( walkTremorIntesity );
        }
    }

    public void OnJump( InputAction.CallbackContext value ) 
    {
        if ( paused.Value ) { return; }
        if ( value.performed && ( grounded || jumpCount < jumps ) )
        {
            tracksFader.Fade( tracksGoingSource2.source, tracksGoingSource2.startVolume, 0f, walkSoundFadeOutDuration, false );
            jump = true;
        }
    }
	#endregion

	#region Movement
    public void CameraLook() 
    {
        if ( !inGestureMode.Value ) {
            xCamRotation -= sensitivity * lookVector.x;
            yCamRotation += sensitivity * lookVector.y;

            xCamRotation %= 360;
            yCamRotation %= 360;
            xCamRotation = Mathf.Clamp( xCamRotation, xRotationLimitsUp, xRotationLimitsDown );
            head.transform.eulerAngles = new Vector3( xCamRotation, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z );
            cc.transform.eulerAngles = new Vector3( jom.cc.transform.eulerAngles.x, yCamRotation,
                cc.transform.eulerAngles.z );
        }
    }

    public void Walk() 
    {
        if ( ( moving && grounded ) || aerialMovement == AerialMovementSettings.FullMovement )
        {
            Vector3 relativeMovementVector = rawMoveVector.x * cc.transform.right + rawMoveVector.y * cc.transform.forward;
            Vector3 finalMovementVector = new Vector3( relativeMovementVector.x * currentSpeed, velocity.y,
                relativeMovementVector.z * currentSpeed );
            cc.Move( finalMovementVector * Time.deltaTime );
            if ( moving && grounded )
                tremorSource.Tremor();
        }
    }

    public void Jump() 
    {
        if ( jump && ( grounded || jumpCount < jumps ) )
        {
            velocity.y = Mathf.Sqrt( jumpHeight * -2 * gravity );
            jump = false;
            if ( aerialMovement != AerialMovementSettings.FullMovement ) velocityOnJump = cc.velocity;
            jumpCount++;

            float previousTremorIntensity = tremorSource.tremorIntensity;
            tremorSource.SetTremorIntesity( jumpTremorIntensity );
            tremorSource.Tremor();
            tremorSource.SetTremorIntesity( previousTremorIntensity );
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

                if ( moving )
                    tracksFader.Fade( tracksGoingSource2.source, 0f, tracksGoingSource2.startVolume, 0.1f, false );

                float previousTremorIntensity = tremorSource.tremorIntensity;
                tremorSource.SetTremorIntesity( landTremorIntensity );
                tremorSource.Tremor();
                tremorSource.SetTremorIntesity( previousTremorIntensity );

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
