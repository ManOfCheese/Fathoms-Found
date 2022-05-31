using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTimeSet<T> : ScriptableObject {

	public List<T> Items = new List<T>();

	public delegate void RunTimeSetEvent( T t );

	public event RunTimeSetEvent OnAdded;
	public event RunTimeSetEvent OnRemoved;

	public void Add( T item ) {
		if ( !Items.Contains( item ) ) {
			Items.Add( item );
			if ( OnAdded != null ) {
				OnAdded.Invoke( item );
			}
		}
	}

	public void Remove( T item ) {
		Items.Remove( item );
        if ( OnRemoved != null ) {
			OnRemoved.Invoke( item );
		}
	}
}
