using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour {

    public int pickUpRange;
    public int dropRange;
    public Vector3 dropPosition;
    public LayerMask pickUpMask;
    public LayerMask dropMask;
    public Transform handPosition;
    public PickUpObject pickUpObject;
    public PickUpObject objectInHand;
    public GameObject cam;
    public GameObject pickUpPrompt;
    public GameObject interactPrompt;
    public Vector3 interactableItemposition;
    public GameObject dropPrompt;
    public List<NPC> NPCsInRange;

    public delegate void EmptyVoidAction();
    public static event EmptyVoidAction LookAtNPC;
    public static event EmptyVoidAction LookAway;

    public delegate void ObjectAction( PickUpObject obj );
    public static event ObjectAction PickUpObject;
    public static event ObjectAction DropObject;

    public delegate void IntAction( Sprite sprite );
    public static event IntAction Communicate;

    private bool looking;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        Debug.DrawRay( cam.transform.position, cam.transform.forward * pickUpRange, Color.yellow, 0 );

        RaycastHit hit;
        if ( Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity ) ) {
            if ( hit.transform.GetComponent<NPC>() ) {
                LookAtNPC?.Invoke();
                looking = true;
			}
		}
        else if ( looking ) {
            looking = false;
            LookAway?.Invoke();
		}

        //Pick up
        if ( objectInHand == null ) {
            if ( !pickUpPrompt.activeSelf && 
                Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, pickUpRange, pickUpMask ) ) {
                pickUpPrompt.gameObject.SetActive( true );
                pickUpObject = hit.transform.GetComponent<PickUpObject>();
            }
            else if ( pickUpPrompt.activeSelf && 
                !Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, pickUpRange, pickUpMask ) ) {
                pickUpPrompt.gameObject.SetActive( false );
                pickUpObject = null;
            }

            if ( pickUpPrompt.activeSelf ) {
                if ( Input.GetKeyDown( KeyCode.E ) ) {
                    pickUpObject.transform.parent = this.transform;
                    pickUpObject.transform.localPosition = handPosition.localPosition;
                    pickUpObject.GetComponent<Rigidbody>().isKinematic = true;
                    objectInHand = pickUpObject;
                    pickUpObject = null;
                    pickUpPrompt.SetActive( false );
                    PickUpObject?.Invoke( objectInHand );
                }
            }
        }
		else {
            if ( !dropPrompt.activeSelf && 
                Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, dropRange, dropMask ) ) {
                dropPrompt.gameObject.SetActive( true );
                dropPosition = hit.point;
            }
            else if ( dropPrompt.activeSelf && 
                !Physics.Raycast( cam.transform.position, cam.transform.forward, out hit, dropRange, dropMask ) ) {
                dropPrompt.gameObject.SetActive( false );
                dropPosition = Vector3.zero;
            }

            if ( dropPrompt.activeSelf ) {
                if ( Input.GetKeyDown( KeyCode.E ) ) {
                    objectInHand.transform.parent = null;
                    objectInHand.transform.localPosition = dropPosition + new Vector3( 0, objectInHand.transform.localScale.y, 0 );
                    objectInHand.GetComponent<Rigidbody>().isKinematic = false;
                    DropObject?.Invoke( objectInHand );
                    objectInHand = null;
                    dropPrompt.SetActive( false );
                }
            }
            if (interactPrompt.activeSelf) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    objectInHand.transform.parent = null;
                    objectInHand.transform.localPosition = interactableItemposition;
                    objectInHand = null;
                }
            }
            
        }

    }

	private void OnTriggerEnter( Collider other ) {
		if ( other.GetComponent<NPC>() ) {
            NPC npc = other.GetComponent<NPC>();
            LookAtNPC += npc.OnLookAtNPC;
            LookAway += npc.OnLookAway;
            DropObject += npc.OnDropUpObject;
            PickUpObject += npc.OnPickUpObject;
            NPCsInRange.Add( npc );
		}
	}

	private void OnTriggerExit( Collider other ) {
        if ( other.GetComponent<NPC>() ) {
            NPC npc = other.GetComponent<NPC>();
            LookAtNPC -= npc.OnLookAtNPC;
            LookAway -= npc.OnLookAway;
            DropObject -= npc.OnDropUpObject;
            PickUpObject -= npc.OnPickUpObject;
            NPCsInRange.Remove( npc );
        }
    }
}
