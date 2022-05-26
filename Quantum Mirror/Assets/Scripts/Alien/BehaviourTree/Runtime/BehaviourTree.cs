using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace TheKiwiCoder {
    public class BehaviourTree : ScriptableObject {
        public BTNode rootNode;
        public BTNode.State treeState = BTNode.State.Running;
        public List<BTNode> nodes = new List<BTNode>();
        public Blackboard blackboard;

		//private void OnValidate()
		//{
  //          if ( rootNode != null )
		//	{
  //              BindBlackboard();
  //          }
		//}

		public BTNode.State Update() {
            if ( rootNode.state == BTNode.State.Running )
                treeState = rootNode.Update();
            return treeState;
        }

        public static List<BTNode> GetChildren( BTNode parent ) {
            List<BTNode> children = new List<BTNode>();

            if ( parent is BTDecoratorNode decorator && decorator.child != null )
                children.Add( decorator.child );

            if ( parent is BTRootNode rootNode && rootNode.child != null )
                children.Add( rootNode.child );

            if ( parent is BTCompositeNode composite )
                return composite.children;

            return children;
        }

        public static void Traverse( BTNode node, System.Action<BTNode> visiter ) {
            if ( node ) {
                visiter.Invoke( node );
                var children = GetChildren( node );
                children.ForEach( ( n ) => Traverse( n, visiter ) );
            }
        }

        public BehaviourTree Clone() {
            BehaviourTree tree = Instantiate( this );
            tree.rootNode = tree.rootNode.Clone();
            tree.nodes = new List<BTNode>();
            Traverse( tree.rootNode, ( n ) => {
                tree.nodes.Add(n);
            } );

            return tree;
        }

        public void BindContext( Context context ) {
            Traverse( rootNode, node => {
                node.context = context;
            } );
        }

        public void BindBlackboard()
        {
            Traverse( rootNode, node => {
                node.blackboard = blackboard;
            } );
        }


        #region Editor Compatibility
#if UNITY_EDITOR

        public BTNode CreateNode( System.Type type ) {
            BTNode node = ScriptableObject.CreateInstance( type ) as BTNode;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            node.blackboard = blackboard;

            Undo.RecordObject( this, "Behaviour Tree (CreateNode)" );
            nodes.Add( node );

            if ( !Application.isPlaying )
                AssetDatabase.AddObjectToAsset( node, this );

            Undo.RegisterCreatedObjectUndo( node, "Behaviour Tree (CreateNode)" );

            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode( BTNode node ) {
            Undo.RecordObject( this, "Behaviour Tree (DeleteNode)" );
            nodes.Remove( node );

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate( node );

            AssetDatabase.SaveAssets();
        }

        public void AddChild( BTNode parent, BTNode child ) {
            if ( parent is BTDecoratorNode decorator ) {
                Undo.RecordObject( decorator, "Behaviour Tree (AddChild)" );
                decorator.child = child;
                EditorUtility.SetDirty( decorator );
            }

            if (parent is BTRootNode rootNode) {
                Undo.RecordObject( rootNode, "Behaviour Tree (AddChild)" );
                rootNode.child = child;
                EditorUtility.SetDirty( rootNode );
            }

            if (parent is BTCompositeNode composite) {
                Undo.RecordObject( composite, "Behaviour Tree (AddChild)" );
                composite.children.Add( child );
                EditorUtility.SetDirty( composite );
            }
        }

        public void RemoveChild(BTNode parent, BTNode child ) {
            if ( parent is BTDecoratorNode decorator ) {
                Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)" );
                decorator.child = null;
                EditorUtility.SetDirty( decorator );
            }

            if ( parent is BTRootNode rootNode ) {
                Undo.RecordObject( rootNode, "Behaviour Tree (RemoveChild)" );
                rootNode.child = null;
                EditorUtility.SetDirty( rootNode );
            }

            if ( parent is BTCompositeNode composite ) {
                Undo.RecordObject( composite, "Behaviour Tree (RemoveChild)" );
                composite.children.Remove( child );
                EditorUtility.SetDirty( composite );
            }
        }
#endif
        #endregion Editor Compatibility
    }
}