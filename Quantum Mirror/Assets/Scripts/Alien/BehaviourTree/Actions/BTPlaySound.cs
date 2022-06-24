using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BTPlaySound : BTActionNode
{

    public AudioClip[] clips;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if ( clips.Length == 0 ) return State.Failure;
        if ( clips.Length == 1 )
            context.manager.PlaySound( clips[ 0 ] );
        else
            context.manager.PlaySound( clips[ Random.Range( 0, clips.Length ) ] );
        return State.Success;
    }
}
