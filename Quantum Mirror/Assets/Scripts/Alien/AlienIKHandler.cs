using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienIKHandler : MonoBehaviour
{

    public NavMeshAgent agent;
    public Animator[] fingerAnimators;
    public Transform hand;

    [SerializeField] private LayerMask terrainLayer = default;
    [SerializeField] private Transform body = default;
    [SerializeField] private float heightOffset = default;
    [SerializeField] private float speed = 1;
    [SerializeField] private float openHandSpeed = 1;
    [SerializeField] private float closeFingerTreshold = 0.2f;
    [SerializeField] private float openFingerTreshold = 0.8f;
    // [SerializeField] AlienIKHandler otherFoot = default;
    public Vector2 randomOffsetRange = Vector2.one;
    [Space( 10 )]
    [SerializeField] float stepDistance = 4;
    [SerializeField] Vector2 stepDistRandomization = Vector2.zero;
    [Space( 10 )]
    [SerializeField] float stepLength = 4;
    [SerializeField] Vector2 stepLengthRandomization = Vector2.zero;
    [Space( 10 )]
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector2 stepHeightRandomization = Vector2.zero;

    private Vector3 footSpacing;
    private Vector3 oldPosition, currentPosition, newPosition;
    private float lerp;
    private float heightRandomization;
    private bool fingersOpen = true;

    private void Awake()
    {
        footSpacing = transform.localPosition;
        currentPosition = newPosition = oldPosition = transform.position;
        // currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = currentPosition;

        Ray ray = new Ray( body.position + footSpacing * 2f, Vector3.down );
        Debug.DrawRay( ray.origin, ray.direction * 10f, Color.white );

        if ( Physics.Raycast( ray, out RaycastHit info, 10, terrainLayer.value ) )
        {
            if ( Vector3.Distance( newPosition, info.point ) > stepDistance * Random.Range( stepDistRandomization.x, stepDistRandomization.y ) && lerp >= 1 )
                // && !otherFoot.IsMoving() 
            {
                lerp = 0;
                //int direction = body.InverseTransformPoint( info.point ).z > body.InverseTransformPoint( newPosition ).z ? 1 : -1;
                Vector3 destinationVector = agent.destination - transform.position;
                newPosition = info.point + ( new Vector3( destinationVector.x, 0f, destinationVector.z ).normalized 
                    * ( stepLength * Random.Range( stepLengthRandomization.x, stepLengthRandomization.y ) ) ) 
                    * Random.Range( randomOffsetRange.x, randomOffsetRange.y ) 
                    + ( new Vector3( 0f, 1f, 0f ) * heightOffset );
                heightRandomization = Random.Range( stepHeightRandomization.x, stepHeightRandomization.y );
            }
        }

        if ( lerp < 1 )
        {
            Vector3 tempPosition = Vector3.Lerp( oldPosition, newPosition, lerp );
            tempPosition.y += Mathf.Sin( lerp * Mathf.PI ) * ( stepHeight * heightRandomization );

            if ( lerp > closeFingerTreshold && lerp < openFingerTreshold && fingersOpen )
            {
                for ( int i = 0; i < fingerAnimators.Length; i++ )
                {
                    fingerAnimators[ i ].speed = openHandSpeed;
                    fingerAnimators[ i ].SetBool( "FingerOpen", false );
                }
                fingersOpen = false;
            }
            else if ( lerp > openFingerTreshold && !fingersOpen )
            {
                for ( int i = 0; i < fingerAnimators.Length; i++ )
                {
                    fingerAnimators[ i ].speed = openHandSpeed;
                    fingerAnimators[ i ].SetBool( "FingerOpen", true );
                }
                fingersOpen = true;
            }
            hand.LookAt( tempPosition + new Vector3( 0f, 1f, 0f ) );

            currentPosition = tempPosition;
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            hand.LookAt( oldPosition + new Vector3( 0f, 1f, 0f ) );
        }
    }

    public bool IsMoving()
    {
        return lerp < 1;
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
        Gizmos.DrawSphere( newPosition, 0.5f );

        Gizmos.color = Color.red;
        Gizmos.DrawSphere( oldPosition, 0.5f );

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere( currentPosition, 0.5f );
    }
}
