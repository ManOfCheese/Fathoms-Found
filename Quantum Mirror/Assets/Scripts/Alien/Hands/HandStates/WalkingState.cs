using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class WalkingState : State<HandController>
{
	#region singleton
	//Create a single instance of this state for all state machines.
	private static WalkingState _instance;

	private WalkingState()
	{
		if ( _instance != null )
		{
			return;
		}

		_instance = this;
	}

	public static WalkingState Instance
	{
		get
		{
			if ( _instance == null )
			{
				new WalkingState();
			}

			return _instance;
		}
	}
	#endregion

	public override void EnterState( HandController _owner )
	{

	}

	public override void UpdateState( HandController _o )
	{
        _o.transform.position = _o.currentPosition;

        Ray ray = new Ray( _o.ikManager.body.position + _o.footSpacing * 2f, Vector3.down );
        Debug.DrawRay( ray.origin, ray.direction * 10f, Color.white );

        if ( Physics.Raycast( ray, out RaycastHit info, 10, _o.terrainLayer.value ) )
        {
            if ( Vector3.Distance( _o.newPosition, info.point ) > _o.stepDistance * Random.Range( _o.stepDistRandomization.x, _o.stepDistRandomization.y ) && 
                _o.lerp >= 1 )
            {
                _o.lerp = 0f;
                Vector3 destinationVector = _o.ikManager.agent.destination - _o.transform.position;
                _o.newPosition = info.point + ( new Vector3( destinationVector.x, 0f, destinationVector.z ).normalized * 
                    ( _o.stepLength * Random.Range( _o.stepLengthRandomization.x, _o.stepLengthRandomization.y ) ) ) + 
                    ( new Vector3( 0f, 1f, 0f ) * _o.heightOffset );
                _o.heightRandomization = Random.Range( _o.stepHeightRandomization.x, _o.stepHeightRandomization.y );
            }
        }

        if ( _o.lerp < 1f )
        {
            Vector3 tempPosition = Vector3.Lerp( _o.oldPosition, _o.newPosition, _o.lerp );
            tempPosition.y += Mathf.Sin( _o.lerp * Mathf.PI ) * ( _o.stepHeight * _o.heightRandomization );

            if ( _o.lerp > _o.closeFingerTreshold && _o.lerp < _o.openFingerTreshold && _o.fingersOpen )
            {
                for ( int i = 0; i < _o.fingerAnimators.Length; i++ )
                {
                    _o.fingerAnimators[ i ].speed = _o.openHandSpeed;
                    _o.fingerAnimators[ i ].SetBool( "FingerOpen", false );
                }
                _o.fingersOpen = false;
            }
            else if ( _o.lerp > _o.openFingerTreshold && !_o.fingersOpen )
            {
                for ( int i = 0; i < _o.fingerAnimators.Length; i++ )
                {
                    _o.fingerAnimators[ i ].speed = _o.openHandSpeed;
                    _o.fingerAnimators[ i ].SetBool( "FingerOpen", true );
                }
                _o.fingersOpen = true;
            }
            _o.handTransform.LookAt( tempPosition + new Vector3( 0f, 1f, 0f ) );

            _o.currentPosition = tempPosition;
            _o.lerp += Time.deltaTime * _o.walkSpeed;
        }
        else
        {
            _o.oldPosition = _o.newPosition;
            _o.handTransform.LookAt( _o.oldPosition + new Vector3( 0f, 1f, 0f ) );
        }
    }

	public override void ExitState( HandController _o )
	{

	}
}
