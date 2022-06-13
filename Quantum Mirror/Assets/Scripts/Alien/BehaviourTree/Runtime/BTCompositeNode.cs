using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public abstract class BTCompositeNode : BTNode {
        [HideInInspector] public List<BTNode> children = new List<BTNode>();

        public override BTNode Clone() {
            BTCompositeNode node = Instantiate( this );
            node.children = children.ConvertAll( c => c.Clone() );
            return node;
        }
    }
}