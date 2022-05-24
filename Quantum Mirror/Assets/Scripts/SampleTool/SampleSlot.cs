using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSlot : MonoBehaviour
{

    public GameObject sampleInSlot;

    public delegate void ObjectEvent( GameObject _object );
    public event ObjectEvent itemSlotted;
    public event ObjectEvent itemRemoved;

	private void Awake()
	{
		if ( transform.childCount > 0 )
			sampleInSlot = transform.GetChild( 0 ).gameObject;
	}

	public void OnItemSlotted( GameObject _object )
	{
        itemSlotted?.Invoke( _object );
        sampleInSlot = _object;
	}

    public void OnItemRemoved( GameObject _object )
    {
        itemRemoved?.Invoke( _object );
        sampleInSlot = null;
    }

}
