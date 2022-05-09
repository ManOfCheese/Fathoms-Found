using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IsLookingAt : DecoratorNode
{

    public Transform other;
    public AlienManager agent;
    public float range;
    
    private bool looked;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        RaycastHit hit;
        if ( Physics.Raycast( other.transform.position, other.transform.forward, out hit, range ) )
        {
            Debug.Log( "bonkus" );
            if ( hit.transform.GetComponentInChildren<AlienManager>() == agent )
            {
                Debug.Log( "bingpot" );
                return child.Update();
            }
        }
        return State.Failure;
    }
}
