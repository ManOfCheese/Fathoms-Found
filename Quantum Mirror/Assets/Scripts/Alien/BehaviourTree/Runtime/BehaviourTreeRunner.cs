using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class BehaviourTreeRunner : MonoBehaviour {

        // The main behaviour tree asset
        public BehaviourTree tree;
        public Blackboard blackBoard;
        public BoolValue paused;

        // Storage container object to hold game object subsystems
        Context context;

        // Start is called before the first frame update
        void Start() {
            context = CreateBehaviourTreeContext();
            tree = tree.Clone();
            tree.blackboard = blackBoard;
            tree.BindBlackboard();
            tree.BindContext( context );
        }

        // Update is called once per frame
        void Update() {
            if ( paused.Value ) { return; }
            if ( tree ) {
                tree.Update();
            }
        }

        Context CreateBehaviourTreeContext() {
            return Context.CreateFromGameObject( gameObject );
        }

        private void OnDrawGizmosSelected() {
            if ( !tree ) {
                return;
            }

            BehaviourTree.Traverse( tree.rootNode, ( n ) => {
                if ( n.drawGizmos ) {
                    n.OnDrawGizmos();
                }
            });
        }
    }
}