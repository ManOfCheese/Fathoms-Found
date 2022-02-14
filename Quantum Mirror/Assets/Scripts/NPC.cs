using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC : MonoBehaviour {

    public GameObject player;
    public Image image;
    public NavMeshAgent agent;
    public List<Sprite> emojis;
    public List<ActionSequence> emojiReactions;
    public List<PickUpObject> objects;
    public List<ActionSequence> objectReactions;

    private Dictionary<PickUpObject, ActionSequence> actionsByObject;
    private List<Action> currentActions; 

	private void Awake() {
        actionsByObject = new Dictionary<PickUpObject, ActionSequence>();
		for ( int i = 0; i < objects.Count; i++ ) {
            actionsByObject.Add( objects[ i ], objectReactions[ i ] );
		}
	}

	private void Update() {
        if ( currentActions != null ) {
            for ( int i = 0; i < currentActions.Count; i++ ) {
                currentActions[ i ].ExecuteAction( this );
            }
        }
	}

	public void OnLookAtNPC() {
        transform.LookAt( player.transform.position );
        transform.rotation = Quaternion.Euler( new Vector3( 0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z ) );
	}

    public void OnLookAway() {
        transform.LookAt( transform.forward );
	}

    public void OnPickUpObject( PickUpObject obj ) {
		for ( int i = 0; i < objects.Count; i++ ) {
            if ( objects[ i ] == obj ) {
                currentActions = objectReactions[ i ].actions;
			}
		}
	}

    public void OnDropUpObject( PickUpObject obj ) {
        for ( int i = 0; i < objects.Count; i++ ) {
            if ( objects[ i ] == obj ) {
                if ( currentActions == objectReactions[ i ].actions ) {
                    currentActions = null;
                    image.sprite = null;
				}
            }
        }
    }

    public void OnCommunicate( Sprite sprite ) {

	}

}
