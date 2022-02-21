using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour {

    public int pickUpRange;
    public int dropRange;
    public Vector3 dropPosition;
    public Vector3 interactableItemPosition;
    public LayerMask pickUpMask;
    public LayerMask dropMask;
    public Animator anim;
    public Transform handPosition;
    public PickUpObject pickUpObject;
    public PickUpObject objectInHand;
    public GameObject cam;
    public GameObject pickUpPrompt;
    public GameObject dropPrompt;
    public GameObject interactPrompt;
    public GameObject usePrompt;
    public List<NPC> NPCsInRange;

    public delegate void EmptyVoidAction();
    public static event EmptyVoidAction LookAtNPC;
    public static event EmptyVoidAction LookAway;

    public delegate void ObjectAction( ObjectType obj );
    public static event ObjectAction PickUpObject;
    public static event ObjectAction DropObject;
    public static event ObjectAction UseObject;

    public int interactRange;
    public InteractObject interactObject;
    public LayerMask interactMask;

    public Move_Action approachPlayer;
    public Move_Action runAway;
    public Transform player;

    private bool looking;

    private void Awake()
    {
        approachPlayer.target = player;
        runAway.target = player;
    }

    // Update is called once per frame
    void Update() 
    {
        Debug.DrawRay( cam.transform.position, cam.transform.forward * pickUpRange, Color.yellow, 0 );

        RaycastHit hit;
        if ( Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity ) ) 
        {
            if ( hit.transform.GetComponent<NPC>() ) 
            {
                LookAtNPC?.Invoke();
                looking = true;
			}
		}
        else if ( looking ) 
        {
            looking = false;
            LookAway?.Invoke();
		}

        //Pick up
        if ( objectInHand == null ) 
        {
            usePrompt.SetActive( false );

            if ( !pickUpPrompt.activeSelf && 
                Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, pickUpRange, pickUpMask ) ) 
            {
                pickUpPrompt.gameObject.SetActive( true );
                pickUpObject = hit.transform.GetComponent<PickUpObject>();
            }
            else if ( pickUpPrompt.activeSelf && 
                !Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, pickUpRange, pickUpMask ) ) 
            {
                pickUpPrompt.gameObject.SetActive( false );
                pickUpObject = null;
            }

            if ( pickUpPrompt.activeSelf ) 
            {
                if ( Input.GetKeyDown( KeyCode.E ) ) 
                {
                    pickUpObject.transform.parent = handPosition.transform;
                    pickUpObject.transform.localPosition = handPosition.localPosition;
                    pickUpObject.transform.rotation = Quaternion.Euler( Vector3.zero );
                    pickUpObject.GetComponent<Rigidbody>().isKinematic = true;
                    objectInHand = pickUpObject;
                    pickUpObject = null;
                    pickUpPrompt.SetActive( false );
                    PickUpObject?.Invoke( objectInHand.objectType );
                }
            }
        }
        else 
        {
            if ( !interactPrompt.activeSelf && Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, interactRange, interactMask ) ) 
            {
                interactPrompt.gameObject.SetActive( true );
                usePrompt.SetActive( false );
                interactObject = hit.transform.GetComponent<InteractObject>();
            }
            else if ( interactPrompt.activeSelf && !Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, interactRange, interactMask ) ) 
            {
                interactPrompt.gameObject.SetActive( false );
                interactObject = null;
            }
            
            if ( !interactPrompt.activeSelf && !dropPrompt.activeSelf && 
                Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, dropRange, dropMask ) ) 
            {
                dropPrompt.gameObject.SetActive( true );
                usePrompt.SetActive( false );
                dropPosition = hit.point;
            }
            else if ( dropPrompt.activeSelf && 
                !Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, dropRange, dropMask ) ) 
            {
                dropPrompt.gameObject.SetActive( false );
                dropPosition = Vector3.zero;
            }

            if ( interactPrompt.activeSelf ) 
            {
                if ( Input.GetKeyDown( KeyCode.E ) ) 
                {
                    objectInHand.transform.parent = null;
                    objectInHand.transform.localPosition = interactableItemPosition;
                    DropObject?.Invoke( objectInHand.objectType );
                    interactPrompt.gameObject.SetActive( false );
                    interactObject = null;
                    objectInHand = null;
                }
            }
            else if ( dropPrompt.activeSelf ) 
            {
                if ( Input.GetKeyDown( KeyCode.E ) ) 
                {
                    objectInHand.transform.parent = null;
                    objectInHand.transform.localPosition = dropPosition + new Vector3( 0, objectInHand.transform.localScale.y, 0 );
                    objectInHand.GetComponent<Rigidbody>().isKinematic = false;
                    DropObject?.Invoke( objectInHand.objectType );
                    objectInHand = null;
                    dropPrompt.SetActive( false );
                }
            }
            else {
                usePrompt.SetActive( true );

                if ( Input.GetKeyDown( KeyCode.E ) ) 
                {
                    anim.SetTrigger( "UseObject" );
                    UseObject?.Invoke( objectInHand.objectType );
                }
            }
        }
    }

	private void OnTriggerEnter( Collider other ) 
    {
		if ( other.GetComponent<NPC>() ) 
        {
            NPC npc = other.GetComponent<NPC>();
            LookAtNPC += npc.OnLookAtNPC;
            LookAway += npc.OnLookAway;
            DropObject += npc.OnDropObject;
            PickUpObject += npc.OnPickUpObject;
            UseObject += npc.OnUseObject;
            NPCsInRange.Add( npc );
            if ( objectInHand != null ) 
                PickUpObject?.Invoke( objectInHand.objectType );
			else 
                npc.Alert();
		}
	}

	private void OnTriggerExit( Collider other ) 
    {
        if ( other.GetComponent<NPC>() ) 
        {
            NPC npc = other.GetComponent<NPC>();
            if ( objectInHand != null ) 
                DropObject?.Invoke( objectInHand.objectType );
            LookAtNPC -= npc.OnLookAtNPC;
            LookAway -= npc.OnLookAway;
            DropObject -= npc.OnDropObject;
            PickUpObject -= npc.OnPickUpObject;
            UseObject -= npc.OnUseObject;
            NPCsInRange.Remove( npc );
            npc.Idle();
        }
    }
}
