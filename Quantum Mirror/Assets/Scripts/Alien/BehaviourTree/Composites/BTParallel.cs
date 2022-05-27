using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheKiwiCoder {
    public class BTParallel : BTCompositeNode {
        List<State> childrenLeftToExecute = new List<State>();

        protected override void OnStart() {
            childrenLeftToExecute.Clear();
            children.ForEach(a => {
                childrenLeftToExecute.Add( State.Running );
            });
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            bool stillRunning = false;
            int failureCount = 0;
            for ( int i = 0; i < childrenLeftToExecute.Count(); ++i ) {
                if ( childrenLeftToExecute[ i ] == State.Running ) {
                    var status = children[ i ].Update();
                    if ( status == State.Failure ) {
                        failureCount++;
                    }

                    if ( status == State.Running ) {
                        stillRunning = true;
                    }

                    childrenLeftToExecute[ i ] = status;
                }
            }

            if ( failureCount == children.Count )
                return State.Failure;
            else
                return stillRunning ? State.Running : State.Success;
        }

        void AbortRunningChildren() {
            for ( int i = 0; i < childrenLeftToExecute.Count(); ++i ) {
                if ( childrenLeftToExecute[ i ] == State.Running ) {
                    children[ i ].Abort();
                }
            }
        }
    }
}