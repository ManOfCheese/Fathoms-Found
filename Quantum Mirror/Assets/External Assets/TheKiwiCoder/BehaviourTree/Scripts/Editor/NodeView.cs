using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

namespace TheKiwiCoder {

    public class NodeView : UnityEditor.Experimental.GraphView.Node {
        public Action<NodeView> OnNodeSelected;
        public BTNode node;
        public Port input;
        public Port output;

        public NodeView( BTNode node ) : base( AssetDatabase.GetAssetPath( BehaviourTreeSettings.GetOrCreateSettings().nodeXml ) ) {
            this.node = node;
            this.node.name = node.GetType().Name;
            this.title = node.name.Replace( "(Clone)", "" ).Replace( "Node", "" );
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();
            SetupDataBinding();
        }

        private void SetupDataBinding() {
            Label descriptionLabel = this.Q<Label>( "description" );
            descriptionLabel.bindingPath = "description";
            descriptionLabel.Bind( new SerializedObject( node ) );
        }

        private void SetupClasses() {
            if ( node is BTActionNode )
                AddToClassList( "action" );
            else if ( node is BTBlackBoardActionNode )
                AddToClassList( "blackbaord" );           
            else if ( node is BTCompositeNode )
                AddToClassList( "composite" );
            else if ( node is BTDecoratorNode )
                AddToClassList( "decorator" );
            else if ( node is BTRootNode )
                AddToClassList( "root");
        }

        private void CreateInputPorts() {
            if ( node is BTActionNode )
                input = new NodePort( Direction.Input, Port.Capacity.Single );
            else if ( node is BTBlackBoardActionNode )
                input = new NodePort( Direction.Input, Port.Capacity.Single );
            else if ( node is BTCompositeNode )
                input = new NodePort( Direction.Input, Port.Capacity.Single );
            else if ( node is BTDecoratorNode )
                input = new NodePort( Direction.Input, Port.Capacity.Single );
            else if ( node is BTRootNode ) {

            }

            if ( input != null ) {
                input.portName = "";
                input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add( input );
            }
        }

        private void CreateOutputPorts() {
            if (node is BTActionNode )
			{

			}
            else if ( node is BTBlackBoardActionNode )
			{

			}
            else if ( node is BTCompositeNode )
                output = new NodePort( Direction.Output, Port.Capacity.Multi );
            else if ( node is BTDecoratorNode )
                output = new NodePort( Direction.Output, Port.Capacity.Single );
            else if ( node is BTRootNode )
                output = new NodePort( Direction.Output, Port.Capacity.Single );

            if ( output != null ) {
                output.portName = "";
                output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add( output );
            }
        }

        public override void SetPosition( Rect newPos ) {
            base.SetPosition( newPos );
            Undo.RecordObject( node, "Behaviour Tree (Set Position" );
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty( node );
        }

        public override void OnSelected() {
            base.OnSelected();
            if ( OnNodeSelected != null ) {
                OnNodeSelected.Invoke( this );
            }
        }

        public void SortChildren() {
            if ( node is BTCompositeNode composite ) {
                composite.children.Sort( SortByHorizontalPosition );
            }
        }

        private int SortByHorizontalPosition( BTNode left, BTNode right ) {
            return left.position.x < right.position.x ? -1 : 1;
        }

        public void UpdateState() {

            RemoveFromClassList( "running" );
            RemoveFromClassList( "failure" );
            RemoveFromClassList( "success" );

            if ( Application.isPlaying ) {
                switch ( node.state ) {
                    case BTNode.State.Running:
                        if (node.started)
                            AddToClassList( "running" );
                        break;
                    case BTNode.State.Failure:
                        AddToClassList( "failure" );
                        break;
                    case BTNode.State.Success:
                        AddToClassList( "success" );
                        break;
                }
            }
        }
    }
}